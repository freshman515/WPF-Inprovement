using CommunityToolkit.Mvvm.Messaging.Messages;

namespace WPFProject01.Messages;

public class ToggleButtonVisibilityMessage :ValueChangedMessage<bool> {
	public ToggleButtonVisibilityMessage(bool value) : base(value) {
	}
}