<template>
  <div class="book-card">
    <div class="book-card__header">
      <h3 class="book-card__title">Modifica libro</h3>
    </div>

    <div class="book-card__body">
      <div class="form-group">
        <label for="title">Titolo</label>
        <input id="title" type="text" class="form-control" v-model.trim="editableBook.title" />
      </div>

      <div class="form-group">
        <label for="authors">Autore/i</label>
        <input id="authors" type="text" class="form-control" v-model.trim="editableBook.authors" />
      </div>

      <div class="form-group">
        <label for="year">Anno di pubblicazione</label>
        <input id="year" type="number" min="0" class="form-control" v-model.number="editableBook.originalPublicationYear" />
      </div>
    </div>

    <div class="book-card__footer">
      <button class="btn btn-secondary" type="button" @click="onCancel">Annulla</button>
      <button class="btn btn-primary" type="button" :disabled="!canSave" @click="onSave">Salva</button>
    </div>
  </div>
</template>

<script setup>
import { computed, reactive, watch, } from 'vue'

const props = defineProps({
  book: {
    type: Object,
    required: true,
  },
})

const emit = defineEmits(['save', 'cancel'])

const editableBook = reactive({
  id: props.book?.id ?? null,
  title: props.book?.title ?? '',
  authors: props.book?.authors ?? '',
  originalPublicationYear: props.book?.originalPublicationYear ?? null,
})

watch(
  () => props.book,
  (newBook) => {
    if (!newBook) return
    editableBook.id = newBook.id ?? null
    editableBook.title = newBook.title ?? ''
    editableBook.authors = newBook.authors ?? ''
    editableBook.originalPublicationYear = newBook.originalPublicationYear ?? null
  },
  { deep: false }
)

const canSave = computed(() => {
  const hasTitle = typeof editableBook.title === 'string' && editableBook.title.trim().length > 0
  const hasAuthors = typeof editableBook.authors === 'string' && editableBook.authors.trim().length > 0
  const hasYear = editableBook.originalPublicationYear != null && String(editableBook.originalPublicationYear).length > 0
  return hasTitle && hasAuthors && hasYear
})

function onSave() {
  const payload = {
    id: editableBook.id,
    title: editableBook.title?.trim() ?? '',
    authors: editableBook.authors?.trim() ?? '',
    originalPublicationYear: editableBook.originalPublicationYear,
  }
  emit('save', payload)
}

function onCancel() {
  emit('cancel')
}
</script>

<style scoped>
.book-card {
  background: #fff;
  border: 1px solid rgba(0, 0, 0, 0.08);
  border-radius: 10px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.06);
  padding: 1rem;
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.book-card__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.book-card__title {
  margin: 0;
  font-size: 1.1rem;
}

.book-card__body {
  display: grid;
  grid-template-columns: 1fr;
  gap: 0.75rem;
}

.form-group {
  display: grid;
  gap: 0.25rem;
}

label {
  font-size: 0.9rem;
}

.form-control {
  display: block;
  width: 100%;
  border: 1px solid #ced4da;
  border-radius: 6px;
  padding: 0.375rem 0.5rem;
}

.book-card__footer {
  display: flex;
  justify-content: flex-end;
  gap: 0.5rem;
}
</style>

