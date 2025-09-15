namespace MmaSolution.AppApi.Infrastrcture.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class RequiredPermissionAttribute : Attribute
{
    public string Permission { get; }

    public RequiredPermissionAttribute(string permission)
    {
        Permission = permission;
    }
}