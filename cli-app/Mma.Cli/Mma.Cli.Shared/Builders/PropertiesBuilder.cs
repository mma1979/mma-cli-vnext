
namespace Mma.Cli.Shared.Builders;

public class PropertiesBuilder
{
    public string SolutionPath { get; private set; }
    public string SolutionName { get; private set; }
    public string ProjectsPath { get; private set; }
    public string Mapper { get; private set; }
    public string EntityName { get; set; }
    public string PropertyName { get; set; }
    public string DataType { get; set; }
    public bool Nullable { get; set; }
    public bool RemoveFlag { get; set; }

    public PropertiesBuilder(string? solutionPath = null)
    {
        SolutionPath = solutionPath ?? Directory.GetCurrentDirectory();
    }

    public static PropertiesBuilder New(string[] args, string mapper, string? solutionPath = null)
    {
        ValidateArgs(args);

        var builder = new PropertiesBuilder(solutionPath)
        {
            Mapper = mapper,
            EntityName = args[2],
            PropertyName = args[3],
            DataType = args[4],
            Nullable = args.Length > 5 ? args[5].ToLower() == "true": false,
            RemoveFlag = args.Contains(Flags.RemoveFlag),
        };

        (builder.SolutionName, builder.ProjectsPath) = BuildHelper.CheckSolutionPath(builder.SolutionPath);
        return builder;
    }

    public PropertiesBuilder UpdateEntityModels()
    {
        var property = BuildProperty(false);
        var modifyModelPath = GetModelPath($"{EntityName}ModifyModel.cs");
        var readModelPath = GetModelPath($"{EntityName}ReadModel.cs");

        if (RemoveFlag)
        {
            RemovePropertyFromFile(modifyModelPath, property);
            RemovePropertyFromFile(readModelPath, property);
        }
        else
        {
            AddPropertyToFile(modifyModelPath, property);
            AddPropertyToFile(readModelPath, property);
        }

        return this;
    }

    public PropertiesBuilder UpdateEntity()
    {
        var property = BuildProperty(true);
        var filePath = GetEntityPath();
        var assignment = $"{PropertyName} = dto.{PropertyName};";

        if (RemoveFlag)
        {
            RemovePropertyFromFile(filePath, property);
            RemoveAssignmentFromFile(filePath, assignment);
        }
        else
        {
            AddPropertyToEntityFile(filePath, property);
            AddAssignmentToEntityFile(filePath, assignment);
        }

        return this;
    }

    public PropertiesBuilder UpdateEntityConfig()
    {
        var configPath = GetConfigPath();
        var configLine = BuildConfigLine();

        if (!string.IsNullOrEmpty(configLine))
        {
            if (RemoveFlag)
                RemovePropertyFromFile(configPath, configLine);
            else
                AddConfigToFile(configPath, configLine);
        }

        return this;
    }

    #region Private Helper Methods

    private static void ValidateArgs(string[] args)
    {
        if (args.Length < 5)
        {
            Output.Error("Not enough arguments use mma -h to get help");
            Environment.Exit(-1);
        }
    }

    private string GetModelPath(string fileName) =>
        Path.Combine(ProjectsPath, $"{SolutionName}.Core", "Models",EntityName, fileName);

    private string GetEntityPath() =>
        Path.Combine(ProjectsPath, $"{SolutionName}.Core", "Database", "Tables", $"{EntityName}.cs");

    private string GetConfigPath() =>
        Path.Combine(ProjectsPath, $"{SolutionName}.EntityFramework", "EntityConfigurations", $"{EntityName}Config.cs");

    private string BuildProperty(bool isPrivateSet)
    {
        var modifier = isPrivateSet ? "private " : "";
        var nullable = Nullable ? "?" : "";
        return $"public {DataType}{nullable} {PropertyName} {{get; {modifier}set;}}";
    }

    private string BuildConfigLine()
    {
        return DataType switch
        {
            PkTypes.STRING => $"builder.Property(e => e.{PropertyName}).HasMaxLength(255);",
            PkTypes.DECIMAL => $"builder.Property(e => e.{PropertyName}).HasColumnType(\"decimal(9,3)\");",
            PkTypes.DATE_TIME => $"builder.Property(e => e.{PropertyName}).HasColumnType(\"datetime\");",
            PkTypes.GUID => $"builder.Property(e => e.{PropertyName}).HasDefaultValue(\"newid()\");",
            _ => string.Empty
        };
    }

    private void AddPropertyToFile(string filePath, string property)
    {
        ModifyFile(filePath, lines =>
        {
            var insertIndex = Math.Max(0, lines.Count - 2);
            lines.Insert(insertIndex, property);
        });
    }

    private void AddPropertyToEntityFile(string filePath, string property)
    {
        ModifyFile(filePath, lines =>
        {
            var validatorLine = lines.FirstOrDefault(l => l.Contains($"{EntityName}Validator _Validator;"));
            if (validatorLine != null)
            {
                var insertIndex = Math.Max(0, lines.IndexOf(validatorLine) - 1);
                lines.Insert(insertIndex, property);
            }
        });
    }

    private void AddAssignmentToEntityFile(string filePath, string assignment)
    {
        ModifyFile(filePath, lines =>
        {
            // Add to constructor
            AddAssignmentToMethod(lines, $"public {EntityName}(", assignment, 10);
        });

        ModifyFile(filePath, lines =>
        {
            // Add to Update method
            AddAssignmentToMethod(lines, $"public {EntityName} Update(", assignment, 10);
        });
    }

    private void AddAssignmentToMethod(List<string> lines, string methodSignature, string assignment, int offset)
    {
        var methodLine = lines.FirstOrDefault(l => l.Contains(methodSignature));
        if (methodLine != null)
        {
            var insertIndex = Math.Min(lines.Count - 1, lines.IndexOf(methodLine) + offset);
            lines.Insert(insertIndex, assignment);
        }
    }

    private void AddConfigToFile(string filePath, string configLine)
    {
        ModifyFile(filePath, lines =>
        {
            var indexLine = lines.FirstOrDefault(l => l.Contains("builder.HasIndex(e => e.IsDeleted);"));
            if (indexLine != null)
            {
                var insertIndex = Math.Min(lines.Count - 1, lines.IndexOf(indexLine) + 2);
                lines.Insert(insertIndex, configLine);
            }
        });
    }

    private void RemovePropertyFromFile(string filePath, string searchText)
    {
        if (string.IsNullOrEmpty(searchText)) return;

        ModifyFile(filePath, lines =>
        {
            var lineToRemove = lines.FirstOrDefault(l => l.Contains(searchText));
            if (lineToRemove != null)
            {
                lines.Remove(lineToRemove);
            }
        });
    }

    private void RemoveAssignmentFromFile(string filePath, string assignment)
    {
        ModifyFile(filePath, lines =>
        {
            // Remove all occurrences of the assignment
            lines.RemoveAll(l => l.Contains(assignment));
        });
    }

    private void ModifyFile(string filePath, Action<List<string>> modifier)
    {
        try
        {
            var lines = File.ReadAllLines(filePath).ToList();
            modifier(lines);

            File.WriteAllText(filePath, string.Join(Environment.NewLine, lines));
        }
        catch (Exception ex)
        {
            Output.Error($"Failed to modify file {filePath}: {ex.Message}");
        }
    }

    #endregion
}