<script setup lang="ts">
import { ref, markRaw, onMounted } from 'vue'
import { VueFlow, useVueFlow } from '@vue-flow/core'
import { Background } from '@vue-flow/background'
import { Plus, Database, LayoutGrid, Save, RotateCcw, Trash2, Check, X, Link, FolderOpen, Settings, Zap } from 'lucide-vue-next'
import { useSchemaStore } from './stores/schema'
import dagre from 'dagre'
import TableNode from './components/TableNode.vue'
import DbConnectionModal from './components/DbConnectionModal.vue'
import TableSelectionModal from './components/TableSelectionModal.vue'
import CwdModal from './components/CwdModal.vue'
import ProjectSettingsModal from './components/ProjectSettingsModal.vue'

const store = useSchemaStore()
const nodeTypes = {
  table: markRaw(TableNode),
} as any

const { onConnect, onEdgeUpdate, onNodeClick, onEdgeClick, onPaneClick, zoomIn, zoomOut, fitView, removeNodes, removeEdges } = useVueFlow()

const selectedNode = ref<any>(null)
const selectedEdge = ref<any>(null)
const showDbModal = ref(false)
const showSelectionModal = ref(false)
const showCwdModal = ref(false)
const showSettingsModal = ref(false)
const activeTab = ref('Design')

const handleGenerate = async () => {
  const success = await store.generateCode()
  if (success) {
    alert('Code generated successfully!')
  } else {
    alert('Generation failed. Check console for details.')
  }
}

onNodeClick((event) => {
  selectedNode.value = event.node
  selectedEdge.value = null
})

onEdgeClick((event) => {
  selectedEdge.value = event.edge
  selectedNode.value = null
})

onPaneClick(() => {
  selectedNode.value = null
  selectedEdge.value = null
})

onEdgeUpdate(({ edge, connection }) => {
  const sourceColumn = connection.sourceHandle?.split('-')[0] || 'id'
  const targetColumn = connection.targetHandle?.split('-')[0] || 'id'
  const sourceTable = store.nodes.find(n => n.id === connection.source)?.data.name || 'Unknown'
  const targetTable = store.nodes.find(n => n.id === connection.target)?.data.name || 'Unknown'

  const edgeIndex = store.edges.findIndex(e => e.id === edge.id)
  if (edgeIndex !== -1) {
    const existingEdge = store.edges[edgeIndex] as any
    if (existingEdge) {
      store.edges[edgeIndex] = {
        ...existingEdge,
        source: connection.source,
        target: connection.target,
        sourceHandle: connection.sourceHandle,
        targetHandle: connection.targetHandle,
        data: { 
          ...existingEdge.data,
          sourceTable, 
          sourceColumn, 
          targetTable, 
          targetColumn 
        }
      }
    }
  }
})

onConnect((params) => {
  const sourceColumn = params.sourceHandle?.split('-')[0] || 'id'
  const targetColumn = params.targetHandle?.split('-')[0] || 'id'
  const sourceTable = store.nodes.find(n => n.id === params.source)?.data.name || 'Unknown'
  const targetTable = store.nodes.find(n => n.id === params.target)?.data.name || 'Unknown'

  const newEdge = {
    ...params,
    id: `e-${params.source}-${params.sourceHandle}-${params.target}-${params.targetHandle}`,
    type: 'default',
    animated: true,
    style: { stroke: '#10b981', strokeWidth: 2 },
    data: { sourceTable, sourceColumn, targetTable, targetColumn }
  }
  store.edges.push(newEdge)
})

const autoLayout = () => {
  const dagreGraph = new dagre.graphlib.Graph()
  dagreGraph.setDefaultEdgeLabel(() => ({}))
  dagreGraph.setGraph({ rankdir: 'LR', nodesep: 120, ranksep: 250 })

  store.nodes.forEach((node) => {
    const nodeWidth = 260
    const nodeHeight = 80 + (node.data.columns?.length || 0) * 44
    dagreGraph.setNode(node.id, { width: nodeWidth, height: nodeHeight })
  })

  store.edges.forEach((edge) => {
    dagreGraph.setEdge(edge.source, edge.target)
  })

  dagre.layout(dagreGraph)

  store.nodes = store.nodes.map((node) => {
    const nodeWithPosition = dagreGraph.node(node.id)
    const nodeHeight = 80 + (node.data.columns?.length || 0) * 44
    return {
      ...node,
      position: {
        x: nodeWithPosition.x - 130,
        y: nodeWithPosition.y - (nodeHeight / 2),
      },
    }
  })

  setTimeout(() => fitView(), 100)
}

