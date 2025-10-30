<template>
        <div class="content-inner">
                <Sidebar @search="handleSearch" @filter-change="handleFilterChange" />
                <main class="main-content">
                        <BooksGrid :books="allBooks" :totalItems="totalItems" :page="page" :totalPages="totalPages"
                                :hasNextPage="hasNextPage" :hasPreviousPage="hasPreviousPage" @page-change="handlePageChange" />
                </main>
        </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount } from 'vue'
import Sidebar from '@/components/SidebarComponent.vue'
import BooksGrid from '@/components/BooksGridComponent.vue'

const searchTerm = ref('')
const filters = ref({ author: '', year: '' })
const page = ref(1)
const pageSize = ref(12)
const allBooks = ref([])
const totalItems = ref(0)
const totalPages = ref(0)
const hasNextPage = ref(false)
const hasPreviousPage = ref(false)

let fetchTimeout = null
let currentAbort = null

const API_BASE = import.meta.env.VITE_API_BASE_URL

const fetchBooks = async () => {
    console.log('fetchBooks')
    clearTimeout(fetchTimeout)
    fetchTimeout = setTimeout(async () => {
        currentAbort?.abort()
        currentAbort = new AbortController()
        try {
            const params = new URLSearchParams({
                pageNumber: String(page.value),
                pageSize: String(pageSize.value),
                title: searchTerm.value,
                author: filters.value.author || '',
                year: filters.value.year || ''
            })
            const res = await fetch(`${API_BASE}/api/books?${params.toString()}`, {
                signal: currentAbort.signal
            })
            if (!res.ok) throw new Error('Fetch error')
            const data = await res.json()
            allBooks.value = data.items || []
            totalItems.value = data.totalItems || 0
            totalPages.value = data.totalPages || 0
            hasNextPage.value = !!data.hasNextPage
            hasPreviousPage.value = !!data.hasPreviousPage
        } catch (err) {
            if (err.name !== 'AbortError') {
                console.error('Errore fetchBooks', err)
            }
        }
    }, 300)
}

const handleSearch = (q) => {
    searchTerm.value = q
    page.value = 1
    fetchBooks()
}

const handleFilterChange = (newFilters) => {
    filters.value = newFilters
    page.value = 1
    fetchBooks()
}

const handlePageChange = (newPage, newPageSize) => {
    page.value = newPage
    if (newPageSize != null) pageSize.value = newPageSize
    fetchBooks()
}

onMounted(() => fetchBooks())
onBeforeUnmount(() => {
    clearTimeout(fetchTimeout)
    currentAbort?.abort()
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

@media (max-width: 768px) {
    .content-wrapper {
        flex-direction: column;
    }
}
</style>
