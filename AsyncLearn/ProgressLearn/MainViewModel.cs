using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ProgressLearn;

public partial class MainViewModel :ObservableObject{
	public MainViewModel() {
		
	}

	[RelayCommand]
	async Task DoJobAsync() {
		
		for (int i = 0; i < 100; i++) {
			await Task.Delay(50);
		}
	}
}