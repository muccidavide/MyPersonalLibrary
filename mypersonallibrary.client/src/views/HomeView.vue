<template>
    <div class="content-inner">
        <Sidebar :collapsed="!isSidebarOpen" @search="handleSearch" @filter-change="handleFilterChange" @toggle="isSidebarOpen = !isSidebarOpen" />
        <main class="main-content">
            <div class="main-wrapper">
                <h1 class="text-2xl font-bold tracking-tight m-0 mb-4 shrink-0">Libreria</h1>

                <BooksGrid class="flex-1 min-h-0" :books="books" :totalItems="totalBooksCount" :page="currentPage" @page-change="handlePageChange" />
            </div>
        </main>
    </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount } from 'vue'
import Sidebar from '@/components/SidebarComponent.vue'
import BooksGrid from '@/components/BooksGridComponent.vue'

const isSidebarOpen = ref(true)
const searchTerm = ref('')
const activeFilters = ref({ author: '', year: '' })
const currentPage = ref(1)
const itemsPerPage = ref(24)
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
    min-width: 0;
    min-height: 0;
    overflow: hidden;
    padding: 1.5rem 2rem;
    display: flex;
    flex-direction: column;
    transition: padding 320ms cubic-bezier(0.22, 1, 0.36, 1);
}

.main-wrapper {
    max-width: 1400px;
    width: 100%;
    margin: 0 auto;
    flex: 1;
    min-height: 0;
    display: flex;
    flex-direction: column;
    transition: max-width 320ms cubic-bezier(0.22, 1, 0.36, 1);
}
</style>
