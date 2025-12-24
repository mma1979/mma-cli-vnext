using System.Data;
using Microsoft.Data.SqlClient;
using Npgsql;
using Mma.Cli.Shared.Models;
using System.Data.Common;

namespace Mma.Cli.UI.Services;

public interface IDatabaseService
{
    Task<bool> TestConnectionAsync(string connectionString, string provider);
    Task<List<string>> GetTablesAsync(string connectionString, string provider);
    Task<ProjectDumpModel> ImportTablesAsync(string connectionString, string provider, List<string> tableNames);
}

public class DatabaseService : IDatabaseService
{
    public async Task<bool> TestConnectionAsync(string connectionString, string provider)
    {
        try
        {
            using var connection = CreateConnection(connectionString, provider);
            await connection.OpenAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<string>> GetTablesAsync(string connectionString, string provider)
    {
        var tables = new List<string>();
        using var connection = CreateConnection(connectionString, provider);
        await connection.OpenAsync();

        var query = provider.ToLower() == "sqlserver" 
            ? "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'"
            : "SELECT tablename FROM pg_catalog.pg_tables WHERE schemaname != 'pg_catalog' AND schemaname != 'information_schema'";

        using var command = connection.CreateCommand();
        command.CommandText = query;

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            tables.Add(reader.GetString(0));
        }

        return tables;
    }

    public async Task<ProjectDumpModel> ImportTablesAsync(string connectionString, string provider, List<string> tableNames)
    {
        var model = new ProjectDumpModel();
        using var connection = CreateConnection(connectionString, provider);
        await connection.OpenAsync();

        var isSqlServer = provider.ToLower() == "sqlserver";
        var tableFilter = string.Join(",", tableNames.Select(t => $"'{t}'"));

        // 1. Extract Columns
        var columnQuery = isSqlServer
            ? $"SELECT TABLE_NAME, COLUMN_NAME, DATA_TYPE, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME IN ({tableFilter})"
            : $"SELECT table_name, column_name, data_type, is_nullable FROM information_schema.columns WHERE table_name IN ({tableFilter})";

        var entities = new Dictionary<string, EntityModel>();
        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = columnQuery;
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var tableName = reader.GetString(0);
                var columnName = reader.GetString(1);
                var dataType = reader.GetString(2);
                var isNullable = reader.GetString(3) == "YES";

                if (!entities.ContainsKey(tableName))
                {
                    var entity = new EntityModel { EntityName = tableName };
                    entities[tableName] = entity;
                    model.Entities!.Add(entity);
                }

                entities[tableName].Rows.Add(new EntityRowModel
                {
                    ColumnName = columnName,
                    DataType = dataType,
                    Nullable = isNullable
                });
            }
        }

        // 2. Extract Relationships
        var fkQuery = isSqlServer
            ? $@"SELECT fk.name, tp.name, cp.name, tr.name, cr.name 
                 FROM sys.foreign_keys AS fk
                 INNER JOIN sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id
                 INNER JOIN sys.tables AS tp ON fkc.parent_object_id = tp.object_id
                 INNER JOIN sys.columns AS cp ON fkc.parent_object_id = cp.object_id AND fkc.parent_column_id = cp.column_id
                 INNER JOIN sys.tables AS tr ON fkc.referenced_object_id = tr.object_id
                 INNER JOIN sys.columns AS cr ON fkc.referenced_object_id = cr.object_id AND fkc.referenced_column_id = cr.column_id
                 WHERE tp.name IN ({tableFilter}) AND tr.name IN ({tableFilter})"
            : $@"SELECT tc.constraint_name, tc.table_name, kcu.column_name, ccu.table_name, ccu.column_name 
                 FROM information_schema.table_constraints AS tc 
                 JOIN information_schema.key_column_usage AS kcu ON tc.constraint_name = kcu.constraint_name
                 JOIN information_schema.constraint_column_usage AS ccu ON ccu.constraint_name = tc.constraint_name
                 WHERE tc.constraint_type = 'FOREIGN KEY' AND tc.table_name IN ({tableFilter}) AND ccu.table_name IN ({tableFilter})";

        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = fkQuery;
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var fkName = reader.GetString(0);
                var sourceTable = reader.GetString(1);
                var sourceCol = reader.GetString(2);
                var targetTable = reader.GetString(3);
                var targetCol = reader.GetString(4);

                model.Relations!.Add(new RelationModel
                {
                    Name = fkName,
                    ParentEntity = entities.ContainsKey(targetTable) ? entities[targetTable] : null,
                    ChiledEntity = entities.ContainsKey(sourceTable) ? entities[sourceTable] : null,
                    // We can store columns in Name or custom property if needed, 
                    // but for now we'll pass enough to reconstruct on frontend
                    Applied = true // Mark as extracted relation
                });
                
                // Track columns for mapping
                var rel = model.Relations.Last();
                rel.Name = $"{fkName}|{sourceCol}|{targetCol}"; 
            }
        }

        return model;
    }

    private DbConnection CreateConnection(string connectionString, string provider)
    {
        return provider.ToLower() switch
        {
            "sqlserver" => new SqlConnection(connectionString),
            "postgresql" => new NpgsqlConnection(connectionString),
            _ => throw new NotSupportedException($"Provider {provider} is not supported")
        };
    }
}
