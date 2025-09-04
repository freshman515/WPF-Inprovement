using System.Windows;

namespace IOC.Services;

public class MessageBoxService:IMessageBoxService {
	public void ShowMessage(string message) {
		MessageBox.Show(message);
	}
}