const addTable = () => {
  const id = (store.nodes.length + 1).toString()
  const newNode = {
    id,
    type: 'table',
    label: `Table_${id}`,
    position: { x: Math.random() * 400 - 200, y: Math.random() * 400 - 200 },
    data: { 
      name: `Table_${id}`,
      columns: [{ name: 'id', type: 'INT', isPk: true, isNullable: false }],
      apiSettings: {
        generateController: true,
        generateService: true,
        operations: ['ReadList', 'ReadSingle', 'Create', 'Update', 'Delete']
      }
    }
  }
  store.nodes.push(newNode)
}

const addColumn = () => {
  if (!selectedNode.value) return
  if (!selectedNode.value.data.columns) selectedNode.value.data.columns = []
  selectedNode.value.data.columns.push({
    name: `column_${selectedNode.value.data.columns.length + 1}`,
    type: 'VARCHAR',
    isPk: false,
    isNullable: true
  })
}

const removeColumn = (index: number) => {
  if (!selectedNode.value) return
  selectedNode.value.data.columns.splice(index, 1)
}

const deleteSelectedTable = () => {
  if (!selectedNode.value) return
  removeNodes([selectedNode.value.id])
  selectedNode.value = null
}

const deleteSelectedEdge = () => {
  if (!selectedEdge.value) return
  removeEdges([selectedEdge.value.id])
  selectedEdge.value = null
}

const resetView = () => fitView()

onMounted(async () => {
  await store.fetchCwd()
  store.loadSchema()
})
</script>

