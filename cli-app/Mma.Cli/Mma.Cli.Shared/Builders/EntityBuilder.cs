namespace Mma.Cli.Shared.Builders;

public sealed class EntityBuilder
{
    private readonly EntityConfig _config;
    private readonly IFileWriter _fileWriter;

    public EntityBuilder(EntityConfig config, IFileWriter? fileWriter = null)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _fileWriter = fileWriter ?? new FileWriter();
    }

    public static EntityBuilder Create(string mapper, string entityName, string pkType, string? solutionPath = null)
    {
        var config = new EntityConfig
        {
            SolutionPath = solutionPath ?? Directory.GetCurrentDirectory(),
            ComponentType = "Entity",
            ComponentName = entityName,
            PkType = pkType,
            Mapper = mapper
        };

        config.InitializePaths();
        return new EntityBuilder(config);
    }

    public static EntityBuilder CreateFromArgs(string[] args, string? solutionPath = null)
    {
        var config = ParseArgs(args, solutionPath!);
        return new EntityBuilder(config);
    }

    private static EntityConfig ParseArgs(string[] args, string solutionPath)
    {
        var mapperFlagIndex = Array.IndexOf(args, Flags.MapperFlag);
        var mapper = mapperFlagIndex >= 0 && mapperFlagIndex + 1 < args.Length
            ? args[mapperFlagIndex + 1].ToLower() switch
            {
                "mapster" => Mappers.Mapster,
                _ => Mappers.AutoMapper
            }
            : Mappers.AutoMapper;

        var config = new EntityConfig
        {
            SolutionPath = solutionPath ?? Directory.GetCurrentDirectory(),
            ComponentType = args.Length > 1 ? args[1] : "Entity",
            ComponentName = args.Length > 2 ? args[2] : throw new ArgumentException("Entity name is required"),
            PkType = args.Length > 3 ? args[3] : PkTypes.GUID,
            Mapper = mapper
        };

        config.InitializePaths();
        return config;
    }

    public EntityBuilder GenerateAll(bool generateController = false)
    {
        GenerateModels();
        GenerateValidator();
        GenerateEntity();
        GenerateEntityConfig();
        UpdateDbContext();
        GenerateService();

        if (generateController)
            GenerateController();

        return this;
    }

    public EntityBuilder GenerateModels()
    {
        var templates = GetTemplateSet();

        // Generate Models
        _fileWriter.WriteTemplate(
            GetModelPath($"{_config.ComponentName}ModifyModel"),
            templates.ModifyModel,
            _config);

        // Generate ReadModel  
        _fileWriter.WriteTemplate(
            GetModelPath($"{_config.ComponentName}ReadModel"),
            templates.ReadModel,
            _config);

        // Update Mapper profile if needed
            UpdateMapperProfile();

        return this;
    }

    public EntityBuilder GenerateValidator()
    {
        _fileWriter.WriteTemplate(
            Path.Combine(_config.ProjectsPath, $"{_config.SolutionName}.Core", "Validations", $"{_config.ComponentName}Validator.cs"),
            Templates.Validator.Template,
            _config);

        return this;
    }

    public EntityBuilder GenerateEntity()
    {
        var templates = GetTemplateSet();
        _fileWriter.WriteTemplate(
            Path.Combine(_config.ProjectsPath, $"{_config.SolutionName}.Core", "Database", "Tables", $"{_config.ComponentName}.cs"),
            templates.Entity,
            _config);

        return this;
    }

    public EntityBuilder GenerateEntityConfig()
    {
        var templates = GetTemplateSet();
        _fileWriter.WriteTemplate(
            Path.Combine(_config.ProjectsPath, $"{_config.SolutionName}.EntityFramework", "EntityConfigurations", $"{_config.ComponentName}Config.cs"),
            templates.EntityConfig,
            _config);

        return this;
    }

    public EntityBuilder UpdateDbContext()
    {
        var contextPath = Path.Combine(_config.ProjectsPath, $"{_config.SolutionName}.EntityFramework", "ApplicationDbContext.cs");
        var updater = new DbContextUpdater(_fileWriter);
        updater.UpdateDbContext(contextPath, _config);

        return this;
    }

    public EntityBuilder GenerateService()
    {
        var templates = GetTemplateSet();
        _fileWriter.WriteTemplate(
            Path.Combine(_config.ProjectsPath, $"{_config.SolutionName}.Services", $"{_config.ComponentName}Service.cs"),
            templates.Service,
            _config);

        // UpdateService Dependency Injection
        UpdateDependencyInjection();

        return this;
    }

    public EntityBuilder GenerateController()
    {
        var templates = GetTemplateSet();
        var entitySetName = BuildHelper.GetSetName(_config.ComponentName);

        _fileWriter.WriteTemplate(
            Path.Combine(_config.ProjectsPath, $"{_config.SolutionName}.AppApi", "Controllers", "v1", $"{entitySetName}Controller.cs"),
            templates.Controller,
            _config);

        return this;
    }

    private string GetModelPath(string fileName)
    {
    
        return Path.Combine(_config.ProjectsPath, $"{_config.SolutionName}.Core", "Models",_config.ComponentName, $"{fileName}.cs");
    }

    private TemplateSet GetTemplateSet()
    {
        return _config.Mapper == Mappers.Mapster
            ? new TemplateSet
            {
                ModifyModel = Templates.Models.ModifyModelTemplate,
                ReadModel = Templates.Models.ReadModelTemplate,
                Entity = Templates.Entity.Template,
                EntityConfig = Templates.EntityConfig.Template,
                Service = Templates.MappsterService.Template,
                Controller = Templates.Controller.Template
            }
            : new TemplateSet
            {
                ModifyModel = Templates.Models.ModifyModelTemplate,
                ReadModel = Templates.Models.ReadModelTemplate,
                Entity = Templates.Entity.Template,
                EntityConfig = Templates.EntityConfig.Template,
                Service = Templates.AutoMapperService.Template,
                Controller = Templates.Controller.Template
            };
    }

    private void UpdateMapperProfile()
    {
        var profilePath = Path.Combine(_config.ProjectsPath, $"{_config.SolutionName}.Services", "ServicesDI.cs");
        var updater = new AutoMapperProfileUpdater(_fileWriter);
        updater.UpdateProfile(profilePath, _config);
    }

    private void UpdateDependencyInjection()
    {
        var profilePath = Path.Combine(_config.ProjectsPath, $"{_config.SolutionName}.Services", "ServicesDI.cs");
        var updater = new DependencyInjectionUpdater(_fileWriter);
        updater.UpdateProfile(profilePath, _config);
    }
}

