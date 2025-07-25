using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFProject01.Controls {
	public class ConnectionIndicator :Control{
		public ConnectionIndicator() {
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(ConnectionIndicator),
				new FrameworkPropertyMetadata(typeof(ConnectionIndicator))//设置这个控件的“默认样式的查找键”就是它自己这个类型。
				);
		}



		public string State {
			get { return (string)GetValue(StateProperty); }
			set { SetValue(StateProperty, value); }
		}

		// Using a DependencyProperty as the backing store for State.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StateProperty =
			DependencyProperty.Register("State", typeof(string), typeof(ConnectionIndicator), new PropertyMetadata("Idle",OnStateChanged));

		//每当State属性变化时，就切换一次状态值
		private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			var ctrl = (ConnectionIndicator)d;
			var state = ctrl.State;
			VisualStateManager.GoToState(ctrl, state, true);
		}

		//模板（ControlTemplate）被应用时会触发。
		public override void OnApplyTemplate() {
			base.OnApplyTemplate();
			VisualStateManager.GoToState(this,State,true);
		}
	}

}
