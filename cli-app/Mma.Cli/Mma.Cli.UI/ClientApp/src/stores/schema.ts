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
      nodes.value = data.nodes || [];
      edges.value = data.edges || [];
      solutionName.value = data.solutionName || "";
      mapper.value = data.mapper || "AutoMapper";
    } catch (error) {
      console.error("Failed to load schema:", error);
    }
  }

  async function saveSchema() {
    isSaving.value = true;
    try {
      await fetch("/api/schema/save", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          path: cwd.value,
          schema: {
            nodes: nodes.value,
            edges: edges.value,
            solutionName: solutionName.value,
            mapper: mapper.value,
          },
        }),
      });
    } catch (error) {
      console.error("Failed to save schema:", error);
    } finally {
      isSaving.value = false;
    }
  }

  async function generateCode() {
    isGenerating.value = true;
    try {
      const response = await fetch("/api/schema/generate", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          path: cwd.value,
          schema: {
            nodes: nodes.value,
            edges: edges.value,
            solutionName: solutionName.value,
            mapper: mapper.value,
          },
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
          const id = (nodes.value.length + newNodes.length + 1).toString();
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
                name: r.columnName,
                type: r.dataType || "VARCHAR",
                isPk: r.columnName.toLowerCase() === "id", // Simple PK heuristic
                isNullable: r.nullable ?? true,
              })) || [
                { name: "id", type: "INT", isPk: true, isNullable: false },
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
