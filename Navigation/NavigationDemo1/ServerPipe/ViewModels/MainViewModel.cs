using Common.Core.Enums;
using Common.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ServerPipe.ViewModels;

public partial class MainViewModel : ObservableObject {
    private readonly IMessageServer _server;
    public ObservableCollection<string> Messages { get; } = new();
    [ObservableProperty] private string _message1 = "Hello from Server";
    public MainViewModel(IMessageServer server) {
        _server = server;
        InitServer();
    }

    private void InitServer() {
        _server.Start();
        _server.OnUi("ShowMessage", args => Messages.Add(args[0].GetValue<string>()));
    }

    [RelayCommand]
    void SendToClientMultiLangName() {
        _server.Broadcast("IsEnglish", true);
    }
    [RelayCommand]
    void SendToClientM() {
        _server.SendTo(ClientId.ClientM, "ShowMessage", "server to M");
    }
    [RelayCommand]
    void SendToClientS() {
        _server.SendTo(ClientId.ClientS, "ShowMessage", "server TO S");
    }
    [RelayCommand]
    void Broadcast() {
        _server.Broadcast("ShowMessage", Message1);

    }

}