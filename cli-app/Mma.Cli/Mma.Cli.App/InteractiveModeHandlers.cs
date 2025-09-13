
using CliWrap;

namespace Mma.Cli.App;

internal class InteractiveModeHandlers
{
    public static readonly string Version = Assembly.GetEntryAssembly()!
       .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "9.0.1";

    public static async Task<int> HandleInteractiveModeAsync()
    {
        var command = Prompt.Select("Select your command",
            [Commands.NEW, Commands.GENERATE, Commands.UI, Commands.WATCH],
            defaultValue: Commands.NEW);

        return command switch
        {
            Commands.NEW => HandleInteractiveNew(),
            Commands.GENERATE => HandleInteractiveGenerate(),
            Commands.UI => await HandleUICommand(),
            Commands.WATCH => HandleWatch(),
            _ => HandleInvalidCommand()
        };

    }

    private static int HandleInteractiveNew()
    {
        var solutionName = Prompt.Input<string>("Enter Solution Name");
        var mapper = Prompt.Select("Select the Mapper",
            [Mappers.AutoMapper, Mappers.Mapster],
            defaultValue: Mappers.AutoMapper);

        SolutionBuilder.New(solutionName, mapper)
            .BuildAsync()
            .Wait();

        Output.Success("Solution Created");
        return 0;
    }

    private static int HandleInteractiveGenerate()
    {
        var componentType = Prompt.Select("Select Component",
            [
                InteractiveComponents.AddEntity, InteractiveComponents.RemoveEntity,
                InteractiveComponents.AddProperty, InteractiveComponents.RemoveProperty,
                InteractiveComponents.AddRelation, InteractiveComponents.RemoveRelation
            ],
            defaultValue: InteractiveComponents.AddEntity);

        return componentType switch
        {
            InteractiveComponents.AddEntity => GenerateEntityInteractive(false),
            InteractiveComponents.RemoveEntity => GenerateEntityInteractive(true),
            InteractiveComponents.AddProperty => GeneratePropertyInteractive(false),
            InteractiveComponents.RemoveProperty => GeneratePropertyInteractive(true),
            InteractiveComponents.AddRelation => GenerateRelationInteractive(false),
            InteractiveComponents.RemoveRelation => GenerateRelationInteractive(true),
            _ => -1
        };
    }

    private static async Task<int> HandleUICommand()
    {
        try
        {
            var executablePath = BuildHelper.GetExecutablePath();

            await CliWrap.Cli.Wrap("dotnet")
                .WithWorkingDirectory(Path.Combine(executablePath, "UI"))
                .WithArguments("cli-ui.dll")
                .WithStandardOutputPipe(PipeTarget.ToDelegate(_ => {
                    Console.Clear();
                    Output.Success("""
                                                

                        .___  ___. .___  ___.      ___      
                        |   \/   | |   \/   |     /   \     
                        |  \  /  | |  \  /  |    /  ^  \    
                        |  |\/|  | |  |\/|  |   /  /_\  \   
                        |  |  |  | |  |  |  |  /  _____  \  
                        |__|  |__| |__|  |__| /__/     \__\ 

                        Now listening on: http://localhost:5000

                        """);
                }))
                .WithStandardErrorPipe(PipeTarget.ToDelegate(o => Output.Error(o.ToString())))
                .ExecuteAsync();

            return 0;
        }
        catch (Exception ex)
        {
            Output.Error($"UI execution failed: {ex.Message}");
            return -1;
        }
    }

    private static int HandleWatch()
    {
        Output.Error("Watch command is not yet implemented");
        return -1;
    }

    private static int HandleInvalidCommand()
    {
        Output.Error("Invalid Command");
        BuildHelper.Help(Version);
        return -1;
    }

