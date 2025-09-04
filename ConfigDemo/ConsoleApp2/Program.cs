

using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;

var demo = new Demo();
demo.NotifyEvent += () => { Console.WriteLine("123"); };
demo.NotifyEvent += () => { Console.WriteLine("345"); };
demo.NotifyEvent += () => { Console.WriteLine("678"); };

demo.Notify();
class Demo {
	private List<Action> _actions = new List<Action>();
	public event Action NotifyEvent {
		add { _actions.Add(value); }
		remove { _actions.Remove(value); }
	}
	public void Notify() {
		foreach (var action in _actions) {
			action();
		}
	}
}

