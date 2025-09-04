using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MessengerDemo.Messengs;

namespace MessengerDemo;
[ObservableObject]
public partial class MainViewModel {
	public MainViewModel() {
		
	}

	[ObservableProperty] string message;
	[RelayCommand]
	void SendMessage() {
		WeakReferenceMessenger.Default.Send(new NotificationMessage(Message));
	}

	[RelayCommand]
	void OpenNewWindow() {
		new NewWindow().Show();
	}

}