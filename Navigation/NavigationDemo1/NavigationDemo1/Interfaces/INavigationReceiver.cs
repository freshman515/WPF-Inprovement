namespace NavigationDemo1.Interfaces;
/// <summary>
/// 目标 VM 实现此接口以强类型接收参数
/// </summary>
public interface INavigationReceiver<TParam> {
    void OnNavigated(TParam parameter);
}
public interface INavigationReceiver {
    void OnNavigated(object? parameter);
}