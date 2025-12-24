<script setup lang="ts">
import { ref, computed } from 'vue'
import { Check, X, Search, Database, ArrowLeft, Filter } from 'lucide-vue-next'
import { useSchemaStore } from '../stores/schema'

const emit = defineEmits(['close', 'back', 'import'])
const store = useSchemaStore()
const searchQuery = ref('')

const filteredTables = computed(() => {
  if (!searchQuery.value) return store.availableTables
  const query = searchQuery.value.toLowerCase()
  return store.availableTables.filter(t => t.toLowerCase().includes(query))
})

const toggleTable = (table: string) => {
  const index = store.selectedTables.indexOf(table)
  if (index === -1) {
    store.selectedTables.push(table)
  } else {
    store.selectedTables.splice(index, 1)
  }
}

const selectAll = () => {
  store.selectedTables = [...store.availableTables]
}

const deselectAll = () => {
  store.selectedTables = []
}

const handleImport = async () => {
  await store.importSelected()
  emit('import')
}
</script>

<template>
  <div class="fixed inset-0 z-[100] flex items-center justify-center p-4 bg-[#0b1121]/80 backdrop-blur-sm animate-in fade-in duration-300">
    <div class="w-full max-w-2xl bg-[#111827] border border-slate-800 rounded-2xl shadow-2xl flex flex-col overflow-hidden max-h-[85vh]">
      
      <!-- Header -->
      <div class="p-6 border-b border-slate-800 bg-slate-900/50">
        <div class="flex items-center justify-between mb-6">
          <div class="flex items-center gap-3">
            <div class="p-2 bg-emerald-500/10 rounded-xl">
              <Database class="w-5 h-5 text-emerald-500" />
            </div>
            <div>
              <h2 class="text-sm font-bold text-slate-100 uppercase tracking-widest">Select Tables to Import</h2>
              <p class="text-[10px] text-slate-500 font-medium mt-0.5">Found {{ store.availableTables.length }} tables in database</p>
            </div>
          </div>
          <button @click="$emit('close')" class="p-2 hover:bg-slate-800 rounded-lg text-slate-500 transition-colors">
            <X class="w-5 h-5" />
          </button>
        </div>

        <!-- Search & Helpers -->
        <div class="flex flex-col gap-4">
          <div class="relative group">
            <Search class="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-500 group-focus-within:text-emerald-500 transition-colors" />
            <input 
              v-model="searchQuery"
              placeholder="Search tables..."
              class="w-full bg-[#0b1121] border border-slate-800 rounded-xl pl-12 pr-4 py-3 text-sm focus:border-emerald-500/50 focus:ring-1 focus:ring-emerald-500/20 focus:outline-none transition-all placeholder-slate-600"
            />
          </div>

          <div class="flex items-center justify-between">
            <div class="flex gap-2">
              <button 
                @click="selectAll"
                class="text-[10px] font-bold text-slate-400 hover:text-emerald-500 uppercase tracking-widest transition-colors flex items-center gap-1.5"
              >
                Select All
              </button>
              <div class="w-[1px] h-3 bg-slate-800 self-center"></div>
              <button 
                @click="deselectAll"
                class="text-[10px] font-bold text-slate-400 hover:text-rose-500 uppercase tracking-widest transition-colors flex items-center gap-1.5"
              >
                Deselect All
              </button>
            </div>
            <span class="text-[10px] font-bold text-emerald-500 bg-emerald-500/10 px-2 py-0.5 rounded uppercase tracking-widest">
              Selected: {{ store.selectedTables.length }}
            </span>
          </div>
        </div>
      </div>

      <!-- Table List -->
      <div class="flex-1 overflow-y-auto p-4 bg-[#0b1121]/50 grid grid-cols-2 gap-2 content-start">
        <div 
          v-for="table in filteredTables" 
          :key="table"
          @click="toggleTable(table)"
          :class="[
            'p-3 rounded-xl border transition-all cursor-pointer flex items-center justify-between group',
            store.selectedTables.includes(table) 
              ? 'bg-emerald-500/5 border-emerald-500/30 ring-1 ring-emerald-500/20' 
              : 'bg-slate-900/30 border-slate-800 hover:border-slate-700'
          ]"
        >
          <div class="flex items-center gap-3">
             <div :class="['w-4 h-4 rounded border transition-colors flex items-center justify-center', store.selectedTables.includes(table) ? 'bg-emerald-500 border-emerald-500' : 'bg-slate-800 border-slate-700 group-hover:border-slate-500']">
               <Check v-if="store.selectedTables.includes(table)" class="w-2.5 h-2.5 text-emerald-950" />
             </div>
             <span :class="['text-xs font-medium', store.selectedTables.includes(table) ? 'text-slate-100' : 'text-slate-400']">{{ table }}</span>
          </div>
        </div>

        <div v-if="filteredTables.length === 0" class="col-span-2 py-12 flex flex-col items-center justify-center text-slate-600">
           <Filter class="w-12 h-12 opacity-20 mb-4" />
           <p class="text-xs font-medium">No tables match your search</p>
        </div>
      </div>

      <!-- Footer -->
      <div class="p-6 border-t border-slate-800 bg-slate-900/50 flex gap-3">
        <button 
          @click="$emit('back')"
          class="flex-1 py-3 px-4 rounded-xl border border-slate-800 text-slate-400 text-xs font-bold uppercase tracking-widest hover:bg-slate-800 transition-all flex items-center justify-center gap-2"
        >
          <ArrowLeft class="w-4 h-4" />
          Back
        </button>
        <button 
          @click="handleImport"
          :disabled="store.selectedTables.length === 0"
          class="flex-[2] py-3 px-4 rounded-xl bg-emerald-500 text-emerald-950 text-xs font-bold uppercase tracking-widest hover:bg-emerald-400 transition-all shadow-lg shadow-emerald-500/20 disabled:opacity-50 disabled:pointer-events-none flex items-center justify-center gap-2"
        >
          <Check class="w-4 h-4" />
          Import Selected Tables
        </button>
      </div>
    </div>
  </div>
</template>
