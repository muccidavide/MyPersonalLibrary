<template>
  <header class="header">
    <div class="container navbar-inner">
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

      <nav class="nav-actions">
        <button class="login-link" @click="openLogin">Login</button>
      </nav>
    </div>
  </header>
</template>

<script setup lang="js">
const emit = defineEmits(['open-login'])

function openLogin() {
  emit('open-login')
}
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
  display: flex;
  align-items: center;
  justify-content: space-between;
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
  font-size: 1.25rem;
  font-weight: 700;
  margin: 0;
}

.nav-actions {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.login-link {
  background: transparent;
  color: white;
  border: 2px solid rgba(255,255,255,0.18);
  padding: 0.45rem 0.9rem;
  border-radius: 6px;
  font-weight: 600;
  cursor: pointer;
}

.login-link:hover {
  background: rgba(255,255,255,0.06);
}
</style>