<template>
  <div class="login-card">
    <h2 class="title">Accedi</h2>

    <form @submit.prevent="onSubmit" novalidate class="form">
      <div class="form-group">
        <label for="email">Email</label>
        <input
          id="email"
          type="email"
          v-model="email"
          :class="{'input-error': errors.email}"
          placeholder="nome@esempio.com"
          autocomplete="username email"
        />
        <div v-if="errors.email" class="error">{{ errors.email }}</div>
      </div>

      <div class="form-group">
        <label for="password">Password</label>
        <input
          id="password"
          type="password"
          v-model="password"
          :class="{'input-error': errors.password}"
          placeholder="La tua password"
          autocomplete="current-password"
        />
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
import { ref, reactive } from 'vue'

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
    emit('login', { email: email.value, password: password.value })
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
  background: #fff;
  box-shadow: 0 6px 20px rgba(0,0,0,0.06);
}

.title {
  margin: 0 0 1rem 0;
  font-size: 1.25rem;
  text-align: center;
}

.form-group {
  margin-bottom: 0.75rem;
  display: flex;
  flex-direction: column;
}

label {
  font-weight: 600;
  margin-bottom: 0.25rem;
}

input {
  padding: 0.6rem 0.75rem;
  border: 1px solid #d1d5db;
  border-radius: 6px;
  font-size: 0.95rem;
}

.input-error {
  border-color: #ef4444;
  box-shadow: 0 0 0 3px rgba(239,68,68,0.08);
}

.error {
  margin-top: 0.35rem;
  color: #ef4444;
  font-size: 0.85rem;
}

.actions {
  margin-top: 1rem;
  display: flex;
  justify-content: center;
}

.btn-primary {
  background: #bc6c25;
  color: white;
  padding: 0.6rem 1rem;
  border: none;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 600;
}

.btn-primary[disabled] {
  opacity: 0.7;
  cursor: default;
}

.submit-error {
  margin-top: 0.75rem;
  text-align: center;
  color: #b91c1c;
  font-size: 0.9rem;
}
</style>
