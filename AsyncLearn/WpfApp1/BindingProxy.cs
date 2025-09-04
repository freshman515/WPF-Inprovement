using System.Security.Cryptography;
using System.Windows;
using System.Windows.Documents;

namespace WpfApp1;

public class BindingProxy:Freezable {
    protected override Freezable CreateInstanceCore() => new BindingProxy();

    public object Data {
        get => (object)GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }
    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register(nameof(Data), typeof(object), typeof(BindingProxy));



}