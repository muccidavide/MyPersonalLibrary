<template>

  <div class="app-container">
    <Navbar @open-login="showLogin = true" />
    <div class="content-wrapper">
      <RouterView />
    </div>
  <teleport to="body">
    <transition name="fade">
      <div v-if="showLogin" class="modal-overlay" @click.self="close" role="dialog" aria-modal="true">
        <div class="modal-card">
          <button class="modal-close" @click="close" aria-label="Chiudi">✕</button>
          <Login @login="handleLogin" />
        </div>
      </div>
    </transition>
  </teleport>
</div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount } from 'vue'
import Navbar from './components/NavBarComponent.vue'
import Login from './components/LoginComponent.vue'
import { RouterView } from 'vue-router'

const showLogin = ref(false)

const close = () => (showLogin.value = false)

const handleLogin = async (credentials) => {
  // integra qui la chiamata API di autenticazione
  console.log('Login richiesto', credentials)
  showLogin.value = false
}

const onKey = (e) => e.key === 'Escape' && close()

onMounted(() => window.addEventListener('keydown', onKey))
onBeforeUnmount(() => window.removeEventListener('keydown', onKey))
</script>

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

.modal-overlay {
  position: fixed;
  inset: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  background: rgba(15, 23, 42, 0.6);
  z-index: 9999;
  padding: 1.25rem;
}

.modal-card {
  width: 100%;
  max-width: 520px;
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.25);
  padding: 1rem;
  position: relative;
  border: 4px solid rgba(188, 108, 37, 0.12);
}

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

.fade-enter-active,
.fade-leave-active {
  transition: opacity .18s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

@media (max-width:768px) {
  .content-wrapper {
    flex-direction: column;
  }

  .modal-card {
    max-width: 100%;
    margin: 0 .5rem;
  }
}
</style>
