#nullable disable

namespace MmaSolution.EntityFramework.Migrations.AuthenticationDb
{
    /// <inheritdoc />
    public partial class AddACLsView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Create View VW_ACLs
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Drop View VW_ACLs");
        }
    }
}
