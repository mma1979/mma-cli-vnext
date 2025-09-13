
namespace Mma.Cli.Shared.Builders;

public class RelationsBuilder
{
    public string SolutionPath { get; private set; } = "";
    public string SolutionName { get; private set; } = "";
    public string ProjectsPath { get; private set; } = "";
    public string Mapper { get; private set; } = "";
    public string ParentEntity { get; set; } = "";
    public string ChiledEntity { get; set; } = "";
    public string ForeignKey { get; set; } = "";
    public string ForeignDataType { get; set; } = "";
    public bool RemoveFlag { get; set; }

    public RelationsBuilder(string? solutionPath = null)
    {
        SolutionPath = solutionPath ?? Directory.GetCurrentDirectory();
    }

    public static RelationsBuilder New(string[] args, string? solutionPath = null)
    {
        ValidateArgs(args);

        var builder = new RelationsBuilder(solutionPath)
        {
            ParentEntity = args[2],
            ChiledEntity = args[3],
            ForeignKey = args[4],
            ForeignDataType = PkTypes.GUID,
            RemoveFlag = args.Contains(Flags.RemoveFlag)
        };

        (builder.SolutionName, builder.ProjectsPath) = BuildHelper.CheckSolutionPath(builder.SolutionPath);
        return builder;
    }

    public RelationsBuilder UpdateParentModels()
    {
        var navigation = BuildProperty(false, true, $"List<{ChiledEntity}>", BuildHelper.GetSetName(ChiledEntity));
        var modifyModelPath = GetModelPath(ParentEntity, $"{ParentEntity}ModifyModel.cs");

        var idProp = File.ReadAllLines(modifyModelPath)
            .FirstOrDefault(l => l.Contains(" Id ")) ?? "public Guid";

        ForeignDataType = idProp.Split(' ')
            .Where(s => !string.IsNullOrEmpty(s))
            .ElementAt(1).Trim();

        ProcessFileOperation(modifyModelPath, navigation, FileOperation.AddOrRemoveProperty);
        return this;
    }

    public RelationsBuilder UpdateChildModels()
    {
        var navigation = BuildProperty(false, true, ParentEntity, ParentEntity);
        var foreignKey = BuildProperty(false, false, ForeignDataType, ForeignKey);

        var modifyModelPath = GetModelPath(ChiledEntity, $"{ChiledEntity}ModifyModel.cs");
        var readModelPath = GetModelPath(ChiledEntity, $"{ChiledEntity}ReadModel.cs");

        if (RemoveFlag)
        {
            RemoveMultiplePropertiesFromFile(modifyModelPath, [foreignKey, navigation]);
            RemovePropertyFromFile(readModelPath, foreignKey);
        }
        else
        {
            AddMultiplePropertiesToFile(modifyModelPath, [foreignKey, navigation]);
            AddPropertyToFile(readModelPath, foreignKey);
        }

        return this;
    }

    public RelationsBuilder UpdateParentEntity()
    {
        var children = BuildHelper.GetSetName(ChiledEntity);
        var navigation = BuildProperty(false, true, $"ICollection<{ChiledEntity}>", children);
        var filePath = GetEntityPath(ParentEntity);
        var assignment = $"{children} ??= new List<{ChiledEntity}>();";

        ProcessEntityUpdate(filePath, navigation, assignment, ParentEntity);
        return this;
    }

    public RelationsBuilder UpdateChildEntity()
    {
        var navigation = BuildProperty(true, true, ParentEntity, ParentEntity);
        var foreignKey = BuildProperty(true, false, ForeignDataType, ForeignKey);
        var filePath = GetEntityPath(ChiledEntity);
        var assignment = $"{ForeignKey} = dto.{ForeignKey};";

        if (RemoveFlag)
        {
            RemoveMultiplePropertiesFromFile(filePath, [foreignKey, navigation]);
            RemoveAssignmentFromEntityFile(filePath, assignment);
        }
        else
        {
            AddMultiplePropertiesToEntityFile(filePath, [foreignKey, navigation], ChiledEntity);
            AddAssignmentToEntityFile(filePath, assignment, ChiledEntity);
        }

        return this;
    }

    public RelationsBuilder UpdateParentEntityConfig()
    {
        var configPath = GetConfigPath(ParentEntity);
        var config = BuildEntityRelationConfig();

        ProcessFileOperation(configPath, config, FileOperation.AddOrRemoveConfig);
        return this;
    }

    #region Private Helper Methods

    private static void ValidateArgs(string[] args)
    {
        if (args.Length < 5) // Changed to 6 since we need ForeignDataType
        {
            Output.Error("Not enough arguments use mma -h to get help");
            Environment.Exit(-1);
        }
    }

