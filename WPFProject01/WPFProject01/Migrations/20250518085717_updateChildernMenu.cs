using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPFProject01.Migrations
{
    /// <inheritdoc />
    public partial class updateChildernMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Menu",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Name", "View" },
                values: new object[] { "界面管理", "SettingsView" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Menu",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Name", "View" },
                values: new object[] { "权限管理", "PermissionView" });
        }
    }
}
