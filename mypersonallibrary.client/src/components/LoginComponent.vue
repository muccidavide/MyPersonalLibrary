<template>
  <div class="login-card">
    <h2 class="title">Accedi</h2>

    <form @submit.prevent="onSubmit" novalidate class="form">
      <div class="form-group">
        <label for="email">Email</label>
        <input id="email" type="email" v-model="email" :class="{ 'input-error': errors.email }"
          placeholder="nome@esempio.com" autocomplete="username email" />
        <div v-if="errors.email" class="error">{{ errors.email }}</div>
      </div>

      <div class="form-group">
        <label for="password">Password</label>
        <input id="password" type="password" v-model="password" :class="{ 'input-error': errors.password }"
          placeholder="La tua password" autocomplete="current-password" />
        <div v-if="errors.password" class="error">{{ errors.password }}</div>
      </div>

      <div class="actions">
        <button type="submit" :disabled="loading" class="btn-primary">
          <span v-if="!loading">Accedi</span>
          <span v-else>Accesso in corso…</span>
        </button>
      </div>

      <div v-if="submitError" class="submit-error">{{ submitError }}</div>
    </form>
  </div>
</template>

<script setup lang="js">
import router from '@/routes/routes'
import { ref, reactive } from 'vue'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL
const emit = defineEmits(['login'])

const email = ref('')
const password = ref('')
const loading = ref(false)
const submitError = ref('')
const errors = reactive({ email: '', password: '' })

function validate() {
  errors.email = ''
  errors.password = ''
  submitError.value = ''

  const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/

  if (!email.value) {
    errors.email = 'L\'email è obbligatoria.'
  } else if (!emailPattern.test(email.value)) {
    errors.email = 'Inserisci un indirizzo email valido.'
  }

  if (!password.value) {
    errors.password = 'La password è obbligatoria.'
  } else if (password.value.length < 6) {
    errors.password = 'La password deve contenere almeno 6 caratteri.'
  }

  return !errors.email && !errors.password
}

async function onSubmit() {
  if (!validate()) return

  loading.value = true
  try {

    const payload = {
      email: email.value,
      password: password.value
    };

    fetch(`${API_BASE_URL}/api/authentication/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(payload)
    })
      .then(async (response) => {
        if (!response.ok) {
          throw new Error('Login failed');
        }

        const data = await response.json();
        localStorage.setItem('accessToken', data.accessToken);
        localStorage.setItem('refreshToken', data.refreshToken);
        localStorage.setItem('expiresAt', data.accessTokenExpiresAtUtc);
        localStorage.setItem('user', JSON.stringify(data.user));

        emit('login')
        return data;
      })
      .catch((error) => {
        console.error('Login error:', error);
      });
  } catch (err) {
    submitError.value = 'Errore durante l\'invio. Riprova.'
    console.error('Errore durante l\'invio', err)
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.login-card {
  max-width: 420px;
  margin: 1rem auto;
  padding: 1.25rem;
  border-radius: 8px;
  background: var(--background);
}

.title {
  margin: 0 0 1rem 0;
  font-size: 1.25rem;
  text-align: center;
  color: var(--foreground);
}

.form-group {
  margin-bottom: 0.75rem;
  display: flex;
  flex-direction: column;
}

label {
  font-weight: 600;
  margin-bottom: 0.25rem;
  color: var(--foreground);
}

input {
  padding: 0.6rem 0.75rem;
  border: 1px solid var(--border);
  border-radius: 6px;
  font-size: 0.95rem;
  color: var(--foreground);
  background: var(--background);
  outline: none;
  transition: border-color 0.15s ease, box-shadow 0.15s ease;
}

input:focus {
  border-color: var(--primary);
  box-shadow: 0 0 0 3px rgba(88, 101, 242, 0.15);
}

.input-error {
  border-color: var(--destructive);
  box-shadow: 0 0 0 3px rgba(180, 74, 192, 0.1);
}

.error {
  margin-top: 0.35rem;
  color: var(--destructive);
  font-size: 0.85rem;
}

.actions {
  margin-top: 1rem;
  display: flex;
  justify-content: center;
}

.btn-primary {
  background: var(--primary);
  color: var(--primary-foreground);
  padding: 0.6rem 1rem;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 600;
  transition: background 0.15s ease;
}

.btn-primary:hover:not([disabled]) {
  background: #4752C4;
}

.btn-primary[disabled] {
  opacity: 0.7;
  cursor: default;
}

.submit-error {
  margin-top: 0.75rem;
  text-align: center;
  color: var(--destructive);
  font-size: 0.9rem;
}
</style>
