using Microsoft.EntityFrameworkCore;
using WPFProject01.Dtos;
using WPFProject01.Entities;

namespace WPFProject01.Repository;

public class UserRepository : IUserRepository {
	private readonly MyDbContext _context;

	public UserRepository(MyDbContext context) {
		_context = context;
	}

	public async Task<List<User>> GetUsersAsync(UserQueryDto? query=null) {
		var express = _context.Users.AsQueryable();
		if (query == null) {
			return await express.ToListAsync();
		}
		if (string.IsNullOrWhiteSpace(query.Username) && string.IsNullOrWhiteSpace(query.Telephone)) {
			return await express.ToListAsync();
		}
		if (!string.IsNullOrWhiteSpace(query.Username)) {
			express = express.Where(u => u.Username.Contains(query.Username)  );
		}
		if (!string.IsNullOrWhiteSpace(query.Telephone)) {
			express = express.Where(u => u.Telephone.Contains(query.Telephone));
		}
		return await express.ToListAsync();

	}

	public async Task<User> GetUserAsync(int id) {
		return await _context.Users.FindAsync(id);
	}

	public async Task<User> GetUserByNameAsync(string name) {
		return await _context.Users.FirstAsync(u => u.Username == name);
	}

	public async Task<User> AddUserAsync(User user) {
		_context.Users.Add(user);
		await _context.SaveChangesAsync();
		return user;
	}

	public async Task<User> UpdateUserAsync(User user) {
		_context.Users.Update(user);
		await _context.SaveChangesAsync();
		return user;
	}

	public async Task<bool> DeleteUserAsync(int id) {
		var user = await _context.Users.FindAsync(id);
		if (user != null) {
			_context.Users.Remove(user);
		}

		return await _context.SaveChangesAsync() > 0;
	}

	public async Task<bool> ExistsByUsernameAsync(string username) {
		return await _context.Users.AnyAsync(x => x.Username == username);
	}

	public async Task<bool> SaveAsync() {
		return await _context.SaveChangesAsync() > 0;
	}
}