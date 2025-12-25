<script setup lang="ts">
import { ref } from 'vue'
import { X, Settings, Check, ChevronDown, Search } from 'lucide-vue-next'
import { useSchemaStore } from '../stores/schema'

const emit = defineEmits(['close'])
const store = useSchemaStore()

const mappers = ['AutoMapper', 'Mapster']
const showMapperDropdown = ref(false)
const searchQuery = ref('')

const selectMapper = (m: string) => {
  store.mapper = m
  showMapperDropdown.value = false
}

const handleSave = async () => {
  if (!store.solutionName) {
    alert('Please enter a solution name')
    return
  }
  
  const result = await store.createSolution()
  if (result.success) {
    emit('close')
  } else {
    alert(`Error: ${result.error}`)
  }
}
</script>

<template>
  <div class="fixed inset-0 z-[100] flex items-center justify-center p-4">
    <!-- Backdrop -->
    <div class="absolute inset-0 bg-slate-950/80 backdrop-blur-sm" @click="emit('close')"></div>

    <!-- Modal Content -->
    <div class="relative w-full max-w-md bg-[#111827] border border-slate-800 rounded-2xl shadow-2xl animate-in zoom-in-95 duration-200">
      
      <!-- Header -->
      <div class="flex items-center justify-between p-6 border-b border-slate-800 bg-slate-900/50 rounded-t-2xl">
        <h2 class="text-lg font-bold text-white flex items-center gap-3">
          <Settings class="w-5 h-5 text-emerald-500" />
          Project Settings
        </h2>
        <button @click="emit('close')" class="p-2 text-slate-500 hover:text-white hover:bg-slate-800 rounded-lg transition-all">
          <X class="w-5 h-5" />
        </button>
      </div>

      <!-- Body -->
      <div class="p-8 space-y-8">
        <!-- Solution Name -->
        <div class="space-y-3">
          <label class="text-[10px] font-bold text-emerald-500 uppercase tracking-[0.2em] block">
            Solution Name
          </label>
          <div class="relative group">
            <input 
              v-model="store.solutionName"
              type="text" 
              placeholder="e.g. MyProject.Solution"
              class="w-full bg-[#0b1121] border border-slate-800 rounded-xl px-5 py-4 text-sm text-white focus:border-emerald-500/50 focus:ring-4 focus:ring-emerald-500/10 focus:outline-none transition-all placeholder-slate-600 font-medium"
            />
          </div>
          <p class="text-[11px] text-slate-500 italic px-1">Used as the base namespace for generated code.</p>
        </div>

        <!-- Mapper Package -->
        <div class="space-y-3">
          <label class="text-[10px] font-bold text-emerald-500 uppercase tracking-[0.2em] block">
            Mapper Package
          </label>
          <div class="relative">
            <button 
              @click="showMapperDropdown = !showMapperDropdown"
              class="w-full flex items-center justify-between bg-[#0b1121] border border-slate-800 rounded-xl px-5 py-4 text-sm text-white hover:border-slate-700 focus:border-emerald-500/50 transition-all outline-none"
            >
              <div class="flex items-center gap-3">
                <div class="w-2 h-2 rounded-full bg-emerald-500 shadow-[0_0_8px_rgba(16,185,129,0.5)]"></div>
                <span class="font-medium">{{ store.mapper }}</span>
              </div>
              <ChevronDown :class="['w-4 h-4 text-slate-500 transition-transform duration-300', showMapperDropdown ? 'rotate-180' : '']" />
            </button>

            <!-- Dropdown Menu -->
            <Transition enter-active-class="transition duration-100 ease-out" enter-from-class="transform scale-95 opacity-0" enter-to-class="transform scale-100 opacity-100" leave-active-class="transition duration-75 ease-in" leave-from-class="transform scale-100 opacity-100" leave-to-class="transform scale-95 opacity-0">
              <div v-if="showMapperDropdown" class="absolute z-10 mt-2 w-full bg-[#111827] border border-slate-800 rounded-xl shadow-2xl overflow-hidden py-1">
                <!-- Dropdown Search -->
                <div class="p-2 border-b border-slate-800/50">
                   <div class="relative">
                     <Search class="absolute left-3 top-1/2 -translate-y-1/2 w-3.5 h-3.5 text-slate-500" />
                     <input 
                       v-model="searchQuery"
                       type="text" 
                       placeholder="Search mapper..."
                       class="w-full bg-[#1f2937]/50 border border-slate-800 rounded-lg pl-9 pr-3 py-2 text-xs text-white focus:outline-none focus:border-emerald-500/30 transition-all"
                     />
                   </div>
                </div>
                <div class="max-h-60 overflow-y-auto pt-1">
                  <button 
                    v-for="m in mappers.filter(x => x.toLowerCase().includes(searchQuery.toLowerCase()))" 
                    :key="m"
                    @click="selectMapper(m)"
                    class="w-full flex items-center justify-between px-4 py-3.5 text-sm text-slate-400 hover:text-white hover:bg-emerald-500/10 transition-all group"
                  >
                    <span :class="[store.mapper === m ? 'text-emerald-400 font-bold' : '']">{{ m }}</span>
                    <Check v-if="store.mapper === m" class="w-4 h-4 text-emerald-500" />
                  </button>
                </div>
              </div>
            </Transition>
          </div>
        </div>
      </div>

      <!-- Footer -->
      <div class="p-6 bg-slate-900/30 border-t border-slate-800 flex items-center justify-end gap-3 rounded-b-2xl">
        <button 
          @click="emit('close')"
          class="px-6 py-2.5 rounded-xl text-sm font-bold text-slate-400 hover:text-white hover:bg-slate-800 transition-all uppercase tracking-widest"
        >
          Cancel
        </button>
        <button 
          @click="handleSave"
          :disabled="store.isGenerating"
          class="px-8 py-2.5 bg-emerald-600 hover:bg-emerald-500 disabled:opacity-50 disabled:cursor-not-allowed text-white rounded-xl text-sm font-bold transition-all shadow-lg shadow-emerald-500/10 hover:shadow-emerald-500/20 active:scale-95 uppercase tracking-widest flex items-center gap-2"
        >
          <span v-if="store.isGenerating" class="w-4 h-4 border-2 border-white/20 border-t-white rounded-full animate-spin"></span>
          {{ store.isGenerating ? 'Building...' : 'Save' }}
        </button>
      </div>
    </div>
  </div>
</template>
