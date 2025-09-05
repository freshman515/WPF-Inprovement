using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RegeionNavigationDemo1.Enums;

namespace RegeionNavigationDemo1.Interfaces;

public interface IToastService {
    void Show(string message, PromptStatus status = PromptStatus.Right, TimeSpan? duration = null, Window owner = null);
    void ShowSuccess(string message, Window owner = null, TimeSpan? duration = null);
    void ShowError(string message, Window owner = null, TimeSpan? duration = null);
    void ShowHint(string message, Window owner = null, TimeSpan? duration = null);
}