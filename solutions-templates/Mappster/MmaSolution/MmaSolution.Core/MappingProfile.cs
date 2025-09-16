namespace MmaSolution.Core;

public class MappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AppAccessControlEntry, AppAccessControlEntryReadModel>();
        config.NewConfig<AppFeatureFlag, AppFeatureFlagModifyModel>();
        config.NewConfig<AppFeatureFlag, AppFeatureFlagReadModel>();
        config.NewConfig<AppFeature, AppFeatureModifyModel>();
        config.NewConfig<AppFeature, AppFeatureReadModel>();
        config.NewConfig<AppLog, AppLogModifyModel>();
        config.NewConfig<AppLog, AppLogReadModel>();
        config.NewConfig<AppRefreshToken, AppRefreshTokenModifyModel>();
        config.NewConfig<AppRefreshToken, AppRefreshTokenReadModel>();
        config.NewConfig<AppResource, AppResourceModifyModel>();
        config.NewConfig<AppResource, AppResourceReadModel>();
        config.NewConfig<AppRoleClaim, AppRoleClaimModifyModel>();
        config.NewConfig<AppRoleClaim, AppRoleClaimReadModel>();
        config.NewConfig<AppRole, AppRoleModifyModel>();
        config.NewConfig<AppRole, AppRoleReadModel>();
        config.NewConfig<AppUserClaim, AppUserClaimModifyModel>();
        config.NewConfig<AppUserClaim, AppUserClaimReadModel>();
        config.NewConfig<AppUserLogin, AppUserLoginModifyModel>();
        config.NewConfig<AppUserLogin, AppUserLoginReadModel>();
        config.NewConfig<AppUser, AppUserModifyModel>();
        config.NewConfig<AppUser, AppUserReadModel>();
        config.NewConfig<AppUserRole, AppUserRoleModifyModel>();
        config.NewConfig<AppUserRole, AppUserRoleReadModel>();
        config.NewConfig<AppUserToken, AppUserTokenModifyModel>();
        config.NewConfig<AppUserToken, AppUserTokenReadModel>();
        config.NewConfig<Attachment, AttachmentModifyModel>();
        config.NewConfig<Attachment, AttachmentReadModel>();
        config.NewConfig<Language, LanguageModifyModel>();
        config.NewConfig<Language, LanguageReadModel>();
        config.NewConfig<Notification, NotificationModifyModel>();
        config.NewConfig<Notification, NotificationReadModel>();
        config.NewConfig<NotificationStatus, NotificationStatusModifyModel>();
        config.NewConfig<NotificationStatus, NotificationStatusReadModel>();
        config.NewConfig<NotificationType, NotificationTypeModifyModel>();
        config.NewConfig<NotificationType, NotificationTypeReadModel>();
        config.NewConfig<Resource, ResourceModifyModel>();
        config.NewConfig<Resource, ResourceReadModel>();
        config.NewConfig<SysSetting, SysSettingModifyModel>();
        config.NewConfig<SysSetting, SysSettingReadModel>();
    }
}