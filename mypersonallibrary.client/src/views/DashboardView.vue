<template>
    <div class="content-inner">
        <Sidebar @search="handleSearch" @filter-change="handleFilterChange" />
        <main class="main-content">
            <div v-if="loading" class="loading">Loading books... Please refresh.</div>

            <Table v-else>
                <TableCaption>Elenco dei libri disponibili</TableCaption>
                <TableHeader>
                    <TableRow>
                        <TableHead>Titolo</TableHead>
                        <TableHead>Autore</TableHead>
                        <TableHead>Anno</TableHead>
                        <TableHead class="text-right">Prezzo</TableHead>
                        <TableHead >Azioni</TableHead>
                    </TableRow>
                </TableHeader>
                <TableBody>
                    <TableRow v-for="book in allBooks" :key="book.id">
                        <TableCell>{{ book.title }}</TableCell>
                        <TableCell>{{ book.authors }}</TableCell>
                        <TableCell>{{ book.originalPublicationYear }}</TableCell>
                        <TableCell class="text-right">{{ book.price }}</TableCell>
                        <TableCell>
                            <button @click="editBook(book)" class="btn btn-sm btn-primary me-2">Edit</button>
                            <button @click="deleteBook(book.id)" class="btn btn-sm btn-danger">Delete</button>
                        </TableCell>
                    </TableRow>
                </TableBody>
            </Table>

            <Pagination @update:page="handlePageChange" :items-per-page="pageSize" :total="totalItems"
                :default-page="1">
                <PaginationContent v-slot="{ items }">
                    <PaginationPrevious />
                    <template v-for="(item, index) in items" >
                        <PaginationItem :key="index" v-if="item.type === 'page'" :value="item.value"
                            :is-active="item.value === page">
                            {{ item.value }}
                        </PaginationItem>
                    </template>
                    <PaginationEllipsis :index="4" />
                    <PaginationNext />
                </PaginationContent>
            </Pagination>
        </main>
    </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount } from 'vue'
import Sidebar from '@/components/SidebarComponent.vue'
import {
    Table,
    TableBody,
    TableCaption,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from '@/components/ui/table'
import {
    Pagination,
    PaginationContent,
    PaginationEllipsis,
    PaginationItem,
    PaginationNext,
    PaginationPrevious,
} from '@/components/ui/pagination'

const searchTerm = ref('')
const filters = ref({ author: '', year: '' })
const page = ref(1)
const pageSize = ref(12)
const allBooks = ref([])
const totalItems = ref(0)
const totalPages = ref(0)
const hasNextPage = ref(false)
const hasPreviousPage = ref(false)
const loading = ref(false)

let fetchTimeout = null
let currentAbort = null

const API_BASE = import.meta.env.VITE_API_BASE_URL

const editBook = async (book) => {
    try {
        const res = await fetch(`${API_BASE}/api/books/${book.id}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(book) // oppure solo i campi modificati
        })
        if (!res.ok) throw new Error('Errore durante la modifica')
        console.log(`Libro ${book.id} modificato con successo`)
        fetchBooks()
    } catch (err) {
        console.error('Errore editBook:', err)
    }
}

const deleteBook = async (bookId) => {
    try {
        const res = await fetch(`${API_BASE}/api/books/${bookId}`, {
            method: 'DELETE'
        })
        if (!res.ok) throw new Error('Errore durante l\'eliminazione')
        console.log(`Libro ${bookId} eliminato con successo`)
        fetchBooks()
    } catch (err) {
        console.error('Errore deleteBook:', err)
    }
}

const fetchBooks = async () => {
    clearTimeout(fetchTimeout)
    fetchTimeout = setTimeout(async () => {
        currentAbort?.abort()
        currentAbort = new AbortController()
        loading.value = true
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
        } finally {
            loading.value = false
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

onMounted(fetchBooks)
onBeforeUnmount(() => {
    clearTimeout(fetchTimeout)
    currentAbort?.abort()
})
</script>

<style scoped>
.text-center {
    text-align: center;
}

.content-inner {
    display: contents;
}

.main-content {
    flex: 1;
    overflow-y: auto;
    padding: 1.5rem;
}

.loading {
    padding: 1rem;
    font-weight: bold;
}

.btn {
    padding: 0.25rem 0.5rem;
    border-radius: 4px;
    font-size: 0.875rem;
    cursor: pointer;
}

.btn-primary {
    background-color: #007bff;
    color: white;
    border: none;
}

.btn-danger {
    background-color: #dc3545;
    color: white;
    border: none;
}

.btn-sm {
    padding: 0.2rem 0.4rem;
}

.me-2 {
    margin-right: 0.5rem;
}
</style>
