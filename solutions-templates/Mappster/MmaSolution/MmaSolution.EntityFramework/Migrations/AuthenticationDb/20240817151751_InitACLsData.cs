#nullable disable

namespace MmaSolution.EntityFramework.Migrations.AuthenticationDb
{
    /// <inheritdoc />
    public partial class InitACLsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            //Init AppResources
            mb.Sql(@"INSERT [dbo].[AppResources] ([Id], [Url], [Description], [ResourceType],  [CreatedBy], [CreatedDate],[IsDeleted]) VALUES
(N'f10e8ad9-8458-4a22-81be-908f6aa61739', N'api/account', NULL, 1,'d3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0),
(N'f10e8ad9-8458-4a22-81be-908f6aa61740', N'api/localization', NULL, 1, 'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0),
(N'f10e8ad9-8458-4a22-81be-908f6aa61741', N'api/notifications', NULL, 1, 'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0),
(N'f10e8ad9-8458-4a22-81be-908f6aa61742', N'api/notification-statuses', NULL, 1,'d3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0),
(N'f10e8ad9-8458-4a22-81be-908f6aa61743', N'api/notification-types', NULL, 1, 'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0),
(N'f10e8ad9-8458-4a22-81be-908f6aa61744', N'api/roles', NULL, 1, 'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0),
(N'f10e8ad9-8458-4a22-81be-908f6aa61745', N'api/syssettings', NULL, 1, 'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0),
(N'f10e8ad9-8458-4a22-81be-908f6aa61746', N'api/attachment', NULL, 1, 'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0);");

            //Init AppUserAccessControlEntries
            mb.Sql(@"INSERT [dbo].[AppAccessControlEntries] ([Id], [ResourcePattern], [PermissionPattern], [FeatureId], [ResourceId], [CreatedBy], [CreatedDate],  [IsDeleted]) VALUES
(N'1eaf731d-7d59-4b02-941f-ba25134c5f47', N'*', N'*', NULL, NULL, 'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0),
(N'1eaf731d-7d59-4b02-941f-ba25134c5f48', N'account/*', N'*', NULL, 'f10e8ad9-8458-4a22-81be-908f6aa61739', 'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0),
(N'1eaf731d-7d59-4b02-941f-ba25134c5f49', N'localization/*', N'*', NULL, 'f10e8ad9-8458-4a22-81be-908f6aa61740', 'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0),
(N'1eaf731d-7d59-4b02-941f-ba25134c5f50', N'localization/*', N'Read', NULL, 'f10e8ad9-8458-4a22-81be-908f6aa61740', 'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0),
(N'1eaf731d-7d59-4b02-941f-ba25134c5f51', N'notification*', N'*', NULL, 'f10e8ad9-8458-4a22-81be-908f6aa61741', 'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0),
(N'1eaf731d-7d59-4b02-941f-ba25134c5f52', N'notification*', N'Read,Update', NULL, 'f10e8ad9-8458-4a22-81be-908f6aa61741', 'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0),
(N'1eaf731d-7d59-4b02-941f-ba25134c5f53', N'roles/*', N'*', NULL, 'f10e8ad9-8458-4a22-81be-908f6aa61744', 'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0),
(N'1eaf731d-7d59-4b02-941f-ba25134c5f54', N'roles/*', N'Read', NULL, 'f10e8ad9-8458-4a22-81be-908f6aa61744', 'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0),
(N'1eaf731d-7d59-4b02-941f-ba25134c5f55', N'syssettings/*', N'*', NULL, 'f10e8ad9-8458-4a22-81be-908f6aa61745', 'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0),
(N'1eaf731d-7d59-4b02-941f-ba25134c5f56', N'syssettings/*', N'Read', NULL, 'f10e8ad9-8458-4a22-81be-908f6aa61745', 'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0),
(N'1eaf731d-7d59-4b02-941f-ba25134c5f57', N'attachment/*', N'*', NULL, 'f10e8ad9-8458-4a22-81be-908f6aa61746', 'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0),
(N'1eaf731d-7d59-4b02-941f-ba25134c5f58', N'attachment/*', N'Read', NULL, 'f10e8ad9-8458-4a22-81be-908f6aa61746', 'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0);");

            // Init AppRoles [ Amdin, User ]

            mb.Sql(@"INSERT [dbo].[AppRoles] ([Id], [CreatedBy], [CreatedDate], [IsDeleted], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES 
(N'6d86280e-f691-4d17-a1c5-d12183f3fafa', 'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0, N'Admin', N'ADMIN', NEWID()),
(N'6d86280e-f691-4d17-a1c5-d12183f3fafb', 'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', GETDATE(), 0, N'User', N'USER', NEWID());");

            // Add Admin User
            mb.Sql("INSERT [dbo].[AppUsers] ([Id], [Hash], [FirstName], [LastName], [Mobile], [CountryCode], [TwoFactorMethod], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted], [DeletedBy], [DeletedDate], [MembershipType], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', -1579056263, N'Admin', N'Admin', NULL, N'+2', 1, NULL, GETDATE(), NULL, NULL, 0, NULL, NULL, 0, N'admin@local.com', N'ADMIN@LOCAL.COM', N'admin@local.com', N'ADMIN@LOCAL.COM', 1, N'D01B2782115F692E0E0D52FC64EFE727F52DDA8CB03703898F1D182BD2517251-73073B1B83DFA10409FA469853F87F71', N'LR2TCJY3QTYEMAG27JLB57NGL6H27HTW', N'd4f06dc9-8e7f-46ed-85e7-c11f4545470c', N'01008983687', 1, 1, NULL, 1, 0);");

            mb.Sql(@"INSERT [dbo].[AppUserRoles] ([UserId], [RoleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [IsDeleted], [DeletedBy], [DeletedDate]) VALUES (N'd3e96e09-d61d-4f99-aeb7-08dcbeb427c4', N'6d86280e-f691-4d17-a1c5-d12183f3fafa', NULL, GETDATE(), NULL, NULL, 0, NULL, NULL);");

            // Admin Role ACLs
            mb.Sql(@"INSERT [dbo].[AppRoleAccessControlEntries] ([AccessControlEntriesId], [AppRolesId]) VALUES (N'1eaf731d-7d59-4b02-941f-ba25134c5f47', N'6d86280e-f691-4d17-a1c5-d12183f3fafa');");

            // User Role ACLs
            mb.Sql(@"INSERT [dbo].[AppRoleAccessControlEntries] ([AccessControlEntriesId], [AppRolesId]) VALUES
 (N'1eaf731d-7d59-4b02-941f-ba25134c5f48', N'6d86280e-f691-4d17-a1c5-d12183f3fafb'),
 (N'1eaf731d-7d59-4b02-941f-ba25134c5f50', N'6d86280e-f691-4d17-a1c5-d12183f3fafb'),
 (N'1eaf731d-7d59-4b02-941f-ba25134c5f52', N'6d86280e-f691-4d17-a1c5-d12183f3fafb'),
 (N'1eaf731d-7d59-4b02-941f-ba25134c5f54', N'6d86280e-f691-4d17-a1c5-d12183f3fafb'),
 (N'1eaf731d-7d59-4b02-941f-ba25134c5f56', N'6d86280e-f691-4d17-a1c5-d12183f3fafb'),
 (N'1eaf731d-7d59-4b02-941f-ba25134c5f58', N'6d86280e-f691-4d17-a1c5-d12183f3fafb');");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            // Remove Roles ACLs
            mb.Sql(@"DELETE FROM [dbo].[AppRoleAccessControlEntries]");

            // Remove Admin User
            mb.Sql("DELETE FROM [dbo].[AppUsers]");

            mb.Sql(@"DELETE FROM [dbo].[AppUserRoles]");

            // Remove AppRoles [ Amdin, User ]

            mb.Sql(@"DELETE FROM [dbo].[AppRoles]");

            // Remove AppUserAccessControlEntries
            mb.Sql(@"DELETE FROM [dbo].[AppAccessControlEntries]");
        }
    }
}
