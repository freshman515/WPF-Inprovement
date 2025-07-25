using WPFProject01.Dtos;
using WPFProject01.Entities;
using WPFProject01.Repository;

namespace WPFProject01.Service;

public interface IUserService {
	Task<bool> RegisterUser(string username, string password, string confirmPassword, string telephone);
	Task<User> Login(string username, string password);
	Task AllSelected(bool isSelected);	
	Task<List<User>> GetUsersAsync(UserQueryDto? query=null)=>GetRepository().GetUsersAsync(query);
	Task<User> GetUserAsync(int id)=>GetRepository().GetUserAsync(id);
	Task<User> GetUserByNameAsync(string name)=>GetRepository().GetUserByNameAsync(name);
	Task<User> AddUserAsync(User user)=>GetRepository().AddUserAsync(user);
	Task<User> UpdateUserAsync(User user)=>GetRepository().UpdateUserAsync(user);
	Task<bool> DeleteUserAsync(int id)=>GetRepository().DeleteUserAsync(id);
	Task<bool> ExistsByUsernameAsync(string username)=>GetRepository().ExistsByUsernameAsync(username);
	Task<bool> SaveAsync()=>GetRepository().SaveAsync();	
	//默认接口
	IUserRepository GetRepository();
}