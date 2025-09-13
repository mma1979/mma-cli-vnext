


using CliWrap;

namespace Mma.Cli.App;

internal static class CommandLineHandlers
{
    public static readonly string Version =  Assembly.GetEntryAssembly()!
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "9.0.1";

    public static async Task<int> HandleCommandLineAsync(string[] args)
    {
        var command = args[0].ToLowerInvariant();

        return command switch
        {
            CommandsFlags.New or CommandsFlags.NewShortHand => await HandleNewCommand(args),
            CommandsFlags.Generate or CommandsFlags.GenerateShortHand => HandleGenerateCommand(args),
            CommandsFlags.UI => await HandleUICommand(),
            CommandsFlags.Import => HandleImportCommand(args),
            CommandsFlags.Help or CommandsFlags.HelpShortHand => HandleHelpCommand(),
            CommandsFlags.Version or CommandsFlags.VersionShortHand => HandleVersionCommand(),
            _ => HandleInvalidCommand()
        };
    }

    

    private static async Task<int> HandleNewCommand(string[] args)
    {
        await SolutionBuilder
            .New(args)
            .BuildAsync();

        Output.Success("Solution Created");
        return 0;
    }

    private static int HandleGenerateCommand(string[] args)
    {
        if (args.Length < 2)
        {
            Output.Error("Generate command requires a component type");
            return -1;
        }

        var component = args[1].ToLowerInvariant();

        return component switch
        {
            ComponentFlags.Entity or ComponentFlags.EntityShortHand => HandleEntityGeneration(args),
            ComponentFlags.Property or ComponentFlags.PropertyShortHand => HandlePropertyGeneration(args),
            ComponentFlags.Relation or ComponentFlags.RelationShortHand => HandleRelationGeneration(args),
            _ => HandleInvalidComponent()
        };
    }

    private static int HandleEntityGeneration(string[] args)
    {
        EntityBuilder.CreateFromArgs(args)
            .GenerateAll(!args.Contains(Flags.ApiFlag));

        Output.Success("Entity files generated");
        return 0;
    }

    private static int HandleVersionCommand()
    {
        Output.Success($"""
.___  ___. .___  ___.      ___      
|   \/   | |   \/   |     /   \     
|  \  /  | |  \  /  |    /  ^  \    
|  |\/|  | |  |\/|  |   /  /_\  \   
|  |  |  | |  |  |  |  /  _____  \  
|__|  |__| |__|  |__| /__/     \__\ 

   mma {Version.Split('+')[0]}
""");
        return 0;
    }
    private static int HandleInvalidCommand()
    {
        Output.Error("Invalid Command");
        BuildHelper.Help(Version);
        return -1;
    }

    private static int HandleInvalidComponent()
    {
        Output.Error("Invalid Component");
        BuildHelper.Help(Version);
        return -1;
    }
    private static int HandlePropertyGeneration(string[] args)
    {
        try
        {
            PropertiesBuilder.New(args, BuildHelper.DetectMapper())
                .UpdateEntityModels()
                .UpdateEntity()
                .UpdateEntityConfig();

            Output.Success("Property has been generated");
            return 0;
        }
        catch (Exception ex)
        {
            Output.Error($"Property generation failed: {ex.Message}");
            return -1;
        }
    }

    private static int HandleRelationGeneration(string[] args)
    {
        RelationsBuilder.New(args)
            .UpdateParentEntity()
            .UpdateParentModels()
            .UpdateChildEntity()
            .UpdateChildModels()
            .UpdateParentEntityConfig();

        Output.Success("Relation has been generated");
        return 0;
    }

    private static int HandleHelpCommand()
    {
        BuildHelper.Help(Version);
        return 0;
    }

    private static int HandleImportCommand(string[] args)
    {
        ImportFactory.New(args).Import();
        return 0;
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
                .WithStandardErrorPipe(PipeTarget.ToDelegate(o=> Output.Error(o.ToString())))
                .ExecuteAsync();

            return 0;
        }
        catch (Exception ex)
        {
            Output.Error($"UI execution failed: {ex.Message}");
            return -1;
        }
    }
}
