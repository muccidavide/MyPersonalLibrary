<template>
  <aside class="sidebar">
    <div class="sidebar-content">
      <div class="sidebar-header">
        <svg 
          xmlns="http://www.w3.org/2000/svg" 
          width="24" 
          height="24" 
          viewBox="0 0 24 24" 
          fill="none" 
          stroke="currentColor" 
          stroke-width="2"
        >
          <circle cx="11" cy="11" r="8"></circle>
          <path d="m21 21-4.35-4.35"></path>
        </svg>
        <h2>Ricerca</h2>
      </div>

      <div class="search-section">
        <div class="search-input-wrapper">
          <input
            type="text"
            v-model="searchQuery"
            @input="onSearchChange"
            placeholder="Cerca libri..."
            class="search-input"
          />
          <svg 
            class="search-icon"
            xmlns="http://www.w3.org/2000/svg" 
            width="20" 
            height="20" 
            viewBox="0 0 24 24" 
            fill="none" 
            stroke="currentColor" 
            stroke-width="2"
          >
            <circle cx="11" cy="11" r="8"></circle>
            <path d="m21 21-4.35-4.35"></path>
          </svg>
        </div>
      </div>

      <div class="filters-section">
        <h3>Filtri</h3>
        
        <div class="filter-group">
          <label for="author-filter">Autore</label>
          <input
            id="author-filter"
            type="text"
            v-model="authorFilter"
            @input="onFilterChange"
            placeholder="Nome autore"
            class="filter-input"
          />
        </div>

        <div class="filter-group">
          <label for="year-filter">Anno di pubblicazione</label>
          <input
            id="year-filter"
            type="number"
            v-model="yearFilter"
            @input="onFilterChange"
            placeholder="Anno"
            class="filter-input"
            min="1000"
            max="2100"
          />
        </div>

        <button @click="clearFilters" class="clear-button">
          <svg 
            xmlns="http://www.w3.org/2000/svg" 
            width="16" 
            height="16" 
            viewBox="0 0 24 24" 
            fill="none" 
            stroke="currentColor" 
            stroke-width="2"
          >
            <path d="M18 6 6 18"></path>
            <path d="m6 6 12 12"></path>
          </svg>
          Cancella filtri
        </button>
      </div>
    </div>
  </aside>
</template>

<script setup lang="js">
import { ref } from 'vue'

const searchQuery = ref('')
const authorFilter = ref('')
const yearFilter = ref('')

const emit = defineEmits(['search', 'filter-change'])

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
  emit('filter-change', {
    author: '',
    year: ''
  })
}
</script>

<style scoped>
.sidebar {
  width: 280px;
  height: 100%;
  background: #bc6c25;
  box-shadow: 2px 0 8px rgba(0, 0, 0, 0.1);
  overflow-y: auto;
}

.sidebar-content {
  padding: 1.5rem;
}

.sidebar-header {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  color: white;
  margin-bottom: 1.5rem;
  padding-bottom: 1rem;
  border-bottom: 2px solid rgba(255, 255, 255, 0.2);
}

.sidebar-header svg {
  flex-shrink: 0;
}

.sidebar-header h2 {
  font-size: 1.25rem;
  font-weight: 700;
  margin: 0;
}

.search-section {
  margin-bottom: 2rem;
}

.search-input-wrapper {
  position: relative;
}

.search-input {
  width: 100%;
  padding: 0.75rem 2.5rem 0.75rem 0.75rem;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-radius: 8px;
  background: rgba(255, 255, 255, 0.95);
  font-size: 0.95rem;
  outline: none;
  transition: all 0.2s ease;
}

.search-input:focus {
  border-color: white;
  background: white;
  box-shadow: 0 0 0 3px rgba(255, 255, 255, 0.2);
}

.search-input::placeholder {
  color: #9ca3af;
}

.search-icon {
  position: absolute;
  right: 0.75rem;
  top: 50%;
  transform: translateY(-50%);
  color: #6b7280;
  pointer-events: none;
}

.filters-section h3 {
  color: white;
  font-size: 1rem;
  font-weight: 600;
  margin: 0 0 1rem 0;
}

.filter-group {
  margin-bottom: 1.25rem;
}

.filter-group label {
  display: block;
  color: white;
  font-size: 0.875rem;
  font-weight: 500;
  margin-bottom: 0.5rem;
}

.filter-input {
  width: 100%;
  padding: 0.625rem 0.75rem;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-radius: 6px;
  background: rgba(255, 255, 255, 0.95);
  font-size: 0.9rem;
  outline: none;
  transition: all 0.2s ease;
}

.filter-input:focus {
  border-color: white;
  background: white;
  box-shadow: 0 0 0 3px rgba(255, 255, 255, 0.2);
}

.filter-input::placeholder {
  color: #9ca3af;
}

.clear-button {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  width: 100%;
  padding: 0.75rem;
  margin-top: 1.5rem;
  background: rgba(255, 255, 255, 0.2);
  color: white;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-radius: 6px;
  font-size: 0.9rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
}

.clear-button:hover {
  background: rgba(255, 255, 255, 0.3);
  border-color: white;
}

.clear-button:active {
  transform: scale(0.98);
}

@media (max-width: 768px) {
  .sidebar {
    width: 100%;
    min-height: auto;
    position: relative;
  }

  .sidebar-content {
    padding: 1rem;
  }

  .sidebar-header h2 {
    font-size: 1.1rem;
  }
}
</style>
