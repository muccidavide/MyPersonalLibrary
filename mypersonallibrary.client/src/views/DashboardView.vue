<template>
    <div class="content-inner">
        <Sidebar @search="handleSearch" @filter-change="handleFilterChange" />
        <main class="main-content">
            <div v-if="isLoadingBooks" class="loading">Loading books... Please refresh.</div>

            <Table v-else>
                <TableCaption>Elenco dei libri disponibili</TableCaption>
                <TableHeader>
                    <TableRow>
                        <TableHead>Titolo</TableHead>
                        <TableHead>Autore</TableHead>
                        <TableHead>Anno</TableHead>
                        <TableHead >Azioni</TableHead>
                    </TableRow>
                </TableHeader>
                <TableBody>
                    <TableRow v-for="book in books" :key="book.id">
                        <TableCell>{{ book.title }}</TableCell>
                        <TableCell>{{ book.authors }}</TableCell>
                        <TableCell>{{ book.originalPublicationYear }}</TableCell>
                        <TableCell>
                            <button @click="openEditModal(book)" class="btn btn-sm btn-primary me-2">Edit</button>
                            <button @click="handleBookDelete(book.id)" class="btn btn-sm btn-danger">Delete</button>
                        </TableCell>
                    </TableRow>
                </TableBody>
            </Table>

            <Pagination @update:page="handlePageChange" :items-per-page="itemsPerPage" :total="totalBooksCount"
                :default-page="1">
                <PaginationContent v-slot="{ items }">
                    <PaginationPrevious />
                    <template v-for="(item, index) in items" >
                        <PaginationItem :key="index" v-if="item.type === 'page'" :value="item.value"
                            :is-active="item.value === currentPage">
                            {{ item.value }}
                        </PaginationItem>
                    </template>
                    <PaginationEllipsis :index="4" />
                    <PaginationNext />
                </PaginationContent>
            </Pagination>
        </main>

        <teleport to="body">
            <transition name="fade">
                <div v-if="isEditModalVisible" class="modal-overlay" @click.self="closeEditModal" role="dialog" aria-modal="true">
                    <div class="modal-card">
                        <BookCardComponent v-if="selectedBookForEdit" :book="selectedBookForEdit" @save="handleBookSave" @cancel="closeEditModal" />
                    </div>
                </div>
            </transition>
        </teleport>
        <ConfirmModalComponent
            :open="isConfirmModalVisible"
            :message="confirmationMessage"
            @confirm="handleConfirmation"
            @cancel="handleConfirmationCancel"
        />
    </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount } from 'vue'
import Sidebar from '@/components/SidebarComponent.vue'
import BookCardComponent from '@/components/BookCardComponent.vue'
import ConfirmModalComponent from '@/components/ConfirmModalComponent.vue'
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
const activeFilters = ref({ author: '', year: '' })
const currentPage = ref(1)
const itemsPerPage = ref(12)
const books = ref([])
const totalBooksCount = ref(0)
const isLoadingBooks = ref(false)
const isEditModalVisible = ref(false)
const selectedBookForEdit = ref(null)
const isConfirmModalVisible = ref(false)
const confirmationMessage = ref('')
const confirmationCallback = ref(null)

let debounceTimeout = null
let abortController = null

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL

const updateBook = async (book) => {
    try {
        const response = await fetch(`${API_BASE_URL}/api/books/${book.id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                title: book.title,
                authors: book.authors,
                originalPublicationYear: book.originalPublicationYear
            })
        })
        if (!response.ok) throw new Error('Errore durante la modifica')
        await fetchBooks()
    } catch (error) {
        console.error('Errore durante la modifica del libro:', error)
    }
}

const openEditModal = (book) => {
    selectedBookForEdit.value = { ...book }
    isEditModalVisible.value = true
}

const closeEditModal = () => {
    isEditModalVisible.value = false
    selectedBookForEdit.value = null
}

const handleBookSave = async (updatedBook) => {
    showConfirmationDialog('Confermi il salvataggio delle modifiche?', async () => {
        await updateBook(updatedBook)
        closeEditModal()
    })
}

const deleteBookById = async (bookId) => {
    try {
        const response = await fetch(`${API_BASE_URL}/api/books/${bookId}`, {
            method: 'DELETE'
        })
        if (!response.ok) throw new Error('Errore durante l\'eliminazione')
        await fetchBooks()
    } catch (error) {
        console.error('Errore durante l\'eliminazione del libro:', error)
    }
}

const handleBookDelete = (bookId) => {
    showConfirmationDialog('Sei sicuro di voler eliminare questo libro?', async () => {
        await deleteBookById(bookId)
    })
}

const showConfirmationDialog = (message, callback) => {
    confirmationMessage.value = message
    confirmationCallback.value = callback
    isConfirmModalVisible.value = true
}

const handleConfirmation = async () => {
    try {
        const callback = confirmationCallback.value
        isConfirmModalVisible.value = false
        confirmationCallback.value = null
        if (typeof callback === 'function') {
            await callback()
        }
    } finally {
        confirmationMessage.value = ''
    }
}

const handleConfirmationCancel = () => {
    isConfirmModalVisible.value = false
    confirmationMessage.value = ''
    confirmationCallback.value = null
}

const fetchBooks = async () => {
    clearTimeout(debounceTimeout)
    debounceTimeout = setTimeout(async () => {
        abortController?.abort()
        abortController = new AbortController()
        isLoadingBooks.value = true
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
        } finally {
            isLoadingBooks.value = false
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

.modal-overlay {
    position: fixed;
    inset: 0;
    display: flex;
    align-items: center;
    justify-content: center;
    background: rgba(15, 23, 42, 0.6);
    z-index: 9999;
    padding: 1.25rem;
}

.modal-card {
    width: 100%;
    max-width: 560px;
    background: #fff;
    border-radius: 12px;
    box-shadow: 0 10px 30px rgba(0, 0, 0, 0.25);
    padding: 1rem;
    position: relative;
    border: 4px solid rgba(188, 108, 37, 0.12);
}

.fade-enter-active,
.fade-leave-active {
    transition: opacity .18s ease;
}

.fade-enter-from,
.fade-leave-to {
    opacity: 0;
}
</style>
