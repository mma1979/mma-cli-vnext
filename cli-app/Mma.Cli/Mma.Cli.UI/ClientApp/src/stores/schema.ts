import { defineStore } from "pinia";
import { ref, watch } from "vue";
import { type Node, type Edge } from "@vue-flow/core";

export const useSchemaStore = defineStore("schema", () => {
  const nodes = ref<Node[]>([]);
  const edges = ref<Edge[]>([]);
  const isSaving = ref(false);
  const isGenerating = ref(false);
  const autoSave = ref(false);

  const solutionName = ref("");
  const mapper = ref("AutoMapper");
  const cwd = ref("");

  // Watch for changes and auto-save if enabled
  watch(
    [nodes, edges, solutionName, mapper],
    () => {
      if (autoSave.value) {
        saveSchema();
      }
    },
    { deep: true }
  );

  const currentBrowserPath = ref("");
  const browserDirectories = ref<any[]>([]);
  const parentBrowserPath = ref<string | null>(null);

  const dbConfig = ref({
    connectionString: "",
    provider: "sqlserver",
  });

  async function fetchCwd() {
    const res = await fetch("/api/schema/project/cwd");
    if (res.ok) {
      const data = await res.json();
      cwd.value = data.cwd;
    }
  }

  async function browseDirectories(path?: string) {
    const url = path
      ? `/api/schema/project/directories?path=${encodeURIComponent(path)}`
      : "/api/schema/project/directories";
    const res = await fetch(url);
    if (res.ok) {
      const data = await res.json();
      currentBrowserPath.value = data.currentPath;
      browserDirectories.value = data.directories;
      parentBrowserPath.value = data.parentPath;
    }
  }

  async function createDirectory(name: string) {
    const newPath = currentBrowserPath.value + "\\" + name;
    const res = await fetch("/api/schema/project/create-directory", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ path: newPath }),
    });

    if (res.ok) {
      await browseDirectories(currentBrowserPath.value);
      return true;
    }
    return false;
  }

  const availableTables = ref<string[]>([]);
  const selectedTables = ref<string[]>([]);

  async function loadSchema() {
    try {
      const url = cwd.value
        ? `/api/schema/load?path=${encodeURIComponent(cwd.value)}`
        : "/api/schema/load";
      const response = await fetch(url);
      const data = await response.json();
      
      if (data.Tables) {
        nodes.value = data.Tables.map((t: any) => ({
          id: t.Id,
          type: "table",
          label: t.Name,
          position: { x: t.X, y: t.Y },
          data: {
            name: t.Name,
            columns: t.Columns.map((c: any) => ({
              id: c.Id,
              name: c.Name,
              type: c.Type,
              isPk: c.IsPrimaryKey,
              isNullable: c.IsNullable,
              isNotNull: c.IsNotNull
            })),
            apiSettings: {
              generateController: t.GenSettings.GenerateController,
              generateService: t.GenSettings.GenerateService,
              operations: [
                t.GenSettings.AllowRead ? 'ReadList' : null,
                t.GenSettings.AllowReadById ? 'ReadSingle' : null,
                t.GenSettings.AllowCreate ? 'Create' : null,
                t.GenSettings.AllowUpdate ? 'Update' : null,
                t.GenSettings.AllowDelete ? 'Delete' : null
              ].filter(Boolean)
            }
          }
        }));
      } else {
        nodes.value = [];
      }

      if (data.Relationships) {
        edges.value = data.Relationships.map((r: any) => ({
          id: r.Id,
          source: r.SourceTableId,
          target: r.TargetTableId,
          sourceHandle: `${r.SourceColumnId}-right`,
          targetHandle: `${r.TargetColumnId}-left`,
          type: "default",
          animated: true,
          style: { stroke: "#10b981", strokeWidth: 2 }
        }));
      } else {
        edges.value = [];
      }

      solutionName.value = data.solutionName || ""; // Keep if still relevant
      mapper.value = data.mapper || "AutoMapper";
    } catch (error) {
      console.error("Failed to load schema:", error);
    }
  }

  async function saveSchema() {
    isSaving.value = true;
    try {
      const schema = getSimplifiedSchema();

      await fetch("/api/schema/save", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          path: cwd.value,
          schema: schema
        }),
      });
    } catch (error) {
      console.error("Failed to save schema:", error);
    } finally {
      isSaving.value = false;
    }
  }

  function getSimplifiedSchema() {
    return {
      Tables: nodes.value.map(n => ({
        Id: n.id,
        Name: n.data.name,
        X: n.position.x,
        Y: n.position.y,
        Columns: n.data.columns.map((c: any) => ({
          Id: c.id || Math.random().toString(36).substr(2, 9),
          Name: c.name,
          Type: c.type,
          IsPrimaryKey: c.isPk,
          IsNullable: c.isNullable,
          IsNotNull: !c.isNullable
        })),
        GenSettings: {
          GenerateController: n.data.apiSettings.generateController,
          GenerateService: n.data.apiSettings.generateService,
          AllowRead: n.data.apiSettings.operations.includes('ReadList'),
          AllowReadById: n.data.apiSettings.operations.includes('ReadSingle'),
          AllowCreate: n.data.apiSettings.operations.includes('Create'),
          AllowUpdate: n.data.apiSettings.operations.includes('Update'),
          AllowDelete: n.data.apiSettings.operations.includes('Delete')
        }
      })),
      Relationships: edges.value.map(e => ({
        Id: e.id,
        SourceTableId: e.source,
        SourceColumnId: e.sourceHandle?.split('-')[0],
        TargetTableId: e.target,
        TargetColumnId: e.targetHandle?.split('-')[0]
      }))
    };
  }

  async function generateCode() {
    isGenerating.value = true;
    try {
      const response = await fetch("/api/schema/generate", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          path: cwd.value,
          schema: getSimplifiedSchema(),
        }),
      });
      if (!response.ok) throw new Error("Generation failed");
      return true;
    } catch (error) {
      console.error("Failed to generate code:", error);
      return false;
    } finally {
      isGenerating.value = false;
    }
  }

  async function createSolution() {
    isGenerating.value = true;
    try {
      const response = await fetch("/api/schema/project/create", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          solutionName: solutionName.value,
          mapper: mapper.value,
          path: cwd.value,
        }),
      });
      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || "Solution creation failed");
      }
      const data = await response.json();
      if (data.path) {
        cwd.value = data.path;
      }
      return { success: true };
    } catch (error: any) {
      console.error("Failed to create solution:", error);
      return { success: false, error: error.message };
    } finally {
      isGenerating.value = false;
    }
  }

  async function testConnection() {
    const res = await fetch("/api/schema/database/test", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(dbConfig.value),
    });
    return res.ok;
  }

  async function fetchTables() {
    const res = await fetch("/api/schema/database/tables", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(dbConfig.value),
    });
    if (res.ok) {
      availableTables.value = await res.json();
    }
  }

  async function importSelected() {
    const res = await fetch("/api/schema/database/import", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        ...dbConfig.value,
        tableNames: selectedTables.value,
      }),
    });
    if (res.ok) {
      const data = await res.json();
      const newNodes: Node[] = [];
      const tableToIdMap: Record<string, string> = {};

      if (data.entities) {
        data.entities.forEach((entity: any, index: number) => {
          const id = Math.random().toString(36).substr(2, 9);
          const tableName = entity.entityName || entity.name;
          tableToIdMap[tableName] = id;

          newNodes.push({
            id,
            type: "table",
            label: tableName,
            position: { x: index * 300, y: 100 },
            data: {
              name: tableName,
              columns: entity.rows?.map((r: any) => ({
                id: Math.random().toString(36).substr(2, 9),
                name: r.columnName,
                type: r.dataType || "VARCHAR",
                isPk: r.columnName.toLowerCase() === "id", // Simple PK heuristic
                isNullable: r.nullable ?? true,
                isNotNull: !(r.nullable ?? true)
              })) || [
                { id: Math.random().toString(36).substr(2, 9), name: "id", type: "INT", isPk: true, isNullable: false, isNotNull: true },
              ],
              apiSettings: {
                generateController: true,
                generateService: true,
                operations: [
                  "ReadList",
                  "ReadSingle",
                  "Create",
                  "Update",
                  "Delete",
                ],
              },
            },
          });
        });
        nodes.value.push(...newNodes);
      }

      if (data.relations) {
        data.relations.forEach((rel: any) => {
          const [name, fkCol, pkCol] = rel.name.split("|");
          const sourceTable = rel.parentEntity?.entityName; // Parent (PK)
          const targetTable = rel.chiledEntity?.entityName; // Child (FK)

          const sourceId = tableToIdMap[sourceTable];
          const targetId = tableToIdMap[targetTable];

          if (sourceId && targetId) {
            edges.value.push({
              id: `e-${sourceId}-${pkCol}-${targetId}-${fkCol}`,
              source: sourceId,
              target: targetId,
              sourceHandle: `${pkCol}-right`,
              targetHandle: `${fkCol}-left`,
              type: "default",
              animated: true,
              style: { stroke: "#10b981", strokeWidth: 2 },
              data: {
                name,
                sourceTable,
                sourceColumn: pkCol,
                targetTable,
                targetColumn: fkCol,
              },
            });
          }
        });
      }
    }
  }

  return {
    nodes,
    edges,
    isSaving,
    autoSave,
    dbConfig,
    availableTables,
    selectedTables,
    cwd,
    currentBrowserPath,
    browserDirectories,
    parentBrowserPath,
    loadSchema,
    saveSchema,
    testConnection,
    fetchTables,
    importSelected,
    fetchCwd,
    browseDirectories,
    createDirectory,
    solutionName,
    mapper,
    isGenerating,
    generateCode,
    createSolution,
    columnTypeMap: [
      { value: "bigint", name: "long" },
      { value: "int", name: "int" },
      { value: "smallint", name: "short" },
      { value: "tinyint", name: "byte" },
      { value: "bit", name: "bool" },
      { value: "decimal", name: "decimal" },
      { value: "float", name: "double" },
      { value: "date", name: "DateOnly" },
      { value: "datetime", name: "DateTime" },
      { value: "datetime2", name: "DateTime" },
      { value: "smalldatetime", name: "DateTime" },
      { value: "nvarchar", name: "string" },
      { value: "varchar", name: "string" },
      { value: "text", name: "string" },
      { value: "uniqueidentifier", name: "Guid" },
    ],
  };
});
