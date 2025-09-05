using RegeionNavigationDemo1.Enums;
using RegeionNavigationDemo1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using RegeionNavigationDemo1.Views;

namespace RegeionNavigationDemo1.Services;

public class ToastService : IToastService {
    private static readonly List<ToastWindow> _activeToasts = [];

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT {
        public int Left, Top, Right, Bottom;
    }
    public void Show(string message, PromptStatus status = PromptStatus.Right, TimeSpan? duration = null, Window owner = null) {
        Application.Current.Dispatcher.Invoke(() =>
        {
            var toast = new ToastWindow(message, status, duration);
            _activeToasts.Add(toast);
            toast.Closed += (s, e) => _activeToasts.Remove(toast);

            toast.Loaded += (s, e) =>
            {
                PositionToast(toast, owner);
                RepositionToasts();
            };

            toast.Show();
        });
    }

    private static void PositionToast(Window toast, Window owner) {
        if (toast.ActualWidth <= 0 || toast.ActualHeight <= 0) {
            toast.Dispatcher.BeginInvoke(
                new Action(() => PositionToast(toast, owner)),
                System.Windows.Threading.DispatcherPriority.Render);
            return;
        }

        double left, top;

        if (owner != null) {
            var hwnd = new WindowInteropHelper(owner).Handle;
            if (GetWindowRect(hwnd, out RECT rect)) {
                var source = PresentationSource.FromVisual(owner);
                var m = source?.CompositionTarget?.TransformFromDevice ?? Matrix.Identity;

                var topLeft = m.Transform(new Point(rect.Left, rect.Top));
                var bottomRight = m.Transform(new Point(rect.Right, rect.Bottom));

                var ownerWidth = bottomRight.X - topLeft.X;
                var ownerHeight = bottomRight.Y - topLeft.Y;

                left = topLeft.X + (ownerWidth - toast.ActualWidth) / 2.0;
                top = topLeft.Y + (ownerHeight - toast.ActualHeight) / 2.0 - 50;
            } else {
                left = owner.Left + (owner.ActualWidth - toast.ActualWidth) / 2.0;
                top = owner.Top + (owner.ActualHeight - toast.ActualHeight) / 2.0 - 50;
            }
        } else {
            left = (SystemParameters.WorkArea.Width - toast.ActualWidth) / 2.0;
            top = (SystemParameters.WorkArea.Height - toast.ActualHeight) / 2.0 - 100;
        }

        toast.Left = left;
        toast.Top = top;
    }

    private static void RepositionToasts() {
        for (int i = 0; i < _activeToasts.Count; i++) {
            double top = SystemParameters.WorkArea.Height - 100 - (i * 80);
            _activeToasts[i].Top = top;
        }
    }

    public void ShowSuccess(string message, Window owner = null, TimeSpan? duration = null)
        => Show(message, PromptStatus.Right, duration, owner);

    public void ShowError(string message, Window owner = null, TimeSpan? duration = null)
        => Show(message, PromptStatus.Error, duration, owner);

    public void ShowHint(string message, Window owner = null, TimeSpan? duration = null)
        => Show(message, PromptStatus.Hint, duration, owner);
}