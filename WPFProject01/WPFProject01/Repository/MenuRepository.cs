using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using WPFProject01.Entities;

namespace WPFProject01.Repository;

public class MenuRepository :IMenuRepository{
	private readonly MyDbContext _dbContext;

	public MenuRepository(MyDbContext dbContext) {
		_dbContext = dbContext;
	}
	public async Task<ObservableCollection<Menu>> GetMenuItemsAsync() {
		var list = await _dbContext.Menus.ToListAsync();
		var dict = list.ToDictionary(m=>m.Id);
		var rootMenus = new List<Menu>();
		foreach (var menu in list) {
			if (menu.ParentId == null) {
				rootMenus.Add(menu);
			}else if (dict.TryGetValue(menu.ParentId.Value, out var parent)) {
				parent.Children.Add(menu);	
			}
		}
		return new ObservableCollection<Menu>(rootMenus);

	}
}