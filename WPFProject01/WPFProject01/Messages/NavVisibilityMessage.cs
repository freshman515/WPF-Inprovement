using CommunityToolkit.Mvvm.Messaging.Messages;

namespace WPFProject01.Messages;

public class NavVisibilityMessage :ValueChangedMessage<bool> {
	public NavVisibilityMessage(bool value) : base(value) {
	}
}