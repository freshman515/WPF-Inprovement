namespace Common.Core.Interfaces;

public interface IDialogReceiver<in TParam> {
    void OnDialogOpened(TParam parameter);
}