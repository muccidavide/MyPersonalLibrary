<template>
  <div class="weather-component">
    <h1>My Personal Library</h1>

    <div v-if="loading" class="loading">
      Loading... Please refresh.
    </div>

    <div v-if="post" class="content">
      <table>
        <thead>
          <tr>
            <th>Title</th>
            <th>Cover</th>
            <th>Auhtors</th>
            <th>Description</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="book in post" :key="book.id">
            <td>{{ book.title }}</td>
            <td> <img :src="book.imageUrl" /></td>
            <td>{{ book.authors }}</td>
            <td>{{ book.description }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script lang="js">
  import { defineComponent } from 'vue';
  const API_BASE = import.meta.env.VITE_API_BASE_URL;

  export default defineComponent({
    data() {
      return {
        loading: false,
        post: null
      };
    },
    async created() {
      console.log(API_BASE)
      await this.fetchData();
    },
    watch: {
      '$route': 'fetchData'
    },
    methods: {
      async fetchData() {
        this.post = null;
        this.loading = true;

        try {
          const API_BASE = import.meta.env.VITE_API_BASE_URL;
          const response = await fetch(`${API_BASE}/api/books`);
          if (!response.ok) throw new Error('Errore nella risposta');
          this.post = await response.json();
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
  th {
    font-weight: bold;
  }

  th, td {
    padding-left: .5rem;
    padding-right: .5rem;
  }

  .weather-component {
    text-align: center;
  }

  table {
    margin-left: auto;
    margin-right: auto;
  }
</style>
