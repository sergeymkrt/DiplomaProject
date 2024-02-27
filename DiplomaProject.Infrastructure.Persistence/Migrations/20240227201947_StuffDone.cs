using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DiplomaProject.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class StuffDone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileDirectory",
                table: "files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "files",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "KeyId",
                table: "files",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "key_sizes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_key_sizes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "keys",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicKey = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    PrivateKey = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    KeySizeID = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false, defaultValue: "testId"),
                    created_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    modified_by = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    modified_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_keys", x => x.id);
                    table.ForeignKey(
                        name: "FK_keys_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_keys_key_sizes_KeySizeID",
                        column: x => x.KeySizeID,
                        principalTable: "key_sizes",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                table: "key_sizes",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 2048, "Size2048" },
                    { 3072, "Size3072" },
                    { 7680, "Size7680" },
                    { 15360, "Size15360" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_files_KeyId",
                table: "files",
                column: "KeyId");

            migrationBuilder.CreateIndex(
                name: "IX_keys_KeySizeID",
                table: "keys",
                column: "KeySizeID");

            migrationBuilder.CreateIndex(
                name: "IX_keys_UserId",
                table: "keys",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_files_keys_KeyId",
                table: "files",
                column: "KeyId",
                principalTable: "keys",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_files_keys_KeyId",
                table: "files");

            migrationBuilder.DropTable(
                name: "keys");

            migrationBuilder.DropTable(
                name: "key_sizes");

            migrationBuilder.DropIndex(
                name: "IX_files_KeyId",
                table: "files");

            migrationBuilder.DropColumn(
                name: "FileDirectory",
                table: "files");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "files");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "files");

            migrationBuilder.DropColumn(
                name: "KeyId",
                table: "files");

            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "files");
        }
    }
}
