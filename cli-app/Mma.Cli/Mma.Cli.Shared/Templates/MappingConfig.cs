namespace Mma.Cli.Shared.Templates;

internal sealed class MappingConfig
{
    public const string AutoMapper = "CreateMap<$EntityName, $EntityNameReadModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();\r\nCreateMap<$EntityName, $EntityNameModifyModel>().IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter();";

    public const string Mapster = "Map<$EntityName, $EntityNameReadModel>();\r\nMap<$EntityName, $EntityNameModifyModel>();";
}
