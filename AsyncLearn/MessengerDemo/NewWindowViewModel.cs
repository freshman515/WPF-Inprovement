using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MessengerDemo.Messengs;

namespace MessengerDemo;

public partial class NewWindowViewModel :ObservableObject {
	[ObservableProperty] private string content = "hello";

	public NewWindowViewModel() {
		WeakReferenceMessenger.Default.Register<NotificationMessage>(this, (r, m) => {
			Content = m.Content;
		});
	}

}