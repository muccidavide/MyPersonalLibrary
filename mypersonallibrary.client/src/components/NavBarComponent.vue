<template>
  <div class="navbar-wrapper">
    <header class="fixed top-0 left-0 right-0 z-50 bg-primary/90 backdrop-blur-md shadow-md border-b border-white/10 transition-all duration-300">
      <div
        class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between gap-3 sm:gap-6"
      >
        <div class="flex items-center gap-3 text-content-inverse min-w-0 flex-1">
          <span
            class="flex items-center justify-center w-10 h-10 rounded-xl bg-white/15 backdrop-blur-sm shrink-0"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="22"
              height="22"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
            >
              <path d="M2 3h6a4 4 0 0 1 4 4v14a3 3 0 0 0-3-3H2z"></path>
              <path d="M22 3h-6a4 4 0 0 0-4 4v14a3 3 0 0 1 3-3h7z"></path>
            </svg>
          </span>
          <h1 class="text-base sm:text-xl font-bold m-0 tracking-tight truncate min-w-0">
            MyPersonalLibrary
          </h1>
        </div>

        <nav class="flex items-center gap-3 shrink-0">
          <button
            v-if="!isLoggedIn"
            class="btn btn-login hover:scale-105 transition-transform"
            @click="openLogin"
          >
            Login
          </button>
          <button
            v-else
            class="btn btn-login hover:scale-105 transition-transform"
            :disabled="loggingOut"
            @click="onLogout"
          >
            <span v-if="!loggingOut">Logout</span>
            <span v-else>Uscita…</span>
          </button>
        </nav>
      </div>
    </header>
    <!-- Spacer for fixed header -->
    <div class="h-16 w-full shrink-0"></div>
  </div>
</template>

<script setup lang="js">
import { ref, watch } from 'vue'
import { useRoute } from 'vue-router'
import router from '@/routes/routes'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL

const emit = defineEmits(['open-login'])

const route = useRoute()
const isLoggedIn = ref(!!localStorage.getItem('accessToken'))
const loggingOut = ref(false)

watch(
  () => route.fullPath,
  () => {
    isLoggedIn.value = !!localStorage.getItem('accessToken')
  }
)

function openLogin() {
  emit('open-login')
}

function clearSession() {
  localStorage.removeItem('accessToken')
  localStorage.removeItem('refreshToken')
  localStorage.removeItem('expiresAt')
  localStorage.removeItem('user')
  isLoggedIn.value = false
}

async function onLogout() {
  if (loggingOut.value) return
  loggingOut.value = true

  const refreshToken = localStorage.getItem('refreshToken')
  const accessToken = localStorage.getItem('accessToken')

  try {
    if (refreshToken) {
      await fetch(`${API_BASE_URL}/api/authentication/logout`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          ...(accessToken ? { Authorization: `Bearer ${accessToken}` } : {})
        },
        body: JSON.stringify({ refreshToken })
      })
    }
  } catch (err) {
    console.error('Logout error:', err)
  } finally {
    clearSession()
    loggingOut.value = false
    if (route.path !== '/') {
      await router.push('/')
    }
  }
}
</script>

<style scoped>
.btn-login {
  background-color: white;
  color: var(--primary);
  border-color: transparent;
  box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
}

.btn-login:hover:not([disabled]) {
  background-color: #e2e8f0;
}

.btn-login[disabled] {
  opacity: 0.7;
  cursor: default;
}
</style>
