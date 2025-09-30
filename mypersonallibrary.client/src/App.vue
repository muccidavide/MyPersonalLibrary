<script setup>
import BooksGrid from './components/BooksGrid.vue'
</script>

<template>
  <header class="">
    <nav class="wrapper mb-4 text-center d-flex justify-content-center align-items-center flex-column flex-lg-row">
      <h1 class="logo extraBold">My Personal Library</h1>
    </nav>
  </header>

  <main>
    <!--  <Panel /> -->
    <BooksGrid msg="You did it!" />
  </main>
</template>

<style scoped>
header {
  line-height: 1.5;
}

.logo {
  color: #283618;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    justify-content: center;
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  nav {
    background-color: var(--color-background-soft);
    width: 100%;
    min-height: 100px;
    margin: 0;
    padding: 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }
}
</style>
