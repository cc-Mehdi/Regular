using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Datalayer.Migrations
{
    /// <inheritdoc />
    public partial class changeLoginsLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSignOut",
                table: "LoginsLog",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LoginToken",
                table: "LoginsLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSignOut",
                table: "LoginsLog");

            migrationBuilder.DropColumn(
                name: "LoginToken",
                table: "LoginsLog");
        }
    }
}
