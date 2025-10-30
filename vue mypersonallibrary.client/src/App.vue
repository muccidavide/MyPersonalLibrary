<script setup>
import { RouterLink, RouterView } from 'vue-router';
import { ref, onBeforeUnmount, onMounted } from 'vue'
import Navbar from './components/Navbar.vue';
import Login from './components/Login.vue'

const showLogin = ref(false)

function close() {
  showLogin.value = false
}

function handleLogin(credentials) {
  // TODO: integra la chiamata API di autenticazione qui.
  console.log('Login richiesto', credentials)
  // Dopo login correttamente:
  showLogin.value = false
}

// Gestione ESC per chiudere il modal
function onKey(e) {
  if (e.key === 'Escape') close()
}

onMounted(() => window.addEventListener('keydown', onKey))
onBeforeUnmount(() => window.removeEventListener('keydown', onKey))
</script>

<template>
  <div class="app-container">
    <Navbar @open-login="showLogin = true" />
    <div class="content-wrapper">
      <router-view />
    </div>
  </div>

  <!-- Teleport: monta il modal direttamente in <body> per evitare problemi di stacking/overflow -->
  <teleport to="body">
    <transition name="fade">
      <div v-if="showLogin" class="modal-overlay" @click.self="close" aria-hidden="false">
        <div class="modal-card" role="dialog" aria-modal="true">
          <button class="modal-close" @click="close" aria-label="Chiudi">✕</button>
          <Login @login="handleLogin" />
        </div>
      </div>
    </transition>
  </teleport>
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

/* overlay fissato e centrato */
.modal-overlay {
  position: fixed;
  inset: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  background: rgba(15, 23, 42, 0.6); /* semi-trasparente scuro */
  z-index: 9999; /* abbastanza alto da sovrastare il layout */
  padding: 1.25rem;
}

/* card centrata con palette del sito */
.modal-card {
  width: 100%;
  max-width: 520px;
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 10px 30px rgba(0,0,0,0.25);
  padding: 1rem;
  position: relative;
  border: 4px solid rgba(188,108,37,0.12); /* palette coerente */
}

/* bottone chiudi */
.modal-close {
  position: absolute;
  top: 8px;
  right: 8px;
  background: transparent;
  border: none;
  font-size: 1.05rem;
  cursor: pointer;
  color: #374151;
}

/* transizioni semplici */
.fade-enter-active, .fade-leave-active {
  transition: opacity .18s ease;
}
.fade-enter-from, .fade-leave-to {
  opacity: 0;
}

@media (max-width: 768px) {
  .content-wrapper {
    flex-direction: column;
  }

  .modal-card {
    max-width: 100%;
    margin: 0 0.5rem;
  }
}
</style>