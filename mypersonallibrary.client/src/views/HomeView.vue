<template>
        <div class="content-inner">
                <Sidebar @search="handleSearch" @filter-change="handleFilterChange" />
                <main class="main-content">
                        <BooksGrid :books="books" :totalItems="totalBooksCount" :page="currentPage" @page-change="handlePageChange" />
                </main>
        </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount } from 'vue'
import Sidebar from '@/components/SidebarComponent.vue'
import BooksGrid from '@/components/BooksGridComponent.vue'

const searchTerm = ref('')
const activeFilters = ref({ author: '', year: '' })
const currentPage = ref(1)
const itemsPerPage = ref(12)
const books = ref([])
const totalBooksCount = ref(0)

let debounceTimeout = null
let abortController = null

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL

const fetchBooks = async () => {
    clearTimeout(debounceTimeout)
    debounceTimeout = setTimeout(async () => {
        abortController?.abort()
        abortController = new AbortController()
        try {
            const params = new URLSearchParams({
                pageNumber: String(currentPage.value),
                pageSize: String(itemsPerPage.value),
                title: searchTerm.value,
                author: activeFilters.value.author || '',
                year: activeFilters.value.year || ''
            })
            const response = await fetch(`${API_BASE_URL}/api/books?${params.toString()}`, {
                signal: abortController.signal
            })
            if (!response.ok) throw new Error('Errore durante il caricamento dei libri')
            const data = await response.json()
            books.value = data.items || []
            totalBooksCount.value = data.totalItems || 0
        } catch (error) {
            if (error.name !== 'AbortError') {
                console.error('Errore durante il caricamento dei libri:', error)
            }
        }
    }, 300)
}

const handleSearch = (searchQuery) => {
    searchTerm.value = searchQuery
    currentPage.value = 1
    fetchBooks()
}

const handleFilterChange = (newFilters) => {
    activeFilters.value = newFilters
    currentPage.value = 1
    fetchBooks()
}

const handlePageChange = (newPage, newPageSize) => {
    currentPage.value = newPage
    if (newPageSize != null) itemsPerPage.value = newPageSize
    fetchBooks()
}

onMounted(fetchBooks)
onBeforeUnmount(() => {
    clearTimeout(debounceTimeout)
    abortController?.abort()
})
</script>

<style scoped>
.content-inner {
    display: contents;
}

.main-content {
    flex: 1;
    overflow-y: auto;
    padding: 1.5rem;
}
</style>
