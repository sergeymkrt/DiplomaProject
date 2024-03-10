using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DiplomaProject.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "access_levels",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_levels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

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
                name: "permission_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SurName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessLevelId = table.Column<int>(type: "int", nullable: false),
                    AccessLevel = table.Column<int>(type: "int", nullable: false),
                    PersonalDirectoryId = table.Column<long>(type: "bigint", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "directories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentDirectoryId = table.Column<long>(type: "bigint", nullable: true),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false, defaultValue: "testId"),
                    created_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    modified_by = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    modified_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_directories", x => x.id);
                    table.ForeignKey(
                        name: "FK_directories_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_directories_directories_ParentDirectoryId",
                        column: x => x.ParentDirectoryId,
                        principalTable: "directories",
                        principalColumn: "id");
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

            migrationBuilder.CreateTable(
                name: "groups",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccessLevelId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DirectoryId = table.Column<long>(type: "bigint", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false, defaultValue: "testId"),
                    created_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    modified_by = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    modified_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groups", x => x.id);
                    table.ForeignKey(
                        name: "FK_groups_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_groups_access_levels_AccessLevelId",
                        column: x => x.AccessLevelId,
                        principalTable: "access_levels",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_groups_directories_DirectoryId",
                        column: x => x.DirectoryId,
                        principalTable: "directories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FileDirectory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    DirectoryId = table.Column<long>(type: "bigint", nullable: false),
                    KeyId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false, defaultValue: "testId"),
                    created_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    modified_by = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    modified_date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_files", x => x.id);
                    table.ForeignKey(
                        name: "FK_files_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_files_directories_DirectoryId",
                        column: x => x.DirectoryId,
                        principalTable: "directories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_files_keys_KeyId",
                        column: x => x.KeyId,
                        principalTable: "keys",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupId = table.Column<long>(type: "bigint", nullable: false),
                    PermissionTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => new { x.UserId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_UserGroups_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroups_groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroups_permission_types_PermissionTypeId",
                        column: x => x.PermissionTypeId,
                        principalTable: "permission_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "access_levels",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Confidential" },
                    { 2, "Secret" },
                    { 3, "TopSecret" }
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

            migrationBuilder.InsertData(
                table: "permission_types",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Editor" },
                    { 2, "Viewer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PersonalDirectoryId",
                table: "AspNetUsers",
                column: "PersonalDirectoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_directories_OwnerId",
                table: "directories",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_directories_ParentDirectoryId",
                table: "directories",
                column: "ParentDirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_files_DirectoryId",
                table: "files",
                column: "DirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_files_KeyId",
                table: "files",
                column: "KeyId");

            migrationBuilder.CreateIndex(
                name: "IX_files_UserId",
                table: "files",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_groups_AccessLevelId",
                table: "groups",
                column: "AccessLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_groups_DirectoryId",
                table: "groups",
                column: "DirectoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_groups_OwnerId",
                table: "groups",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_keys_KeySizeID",
                table: "keys",
                column: "KeySizeID");

            migrationBuilder.CreateIndex(
                name: "IX_keys_UserId",
                table: "keys",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_GroupId",
                table: "UserGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_PermissionTypeId",
                table: "UserGroups",
                column: "PermissionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_directories_PersonalDirectoryId",
                table: "AspNetUsers",
                column: "PersonalDirectoryId",
                principalTable: "directories",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_directories_AspNetUsers_OwnerId",
                table: "directories");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "keys");

            migrationBuilder.DropTable(
                name: "groups");

            migrationBuilder.DropTable(
                name: "permission_types");

            migrationBuilder.DropTable(
                name: "key_sizes");

            migrationBuilder.DropTable(
                name: "access_levels");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "directories");
        }
    }
}
