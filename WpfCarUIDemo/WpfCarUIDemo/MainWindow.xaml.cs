using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfCarUIDemo {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		//HT 消息
		private const int HTLEFT = 10;
		private const int HTRIGHT = 11;
		private const int HTTOP = 12;
		private const int HTTOPLEFT = 13;
		private const int HTTOPRight = 14;
		private const int HTBOTTOM = 15;
		private const int HTBOTTOMLEFT = 16;
		private const int HTBOTTOMRight = 17;

		public MainWindow() {
			InitializeComponent();
			int a = 0;
			this.StateChanged += MainWindow_OnStateChanged;
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e) {
			Application.Current.Shutdown();
		}

		private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			if (e.LeftButton == MouseButtonState.Pressed) {
				this.DragMove();
			}
		}
		//	你重写它，是为了在 窗口完全创建并拥有句柄（HWND）之后，执行自己的初始化逻辑。
		//	这个时机比 Loaded 更早，且是窗口句柄可用的最佳入口。
		//protected override void OnSourceInitialized(EventArgs e) {
		//	base.OnSourceInitialized(e);
		//	var source = (HwndSource)PresentationSource.FromVisual(this);  //这行是获取当前窗口对应的 HwndSource（Windows 句柄源）。
		//	source.AddHook(Wndproc); //给这个 WPF 窗口添加一个 Win32 消息钩子（Hook） 当系统向窗口发送消息（如鼠标点击、拖动、命中测试、DPI 更改等）时，WPF 会先调用你注册的这个 WndProc 方法。
		//}

		//private IntPtr Wndproc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
		//	const int WM_NCHITTEST = 0x0084;  //询问鼠标当前在窗口的哪个区域（标题栏？边框？客户区？）
		//	if (msg == WM_NCHITTEST) { //判断系统发来的是否是 WM_NCHITTEST 消息。 
		//		handled = true;  //	是否标记“我已经处理了这个消息”，如果设为 true，系统就不会继续处理这条消息
		//		return HandleHintTest(lParam);
		//	}
		//	return IntPtr.Zero;
		//}

		private IntPtr HandleHintTest(IntPtr lParam) {
			Point point = new Point((short)lParam.ToInt32(), (short)(lParam.ToInt32() >> 16));  // 取出鼠标在屏幕上的坐标
			Point mousePoint = this.PointFromScreen(point);  //把鼠标在屏幕上的坐标转换为 wpf的窗口内部坐标

			if (mousePoint.X > 5 && mousePoint.X < this.ActualWidth - 5 && mousePoint.Y > 5 &&
				mousePoint.Y < this.ActualHeight - 5) {
				return new IntPtr(1); //HTCLINT;
			}

			if (mousePoint.X <= 5) {
				if (mousePoint.Y <= 5) return HTTOPLEFT;
				if (mousePoint.Y >= ActualHeight - 5) return HTBOTTOMLEFT;
				return HTLEFT;
			}

			if (mousePoint.X >= this.ActualWidth - 5) {
				if (mousePoint.Y <= 5) return HTTOPRight;
				if (mousePoint.Y >= ActualHeight - 5) return HTBOTTOMRight;
				return HTRIGHT;
			}

			if (mousePoint.Y <= 5) return HTTOP;
			if (mousePoint.Y >= this.ActualHeight - 5) return HTBOTTOM;
			return IntPtr.Zero; //继续传播到框架内部的WIndow process处理
		}

		private void RestoreButton_Click(object sender, RoutedEventArgs e) {
			this.WindowState = WindowState.Normal;

		}

		private void MaximizeButton_Click(object sender, RoutedEventArgs e) {
			this.WindowState = WindowState.Maximized;

		}

		private void MainWindow_OnStateChanged(object? sender, EventArgs e) {
			ToggleMaximizeRestoreButtons();
		}

		private void ToggleMaximizeRestoreButtons() {
			if (this.WindowState == WindowState.Maximized) {
				Maximize_Button.Visibility = Visibility.Collapsed;
				Restore_Button .Visibility = Visibility.Visible;
			}
			else {
				Maximize_Button.Visibility = Visibility.Visible;
				Restore_Button .Visibility = Visibility.Collapsed;
			}
		}

		private void MinimizeButton_Click(object sender, RoutedEventArgs e) {
			this.WindowState = WindowState.Minimized;
		}
	}
}