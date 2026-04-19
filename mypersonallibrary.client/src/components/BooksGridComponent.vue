<template>
  <div class="books-grid-component flex flex-col lg:h-full lg:min-h-0">
    <div v-if="isLoading" class="flex-1 flex items-center justify-center text-content-muted">
      Caricamento in corso...
    </div>

    <div v-else-if="!books.length" class="flex-1 flex items-center justify-center text-content-muted">
      Nessun libro trovato.
    </div>

    <div v-else class="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-6 xl:grid-cols-8 gap-3 md:gap-4 lg:flex-1 lg:min-h-0">
      <article
        v-for="book in books"
        :key="book.id"
        class="book-card group relative overflow-hidden rounded-xl shadow-md cursor-pointer transition-all duration-300 hover:-translate-y-1 hover:shadow-xl aspect-[3/4]"
        @click="onBookClick(book)"
      >
        <div
          class="absolute inset-0 bg-cover bg-center transition-transform duration-500 group-hover:scale-105"
          :style="coverStyle(book)"
          role="img"
          :aria-label="book.title || 'Cover libro'"
        ></div>

        <div class="absolute inset-0 bg-gradient-to-t from-black/85 via-black/30 to-transparent"></div>

        <div class="relative h-full flex flex-col justify-end py-2.5 px-3.5 text-white">
          <h3 class="book-card__title font-display font-bold leading-tight line-clamp-2 drop-shadow-md">
            {{ book.title || 'Titolo sconosciuto' }}
          </h3>
          <p class="text-[10px] text-white/80 line-clamp-1 leading-snug mt-0.5">
            {{ book.authors || 'Autore sconosciuto' }}
          </p>
        </div>
      </article>
    </div>

    <BookDetailsModal
      :open="isDetailsModalOpen"
      :book="selectedBook"
      @close="closeDetailsModal"
      @view-details="onViewDetails"
    />

    <nav v-if="totalItems > 0" class="shrink-0 flex justify-center pt-4" aria-label="Paginazione libri">
      <Pagination @update:page="handlePageChange" v-slot="{ page }" :items-per-page="itemsPerPage" :total="totalItems"
        :default-page="1">
        <PaginationContent v-slot="{ items }" class="flex flex-wrap items-center gap-1">
          <PaginationPrevious />
          <template v-for="(item, index) in items">
            <PaginationItem :key="index" v-if="item.type === 'page'" :value="item.value" :is-active="item.value === page">
              {{ item.value }}
            </PaginationItem>
          </template>
          <PaginationEllipsis :index="4" />
          <PaginationNext />
        </PaginationContent>
      </Pagination>
    </nav>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import {
  Pagination,
  PaginationContent,
  PaginationEllipsis,
  PaginationItem,
  PaginationNext,
  PaginationPrevious,
} from '@/components/ui/pagination'
import BookDetailsModal from '@/components/BookDetailsModalComponent.vue'

defineProps({
  page: {
    type: Number,
    default: 1
  },
  books: {
    type: Array,
    required: true,
    default: () => []
  },
  totalItems: {
    type: Number,
    required: true
  },
  isLoading: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['page-change', 'book-click', 'view-details'])

const itemsPerPage = ref(24)
const selectedBook = ref(null)
const isDetailsModalOpen = ref(false)

const FALLBACK_GRADIENT = 'linear-gradient(135deg, #4752C4 0%, #7A84F6 100%)'

const coverStyle = (book) => ({
  backgroundImage: book?.imageUrl ? `url(${book.imageUrl})` : FALLBACK_GRADIENT,
})

const handlePageChange = (pageNumber) => {
  emit('page-change', pageNumber, itemsPerPage.value)
}

const onBookClick = (book) => {
  selectedBook.value = book
  isDetailsModalOpen.value = true
  emit('book-click', book)
}

const closeDetailsModal = () => {
  isDetailsModalOpen.value = false
}

const onViewDetails = (book) => {
  emit('view-details', book)
}
</script>

<style scoped>
.book-card {
  background: var(--card);
}

.book-card__title {
  font-size: 11px;
}
</style>
