
using System.Collections.ObjectModel;
using WPFProject01.Entities;

namespace WPFProject01.Repository;

public interface IMenuRepository {
	Task<ObservableCollection<Menu>> GetMenuItemsAsync();
}