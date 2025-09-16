namespace MmaSolution.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppAccessControlEntry, AppAccessControlEntryModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppAccessControlEntry, AppAccessControlEntryReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppFeatureFlag, AppFeatureFlagModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppFeatureFlag, AppFeatureFlagReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppFeature, AppFeatureModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppFeature, AppFeatureReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppLog, AppLogModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppLog, AppLogReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppRefreshToken, AppRefreshTokenModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppRefreshToken, AppRefreshTokenReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppResource, AppResourceModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppResource, AppResourceReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppRoleClaim, AppRoleClaimModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppRoleClaim, AppRoleClaimReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppRole, AppRoleModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppRole, AppRoleReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppUserClaim, AppUserClaimModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppUserClaim, AppUserClaimReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppUserLogin, AppUserLoginModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppUserLogin, AppUserLoginReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppUser, AppUserModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppUser, AppUserReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppUserRole, AppUserRoleModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppUserRole, AppUserRoleReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppUserToken, AppUserTokenModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<AppUserToken, AppUserTokenReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<Attachment, AttachmentModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<Attachment, AttachmentReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<Language, LanguageModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<Language, LanguageReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<Notification, NotificationModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<Notification, NotificationReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<NotificationStatus, NotificationStatusModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<NotificationStatus, NotificationStatusReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<NotificationType, NotificationTypeModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<NotificationType, NotificationTypeReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<Resource, ResourceModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<Resource, ResourceReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<SysSetting, SysSettingModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
            CreateMap<SysSetting, SysSettingReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();



        }
    }
}