// Supporting classes
public class EntityConfig
{
    public string SolutionPath { get; set; } = "";
    public string SolutionName { get; set; } = "";
    public string ProjectsPath { get; set; } = "";
    public string ComponentType { get; set; } = "";
    public string ComponentName { get; set; } = "";
    public string PkType { get; set; } = "";
    public string Mapper { get; set; } = "";
    public string EntitySetName => BuildHelper.GetSetName(ComponentName);
    public string EntityVarName => $"{ComponentName[0].ToString().ToLower()}{ComponentName.AsSpan(1)}";

    public void InitializePaths()
    {
        (SolutionName, ProjectsPath) = BuildHelper.CheckSolutionPath(SolutionPath);
    }

    public Dictionary<string, string> GetReplacements()
    {
        return new Dictionary<string, string>
        {
            ["$SolutionName"] = SolutionName,
            ["$EntityName"] = ComponentName,
            ["$EntityVarName"] = EntityVarName,
            ["$EntitySetName"] = EntitySetName,
            ["$PK"] = PkType
        };
    }
}

public class TemplateSet
{
    public string ModifyModel { get; set; }
    public string ReadModel { get; set; }
    public string Entity { get; set; }
    public string EntityConfig { get; set; }
    public string Service { get; set; }
    public string Controller { get; set; }
}

