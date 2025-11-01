<template>
  <div class="books-grid-component">
    <div v-if="isLoading" class="loading">
      Loading... Please refresh.
    </div>
    <div class="card-container">
      <div class="row">
        <div v-for="book in books" :key="book.id"
          class="col-12 col-lg-4  col-xl-2 text-center my-3 justify-content-center d-flex">
          <div class="ui-card">
            <div class="card-image" :style="{ backgroundImage: `url(${book.imageUrl})` }">
            </div>
          </div>
        </div>
      </div>
    </div>
    <Pagination @update:page="handlePageChange" v-slot="{ page }" :items-per-page="itemsPerPage" :total="totalItems"
      :default-page="1">
      <PaginationContent v-slot="{ items }">
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
  }
})

const emit = defineEmits(['page-change'])

const isLoading = ref(false)
const itemsPerPage = ref(12)

const handlePageChange = (pageNumber) => {
  emit('page-change', pageNumber, itemsPerPage.value)
}
</script>

<style scoped>
.card-container {
  min-height: 80vh;
}

.card-image {
  aspect-ratio: 2 / 3;
  background-repeat: no-repeat;
  background-size: 100%;
  background-position: center;
}

.ui-card {
  background: #f0efe1;
  width: 100%;
  display: flex;
  flex-direction: column;
  padding: 3px;
  border: 1px solid transparent;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  transition: all 0.3s ease-in-out;
  cursor: pointer;
  overflow: hidden;
}

.ui-card:hover {
  border: 1px solid white;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
  transform: translateY(-5px);
}
</style>
