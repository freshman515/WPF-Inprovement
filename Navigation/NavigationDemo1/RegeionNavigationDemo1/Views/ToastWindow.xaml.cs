using System.Windows;
using System.Windows.Media.Animation;
using RegeionNavigationDemo1.Enums;

namespace RegeionNavigationDemo1.Views {
	/// <summary>
	/// ToastWindow.xaml 的交互逻辑
	/// </summary>
	public partial class ToastWindow : Window {
		public string Status { get; }
		private readonly TimeSpan _duration;
		public string Message { get; }

		public ToastWindow(string message, PromptStatus status = PromptStatus.Right, TimeSpan? duration = null) {
			_duration = duration ?? TimeSpan.FromSeconds(1);
			Status = status.ToString(); 
			Message = string.IsNullOrEmpty(message) ? GetDefaultMessage(status) : message;
			InitializeComponent();
			DataContext = this;
			Loaded += ToastWindow_Loaded;
			Opacity = 0;
		}

		private string GetDefaultMessage(PromptStatus status) {
			//var isEnglish = GlobalParameter.Instance.Language == "en-US";
			var isEnglish = false;
			switch (status) {
				case PromptStatus.Right:
					return isEnglish ? "OK" : "成功";
				case PromptStatus.Error:
					return isEnglish ? "Error" : "失败";
				case PromptStatus.Hint:
					return isEnglish ? "Warning" : "提示";
				default:
					return isEnglish ? "OK" : "成功";

			}
		}


		private async void ToastWindow_Loaded(object sender, RoutedEventArgs e) {
			var anim = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(200)));
			BeginAnimation(OpacityProperty, anim);
			await Task.Delay(_duration);
			var fadeOut = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(300)));
			fadeOut.Completed += (s2, e2) => this.Close();
			BeginAnimation(OpacityProperty, fadeOut);
		}

	}
}
