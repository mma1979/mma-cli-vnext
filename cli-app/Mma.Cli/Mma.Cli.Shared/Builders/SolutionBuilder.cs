

namespace Mma.Cli.Shared.Builders;

public sealed class SolutionBuilder
{
    private readonly MapperConfig _config;

    public string SolutionName { get; }
    public string SolutionPath { get; private set; } = "";
    public string ProjectsPath { get; private set; } = "";
    public string Mapper { get; }

    private SolutionBuilder(string solutionName, string mapper)
    {
        SolutionName = solutionName;
        Mapper = mapper;
        _config = GetMapperConfig(mapper);
    }

    public static SolutionBuilder New(string solutionName, string mapper)
        => new(solutionName, mapper);

    public static SolutionBuilder New(string[] args)
    {
        var solutionName = args[1];
        var mapperFlagIndex = Array.IndexOf(args, Flags.MapperFlag);
        var mapper = mapperFlagIndex >= 0 && mapperFlagIndex + 1 < args.Length
            ? args[mapperFlagIndex + 1].ToLower() switch
            {
                "mapster" => Mappers.Mapster,
                _ => Mappers.AutoMapper
            }
            : Mappers.AutoMapper;

        return new SolutionBuilder(solutionName, mapper);
    }

    public async Task<SolutionBuilder> BuildAsync()
    {
        CreateSolutionDirectory();
        await ExtractSolutionAsync();
        RootRenameAndReplace();
        RenameDirectoriesAndFiles();
        await ReplaceNamespacesAsync();
        CreateMmaFolder();
        return this;
    }

    private SolutionBuilder CreateSolutionDirectory()
    {
        SolutionPath = Path.Combine(Directory.GetCurrentDirectory(), SolutionName);
        Output.Warning($"Creating {SolutionPath}");
        Directory.CreateDirectory(SolutionPath);
        return this;
    }

    private async Task ExtractSolutionAsync()
    {
        var zipFile = _config.ZipResource;
        var zipPath = Path.Combine(SolutionPath, "solution.zip");

        await File.WriteAllBytesAsync(zipPath, zipFile);

        var fz = new FastZip();
        fz.ExtractZip(zipPath, SolutionPath, "");

        File.Delete(zipPath);
    }

    private void RootRenameAndReplace()
    {
        var oldSolutionFile = Path.Combine(SolutionPath, $"{_config.TemplateName}.sln");
        var newSolutionFile = Path.Combine(SolutionPath, $"{SolutionName}.sln");

        ReplaceFileContent(oldSolutionFile, newSolutionFile, _config.TemplateName, SolutionName);

        var oldProjectsPath = Path.Combine(SolutionPath, _config.TemplateName);
        var newProjectsPath = Path.Combine(SolutionPath, SolutionName);

        Directory.Move(oldProjectsPath, newProjectsPath);
        ProjectsPath = newProjectsPath;
    }

    private void RenameDirectoriesAndFiles()
    {
        // Rename project directories
        var dirs = Directory.GetDirectories(ProjectsPath);
        Parallel.ForEach(dirs, dir =>
            Directory.Move(dir, dir.Replace(_config.TemplateName, SolutionName)));

        // Rename .csproj files
        var csprojFiles = Directory.GetFiles(ProjectsPath, "*.csproj", SearchOption.AllDirectories);
        Parallel.ForEach(csprojFiles, file =>
            File.Move(file, file.Replace(_config.TemplateName, SolutionName)));
    }

    private async Task ReplaceNamespacesAsync()
    {
        var filePattern = new Regex(@"^.+\.(cs|json|csproj|cshtml)$");
        var files = Directory.GetFiles(ProjectsPath, "*.*", SearchOption.AllDirectories)
            .Where(file => filePattern.IsMatch(file))
            .ToArray();

        await Task.Run(() => Parallel.ForEach(files, ReplaceFileNamespaces));
    }

    private void ReplaceFileNamespaces(string filePath)
    {
        var content = File.ReadAllText(filePath);
        var updatedContent = ApplyAllReplacements(content);
        File.WriteAllText(filePath, updatedContent);
    }

    private string ApplyAllReplacements(string content)
    {
        var lowerSolutionName = SolutionName.ToLower().Replace(".", "-");

        return content
            .Replace("MmaSolution", SolutionName)
            .Replace("mmasolution", lowerSolutionName)
            .Replace("mma-solution", lowerSolutionName);
    }

    private void ReplaceFileContent(string sourcePath, string targetPath, string oldValue, string newValue)
    {
        var content = File.ReadAllText(sourcePath);
        var updatedContent = content.Replace(oldValue, newValue);
        File.WriteAllText(targetPath, updatedContent);

        if (sourcePath != targetPath)
            File.Delete(sourcePath);
    }

    private void CreateMmaFolder()
    {
        var mmaDir = Directory.CreateDirectory(Path.Combine(SolutionPath, ".mma"));
        mmaDir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

        var projectData = new
        {
            Project = new
            {
                Name = SolutionName,
                Path = SolutionPath
            },
            Entities = new List<object>(),
            Rows = new List<object>(),
            Relations = new List<object>()
        };

        var json = JsonConvert.SerializeObject(projectData, Newtonsoft.Json.Formatting.Indented);
        var projectFile = Path.Combine(mmaDir.FullName, "project.mma");
        File.WriteAllText(projectFile, json, Encoding.UTF8);
    }

    private static MapperConfig GetMapperConfig(string mapper) => mapper switch
    {
        Mappers.Mapster => new MapperConfig("MmaSolution", Resources.Solutions.Mappster),
        _ => new MapperConfig("MmaSolution", Resources.Solutions.AutoMapper)
    };

    private record MapperConfig(string TemplateName, byte[] ZipResource);
}