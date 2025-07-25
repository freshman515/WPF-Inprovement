using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Extensions.Configuration;
using LiveChartsCore.SkiaSharpView.WPF;

namespace WPFProject01.ViewModels;

[ObservableObject]
public partial class AboutViewModel {
	[ObservableProperty] private string _version;
	
	public IEnumerable<ISeries> Series { get; set; }
	public IEnumerable<ISeries> Series2 { get; set; }
	public string[] Labels { get; set; }
	public AboutViewModel() {
		readConfiguration();
		Series = new ISeries[] {
			new LineSeries<double>() {
				Values = [3, 5, 7, 9, 2, 4, 8, 4]
			}

		};
		var axis = new Axis(); // 如果这里能 new 出来，就说明你能在 XAML 里用 <lvc:Axis />
	
		Series2 = new ISeries[] {
			new ColumnSeries<double>() {
				Name = "销售额",
				Values =new double[] { 500, 720, 610, 900, 650, 1100, 850 }
			}
		};
		Labels = ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"];
	}

	private void readConfiguration() {
		var configurationRoot = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
			.AddJsonFile("appsettings.json").Build();
		Version = configurationRoot["Version"] ?? string.Empty;
	}
}