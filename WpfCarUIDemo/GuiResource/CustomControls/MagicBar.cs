using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Jamesnet.Wpf.Animation;
using Jamesnet.Wpf.Controls;

namespace GuiResource.CustomControls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:GuiResource.CustomControls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:GuiResource.CustomControls;assembly=GuiResource.CustomControls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class MagicBar : ListBox
    {
       //private ValueItem vi ;
       //private Storyboard sb ;
        static MagicBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MagicBar), new FrameworkPropertyMetadata(typeof(MagicBar)));
        }

        public override void OnApplyTemplate() {
            base.OnApplyTemplate();
            Grid circle = (Grid)GetTemplateChild("PART_Circle");
            InitStoryBoard(circle);
        }
        private DoubleAnimation _moveAnimation;
        private Storyboard sb;
        private void InitStoryBoard(Grid circle) {
            sb = new Storyboard();

            _moveAnimation = new DoubleAnimation {
                Duration = TimeSpan.FromMilliseconds(500),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            Storyboard.SetTarget(_moveAnimation, circle);
            Storyboard.SetTargetProperty(_moveAnimation, new PropertyPath("(Canvas.Left)"));

            sb.Children.Add(_moveAnimation);
        }
        protected override void OnSelectionChanged(SelectionChangedEventArgs e) {
            base.OnSelectionChanged(e);

            // 设置目标值
            _moveAnimation.To = SelectedIndex * 80;

            // 重新开始动画
            sb.Begin();
        }
        //private void InitStoryBoard(Grid circle) {
        //    vi = new ValueItem();
        //    sb = new Storyboard();
        //    vi.Mode = EasingFunctionBaseMode.QuinticEaseInOut;
        //    vi.Property = new PropertyPath(Canvas.LeftProperty);
        //    vi.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 500));
        //    Storyboard.SetTarget(vi,circle);
        //    Storyboard.SetTargetProperty(vi,vi.Property);
        //    sb.Children.Add(vi);
        //}

        //protected override void OnSelectionChanged(SelectionChangedEventArgs e) {
        //    base.OnSelectionChanged(e);
        //    vi.To  = SelectedIndex*80;
        //    sb.Begin();

        //}
    }
}
