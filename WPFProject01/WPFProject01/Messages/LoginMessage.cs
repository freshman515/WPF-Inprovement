using CommunityToolkit.Mvvm.Messaging.Messages;
using WPFProject01.Entities;

namespace WPFProject01.Messages;

public class LoginMessage:ValueChangedMessage<bool> {
	public LoginMessage(bool value) : base(value) {
	}
}