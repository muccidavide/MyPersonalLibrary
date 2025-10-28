<template>
  <div class="books-grid-component">
    <div v-if="loading" class="loading">
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
    <Pagination @update:page="handlePageChange" v-slot="{ page }" :items-per-page="pageSize" :total="totalItems"
      :default-page="1">
      <PaginationContent v-slot="{ items }">
        <PaginationPrevious />
        <template v-for="(item, index) in items" :key="index">
          <PaginationItem v-if="item.type === 'page'" :value="item.value" :is-active="item.value === page">
            {{ item.value }}
          </PaginationItem>
        </template>
        <PaginationEllipsis :index="4" />
        <PaginationNext />
      </PaginationContent>
    </Pagination>
  </div>
</template>

<script lang="js">
import { defineComponent } from 'vue';
import {
  Pagination,
  PaginationContent,
  PaginationEllipsis,
  PaginationItem,
  PaginationNext,
  PaginationPrevious,
} from '@/components/ui/pagination'

export default defineComponent(
  {
    name: 'BooksGrid',
    components: {
      Pagination,
      PaginationContent,
      PaginationEllipsis,
      PaginationItem,
      PaginationNext,
      PaginationPrevious
    },
    props: {
      page: {
        type: Number,
        default: 1
      },
      books: {
        type: Array,
        required: true
      },
      totalItems: {
        type: Number,
        required: true
      },
      totalPages: {
        type: Number,
        required: false
      },
      hasNextPage: {
        type: Boolean,
        required: false
      },
      hasPreviousPage: {
        type: Boolean,
        required: false
      }

    },
    data() {
      return {
        loading: false,
        pageSize: 12
      };
    },
    async created() {
    },
    watch: {
      '$route': 'fetchData'
    },
    methods: {

      async handlePageChange(pageNumber) {
        this.$emit('page-change', pageNumber, this.pageSize);
      },

    },
  });
</script>

<style scoped>
.card-container {
  min-height: 80vh;
}

.mpl-card {

  border: 1px solid #ccc;
  border-radius: 8px;
  padding: 10px;
  box-shadow: 2px 2px 12px rgba(0, 0, 0, 0.1);
  transition: transform 0.2s;
  width: 100%;
}

.card-image {
  aspect-ratio: 2 / 3;
  background-repeat: no-repeat;
  background-size: 100%;
  background-position: center;
}

.mpl-card img {
  max-width: 100%;
  min-width: 300px;
  aspect-ratio: 2/3;
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

.card-title {
  padding: 10px 15px;
  font-size: 14px;
  border-top: 1px solid rgba(255, 255, 255, 0.1);
}

.ui-card:hover {
  border: 1px solid white;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
  transform: translateY(-5px);
}

@media screen and (max-width: 600px) {
  .mpl-card img {
    min-width: 80vw;
  }
}
</style>