    private string GetModelPath(string entity, string fileName) =>
        Path.Combine(ProjectsPath, $"{SolutionName}.Core", "Models", entity,fileName);

    private string GetEntityPath(string entityName) =>
        Path.Combine(ProjectsPath, $"{SolutionName}.Core", "Database", "Tables", $"{entityName}.cs");

    private string GetConfigPath(string entityName) =>
        Path.Combine(ProjectsPath, $"{SolutionName}.EntityFramework", "EntityConfigurations", $"{entityName}Config.cs");

    private string BuildProperty(bool isPrivateSet, bool isVirtual, string type, string name)
    {
        var modifier = isPrivateSet ? "private " : "";
        var virtualized = isVirtual ? "virtual " : "";
        return $"public {virtualized}{type}? {name} {{get; {modifier}set;}}";
    }

    private string BuildEntityRelationConfig()
    {
        var childrenName = BuildHelper.GetSetName(ChiledEntity);
        return $"builder.HasMany(e => e.{childrenName}).WithOne(e => e.{ParentEntity}).HasForeignKey(e => e.{ForeignKey}).OnDelete(DeleteBehavior.Cascade);";
    }

    private enum FileOperation
    {
        AddOrRemoveProperty,
        AddOrRemoveConfig
    }

    private void ProcessFileOperation(string filePath, string content, FileOperation operation)
    {
        if (string.IsNullOrEmpty(content)) return;

        if (RemoveFlag)
            RemovePropertyFromFile(filePath, content);
        else
        {
            switch (operation)
            {
                case FileOperation.AddOrRemoveProperty:
                    AddPropertyToFile(filePath, content);
                    break;
                case FileOperation.AddOrRemoveConfig:
                    AddConfigToFile(filePath, content);
                    break;
            }
        }
    }

    private void ProcessEntityUpdate(string filePath, string navigation, string assignment, string entityName)
    {
        if (RemoveFlag)
        {
            RemovePropertyFromFile(filePath, navigation);
            RemoveAssignmentFromEntityFile(filePath, assignment);
        }
        else
        {
            AddPropertyToEntityFile(filePath, navigation, entityName);
            AddAssignmentToEntityFile(filePath, assignment, entityName);
        }
    }

    private void AddPropertyToFile(string filePath, string property)
    {
        ModifyFile(filePath, lines =>
        {
            var insertIndex = Math.Max(0, lines.Count - 2);
            lines.Insert(insertIndex, property);
        });
    }

    private void AddMultiplePropertiesToFile(string filePath, string[] properties)
    {
        ModifyFile(filePath, lines =>
        {
            var insertIndex = Math.Max(0, lines.Count - 2);
            for (int i = properties.Length - 1; i >= 0; i--)
            {
                lines.Insert(insertIndex, properties[i]);
            }
        });
    }

    private void AddPropertyToEntityFile(string filePath, string property, string entityName)
    {
        ModifyFile(filePath, lines =>
        {
            var validatorLine = lines.FirstOrDefault(l => l.Contains($"{entityName}Validator _Validator;"));
            if (validatorLine != null)
            {
                var insertIndex = Math.Max(0, lines.IndexOf(validatorLine) - 1);
                lines.Insert(insertIndex, property);
            }
        });
    }

    private void AddMultiplePropertiesToEntityFile(string filePath, string[] properties, string entityName)
    {
        ModifyFile(filePath, lines =>
        {
            var validatorLine = lines.FirstOrDefault(l => l.Contains($"{entityName}Validator _Validator;"));
            if (validatorLine != null)
            {
                var insertIndex = Math.Max(0, lines.IndexOf(validatorLine) - 1);
                for (int i = properties.Length - 1; i >= 0; i--)
                {
                    lines.Insert(insertIndex, properties[i]);
                }
            }
        });
    }

    private void AddAssignmentToEntityFile(string filePath, string assignment, string entityName)
    {
        ModifyFile(filePath, lines =>
        {
            // Add to constructor
            AddAssignmentToMethod(lines, $"public {entityName}(", assignment, 10);
        });

        ModifyFile(filePath, lines =>
        {
            // Add to Update method
            AddAssignmentToMethod(lines, $"public {entityName} Update(", assignment, 10);
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

    private void RemoveMultiplePropertiesFromFile(string filePath, string[] searchTexts)
    {
        ModifyFile(filePath, lines =>
        {
            foreach (var searchText in searchTexts)
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    var lineToRemove = lines.FirstOrDefault(l => l.Contains(searchText));
                    if (lineToRemove != null)
                    {
                        lines.Remove(lineToRemove);
                    }
                }
            }
        });
    }

    private void RemoveAssignmentFromEntityFile(string filePath, string assignment)
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
