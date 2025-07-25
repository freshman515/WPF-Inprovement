using WPFProject01.Dtos;
using WPFProject01.Entities;

namespace WPFProject01.Repository;

public interface IUserRepository {
	Task<List<User>> GetUsersAsync(UserQueryDto? query=null);
	Task<User> GetUserAsync(int id);
	Task<User> GetUserByNameAsync(string name);
	Task<User> AddUserAsync(User user);
	Task<User> UpdateUserAsync(User user);
	Task<bool> DeleteUserAsync(int id);
	Task<bool> ExistsByUsernameAsync(string username);
	Task<bool> SaveAsync();
}