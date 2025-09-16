using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthenticationDbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class InitIdentityDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");
            #region Schema Creation

            migrationBuilder.CreateTable(
                name: "AppFeatures",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    Scope = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", maxLength: 1500, nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppResources",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ResourceType = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppRoles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorMethod = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppFeatureFlags",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeatureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScopeIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppFeatureFlags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppFeatureFlags_AppFeatures_FeatureId",
                        column: x => x.FeatureId,
                        principalSchema: "dbo",
                        principalTable: "AppFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppAccessControlEntries",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResourcePattern = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PermissionPattern = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FeatureId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAccessControlEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppAccessControlEntries_AppFeatures_FeatureId",
                        column: x => x.FeatureId,
                        principalSchema: "dbo",
                        principalTable: "AppFeatures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppAccessControlEntries_AppResources_ResourceId",
                        column: x => x.ResourceId,
                        principalSchema: "dbo",
                        principalTable: "AppResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppRoleClaims",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppRoleClaims_AppRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppRefreshTokens",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    JwtId = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hash = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppRefreshTokens_AppUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserClaims",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserClaims_AppUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserLogins",
                schema: "dbo",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AppUserLogins_AppUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserRoles",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AppUserRoles_AppRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserRoles_AppUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserTokens",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AppUserTokens_AppUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppRoleAccessControlEntries",
                schema: "dbo",
                columns: table => new
                {
                    AccessControlEntriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppRolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleAccessControlEntries", x => new { x.AccessControlEntriesId, x.AppRolesId });
                    table.ForeignKey(
                        name: "FK_AppRoleAccessControlEntries_AppAccessControlEntries_AccessControlEntriesId",
                        column: x => x.AccessControlEntriesId,
                        principalSchema: "dbo",
                        principalTable: "AppAccessControlEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppRoleAccessControlEntries_AppRoles_AppRolesId",
                        column: x => x.AppRolesId,
                        principalSchema: "dbo",
                        principalTable: "AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserAccessControlEntries",
                schema: "dbo",
                columns: table => new
                {
                    AccessControlEntriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppUsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserAccessControlEntries", x => new { x.AccessControlEntriesId, x.AppUsersId });
                    table.ForeignKey(
                        name: "FK_AppUserAccessControlEntries_AppAccessControlEntries_AccessControlEntriesId",
                        column: x => x.AccessControlEntriesId,
                        principalSchema: "dbo",
                        principalTable: "AppAccessControlEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserAccessControlEntries_AppUsers_AppUsersId",
                        column: x => x.AppUsersId,
                        principalSchema: "dbo",
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            

            migrationBuilder.CreateIndex(
                name: "IX_AppAccessControlEntries_FeatureId",
                schema: "dbo",
                table: "AppAccessControlEntries",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAccessControlEntries_IsDeleted",
                schema: "dbo",
                table: "AppAccessControlEntries",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AppAccessControlEntries_ResourceId",
                schema: "dbo",
                table: "AppAccessControlEntries",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_AppFeatureFlags_FeatureId",
                schema: "dbo",
                table: "AppFeatureFlags",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_AppFeatureFlags_IsDeleted",
                schema: "dbo",
                table: "AppFeatureFlags",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AppFeatures_IsDeleted",
                schema: "dbo",
                table: "AppFeatures",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AppRefreshTokens_Hash",
                schema: "dbo",
                table: "AppRefreshTokens",
                column: "Hash");

            migrationBuilder.CreateIndex(
                name: "IX_AppRefreshTokens_IsDeleted",
                schema: "dbo",
                table: "AppRefreshTokens",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AppRefreshTokens_UserId",
                schema: "dbo",
                table: "AppRefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppResources_IsDeleted",
                schema: "dbo",
                table: "AppResources",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AppRoleAccessControlEntries_AppRolesId",
                schema: "dbo",
                table: "AppRoleAccessControlEntries",
                column: "AppRolesId");

            migrationBuilder.CreateIndex(
                name: "IX_AppRoleClaims_IsDeleted",
                schema: "dbo",
                table: "AppRoleClaims",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AppRoleClaims_RoleId",
                schema: "dbo",
                table: "AppRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppRoles_IsDeleted",
                schema: "dbo",
                table: "AppRoles",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "dbo",
                table: "AppRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserAccessControlEntries_AppUsersId",
                schema: "dbo",
                table: "AppUserAccessControlEntries",
                column: "AppUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserClaims_IsDeleted",
                schema: "dbo",
                table: "AppUserClaims",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserClaims_UserId",
                schema: "dbo",
                table: "AppUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserLogins_IsDeleted",
                schema: "dbo",
                table: "AppUserLogins",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserLogins_UserId",
                schema: "dbo",
                table: "AppUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRoles_IsDeleted",
                schema: "dbo",
                table: "AppUserRoles",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRoles_RoleId",
                schema: "dbo",
                table: "AppUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRoles_UserId",
                schema: "dbo",
                table: "AppUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "dbo",
                table: "AppUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_IsDeleted",
                schema: "dbo",
                table: "AppUsers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "dbo",
                table: "AppUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserTokens_IsDeleted",
                schema: "dbo",
                table: "AppUserTokens",
                column: "IsDeleted");

            #endregion

            #region ACLsView    
            migrationBuilder.Sql(@"Create View dbo.VW_ACLs
As

with UsersAcl as(
select u.Id UserId, null as RoleId,
acl1.ResourcePattern, acl1.PermissionPattern,
res1.Id ResourceId, res1.Url ResourceUrl,
f1.Scope FeatureScope, f1.IsEnabled FeatureIsEnabled,
ff1.ScopeIdentifier FlagScopeIdentifier, ff1.IsEnabled FlagIsEnabled
from AppUsers u
left join AppUserAccessControlEntries uacl on u.Id=uacl.AppUsersId
left join AppAccessControlEntries acl1 on uacl.AccessControlEntriesId=acl1.Id
left join AppResources res1 on acl1.ResourceId=res1.Id
left join AppFeatures f1 on acl1.FeatureId=f1.Id
left join AppFeatureFlags ff1 on f1.Id=ff1.FeatureId
),
RolesAcl as (
select u.Id UserId, r.Id RoleId, 
acl2.ResourcePattern ResourcePattern, acl2.PermissionPattern PermissionPattern,
res2.Id ResourceId, res2.Url ResourceUrl,
f2.Scope RFeatureScope, f2.IsEnabled FeatureIsEnabled,
ff2.ScopeIdentifier FlagScopeIdentifier, ff2.IsEnabled FlagIsEnabled
from AppUsers u
Join AppUserRoles ur on u.Id=ur.UserId
Join AppRoles r on ur.RoleId=r.Id
left join AppRoleAccessControlEntries racl on r.Id=racl.AppRolesId
left join AppAccessControlEntries acl2 on racl.AccessControlEntriesId=acl2.Id
left join AppResources res2 on acl2.ResourceId=res2.Id
left join AppFeatures f2 on acl2.FeatureId=f2.Id
left join AppFeatureFlags ff2 on f2.Id=ff2.FeatureId
),
ACLs as (
select NEWID() Id, * from UsersAcl
Union
select NEWID() Id, * from RolesAcl
)

select * from ACLs");
            #endregion

            #region Seed Data   
            migrationBuilder.InsertData(
                 schema: "dbo",
                 table: "AppResources",
                 columns: new[] { "Id", "CreatedBy", "DeletedBy", "DeletedDate", "Description", "ModifiedBy", "ModifiedDate", "ResourceType", "Url" },
                 values: new object[,]
                 {
                    { new Guid("f10e8ad9-8458-4a22-81be-908f6aa61738"), null, null, null, "Match all resources for admin", null, null, 1, "*" },
                    { new Guid("f10e8ad9-8458-4a22-81be-908f6aa61739"), null, null, null, null, null, null, 1, "api/account" },
                    { new Guid("f10e8ad9-8458-4a22-81be-908f6aa61740"), null, null, null, null, null, null, 1, "api/localization" },
                    { new Guid("f10e8ad9-8458-4a22-81be-908f6aa61741"), null, null, null, null, null, null, 1, "api/notifications" },
                    { new Guid("f10e8ad9-8458-4a22-81be-908f6aa61742"), null, null, null, null, null, null, 1, "api/notification-statuses" },
                    { new Guid("f10e8ad9-8458-4a22-81be-908f6aa61743"), null, null, null, null, null, null, 1, "api/notification-types" },
                    { new Guid("f10e8ad9-8458-4a22-81be-908f6aa61744"), null, null, null, null, null, null, 1, "api/roles" },
                    { new Guid("f10e8ad9-8458-4a22-81be-908f6aa61745"), null, null, null, null, null, null, 1, "api/syssettings" },
                    { new Guid("f10e8ad9-8458-4a22-81be-908f6aa61746"), null, null, null, null, null, null, 1, "api/attachment" }
                 });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "IsDeleted", "ModifiedBy", "ModifiedDate", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("6d86280e-f691-4d17-a1c5-d12183f3fafa"), "e6c6df1d-e2f8-4e1f-9926-81ec842e7c84", new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"), new DateTime(2025, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, null, null, "Admin", "ADMIN" },
                    { new Guid("6d86280e-f691-4d17-a1c5-d12183f3fafb"), "ccce9a7d-1a2c-4faf-9e76-ec3740ef4f10", new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"), new DateTime(2025, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, null, null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "Email", "EmailConfirmed", "FullName", "IsDeleted", "LockoutEnabled", "LockoutEnd", "ModifiedBy", "ModifiedDate", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureUrl", "SecurityStamp", "TwoFactorEnabled", "TwoFactorMethod", "UserName" },
                values: new object[] { new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"), 0, "d4f06dc9-8e7f-46ed-85e7-c11f4545470c", null, new DateTime(2025, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "admin@local.com", true, "Super Admin", false, true, null, null, null, "ADMIN@LOCAL.COM", "ADMIN@LOCAL.COM", "D01B2782115F692E0E0D52FC64EFE727F52DDA8CB03703898F1D182BD2517251-73073B1B83DFA10409FA469853F87F71", "+201008983687", true, null, "LR2TCJY3QTYEMAG27JLB57NGL6H27HTW", true, 1, "admin@local.com" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "AppAccessControlEntries",
                columns: new[] { "Id", "CreatedBy", "DeletedBy", "DeletedDate", "FeatureId", "ModifiedBy", "ModifiedDate", "PermissionPattern", "ResourceId", "ResourcePattern" },
                values: new object[,]
                {
                    { new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f47"), null, null, null, null, null, null, "*", new Guid("f10e8ad9-8458-4a22-81be-908f6aa61738"), "*" },
                    { new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f48"), null, null, null, null, null, null, "*", new Guid("f10e8ad9-8458-4a22-81be-908f6aa61739"), "account/*" },
                    { new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f49"), null, null, null, null, null, null, "*", new Guid("f10e8ad9-8458-4a22-81be-908f6aa61740"), "localization/*" },
                    { new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f50"), null, null, null, null, null, null, "Read", new Guid("f10e8ad9-8458-4a22-81be-908f6aa61740"), "localization/*" },
                    { new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f51"), null, null, null, null, null, null, "*", new Guid("f10e8ad9-8458-4a22-81be-908f6aa61741"), "notification*" },
                    { new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f52"), null, null, null, null, null, null, "Read,Update", new Guid("f10e8ad9-8458-4a22-81be-908f6aa61741"), "notification*" },
                    { new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f53"), null, null, null, null, null, null, "*", new Guid("f10e8ad9-8458-4a22-81be-908f6aa61744"), "roles/*" },
                    { new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f54"), null, null, null, null, null, null, "Read", new Guid("f10e8ad9-8458-4a22-81be-908f6aa61744"), "roles/*" },
                    { new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f55"), null, null, null, null, null, null, "*", new Guid("f10e8ad9-8458-4a22-81be-908f6aa61745"), "syssettings/*" },
                    { new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f56"), null, null, null, null, null, null, "Read", new Guid("f10e8ad9-8458-4a22-81be-908f6aa61745"), "syssettings/*" },
                    { new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f57"), null, null, null, null, null, null, "*", new Guid("f10e8ad9-8458-4a22-81be-908f6aa61746"), "attachment/*" },
                    { new Guid("1eaf731d-7d59-4b02-941f-ba25134c5f58"), null, null, null, null, null, null, "Read", new Guid("f10e8ad9-8458-4a22-81be-908f6aa61746"), "attachment/*" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "AppUserRoles",
                columns: new[] { "RoleId", "UserId", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "IsDeleted", "ModifiedBy", "ModifiedDate" },
                values: new object[] { new Guid("6d86280e-f691-4d17-a1c5-d12183f3fafa"), new Guid("d3e96e09-d61d-4f99-aeb7-08dcbeb427c4"), null, new DateTime(2025, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, null, null });

            // Admin Role ACLs
            migrationBuilder.Sql(@"INSERT [dbo].[AppRoleAccessControlEntries] ([AccessControlEntriesId], [AppRolesId]) VALUES (N'1eaf731d-7d59-4b02-941f-ba25134c5f47', N'6d86280e-f691-4d17-a1c5-d12183f3fafa');");

            // User Role ACLs

            migrationBuilder.Sql("""
                INSERT [dbo].[AppRoleAccessControlEntries] ([AccessControlEntriesId], [AppRolesId]) VALUES
                (N'1eaf731d-7d59-4b02-941f-ba25134c5f48', N'6d86280e-f691-4d17-a1c5-d12183f3fafb'),
                (N'1eaf731d-7d59-4b02-941f-ba25134c5f50', N'6d86280e-f691-4d17-a1c5-d12183f3fafb'),
                (N'1eaf731d-7d59-4b02-941f-ba25134c5f52', N'6d86280e-f691-4d17-a1c5-d12183f3fafb'),
                (N'1eaf731d-7d59-4b02-941f-ba25134c5f54', N'6d86280e-f691-4d17-a1c5-d12183f3fafb'),
                (N'1eaf731d-7d59-4b02-941f-ba25134c5f56', N'6d86280e-f691-4d17-a1c5-d12183f3fafb'),
                (N'1eaf731d-7d59-4b02-941f-ba25134c5f58', N'6d86280e-f691-4d17-a1c5-d12183f3fafb');
                """);
            #endregion
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppFeatureFlags",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppRefreshTokens",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppRoleAccessControlEntries",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppRoleClaims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppUserAccessControlEntries",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppUserClaims",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppUserLogins",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppUserRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppUserTokens",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppAccessControlEntries",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppUsers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppFeatures",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AppResources",
                schema: "dbo");
        }
    }
}
