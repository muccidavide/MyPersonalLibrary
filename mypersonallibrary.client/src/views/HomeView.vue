<template>
    <div class="content-inner">
        <Sidebar :collapsed="!isSidebarOpen" @search="handleSearch" @filter-change="handleFilterChange" @toggle="isSidebarOpen = !isSidebarOpen" />
        <main class="main-content">
            <div class="main-wrapper">
                <h1 class="text-2xl font-bold tracking-tight m-0 mb-4 shrink-0">Libreria</h1>

                <div
                    v-if="connectionError"
                    class="flex-1 flex items-center justify-center"
                    role="alert"
                    aria-live="polite"
                >
                    <div class="max-w-md text-center px-6 py-8 bg-surface rounded-xl border border-border shadow-md">
                        <h2 class="font-display font-bold text-xl text-content mb-2">
                            Il servizio si sta avviando
                        </h2>
                        <p class="text-sm text-content-muted mb-5">
                            Non riusciamo a contattare il server in questo momento. Per favore, ricarica la pagina tra qualche istante.
                        </p>
                        <button
                            type="button"
                            class="inline-flex items-center gap-2 px-4 py-2 rounded-md bg-black text-white text-sm font-semibold hover:bg-neutral-800 transition-colors"
                            @click="reloadPage"
                        >
                            Ricarica la pagina
                        </button>
                    </div>
                </div>

                <BooksGrid
                    v-else
                    class="flex-1 min-h-0"
                    :books="books"
                    :totalItems="totalBooksCount"
                    :page="currentPage"
                    :isLoading="isInitialLoading"
                    @page-change="handlePageChange"
                />
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
const connectionError = ref(false)
const isInitialLoading = ref(true)

let debounceTimeout = null
let abortController = null
let retryTimeout = null

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL

const INITIAL_MAX_ATTEMPTS = 4
const INITIAL_RETRY_DELAY_MS = 3000

const doFetch = async () => {
    abortController?.abort()
    abortController = new AbortController()
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
}

const fetchBooks = () => {
    clearTimeout(debounceTimeout)
    debounceTimeout = setTimeout(async () => {
        try {
            await doFetch()
        } catch (error) {
            if (error.name !== 'AbortError') {
                console.error('Errore durante il caricamento dei libri:', error)
            }
        }
    }, 300)
}

const initialLoad = async (attempt = 1) => {
    try {
        await doFetch()
        isInitialLoading.value = false
        connectionError.value = false
    } catch (error) {
        if (error.name === 'AbortError') return
        if (attempt < INITIAL_MAX_ATTEMPTS) {
            retryTimeout = setTimeout(() => initialLoad(attempt + 1), INITIAL_RETRY_DELAY_MS)
        } else {
            isInitialLoading.value = false
            connectionError.value = true
            console.error('Errore durante il caricamento iniziale dei libri:', error)
        }
    }
}

const reloadPage = () => window.location.reload()

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

onMounted(() => initialLoad())
onBeforeUnmount(() => {
    clearTimeout(debounceTimeout)
    clearTimeout(retryTimeout)
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
