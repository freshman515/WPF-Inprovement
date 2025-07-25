using WPFProject01.enums;

namespace WPFProject01.Messages;

public class DialogMessage {
	public string Title { get; set; } = "提示";
	public string Content { get; set; }
	public DialogButtons Buttons { get; set; } = DialogButtons.OK;
	public TaskCompletionSource<bool?> TaskSource { get; } = new();

	public DialogMessage() {
		
	}
	public DialogMessage( string content) {
		Content = content;
	}

	public DialogMessage(string title, string content) {
		Title = title;
		Content = content;
	}
}