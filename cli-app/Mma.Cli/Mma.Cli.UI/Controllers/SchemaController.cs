using Microsoft.AspNetCore.Mvc;
using Mma.Cli.UI.Services;

namespace Mma.Cli.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchemaController : ControllerBase
{
    private readonly IDatabaseService _databaseService;
    private string SchemaPath => Path.Combine(Directory.GetCurrentDirectory(), "schemaforge.json");

    public SchemaController(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [HttpGet("load")]
    public IActionResult Load([FromQuery] string? path)
    {
        var targetPath = string.IsNullOrEmpty(path) ? SchemaPath : Path.Combine(path, "schemaforge.json");
        
        if (!System.IO.File.Exists(targetPath))
        {
            return Ok(new { nodes = new object[] { }, edges = new object[] { } });
        }

        var json = System.IO.File.ReadAllText(targetPath);
        return Content(json, "application/json");
    }

    [HttpPost("save")]
    public IActionResult Save([FromBody] SchemaSaveRequest request)
    {
        var targetPath = string.IsNullOrEmpty(request.Path) ? SchemaPath : Path.Combine(request.Path, "schemaforge.json");
        var json = System.Text.Json.JsonSerializer.Serialize(request.Schema, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        System.IO.File.WriteAllText(targetPath, json);
        return Ok(new { Message = "Schema saved successfully" });
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] SchemaSaveRequest request)
    {
        // 1. Save first to ensure we use the latest
        Save(request);

        try
        {
            // Placeholder for CLI / Service generation logic
            // In a real scenario, this would call the MMA CLI core
            await Task.Delay(2000); 

            return Ok(new { Message = "Code generated successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public class SchemaSaveRequest
    {
        public string? Path { get; set; }
        public object Schema { get; set; } = null!;
    }

    [HttpGet("project/cwd")]
    public IActionResult GetCwd()
    {
        return Ok(new { cwd = Directory.GetCurrentDirectory() });
    }

    [HttpGet("project/directories")]
    public IActionResult GetDirectories([FromQuery] string? path)
    {
        try
        {
            var targetPath = string.IsNullOrEmpty(path) ? Directory.GetCurrentDirectory() : path;
            if (!Directory.Exists(targetPath)) return BadRequest("Directory does not exist");

            var dirs = Directory.GetDirectories(targetPath)
                .Select(d => new { 
                    name = Path.GetFileName(d), 
                    path = d 
                })
                .ToList();

            var parent = Directory.GetParent(targetPath)?.FullName;

            return Ok(new { 
                currentPath = targetPath,
                parentPath = parent,
                directories = dirs 
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("project/create-directory")]
    public IActionResult CreateDirectory([FromBody] CreateDirectoryRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Path)) return BadRequest("Path is required");
            if (!Directory.Exists(request.Path))
            {
                Directory.CreateDirectory(request.Path);
                return Ok(new { Message = "Directory created successfully" });
            }
            return BadRequest("Directory already exists");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public class CreateDirectoryRequest
    {
        public string Path { get; set; } = "";
    }

    [HttpPost("database/test")]
    public async Task<IActionResult> TestConnection([FromBody] DbConnectionRequest request)
    {
        var success = await _databaseService.TestConnectionAsync(request.ConnectionString, request.Provider);
        return success ? Ok() : BadRequest("Connection failed");
    }

    [HttpPost("database/tables")]
    public async Task<IActionResult> GetTables([FromBody] DbConnectionRequest request)
    {
        var tables = await _databaseService.GetTablesAsync(request.ConnectionString, request.Provider);
        return Ok(tables);
    }

    [HttpPost("database/import")]
    public async Task<IActionResult> ImportTables([FromBody] DbImportRequest request)
    {
        var project = await _databaseService.ImportTablesAsync(request.ConnectionString, request.Provider, request.TableNames);
        return Ok(project);
    }

    [HttpPost("project/create")]
    public async Task<IActionResult> CreateSolution([FromBody] CreateSolutionRequest request)
    {
        try
        {
            var builder = string.IsNullOrEmpty(request.Path)
                ? Mma.Cli.Shared.Builders.SolutionBuilder.New(request.SolutionName, request.Mapper)
                : Mma.Cli.Shared.Builders.SolutionBuilder.New(request.SolutionName, request.Mapper, request.Path);

            await builder.BuildAsync();

            return Ok(new { Message = "Solution created successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public class CreateSolutionRequest
{
    public string SolutionName { get; set; } = "";
    public string Mapper { get; set; } = "AutoMapper";
    public string? Path { get; set; }
}

public class DbConnectionRequest
{
    public string ConnectionString { get; set; } = "";
    public string Provider { get; set; } = "sqlserver";
}

public class DbImportRequest : DbConnectionRequest
{
    public List<string> TableNames { get; set; } = new();
}
