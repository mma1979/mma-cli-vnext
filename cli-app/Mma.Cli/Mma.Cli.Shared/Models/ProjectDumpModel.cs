namespace Mma.Cli.Shared.Models;

public class ProjectDumpModel
{
    public ProjectModel? Project { get; set; }
    public List<EntityModel>? Entities { get; set; }
    public List<EntityRowModel>? Rows { get; set; }
    public List<RelationModel>? Relations { get; set; }

    public ProjectDumpModel()
    {
        Entities = new List<EntityModel>();
        Rows = new List<EntityRowModel>();
        Relations = new List<RelationModel>();

    }
}
