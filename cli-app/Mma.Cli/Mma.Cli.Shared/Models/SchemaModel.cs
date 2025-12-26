using System.Text.Json.Serialization;

namespace Mma.Cli.Shared.Models;

public class SchemaModel
{
    public List<TableModel> Tables { get; set; } = new();
    public List<RelationshipModel> Relationships { get; set; } = new();
}

public class TableModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "";
    public double X { get; set; }
    public double Y { get; set; }
    public List<ColumnModel> Columns { get; set; } = new();
    public GenSettingsModel GenSettings { get; set; } = new();
}

public class ColumnModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "";
    public string Type { get; set; } = "NVARCHAR(MAX)";
    public bool IsPrimaryKey { get; set; }
    public bool IsNullable { get; set; }
    public bool IsNotNull { get; set; }
}

public class GenSettingsModel
{
    public bool GenerateController { get; set; } = true;
    public bool GenerateService { get; set; } = true;
    public bool AllowRead { get; set; } = true;
    public bool AllowReadById { get; set; } = true;
    public bool AllowCreate { get; set; } = true;
    public bool AllowUpdate { get; set; } = true;
    public bool AllowDelete { get; set; } = true;
}

public class RelationshipModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string SourceTableId { get; set; } = "";
    public string SourceColumnId { get; set; } = "";
    public string TargetTableId { get; set; } = "";
    public string TargetColumnId { get; set; } = "";
}
