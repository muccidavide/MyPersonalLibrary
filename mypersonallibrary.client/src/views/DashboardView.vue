<template>
    <div class="content-inner">
        <Sidebar :collapsed="!isSidebarOpen" @search="handleSearch" @filter-change="handleFilterChange" @toggle="isSidebarOpen = !isSidebarOpen" />
        <main class="main-content">
            <div class="main-wrapper">
                <h1 class="text-2xl font-bold tracking-tight m-0 mb-6">Libreria</h1>

                <div v-if="isLoadingBooks" class="loading">Loading books... Please refresh.</div>

                <Table v-else>
                <TableCaption>Elenco dei libri disponibili</TableCaption>
                <TableHeader>
                    <TableRow>
                        <TableHead>Titolo</TableHead>
                        <TableHead>Autore</TableHead>
                        <TableHead>Anno</TableHead>
                        <TableHead>Azioni</TableHead>
                    </TableRow>
                </TableHeader>
                <TableBody>
                    <TableRow v-for="book in books" :key="book.id">
                        <TableCell>{{ book.title }}</TableCell>
                        <TableCell>{{ book.authors }}</TableCell>
                        <TableCell>{{ book.originalPublicationYear }}</TableCell>
                        <TableCell>
                            <button @click="openEditModal(book)"
                                class="btn btn-sm btn-primary" >Edit</button>
                            <button @click="handleBookDelete(book.id)"
                                class="btn btn-red btn-sm btn-destructive ms-2">Delete</button>
                        </TableCell>
                    </TableRow>
                </TableBody>
            </Table>

            <Pagination @update:page="handlePageChange" :items-per-page="itemsPerPage" :total="totalBooksCount"
                :default-page="1">
                <PaginationContent v-slot="{ items }">
                    <PaginationPrevious />
                    <template v-for="(item, index) in items">
                        <PaginationItem :key="index" v-if="item.type === 'page'" :value="item.value"
                            :is-active="item.value === currentPage">
                            {{ item.value }}
                        </PaginationItem>
                    </template>
                    <PaginationEllipsis :index="4" />
                    <PaginationNext />
                </PaginationContent>
            </Pagination>
            </div>
        </main>

        <teleport to="body">
            <transition name="fade">
                <div v-if="isEditModalVisible"
                    class="fixed inset-0 flex items-center justify-center bg-[rgba(6,6,7,0.6)] z-[9999] p-5"
                    @click.self="closeEditModal" role="dialog" aria-modal="true">
                    <div class="w-full max-w-[560px] bg-surface rounded-xl shadow-[0_10px_30px_rgba(0,0,0,0.25)] p-4 relative border border-border">
                        <BookCardComponent v-if="selectedBookForEdit" :book="selectedBookForEdit" @save="handleBookSave"
                            @cancel="closeEditModal" />
                    </div>
                </div>
            </transition>
        </teleport>
        <ConfirmModalComponent :open="isConfirmModalVisible" :message="confirmationMessage"
            @confirm="handleConfirmation" @cancel="handleConfirmationCancel" />
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

const isSidebarOpen = ref(true)
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
        const token = localStorage.getItem('accessToken');
        const response = await fetch(`${API_BASE_URL}/api/books/${book.id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
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
        const token = localStorage.getItem('accessToken');
        const response = await fetch(`${API_BASE_URL}/api/books/${bookId}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }

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
    min-width: 0;
    overflow-y: auto;
    padding: 2rem 2.5rem;
    transition: padding 320ms cubic-bezier(0.22, 1, 0.36, 1);
}

.main-wrapper {
    max-width: 1200px;
    margin: 0 auto;
    transition: max-width 320ms cubic-bezier(0.22, 1, 0.36, 1);
}

.loading {
    padding: 1rem;
    font-weight: 500;
    color: var(--muted-foreground);
}

.fade-enter-active,
.fade-leave-active {
    transition: opacity .18s ease;
}

.fade-enter-from,
.fade-leave-to {
    opacity: 0;
}

.btn{
    width: 60px;
}

.btn-red{
    background-color: red;
    color: white;
}
</style>
