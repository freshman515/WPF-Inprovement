namespace RegeionNavigationDemo1.Interfaces;
public interface INavigationReceiver<TParam> {
    void OnNavigated(TParam parameter);
}
public interface INavigationReceiver {
    void OnNavigated(object? parameter);
}