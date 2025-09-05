namespace RegeionNavigationDemo1.Interfaces;

public interface IDialogReceiver<in TParam> {
    void OnDialogOpened(TParam parameter);
}