using System.Windows;
using Common.Core.Enums;

namespace Common.Core.Interfaces;

public interface INotificationService {
    void Show(string message, PromptStatus status = PromptStatus.Right, TimeSpan? duration = null, Window owner = null);
    void ShowSuccess(string message, Window owner = null, TimeSpan? duration = null);
    void ShowError(string message, Window owner = null, TimeSpan? duration = null);
    void ShowHint(string message, Window owner = null, TimeSpan? duration = null);
}