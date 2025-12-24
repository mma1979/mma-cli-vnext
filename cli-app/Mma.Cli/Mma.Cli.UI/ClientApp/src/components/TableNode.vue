<script setup lang="ts">
import { Handle, Position } from '@vue-flow/core'
import { Database, GripVertical } from 'lucide-vue-next'
import { useSchemaStore } from '../stores/schema'

const store = useSchemaStore()

defineProps<{
  selected?: boolean
  data: {
    name: string
    columns: Array<{
      name: string
      type: string
      isPk?: boolean
      isNullable?: boolean
    }>
  }
}>()

const getTypeName = (value: string) => {
  const type = store.columnTypeMap.find(t => (t.value || '').toLowerCase() === (value || '').toLowerCase() || t.name === value)
  return type ? type.name : value
}
</script>

<template>
  <div 
    class="min-w-[220px] bg-[#111827] border rounded-2xl shadow-2xl overflow-hidden group transition-all duration-300"
    :class="[selected ? 'border-emerald-500 ring-4 ring-emerald-500/10' : 'border-slate-800 hover:border-slate-700']"
  >
    <!-- Header -->
    <div class="bg-slate-900/50 p-4 border-b border-slate-800 flex items-center justify-between">
      <div class="flex items-center gap-3">
        <div class="p-1.5 bg-emerald-500/10 rounded-lg group-hover:bg-emerald-500/20 transition-colors">
          <Database class="w-4 h-4 text-emerald-500" />
        </div>
        <span class="font-bold text-slate-100 text-[13px] tracking-tight uppercase">{{ data.name }}</span>
      </div>
      <GripVertical class="w-4 h-4 text-slate-600 opacity-0 group-hover:opacity-100 transition-opacity cursor-grab" />
    </div>

    <!-- Columns -->
    <div class="p-2 space-y-1">
      <div 
        v-for="column in data.columns" 
        :key="column.name"
        class="relative flex items-center justify-between px-3 py-2.5 hover:bg-slate-800/50 rounded-xl transition-all group/column"
      >
        <!-- Column Name & Icon -->
        <div class="flex items-center gap-2.5">
          <div v-if="column.isPk" class="w-1.5 h-1.5 rounded-full bg-emerald-500 shadow-[0_0_8px_rgba(16,185,129,0.5)]"></div>
          <span :class="['text-[12px] font-medium transition-colors', column.isPk ? 'text-emerald-400' : 'text-slate-300 group-hover/column:text-white']">
            {{ column.name }}
          </span>
        </div>

        <!-- Column Type -->
        <span class="text-[9px] font-bold text-slate-500 bg-slate-800/50 px-2 py-0.5 rounded-md border border-slate-700/50 group-hover/column:border-slate-600 transition-colors">
          {{ getTypeName(column.type) }}
        </span>

        <!-- Handles -->
        <Handle
          type="target"
          :position="Position.Left"
          :id="`${column.name}-left`"
          class="!opacity-0 group-hover/column:!opacity-100 transition-opacity !left-[-4px] !w-2 !h-2 !bg-emerald-500 !border-2 !border-slate-900"
        />
        <Handle
          type="source"
          :position="Position.Right"
          :id="`${column.name}-right`"
          class="!opacity-0 group-hover/column:!opacity-100 transition-opacity !right-[-4px] !w-2 !h-2 !bg-emerald-500 !border-2 !border-slate-900"
        />
      </div>
    </div>
  </div>
</template>

<style scoped>
.vue-flow__handle {
  z-index: 10;
}
</style>
