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

namespace ProgressLearn {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			this.DataContext = new MainViewModel();
		}
		
		//private async void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
		//	try {
		//		button.IsEnabled = false;
		//		var progress = new Progress<double>(_ => progressBar.Value +=1);
		//		await DoJobAsync(progress);
		//		button.IsEnabled = true;
		//	} catch ( Exception ex) {


		//	}
		//}

		async Task DoJobAsync(IProgress<double> progress) {
			for (int i = 1; i <= 100; i++) {
				await Task.Delay(50).ConfigureAwait(false);
				//progressBar.Value = i;
				progress.Report(0);
				;
			}
		}
	}
}