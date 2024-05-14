using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Datalayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSomeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_User_Project_ProjectId",
                table: "User_Project",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Project_UserId",
                table: "User_Project",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_Project_OrganizationId",
                table: "Organization_Project",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_Project_ProjectId",
                table: "Organization_Project",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organization_Project_Organizations_OrganizationId",
                table: "Organization_Project",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Organization_Project_Projects_ProjectId",
                table: "Organization_Project",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Project_Projects_ProjectId",
                table: "User_Project",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Project_Users_UserId",
                table: "User_Project",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organization_Project_Organizations_OrganizationId",
                table: "Organization_Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Organization_Project_Projects_ProjectId",
                table: "Organization_Project");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Project_Projects_ProjectId",
                table: "User_Project");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Project_Users_UserId",
                table: "User_Project");

            migrationBuilder.DropIndex(
                name: "IX_User_Project_ProjectId",
                table: "User_Project");

            migrationBuilder.DropIndex(
                name: "IX_User_Project_UserId",
                table: "User_Project");

            migrationBuilder.DropIndex(
                name: "IX_Organization_Project_OrganizationId",
                table: "Organization_Project");

            migrationBuilder.DropIndex(
                name: "IX_Organization_Project_ProjectId",
                table: "Organization_Project");
        }
    }
}
