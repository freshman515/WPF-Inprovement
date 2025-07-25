using Microsoft.EntityFrameworkCore;
using WPFProject01.Entities;
using WPFProject01.enums;

namespace WPFProject01;

public class MyDbContext : DbContext {
	public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) {
	}

	public DbSet<User> Users { get; set; }
	public DbSet<Menu> Menus { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);
		modelBuilder.Entity<User>(entity => {
			entity.ToTable("User");
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Username).IsRequired();
			entity.Property(e => e.Password).IsRequired();
			entity.Property(e => e.Role)
				.HasConversion<int>();
			entity.HasData(
				new User { IsSelected = false,Id = 1, Username = "admin",Telephone = "18183041275", Password = "admin", Role = Role.Admin });
		});

		modelBuilder.Entity<Menu>(entity => {
			entity.ToTable("Menu");
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Name).IsRequired();
			entity.Property(e => e.Icon).IsRequired();

			entity.HasData(
				new Menu { Id = 1, Name = "主页", Icon = "Home", View = "HomeView", ParentId = null },
				new Menu { Id = 2, Name = "系统设置", Icon = "Settings", ParentId = null },
				new Menu { Id = 3, Name = "用户管理", Icon = "AccountCircle", View = "UserView", ParentId = 2 },
				new Menu { Id = 4, Name = "界面管理", Icon = "Lock", View = "SettingsView", ParentId = 2 },
				new Menu { Id = 5, Name = "日志查看", Icon = "History", View = "LogView", ParentId = null },
				new Menu { Id = 6, Name = "关于", Icon = "Information", View = "AboutView", ParentId = null }
			);
		});
	}
}