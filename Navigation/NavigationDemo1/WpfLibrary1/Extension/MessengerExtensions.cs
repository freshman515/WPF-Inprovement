using CommunityToolkit.Mvvm.Messaging;

namespace Common.Core.Extension;

public static class MessengerExtensions {
	public static void OnAsync<TMessage>(this IMessenger messenger, object recipient, Func<TMessage, Task> handler) where TMessage : class => messenger.Register<TMessage>(recipient, async (r, m) => await handler(m));
	public static void On<TMessage>(this IMessenger messenger, object recipient, Func<TMessage, Task> handler) where TMessage : class => messenger.Register<TMessage>(recipient, (r, m) => handler(m));
}