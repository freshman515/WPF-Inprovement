using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsyncLearn;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();
    }
    CancellationTokenSource cts = new CancellationTokenSource();

    private async void Job() {
        try {
            var task = Task.Delay(10000, cts.Token);
            Thread.Sleep(3000);
            cts.Cancel();
            await task;
        } catch (Exception e) {
            MyTextBox.Text = e.Message;
                
        } finally {
            cts.Dispose();
        }
    }

    private async void ButtonClickHandler(object sender, RoutedEventArgs e) {

        var tasks = new List<Task>();
        for (int i = 0; i < 5; i++) {
            tasks.Add(FooAsync());

        }

        await Task.WhenAll(tasks);

        //var task = FooAsync();  //任务启动
        //this.OutputTextBlock.Text = "Doing...."; //UI线程未阻塞 继续执行
        //await task; // 等待任务完成 ,UI线程未阻塞，但是后续的代码会被挂起，等任务完成后继续执行
        this.OutputTextBlock.Text = "Nicely Done";
    }

    private async Task<string> FooAsync() {
        MyTextBox.Text += "Hello";
        await Task.Delay(3000);
        MyTextBox.Text += "world";

        return "Done";

    }

    //private async Task<int> HeavyJob() {
    //    await Task.Delay(3000).ConfigureAwait(false); //异步等待3秒，等待ui线程未阻塞，此时就会死锁，因为ui线程被阻塞了
    //    return 123;
    //}
    private int HeavyJob() {
        Thread.Sleep(3000);
        return 10;
    }

    private async void ButtonClickHandler2(object sender, RoutedEventArgs e) {
        //var res = await Task.Run(HeavyJob);
        //MyTextBox.Text = res.ToString();

        Job();
    }
}

interface IMyFoo {
    Task<string> FooAsync();
}


public class MyFoo : IMyFoo {
    public Task<string> FooAsync() {
        return Task.FromResult("123");
    }
}