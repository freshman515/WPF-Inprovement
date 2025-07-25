using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPFProject01.Migrations
{
    /// <inheritdoc />
    public partial class addChildernMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "View",
                table: "Menu",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "Menu",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ParentId",
                table: "Menu",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Menu",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "MenuId", "ParentId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Menu",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "MenuId", "Name", "ParentId", "View" },
                values: new object[] { null, "系统设置", null, null });

            migrationBuilder.UpdateData(
                table: "Menu",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "MenuId", "ParentId" },
                values: new object[] { null, 2L });

            migrationBuilder.UpdateData(
                table: "Menu",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Icon", "MenuId", "Name", "ParentId", "View" },
                values: new object[] { "Lock", null, "权限管理", 2L, "PermissionView" });

            migrationBuilder.UpdateData(
                table: "Menu",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Icon", "MenuId", "Name", "ParentId", "View" },
                values: new object[] { "History", null, "日志查看", null, "LogView" });

            migrationBuilder.InsertData(
                table: "Menu",
                columns: new[] { "Id", "Icon", "MenuId", "Name", "ParentId", "View" },
                values: new object[] { 6, "Information", null, "关于", null, "AboutView" });

            migrationBuilder.CreateIndex(
                name: "IX_Menu_MenuId",
                table: "Menu",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menu_Menu_MenuId",
                table: "Menu",
                column: "MenuId",
                principalTable: "Menu",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menu_Menu_MenuId",
                table: "Menu");

            migrationBuilder.DropIndex(
                name: "IX_Menu_MenuId",
                table: "Menu");

            migrationBuilder.DeleteData(
                table: "Menu",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Menu");

            migrationBuilder.AlterColumn<string>(
                name: "View",
                table: "Menu",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Menu",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Name", "View" },
                values: new object[] { "设置", "SettingsView" });

            migrationBuilder.UpdateData(
                table: "Menu",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Icon", "Name", "View" },
                values: new object[] { "History", "日志查看", "LogView" });

            migrationBuilder.UpdateData(
                table: "Menu",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Icon", "Name", "View" },
                values: new object[] { "Information", "关于", "AboutView" });
        }
    }
}
