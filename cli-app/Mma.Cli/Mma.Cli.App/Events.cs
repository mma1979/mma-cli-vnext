namespace Mma.Cli.App;

internal static class Events
{
    public static void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        var dotnet = Process.GetProcessesByName("dotnet").ToList();
        var mma = Process.GetProcessesByName("mma-cli").ToList();
        if (dotnet.Any())
            dotnet.ForEach(p => p.Kill());

        if (mma.Any())
            mma.ForEach(p => p.Kill());
    }


}