public interface IFileWriter
{
    void WriteTemplate(string path, string template, EntityConfig config);
    List<string> ReadLines(string path);
    void WriteLines(string path, IEnumerable<string> lines);
}

public class FileWriter : IFileWriter
{
    public void WriteTemplate(string path, string template, EntityConfig config)
    {
        var content = ReplaceTokens(template, config.GetReplacements());

        var directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        File.WriteAllText(path, content);
    }

    public List<string> ReadLines(string path)
    {
        return File.ReadAllLines(path).ToList();
    }

    public void WriteLines(string path, IEnumerable<string> lines)
    {
        File.WriteAllLines(path, lines);
    }

    private static string ReplaceTokens(string template, Dictionary<string, string> replacements)
    {
        return replacements.Aggregate(template, (current, kvp) => current.Replace(kvp.Key, kvp.Value));
    }
}

public class DbContextUpdater
{
    private readonly IFileWriter _fileWriter;

    public DbContextUpdater(IFileWriter fileWriter)
    {
        _fileWriter = fileWriter;
    }

    public void UpdateDbContext(string contextPath, EntityConfig config)
    {
        var lines = _fileWriter.ReadLines(contextPath);

        InsertDbSetEntry(lines, config);
        InsertConfigEntry(lines, config);

        _fileWriter.WriteLines(contextPath, lines);
    }

    private static void InsertDbSetEntry(List<string> lines, EntityConfig config)
    {
        var lastDbSetIndex = lines.FindLastIndex(l => l.Contains("public virtual DbSet<"));
        if (lastDbSetIndex >= 0)
        {
            var dbSetEntry = "public virtual DbSet<$EntityName> $EntitySetName { get; set; }"
                .Replace("$EntityName", config.ComponentName)
                .Replace("$EntitySetName", config.EntitySetName);
            lines.Insert(lastDbSetIndex + 1, dbSetEntry);
        }
    }

    private static void InsertConfigEntry(List<string> lines, EntityConfig config)
    {
        var lastConfigIndex = lines.FindLastIndex(l => l.Contains("modelBuilder.ApplyConfiguration(new"));
        if (lastConfigIndex >= 0)
        {
            var configEntry = "modelBuilder.ApplyConfiguration(new $EntityNameConfig());"
                .Replace("$EntityName", config.ComponentName);
            lines.Insert(lastConfigIndex + 1, configEntry);
        }
    }
}

public class AutoMapperProfileUpdater
{
    private readonly IFileWriter _fileWriter;

    public AutoMapperProfileUpdater(IFileWriter fileWriter)
    {
        _fileWriter = fileWriter;
    }

    public void UpdateProfile(string profilePath, EntityConfig config)
    {
        var lines = _fileWriter.ReadLines(profilePath);

        var lastSemicolonIndex = lines.FindLastIndex(l => l.EndsWith(";"));
        if (lastSemicolonIndex >= 0)
        {
            var configTemplate = config.Mapper == Mappers.AutoMapper ?
                Templates.MappingConfig.AutoMapper.Replace("$EntityName", config.ComponentName) :
                Templates.MappingConfig.Mapster.Replace("$EntityName", config.ComponentName);
            lines.Insert(lastSemicolonIndex + 1, configTemplate);
        }

        _fileWriter.WriteLines(profilePath, lines);
    }
}

public class DependencyInjectionUpdater
{
    private readonly IFileWriter _fileWriter;

    public DependencyInjectionUpdater(IFileWriter fileWriter)
    {
        _fileWriter = fileWriter;
    }

    public void UpdateProfile(string filePath, EntityConfig config)
    {
        var lines = _fileWriter.ReadLines(filePath);

        var lastSemicolonIndex = lines.FindLastIndex(l => l.EndsWith("return builder;"));
        if (lastSemicolonIndex >= 0)
        {
            var configTemplate = "builder.Services.AddTransient<$EntityNameService>();"
                .Replace("$EntityName", config.ComponentName);
            lines.Insert(lastSemicolonIndex + 1, configTemplate);
        }

        _fileWriter.WriteLines(filePath, lines);
    }
}
