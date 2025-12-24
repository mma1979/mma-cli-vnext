using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
ConfigureServices(services);
var serviceProvider = services.BuildServiceProvider();

var configDirectory = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
    ".mma");


var configPath = Path.Combine(configDirectory, "state.json");

string currentVersion = typeof(Program).Assembly.GetName().Version?.ToString() ?? "9.0.6";
string? savedVersion = null;

if (File.Exists(configPath))
{
    var state = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(configPath));
    savedVersion = state?["version"];
}

if (savedVersion != currentVersion)
{
    Console.WriteLine(savedVersion == null ? "First install detected." : "Update detected.");
    RunInstallOrUpdateEvents();

    Directory.CreateDirectory(Path.GetDirectoryName(configPath)!);
    File.WriteAllText(configPath, JsonSerializer.Serialize(new { version = currentVersion }));
}

void RunInstallOrUpdateEvents()
{
    //update state.json
    if (!Directory.Exists(configDirectory))
    {
        Directory.CreateDirectory(configDirectory);
    }

    File.WriteAllText(configPath, JsonSerializer.Serialize(new { version = currentVersion }));

    // download and update projects templates
}

Console.CancelKeyPress += Events.OnCancelKeyPress;

try
{
    var exitCode = args.Length > 0
        ? await serviceProvider.GetRequiredService<CommandLineHandlers>().HandleCommandLineAsync(args)
        : await serviceProvider.GetRequiredService<InteractiveModeHandlers>().HandleInteractiveModeAsync();

    Environment.Exit(exitCode);
}
catch (Exception ex)
{
    Output.Error($"An error occurred: {ex.Message}");
    Environment.Exit(-1);
}

void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<CommandLineHandlers>();
    services.AddSingleton<InteractiveModeHandlers>();
}


