namespace MessengerDemo.Messengs;

public class NotificationMessage {
	public string Content { get; set; }
	public NotificationMessage(string content) {
		Content = content;
	}
}