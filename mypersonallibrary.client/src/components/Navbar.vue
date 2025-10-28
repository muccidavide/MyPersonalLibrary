<template>
  <header class="header">
    <div class="container">
      <div class="brand">
        <svg 
          xmlns="http://www.w3.org/2000/svg" 
          width="32" 
          height="32" 
          viewBox="0 0 24 24" 
          fill="none" 
          stroke="currentColor" 
          stroke-width="2"
        >
          <path d="M2 3h6a4 4 0 0 1 4 4v14a3 3 0 0 0-3-3H2z"></path>
          <path d="M22 3h-6a4 4 0 0 0-4 4v14a3 3 0 0 1 3-3h7z"></path>
        </svg>
        <h1>MyPersonalLibrary</h1>
      </div>
    </div>
  </header>
</template>

<script setup lang="js">
</script>

<style scoped>
.header {
  background: linear-gradient(to right, #bc6c25 0%, #bc6c25 280px, #d97706, #ea580c, #f97316);
  padding: 1.5rem 0;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1.5rem;
}

.brand {
  display: flex;
  align-items: center;
  gap: 1rem;
  color: white;
}

.brand svg {
  flex-shrink: 0;
}

.brand h1 {
  font-size: 1.75rem;
  font-weight: 700;
  margin: 0;
}

@media (max-width: 768px) {
  .brand h1 {
    font-size: 1.5rem;
  }
}
</style>
