using WPFProject01.Entities;
using WPFProject01.Repository;

namespace WPFProject01.Service;

public class UserService : IUserService {
	private readonly IUserRepository _userRepository;

	public UserService(IUserRepository userRepository) {
		_userRepository = userRepository;
	}

	public async Task<bool> RegisterUser(string username, string password, string confirmPassword, string telephone) {
		if (!(username.Length < 3 || username.Length > 20)) {
			var isExist = await _userRepository.ExistsByUsernameAsync(username);
			if (!isExist) {
				if (password == confirmPassword) {
					var user = new User()
						{ Username = username, Password = password, Telephone = telephone, IsSelected = false };
					await _userRepository.AddUserAsync(user);
					return true;
				}
			}
		}

		return false;
	}

	public async Task<User> Login(string username, string password) {
		var user = await _userRepository.GetUserByNameAsync(username);
		if (user.Password == password) {
			return user;
		}

		return null;
	}

	public async Task AllSelected(bool isSelected) {
		var users = await _userRepository.GetUsersAsync();
		users.ForEach(user => user.IsSelected = isSelected);
		await _userRepository.SaveAsync();
	}

	public IUserRepository GetRepository() {
		return _userRepository;
	}
}