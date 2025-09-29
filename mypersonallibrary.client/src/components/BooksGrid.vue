<template>
  <div class="weather-component">
    <div v-if="loading" class="loading">
      Loading... Please refresh.
    </div>


    <div class="card-container">
      <div class="row">
        <div v-for="book in books" :key="book.id"
          class="col-12 col-lg-4  col-xl-2 text-center my-3 justify-content-center d-flex">

          <div class="ui-card">
            <div class="card-image" :style="{ backgroundImage: `url(${book.imageUrl})` }">
              <!--  <img :src="book.imageUrl" class="img-fluid w-100 d-block" /> -->
            </div>
            <!--             <div class="card-title">
              <p class="montserrat-semibold">{{ book.title }}</p>
              <p class="montserrat-medium">{{ book.authors }}</p>
            </div> -->
          </div>


        </div>
      </div>

    </div>
  </div>
</template>

<script lang="js">
import { defineComponent } from 'vue';

export default defineComponent({
  data() {
    return {
      loading: false,
      books: null
    };
  },
  async created() {
    await this.fetchData();
  },
  watch: {
    '$route': 'fetchData'
  },
  methods: {
    async fetchData() {
      this.books = null;
      this.loading = true;

      try {
        const API_BASE = import.meta.env.VITE_API_BASE_URL;
        const response = await fetch(`${API_BASE}/api/books`);
        if (!response.ok) throw new Error('Errore nella risposta');
        this.books = await response.json();
        console.log(this.books);

      } catch (error) {
        console.error('Errore nel fetch:', error);
      } finally {
        this.loading = false;
      }
    }
  },
});
</script>

<style scoped>
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
