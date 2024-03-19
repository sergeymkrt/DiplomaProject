using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomaProject.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class changeAccessLevelCases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "access_levels",
                keyColumn: "id",
                keyValue: 1,
                column: "name",
                value: "Basic");

            migrationBuilder.UpdateData(
                table: "access_levels",
                keyColumn: "id",
                keyValue: 2,
                column: "name",
                value: "Confidential");

            migrationBuilder.UpdateData(
                table: "access_levels",
                keyColumn: "id",
                keyValue: 3,
                column: "name",
                value: "Secret");

            migrationBuilder.InsertData(
                table: "access_levels",
                columns: new[] { "id", "name" },
                values: new object[] { 4, "TopSecret" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "access_levels",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "access_levels",
                keyColumn: "id",
                keyValue: 1,
                column: "name",
                value: "Confidential");

            migrationBuilder.UpdateData(
                table: "access_levels",
                keyColumn: "id",
                keyValue: 2,
                column: "name",
                value: "Secret");

            migrationBuilder.UpdateData(
                table: "access_levels",
                keyColumn: "id",
                keyValue: 3,
                column: "name",
                value: "TopSecret");
        }
    }
}
