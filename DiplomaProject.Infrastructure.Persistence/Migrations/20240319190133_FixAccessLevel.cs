using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomaProject.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixAccessLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessLevel",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AccessLevelId",
                table: "AspNetUsers",
                column: "AccessLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_access_levels_AccessLevelId",
                table: "AspNetUsers",
                column: "AccessLevelId",
                principalTable: "access_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_access_levels_AccessLevelId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AccessLevelId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "AccessLevel",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
