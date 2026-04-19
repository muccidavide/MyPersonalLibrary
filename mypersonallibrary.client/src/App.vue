<template>
  <div class="app-container">
    <Navbar @open-login="showLogin = true" />
    <div class="content-wrapper">
      <RouterView />
    </div>
    <teleport to="body">
      <transition name="fade">
        <div v-if="showLogin"
          class="fixed inset-0 flex items-center justify-center bg-[rgba(6,6,7,0.6)] z-[9999] p-5"
          @click.self="close" role="dialog" aria-modal="true">
          <div class="w-full max-w-[520px] bg-surface rounded-xl shadow-[0_10px_30px_rgba(0,0,0,0.25)] p-4 relative border border-border">
            <button
              class="absolute top-2 right-2 bg-transparent border-none text-content-muted text-[1.05rem] cursor-pointer hover:text-content transition-colors"
              @click="close" aria-label="Chiudi">✕</button>
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
import router from './routes/routes'


const showLogin = ref(false)

const close = () => (showLogin.value = false)

const handleLogin = async () => {
  showLogin.value = false
  await router.push('/dashboard')
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

.fade-enter-active,
.fade-leave-active {
  transition: opacity .18s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

@media (max-width: 1023px) {
  .app-container {
    height: auto;
    min-height: 100vh;
    overflow: visible;
  }
  .content-wrapper {
    flex-direction: column;
    overflow: visible;
  }
}
</style>
