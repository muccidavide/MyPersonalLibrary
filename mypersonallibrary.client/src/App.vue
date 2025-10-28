<script setup>
import { ref, computed, onMounted } from 'vue'
import Sidebar from './components/Sidebar.vue'
import BooksGrid from './components/BooksGrid.vue'
import Navbar from './components/Navbar.vue';


const searchTerm = ref('')
const filters = ref({ author: '', year: '' })
const page = ref(1)
const pageSize = ref(12)
const allBooks = ref([])
const totalItems = ref(0)
const totalPages = ref(0)
const hasNextPage = ref(false)
const hasPreviousPage = ref(false)  

const handleSearch = (query) => {
  searchTerm.value = query
  fetchBooks()
}

const handleFilterChange = (newFilters) => {
  filters.value = newFilters
  fetchBooks()
}

const handlePageChange = (newPage, pageSize) => {
  // TODO: Update page and pageSize refs and fetch books for the new page
  page.value = newPage
  pageSize = pageSize
  console.log('Page changed to:', newPage, 'with page size:', pageSize)
  fetchBooks()
}

let fetchTimeout = null

const fetchBooks = async () => {
  clearTimeout(fetchTimeout)
  fetchTimeout = setTimeout(async () => {
    console.log('Fetching books with:', {
      page: page.value,
      pageSize: pageSize.value,
      searchTerm: searchTerm.value,
      filters: filters.value
    })

    const API_BASE = import.meta.env.VITE_API_BASE_URL
    const response = await fetch(`${API_BASE}/api/books?pageNumber=${encodeURIComponent(page.value)}&pageSize=${encodeURIComponent(pageSize.value)}&title=${encodeURIComponent(searchTerm.value)}&author=${encodeURIComponent(filters.value.author)}&year=${encodeURIComponent(filters.value.year)}`)

    const data = await response.json()
    allBooks.value = data.items
    totalItems.value = data.totalItems
    totalPages.value = data.totalPages
    hasNextPage.value = data.hasNextPage
    hasPreviousPage.value = data.hasPreviousPage

    console.log('Fetched books data:', allBooks.value)
  }, 300)
}


onMounted(fetchBooks)
</script>

<template>
  <div class="app-container">
    <Navbar />
    <div class="content-wrapper">
      <Sidebar  @search="handleSearch" @filter-change="handleFilterChange"/>
      <main class="main-content">
      <BooksGrid :books="allBooks" :totalItems="totalItems" :page="page" :totalPages="totalPages" :hasNextPage="hasNextPage" :hasPreviousPage="hasPreviousPage"  @page-change="handlePageChange" />
      </main>
    </div>
  </div>
</template>

<style scoped>
.app-container {
  height: 100vh;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.content-wrapper {
  display: flex;
  flex: 1;
  overflow: hidden;
}

.main-content {
  flex: 1;
  overflow-y: auto;
  padding: 1.5rem;
}

@media (max-width: 768px) {
  .content-wrapper {
    flex-direction: column;
  }
  
  .main-content {
    padding: 1rem;
  }
}
</style>
