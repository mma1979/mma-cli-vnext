namespace MmaSolution.Core.Consts;

public class PkTypes
{
    public const string GUID = "Guid";
    public const string INT = "int";
    public const string LONG = "long";
    public const string STRING = "string";
    public const string DECIMAL = "decimal";
    public const string FLOAT = "float";
    public const string BOOL = "bool";
    public const string DATE_TIME = "DateTime";
    public static List<Type> ALL = [typeof(Guid), typeof(int), typeof(long), typeof(string), typeof(decimal), typeof(float), typeof(bool), typeof(DateTime)];

}
