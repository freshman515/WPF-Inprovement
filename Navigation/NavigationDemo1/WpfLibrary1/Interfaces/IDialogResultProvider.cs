namespace Common.Core.Interfaces;

public interface IDialogResultProvider<out TResult> {
    TResult? GetDialogResult();
}