using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFProject01.Controls;

public partial class UserCard : UserControl {
	public UserCard() {
		InitializeComponent();
	}

	public string UserName {
		get => (string)GetValue(UserNameProperty);
		set => SetValue(UserNameProperty, value);
	}

	public static readonly DependencyProperty UserNameProperty =
		DependencyProperty.Register("UserName", typeof(string), typeof(UserCard), new PropertyMetadata(""));

	public string Title {
		get => (string)GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	}

	public static readonly DependencyProperty TitleProperty =
		DependencyProperty.Register("Title", typeof(string), typeof(UserCard), new PropertyMetadata(""));

	public string Email {
		get => (string)GetValue(EmailProp);
		set => SetValue(EmailProp, value);
	}

	public static readonly DependencyProperty EmailProp =
		DependencyProperty.Register("Email", typeof(string), typeof(UserCard), new PropertyMetadata(""));

	public ImageSource Avatar {
		get => (ImageSource)GetValue(AvatarProp);
		set => SetValue(AvatarProp, value);
	}

	public static readonly DependencyProperty AvatarProp =
		DependencyProperty.Register("Avatar", typeof(ImageSource), typeof(UserCard), new PropertyMetadata(null));

	public double ImgWidth {
		get => (double)GetValue(ImgWidthProp);
		set => SetValue(ImgWidthProp, value);
	}

	public static readonly DependencyProperty ImgWidthProp =
		DependencyProperty.Register("ImgWidth", typeof(double), typeof(UserCard), new PropertyMetadata(50.0));

	public double ImgHeight {
		get => (double)GetValue(ImgHeightProp);
		set => SetValue(ImgHeightProp, value);
	}

	public static readonly DependencyProperty ImgHeightProp =
		DependencyProperty.Register("ImgHeight", typeof(double), typeof(UserCard), new PropertyMetadata(50.0));


	
}