<template>
  <div class="flex h-screen w-screen bg-[#0b1121] text-slate-200 overflow-hidden font-sans relative">
    
    <CwdModal v-if="showCwdModal" @close="showCwdModal = false" @select="showCwdModal = false" />
    <DbConnectionModal v-if="showDbModal" @close="showDbModal = false" @next="showDbModal = false; showSelectionModal = true" />
    <TableSelectionModal v-if="showSelectionModal" @close="showSelectionModal = false" @back="showSelectionModal = false; showDbModal = true" @import="showSelectionModal = false" />
    <ProjectSettingsModal v-if="showSettingsModal" @close="showSettingsModal = false" />

    <!-- Floating Top Toolbar -->
    <div class="absolute top-6 left-1/2 -translate-x-1/2 z-50">
      <div class="bg-[#151c2e]/80 backdrop-blur-md border border-slate-700/50 rounded-xl px-2 py-1.5 flex items-center gap-1 shadow-2xl">
        <button @click="addTable" class="bg-[#10b981] hover:bg-[#059669] text-white px-3 py-1.5 rounded-lg text-sm font-semibold flex items-center gap-2 transition-all mr-2">
          <Plus class="w-4 h-4" />
          Table
        </button>

        <div class="flex items-center">
          <button @click="showCwdModal = true" class="p-2 hover:bg-slate-700/50 rounded-lg text-slate-400 hover:text-white transition-colors flex items-center gap-2 px-3 group">
            <FolderOpen class="w-4 h-4 text-amber-500/80" />
            <span class="text-sm font-medium">{{ store.cwd ? store.cwd.split('\\').pop() : 'SchemaForge' }}</span>
          </button>
          
          <button @click="showDbModal = true" class="p-2 hover:bg-slate-700/50 rounded-lg text-slate-400 hover:text-white transition-colors flex items-center gap-2 px-3">
            <Database class="w-4 h-4 text-emerald-500/80" />
            <span class="text-sm font-medium">DB Migration</span>
          </button>

          <button @click="autoLayout" class="p-2 hover:bg-slate-700/50 rounded-lg text-slate-400 hover:text-white transition-colors flex items-center gap-2 px-3">
            <LayoutGrid class="w-4 h-4 text-indigo-400/80" />
            <span class="text-sm font-medium">Layout</span>
          </button>

          <button @click="showSettingsModal = true" class="p-2 hover:bg-slate-700/50 rounded-lg text-slate-400 hover:text-white transition-colors flex items-center gap-2 px-3">
            <Settings class="w-4 h-4 text-slate-500" />
            <span class="text-sm font-medium">Settings</span>
          </button>

          <button @click="store.saveSchema" :disabled="store.isSaving" class="p-2 hover:bg-slate-700/50 rounded-lg text-slate-400 hover:text-white transition-colors flex items-center gap-2 px-3 disabled:opacity-50 border-r border-slate-700/50">
            <RotateCcw v-if="store.isSaving" class="w-4 h-4 animate-spin" />
            <Save v-else class="w-4 h-4 text-emerald-500/80" />
            <span class="text-sm font-medium">{{ store.isSaving ? 'Saving...' : 'Save' }}</span>
          </button>
        </div>

        <button 
          @click="handleGenerate" 
          :disabled="store.isGenerating"
          class="ml-2 bg-gradient-to-r from-emerald-600 to-teal-600 hover:from-emerald-500 hover:to-teal-500 text-white px-4 py-1.5 rounded-lg text-sm font-bold flex items-center gap-2 transition-all shadow-lg shadow-emerald-500/10 active:scale-95 disabled:opacity-50 disabled:grayscale"
        >
          <Zap v-if="!store.isGenerating" class="w-4 h-4 fill-emerald-200/20" />
          <RotateCcw v-else class="w-4 h-4 animate-spin" />
          {{ store.isGenerating ? 'Generating...' : 'Generate' }}
        </button>

        <div class="h-6 w-[1px] bg-slate-700/50 mx-2"></div>

        <div class="flex items-center gap-3 px-2">
          <label class="relative inline-flex items-center cursor-pointer scale-90">
            <input type="checkbox" v-model="store.autoSave" class="sr-only peer">
            <div class="w-9 h-5 bg-slate-700 peer-focus:outline-none rounded-full peer peer-checked:after:translate-x-full after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:rounded-full after:h-4 after:w-4 after:transition-all peer-checked:bg-emerald-600"></div>
            <span class="ml-2 text-[10px] font-bold text-slate-500 uppercase tracking-widest">Auto-Save</span>
          </label>
        </div>
      </div>
    </div>

    <div class="flex-1 relative overflow-hidden">
      <VueFlow 
        v-model:nodes="store.nodes" 
        v-model:edges="store.edges" 
        :node-types="nodeTypes" 
        :fit-view-on-init="true" 
        :updatable="true"
        class="bg-[#0b1121]"
      >
        <Background pattern-color="#1e293b" :gap="20" :size="0.5" />
      </VueFlow>
    </div>

    <!-- Zoom Controls -->
    <div class="absolute bottom-8 left-1/2 -translate-x-1/2 z-50">
      <div class="bg-[#151c2e]/90 backdrop-blur-md border border-slate-700/50 rounded-lg flex items-center shadow-2xl p-0.5">
        <button @click="() => zoomIn()" class="p-2 hover:bg-slate-700/50 rounded-md text-slate-400 hover:text-white transition-colors px-3"><Plus class="w-4 h-4" /></button>
        <div class="h-4 w-[1px] bg-slate-700/50"></div>
        <span class="px-4 text-[11px] font-bold text-slate-400 min-w-[60px] text-center uppercase tracking-widest">100%</span>
        <div class="h-4 w-[1px] bg-slate-700/50"></div>
        <button @click="() => zoomOut()" class="p-2 hover:bg-slate-700/50 rounded-md text-slate-400 hover:text-white transition-colors px-3 text-lg leading-none">-</button>
        <div class="h-4 w-[1px] bg-slate-700/50 mx-1"></div>
        <button @click="resetView" class="bg-[#1e293b] hover:bg-slate-700 text-slate-300 px-4 py-1.5 rounded text-[10px] font-bold uppercase tracking-wider transition-colors border border-slate-700/50">Reset</button>
      </div>
    </div>

    <!-- Properties Panel -->
    <Transition name="slide">
      <div v-if="selectedNode || selectedEdge" class="absolute top-6 right-6 bottom-6 w-96 bg-[#111827] border border-slate-800 rounded-2xl shadow-2xl z-[60] flex flex-col overflow-hidden">
        
        <!-- --- TABLE PROPERTIES --- -->
        <template v-if="selectedNode">
          <div class="flex border-b border-slate-800">
            <button v-for="tab in ['Design', 'CodeGen']" :key="tab" @click="activeTab = tab" :class="['flex-1 py-4 text-xs font-bold uppercase tracking-widest transition-all relative', activeTab === tab ? 'text-emerald-500' : 'text-slate-500 hover:text-slate-300']">
              {{ tab }}
              <div v-if="activeTab === tab" class="absolute bottom-0 left-4 right-4 h-0.5 bg-emerald-500 rounded-t-full"></div>
            </button>
          </div>

          <div class="flex-1 overflow-y-auto p-6 space-y-8 scrollbar-thin scrollbar-thumb-slate-800">
            <div v-if="activeTab === 'Design'" class="space-y-8 animate-in fade-in duration-300">
               <div>
                 <label class="text-[10px] font-bold text-emerald-500 uppercase tracking-widest block mb-3">Table Properties</label>
                 <input v-model="selectedNode.data.name" class="w-full bg-[#0b1121] border border-slate-800 rounded-xl px-4 py-3 text-sm focus:border-emerald-500/50 focus:ring-1 focus:ring-emerald-500/20 focus:outline-none transition-all placeholder-slate-600" placeholder="Table Name"/>
               </div>

               <div>
                 <div class="flex items-center justify-between mb-4">
                   <label class="text-[10px] font-bold text-emerald-500 uppercase tracking-widest">Columns</label>
                   <button @click="addColumn" class="bg-emerald-500 hover:bg-emerald-400 text-emerald-950 px-3 py-1 rounded-lg text-[10px] font-bold flex items-center gap-1.5 transition-all shadow-lg shadow-emerald-500/10">
                     <Plus class="w-3.5 h-3.5" /> Add
                   </button>
                 </div>
                 <div class="space-y-3">
                   <div v-for="(col, index) in selectedNode.data.columns" :key="index" class="p-4 bg-[#1f2937]/50 border border-slate-800 rounded-xl space-y-3 group hover:border-slate-700 transition-colors">
                     <div class="flex items-center gap-2">
                       <input v-model="col.name" class="flex-1 bg-transparent border-none p-0 text-sm font-semibold focus:ring-0 placeholder-slate-600" placeholder="Column Name"/>
                       <div class="flex gap-1">
                         <button @click="col.isPk = !col.isPk" :class="['px-1.5 py-0.5 rounded text-[9px] font-bold border transition-all', col.isPk ? 'bg-emerald-500 text-emerald-950 border-emerald-500' : 'bg-slate-800 text-slate-500 border-slate-700']">PK</button>
                         <button @click="col.isNullable = !col.isNullable" :class="['px-1.5 py-0.5 rounded text-[9px] font-bold border transition-all', !col.isNullable ? 'bg-indigo-500 text-white border-indigo-500' : 'bg-slate-800 text-slate-500 border-slate-700']">NN</button>
                       </div>
                     </div>
                     <div class="flex items-center gap-2">
                       <select v-model="col.type" class="flex-1 bg-[#0b1121] border border-slate-800 rounded-lg px-2 py-1.5 text-[11px] focus:border-emerald-500/50 focus:outline-none">
                         <option v-for="t in store.columnTypeMap" :key="t.value" :value="t.value">{{ t.name }}</option>
                       </select>
                       <button @click="removeColumn(index as number)" class="p-1.5 text-slate-600 hover:text-rose-500 hover:bg-rose-500/10 rounded-lg transition-all"><X class="w-3.5 h-3.5" /></button>
                     </div>
                   </div>
                 </div>
               </div>
            </div>

            <div v-if="activeTab === 'CodeGen'" class="space-y-8 animate-in fade-in duration-300">
               <div>
                 <label class="text-[10px] font-bold text-emerald-500 uppercase tracking-widest block mb-4">Controller Settings</label>
                 <div class="flex items-center justify-between p-4 bg-[#1f2937]/30 border border-slate-800 rounded-xl">
                    <span class="text-sm font-medium text-slate-300">Generate API Controller?</span>
                    <button @click="selectedNode.data.apiSettings.generateController = !selectedNode.data.apiSettings.generateController" :class="['w-10 h-5 rounded-full transition-all relative', selectedNode.data.apiSettings.generateController ? 'bg-emerald-500' : 'bg-slate-700']">
                      <div :class="['absolute top-1 w-3 h-3 bg-white rounded-full transition-all', selectedNode.data.apiSettings.generateController ? 'left-6' : 'left-1']"></div>
                    </button>
                 </div>
               </div>
               <div>
                 <label class="text-[10px] font-bold text-emerald-500 uppercase tracking-widest block mb-4">Service Settings</label>
                 <div class="space-y-3">
                   <div class="flex items-center justify-between p-4 bg-[#1f2937]/30 border border-slate-800 rounded-xl">
                      <span class="text-sm font-medium text-slate-300">Generate API Service?</span>
                      <button @click="selectedNode.data.apiSettings.generateService = !selectedNode.data.apiSettings.generateService" :class="['w-10 h-5 rounded-full transition-all relative', selectedNode.data.apiSettings.generateService ? 'bg-emerald-500' : 'bg-slate-700']">
                        <div :class="['absolute top-1 w-3 h-3 bg-white rounded-full transition-all', selectedNode.data.apiSettings.generateService ? 'left-6' : 'left-1']"></div>
                      </button>
                   </div>
                   <div class="pl-4 space-y-3">
                     <div v-for="op in ['ReadList', 'ReadSingle', 'Create', 'Update', 'Delete']" :key="op" class="flex items-center gap-3 group cursor-pointer">
                       <Check class="w-3 h-3 text-emerald-500" />
                       <span class="text-[13px] text-slate-400 font-medium">{{ op }}</span>
                     </div>
                   </div>
                 </div>
               </div>
            </div>
          </div>

          <div class="p-6 border-t border-slate-800 bg-[#111827]/50 mt-auto">
            <button @click="deleteSelectedTable" class="w-full flex items-center justify-center gap-2 py-3 px-4 rounded-xl border border-rose-500/30 text-rose-500 text-xs font-bold uppercase tracking-widest hover:bg-rose-500 hover:text-white transition-all duration-300 group">
              <Trash2 class="w-4 h-4 group-hover:animate-bounce" /> Delete Table
            </button>
          </div>
        </template>

        <!-- --- RELATIONSHIP PROPERTIES --- -->
        <template v-else-if="selectedEdge">
          <div class="p-6 border-b border-slate-800 flex items-center justify-between">
            <h2 class="text-xs font-bold text-emerald-500 uppercase tracking-widest flex items-center gap-2">
              <Link class="w-4 h-4" /> Relationship Properties
            </h2>
          </div>
          <div class="flex-1 overflow-y-auto p-6 space-y-8 animate-in fade-in duration-300">
            <div>
              <label class="text-[10px] font-bold text-emerald-500 uppercase tracking-widest block mb-2">Source: {{ selectedEdge.data.sourceTable }}</label>
              <div class="bg-[#0b1121] border border-slate-800 rounded-xl px-4 py-3 text-sm text-slate-300 font-mono">{{ selectedEdge.data.sourceColumn }}</div>
            </div>
            <div>
              <label class="text-[10px] font-bold text-emerald-500 uppercase tracking-widest block mb-2">Target: {{ selectedEdge.data.targetTable }}</label>
              <div class="bg-[#0b1121] border border-slate-800 rounded-xl px-4 py-3 text-sm text-slate-300 font-mono">{{ selectedEdge.data.targetColumn }}</div>
            </div>
          </div>
          <div class="p-6 border-t border-slate-800 bg-[#111827]/50 mt-auto">
            <button @click="deleteSelectedEdge" class="w-full flex items-center justify-center gap-2 py-3 px-4 rounded-xl border border-rose-500/30 text-rose-500 text-xs font-bold uppercase tracking-widest hover:bg-rose-500 hover:text-white transition-all duration-300 group">
              <Trash2 class="w-4 h-4 group-hover:animate-bounce" /> Delete Relationship
            </button>
          </div>
        </template>
      </div>
    </Transition>
  </div>
