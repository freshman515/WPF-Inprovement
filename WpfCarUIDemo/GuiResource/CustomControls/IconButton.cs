using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GuiResource.CustomControls;

public class IconButton : Button {
	public Drawing Icon {
		get => (Drawing)GetValue(IconProperty);
		set => SetValue(IconProperty, value);
	}

	public static readonly DependencyProperty IconProperty =
		DependencyProperty.Register(nameof(Icon), typeof(Drawing), typeof(IconButton), new PropertyMetadata(null));
 

}