<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { Folder, FolderOpen, ArrowLeft, X, Check, ChevronRight, Plus, Hash } from 'lucide-vue-next'
import { useSchemaStore } from '../stores/schema'

const emit = defineEmits(['close', 'select'])
const store = useSchemaStore()

const editablePath = ref('')
const newFolderName = ref('')
const isCreatingFolder = ref(false)

const navigateTo = (path: string) => {
  store.browseDirectories(path)
  editablePath.value = path
}

const handlePathEdit = () => {
  if (editablePath.value) {
    store.browseDirectories(editablePath.value)
  }
}

const toggleCreateFolder = () => {
  isCreatingFolder.value = !isCreatingFolder.value
  newFolderName.value = ''
}

const handleCreateFolder = async () => {
  if (newFolderName.value) {
    const success = await store.createDirectory(newFolderName.value)
    if (success) {
      isCreatingFolder.value = false
      newFolderName.value = ''
    }
  }
}

const selectCurrent = () => {
  store.cwd = store.currentBrowserPath
  emit('select', store.cwd)
}

onMounted(async () => {
  await store.browseDirectories()
  editablePath.value = store.currentBrowserPath
})
</script>

<template>
  <div class="fixed inset-0 z-[100] flex items-center justify-center p-4 bg-[#0b1121]/80 backdrop-blur-sm animate-in fade-in duration-300">
    <div class="w-full max-w-2xl bg-[#111827] border border-slate-800 rounded-2xl shadow-2xl flex flex-col overflow-hidden max-h-[80vh]">
      
      <!-- Header -->
      <div class="p-6 border-b border-slate-800 flex items-center justify-between bg-slate-900/50">
        <div class="flex items-center gap-3">
          <div class="p-2 bg-emerald-500/10 rounded-xl">
            <FolderOpen class="w-5 h-5 text-emerald-500" />
          </div>
          <div>
            <h2 class="text-sm font-bold text-slate-100 uppercase tracking-widest">Select Working Directory</h2>
            <p class="text-[10px] text-slate-500 font-medium mt-0.5">Choose your project destination</p>
          </div>
        </div>
        <button @click="$emit('close')" class="p-2 hover:bg-slate-800 rounded-lg text-slate-500 transition-colors">
          <X class="w-5 h-5" />
        </button>
      </div>

      <!-- Navigation Bar (Editable Path) -->
      <div class="px-6 py-4 bg-slate-900/30 border-b border-slate-800 flex flex-col gap-3">
        <div class="flex items-center gap-2">
          <button 
            @click="navigateTo(store.parentBrowserPath!)" 
            :disabled="!store.parentBrowserPath"
            class="p-2 bg-slate-800/50 hover:bg-slate-800 rounded-xl text-slate-400 disabled:opacity-30 disabled:pointer-events-none transition-all group"
          >
            <ArrowLeft class="w-4 h-4 group-hover:-translate-x-0.5 transition-transform" />
          </button>
          
          <div class="flex-1 relative group">
            <Hash class="absolute left-3.5 top-1/2 -translate-y-1/2 w-3.5 h-3.5 text-slate-600 group-focus-within:text-emerald-500 transition-colors" />
            <input 
              v-model="editablePath"
              @keydown.enter="handlePathEdit"
              placeholder="Paste or edit folder path..."
              class="w-full bg-[#0b1121] border border-slate-800 rounded-xl pl-10 pr-4 py-2.5 text-xs font-mono text-slate-400 focus:text-slate-100 focus:border-emerald-500/50 focus:ring-1 focus:ring-emerald-500/20 focus:outline-none transition-all"
            />
          </div>

          <button 
            @click="toggleCreateFolder"
            :class="['p-2.5 rounded-xl border transition-all flex items-center gap-2 group', isCreatingFolder ? 'bg-indigo-500/10 border-indigo-500/30 text-indigo-400' : 'bg-slate-800/50 border-slate-800 text-slate-400 hover:text-white hover:border-slate-700']"
            title="Create New Folder"
          >
            <Plus class="w-4 h-4 group-hover:rotate-90 transition-transform" />
          </button>
        </div>

        <!-- Create Folder Inline UI -->
        <div v-if="isCreatingFolder" class="flex items-center gap-2 animate-in slide-in-from-top-2 duration-300">
          <div class="flex-1 relative">
            <input 
              v-model="newFolderName"
              @keydown.enter="handleCreateFolder"
              @keydown.esc="isCreatingFolder = false"
              placeholder="Folder Name"
              class="w-full bg-indigo-500/5 border border-indigo-500/30 rounded-lg px-4 py-2 text-xs text-indigo-100 focus:outline-none focus:ring-1 focus:ring-indigo-500/30"
              autoFocus
            />
          </div>
          <button 
            @click="handleCreateFolder"
            class="p-2 bg-indigo-500 hover:bg-indigo-400 text-white rounded-lg transition-colors"
          >
            <Check class="w-4 h-4" />
          </button>
          <button 
            @click="isCreatingFolder = false"
            class="p-2 bg-slate-800 hover:bg-slate-700 text-slate-400 rounded-lg transition-colors"
          >
            <X class="w-4 h-4" />
          </button>
        </div>
      </div>

      <!-- Directories List -->
      <div class="flex-1 overflow-y-auto p-4 space-y-1 bg-[#0b1121]/50 scrollbar-thin scrollbar-thumb-slate-800">
        <div 
          v-for="dir in store.browserDirectories" 
          :key="dir.path"
          @click="navigateTo(dir.path)"
          class="flex items-center justify-between p-3 hover:bg-slate-800/50 rounded-xl cursor-pointer group transition-all"
        >
          <div class="flex items-center gap-3">
            <div class="w-8 h-8 rounded-lg bg-slate-800/30 flex items-center justify-center group-hover:bg-emerald-500/10 transition-colors">
              <Folder class="w-4 h-4 text-slate-500 group-hover:text-emerald-500 transition-colors" />
            </div>
            <span class="text-xs font-medium text-slate-300 group-hover:text-white transition-colors">{{ dir.name }}</span>
          </div>
          <ChevronRight class="w-4 h-4 text-slate-800 group-hover:text-slate-600 transition-colors" />
        </div>

        <div v-if="store.browserDirectories.length === 0" class="flex flex-col items-center justify-center py-16 text-slate-600">
           <Folder class="w-12 h-12 opacity-10 mb-4" />
           <p class="text-xs font-medium opacity-40 uppercase tracking-widest">No Subdirectories</p>
        </div>
      </div>

      <!-- Footer -->
      <div class="p-6 border-t border-slate-800 bg-slate-900/50 flex gap-4">
        <button 
          @click="$emit('close')"
          class="flex-1 py-3 px-4 rounded-xl border border-slate-800 text-slate-500 text-xs font-bold uppercase tracking-widest hover:bg-slate-800 hover:text-slate-300 transition-all"
        >
          Cancel
        </button>
        <button 
          @click="selectCurrent"
          class="flex-[2] py-3 px-4 rounded-xl bg-emerald-500 text-emerald-950 text-xs font-bold uppercase tracking-widest hover:bg-emerald-400 transition-all shadow-lg shadow-emerald-500/20 flex items-center justify-center gap-2"
        >
          <Check class="w-4 h-4" />
          Select Current Folder
        </button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.scrollbar-thin::-webkit-scrollbar {
  width: 4px;
}
.scrollbar-thin::-webkit-scrollbar-thumb {
  background: #1e293b;
  border-radius: 20px;
}
</style>
