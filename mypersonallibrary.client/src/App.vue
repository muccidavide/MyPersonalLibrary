<script setup>
import BooksGrid from './components/BooksGrid.vue';
import Navbar from './components/Navbar.vue';
import Sidebar from './components/Sidebar.vue';
</script>

<template>
  <div class="app-container">
    <Navbar />
    <div class="content-wrapper">
      <Sidebar />
      <main class="main-content">
        <BooksGrid msg="You did it!" />
      </main>
    </div>
  </div>
</template>

<style scoped>
.app-container {
  height: 100vh;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.content-wrapper {
  display: flex;
  flex: 1;
  overflow: hidden;
}

.main-content {
  flex: 1;
  overflow-y: auto;
  padding: 1.5rem;
}

@media (max-width: 768px) {
  .content-wrapper {
    flex-direction: column;
  }
  
  .main-content {
    padding: 1rem;
  }
}
</style>
