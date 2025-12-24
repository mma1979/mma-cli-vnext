<script setup lang="ts">
import { ref } from 'vue'
import { useSchemaStore } from '../stores/schema'
import { X, ChevronRight, Loader2 } from 'lucide-vue-next'

const store = useSchemaStore()
const emit = defineEmits(['close', 'next'])
const isTesting = ref(false)
const error = ref('')

async function handleNext() {
  isTesting.value = true
  error.value = ''
  const success = await store.testConnection()
  if (success) {
    await store.fetchTables()
    emit('next')
  } else {
    error.value = 'Failed to connect to database. Please check your credentials.'
  }
  isTesting.value = false
}
</script>

<template>
  <div class="fixed inset-0 z-[100] flex items-center justify-center p-4 bg-[#0b1121]/80 backdrop-blur-sm">
    <div class="w-full max-w-md bg-[#151c2e] border border-slate-700/50 rounded-2xl shadow-2xl overflow-hidden animate-in fade-in zoom-in duration-200">
      <div class="p-6 border-b border-slate-700/50 flex items-center justify-between">
        <h2 class="text-xl font-bold text-white">Connect Database</h2>
        <button @click="$emit('close')" class="p-1 hover:bg-slate-700/50 rounded-lg text-slate-400 hover:text-white transition-colors">
          <X class="w-5 h-5" />
        </button>
      </div>

      <div class="p-6 space-y-4">
        <div>
          <label class="text-[10px] font-bold text-slate-500 uppercase tracking-widest block mb-1.5">Provider</label>
          <select 
            v-model="store.dbConfig.provider"
            class="w-full bg-[#0b1121] border border-slate-700 rounded-lg px-3 py-2.5 text-sm focus:border-emerald-500 focus:outline-none transition-colors appearance-none"
          >
            <option value="sqlserver">Microsoft SQL Server</option>
            <option value="postgresql">PostgreSQL</option>
          </select>
        </div>

        <div>
          <label class="text-[10px] font-bold text-slate-500 uppercase tracking-widest block mb-1.5">Connection String</label>
          <textarea 
            v-model="store.dbConfig.connectionString"
            rows="3"
            class="w-full bg-[#0b1121] border border-slate-700 rounded-lg px-3 py-2 text-sm focus:border-emerald-500 focus:outline-none transition-colors resize-none mb-1"
            placeholder="Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;"
          ></textarea>
           <p v-if="error" class="text-[11px] text-rose-500 font-medium">{{ error }}</p>
        </div>
      </div>

      <div class="p-6 bg-slate-900/50 border-t border-slate-700/50 flex justify-end gap-3">
        <button 
          @click="$emit('close')"
          class="px-4 py-2 rounded-lg text-sm font-semibold text-slate-400 hover:text-white transition-colors"
        >
          Cancel
        </button>
        <button 
          @click="handleNext"
          :disabled="isTesting || !store.dbConfig.connectionString"
          class="bg-emerald-600 hover:bg-emerald-500 disabled:opacity-50 disabled:cursor-not-allowed text-white px-5 py-2 rounded-lg text-sm font-bold flex items-center gap-2 transition-all"
        >
          <Loader2 v-if="isTesting" class="w-4 h-4 animate-spin" />
          <span v-else>Next: Select Tables</span>
          <ChevronRight v-if="!isTesting" class="w-4 h-4" />
        </button>
      </div>
    </div>
  </div>
</template>