</template>

<style>
@import '@vue-flow/core/dist/style.css';
@import '@vue-flow/core/dist/theme-default.css';

.vue-flow__node-table { padding: 0 !important; border: none !important; background: transparent !important; }
.vue-flow__handle { width: 10px !important; height: 10px !important; background-color: #10b981 !important; border: 4px solid #0b1121 !important; transition: all 0.2s cubic-bezier(0.175, 0.885, 0.32, 1.275); }
.vue-flow__handle:hover { transform: scale(1.4); background-color: #34d399 !important; box-shadow: 0 0 10px rgba(16, 185, 129, 0.5); }
.vue-flow__edge { cursor: pointer; }
.vue-flow__edge.selected .vue-flow__edge-path { stroke: #10b981 !important; stroke-width: 4; filter: drop-shadow(0 0 5px rgba(16, 185, 129, 0.5)); }
.vue-flow__edge-path { stroke: #1e293b; stroke-width: 3; transition: all 0.2s ease; }
.vue-flow__edge:hover .vue-flow__edge-path { stroke: #334155; }
.slide-enter-active, .slide-leave-active { transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1); }
.slide-enter-from { transform: translateX(100%); opacity: 0; }
.slide-leave-to { transform: translateX(100%); opacity: 0; }
.scrollbar-thin::-webkit-scrollbar { width: 4px; }
.scrollbar-thin::-webkit-scrollbar-thumb { background: #334155; border-radius: 20px; }
</style>
