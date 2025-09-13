




Console.CancelKeyPress += Events.OnCancelKeyPress;

try
{
    var exitCode = args.Length > 0
        ? await CommandLineHandlers.HandleCommandLineAsync(args)
        : await InteractiveModeHandlers.HandleInteractiveModeAsync();

    Environment.Exit(exitCode);
}
catch (Exception ex)
{
    Output.Error($"An error occurred: {ex.Message}");
    Environment.Exit(-1);
}


