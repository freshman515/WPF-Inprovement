using System.Windows.Controls;

namespace WPFProject01.Service;

public interface IDialogService {
	Task<T?> ShowDialog<T>(UserControl? control,object? viewModel);	
}