<template>
  <aside class="sidebar" :class="{ 'sidebar-collapsed': collapsed }">
    <button
      v-if="collapsed"
      type="button"
      class="btn-icon sidebar-expand-btn"
      @click="emit('toggle')"
      aria-label="Mostra sidebar"
      title="Mostra sidebar"
    >
      <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none"
        stroke="currentColor" stroke-width="2.25" stroke-linecap="round" stroke-linejoin="round">
        <path d="M9 18l6-6-6-6"></path>
      </svg>
    </button>

    <div class="sidebar-inner" :aria-hidden="collapsed">
      <div class="flex items-center gap-3 pb-3 mb-3 border-b border-border">
        <span class="flex items-center justify-center w-9 h-9 rounded-xl bg-primary/10 text-primary shrink-0">
          <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none"
            stroke="currentColor" stroke-width="2.25" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="11" cy="11" r="8"></circle>
            <path d="m21 21-4.35-4.35"></path>
          </svg>
        </span>
        <h2 class="text-lg font-semibold m-0 text-content tracking-tight flex-1">Ricerca</h2>
        <button
          type="button"
          class="btn-icon"
          @click="emit('toggle')"
          aria-label="Nascondi sidebar"
          title="Nascondi sidebar"
        >
          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none"
            stroke="currentColor" stroke-width="2.25" stroke-linecap="round" stroke-linejoin="round">
            <path d="M15 18l-6-6 6-6"></path>
          </svg>
        </button>
      </div>

      <div class="mb-2">
        <div class="relative">
          <svg class="absolute left-3.5 top-1/2 -translate-y-1/2 text-content-subtle pointer-events-none"
            xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24"
            fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="11" cy="11" r="8"></circle>
            <path d="m21 21-4.35-4.35"></path>
          </svg>
          <input type="text" v-model="searchQuery" @input="onSearchChange" placeholder="Cerca libri..."
            class="field-input !pl-10" />
        </div>
      </div>

      <div class="my-3">
        <div class="space-y-5">
          <div>
            <label for="author-filter" class="block text-content text-sm font-medium mb-2">Autore:</label>
            <input id="author-filter" type="text" v-model="authorFilter" @input="onFilterChange"
              placeholder="Nome autore" class="field-input" />
          </div>

          <div class="my-3">
            <label for="year-filter" class="block text-content text-sm font-medium mb-2">Anno di pubblicazione:</label>
            <input id="year-filter" type="number" v-model="yearFilter" @input="onFilterChange"
              placeholder="Es. 1998" class="field-input" min="1000" max="2100" />
          </div>
        </div>

        <button @click="clearFilters" class="btn btn-red w-full mt-8">
          <span>Reset filtri</span>
        </button>
      </div>
    </div>
  </aside>
</template>

<script setup lang="js">
import { ref } from 'vue'

defineProps({
  collapsed: { type: Boolean, default: false }
})

const searchQuery = ref('')
const authorFilter = ref('')
const yearFilter = ref('')

const emit = defineEmits(['search', 'filter-change', 'toggle'])

const onSearchChange = () => {
  emit('search', searchQuery.value)
}

const onFilterChange = () => {
  emit('filter-change', {
    author: authorFilter.value,
    year: yearFilter.value
  })
}

const clearFilters = () => {
  searchQuery.value = ''
  authorFilter.value = ''
  yearFilter.value = ''
  emit('search', '')
  emit('filter-change', { author: '', year: '' })
}
</script>

<style scoped>
.sidebar {
  position: relative;
  width: 300px;
  flex-shrink: 0;
  height: 100%;
  background: var(--card);
  border-right: 1px solid var(--border);
  overflow: hidden;
  transition:
    width 320ms cubic-bezier(0.22, 1, 0.36, 1),
    border-color 320ms ease;
}

.sidebar-inner {
  width: 300px;
  height: 100%;
  overflow-y: auto;
  padding: 1.75rem 1.5rem 2rem;
  transition: opacity 220ms ease, transform 320ms cubic-bezier(0.22, 1, 0.36, 1);
}

.sidebar-collapsed {
  width: 64px;
}

.sidebar-collapsed .sidebar-inner {
  opacity: 0;
  transform: translateX(-12px);
  pointer-events: none;
}

.sidebar-expand-btn {
  position: absolute;
  top: 1.25rem;
  left: 50%;
  transform: translateX(-50%);
  z-index: 2;
}

.field-input {
  width: 100%;
  padding: 0.7rem 0.9rem;
  font-size: 0.9rem;
  font-family: inherit;
  border: 1px solid var(--border);
  border-radius: 10px;
  background: var(--background);
  color: var(--foreground);
  outline: none;
  transition: border-color 180ms ease, box-shadow 180ms ease, background 180ms ease;
}

.field-input::placeholder {
  color: var(--color-content-subtle, #80848E);
}

.field-input:hover {
  border-color: var(--color-border-strong, #c4c9d2);
}

.field-input:focus {
  border-color: var(--primary);
  box-shadow: 0 0 0 4px rgba(88, 101, 242, 0.15);
}

.btn-red {
    background-color: red;
    color: white;
}

.btn-red:hover {
    background-color: #cc0000;
    color: white;
}

@media (max-width: 768px) {
  .sidebar {
    width: 100%;
  }
  .sidebar-inner {
    width: 100%;
  }
  .sidebar-collapsed {
    width: 64px;
  }
}
</style>