    private static int GenerateEntityInteractive(bool performRemove)
    {
        var entityName = Prompt.Input<string>("Enter Entity Name");
        var pkType = Prompt.Select("Select PK type",
            [PkTypes.GUID, PkTypes.INT, PkTypes.LONG, PkTypes.DECIMAL, PkTypes.FLOAT, PkTypes.STRING, PkTypes.BOOL, PkTypes.DATE_TIME],
            defaultValue: PkTypes.GUID);
        var generateApi = Prompt.Select("Generate API controller?", ["Yes", "No"], defaultValue: "Yes") == "Yes";

        var args = BuildEntityArgs(entityName, pkType, generateApi, performRemove);

        EntityBuilder.CreateFromArgs(args)
            .GenerateAll(generateApi);

        Output.Success("Entity files generated");
        LogEquivalentCommand("entity", entityName, pkType, generateApi, performRemove);
        return 0;
    }

    private static int GeneratePropertyInteractive(bool performRemove)
    {
        var entityName = Prompt.Input<string>("Enter Entity Name");
        var propertyName = Prompt.Input<string>("Enter Property Name");
        var pType = Prompt.Select("Select Property type",
            [PkTypes.GUID, PkTypes.INT, PkTypes.LONG, PkTypes.DECIMAL, PkTypes.FLOAT, PkTypes.STRING, PkTypes.BOOL, PkTypes.DATE_TIME],
            defaultValue: PkTypes.GUID);
        var nullable = Prompt.Select("Is Nullable?", ["Yes", "No"], defaultValue: "Yes") == "Yes";

        var args = BuildPropertyArgs(entityName, propertyName, pType, nullable, performRemove);

        PropertiesBuilder.New(args, BuildHelper.DetectMapper())
            .UpdateEntityModels()
            .UpdateEntity()
            .UpdateEntityConfig();

        Output.Success("Property has been generated");
        LogEquivalentCommand("property", entityName, propertyName, pType, nullable, performRemove);
        return 0;
    }

    private static int GenerateRelationInteractive(bool performRemove)
    {
        var parentEntityName = Prompt.Input<string>("Enter Reference Entity Name");
        var childEntityName = Prompt.Input<string>("Enter Child Entity Name");
        var foreignKeyName = Prompt.Input<string>("Enter Foreign Key Name:");
       
        var args = BuildRelationArgs(parentEntityName, childEntityName, foreignKeyName, performRemove);

        RelationsBuilder.New(args)
           .UpdateParentEntity()
            .UpdateParentModels()
            .UpdateChildEntity()
            .UpdateChildModels()
            .UpdateParentEntityConfig();

        Output.Success("Relation has been generated");
        LogEquivalentCommand("relation", parentEntityName, childEntityName, foreignKeyName, performRemove);
        return 0;
    }

    // Helper methods for building command arguments
    private static string[] BuildEntityArgs(string entityName, string pkType, bool generateApi, bool performRemove)
    {
        var args = new List<string> { "g", "e", entityName, pkType, Flags.MapperFlag, BuildHelper.DetectMapper() };
        if (!generateApi) args.Add("--no-api");
        if (performRemove) args.Add("--remove");
        return args.ToArray();
    }

    private static string[] BuildPropertyArgs(string entityName, string propertyName, string pType, bool nullable, bool performRemove)
    {
        var args = new List<string> { "g", "p", entityName, propertyName, pType, nullable.ToString().ToLower() };
        if (performRemove) args.Add("--remove");
        return args.ToArray();
    }

    private static string[] BuildRelationArgs(string parentEntity, string childEntity, string foreignKey,  bool performRemove)
    {
        var args = new List<string> { "g", "r", parentEntity, childEntity, foreignKey };
        if (performRemove) args.Add("--remove");
        return args.ToArray();
    }

    private static void LogEquivalentCommand(string type, params object[] parameters)
    {
        var commandParts = new List<string> { "mma", "g" };
        commandParts.AddRange(parameters.Select(p => p.ToString()!));
        Output.Warning($"Equivalent Command: {string.Join(" ", commandParts)}");
    }
}
