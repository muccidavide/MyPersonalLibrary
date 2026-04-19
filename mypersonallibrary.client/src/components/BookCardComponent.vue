<template>
  <div class="book-card">
    <div class="book-card__header">
      <h3 class="book-card__title">Modifica libro</h3>
    </div>

    <div class="book-card__body">
      <div class="book-card__cover-wrapper">
        <div class="book-card__cover">
          <img
            v-if="editableBook.imageUrl"
            :src="editableBook.imageUrl"
            :alt="editableBook.title || 'Cover libro'"
            class="book-card__cover-img"
          />
          <div
            v-else
            class="book-card__cover-fallback"
            role="img"
            :aria-label="editableBook.title || 'Cover libro'"
          ></div>
        </div>
      </div>

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
      <button
        class="px-3 py-1.5 rounded text-sm cursor-pointer bg-surface-muted text-content border border-border hover:bg-border-strong transition-colors"
        type="button" @click="onCancel">Annulla</button>
      <button
        class="px-3 py-1.5 rounded text-sm cursor-pointer bg-primary text-content-inverse border-none hover:bg-primary-dark transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
        type="button" :disabled="!canSave" @click="onSave">Salva</button>
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
  imageUrl: props.book?.imageUrl ?? '',
})

watch(
  () => props.book,
  (newBook) => {
    if (!newBook) return
    editableBook.id = newBook.id ?? null
    editableBook.title = newBook.title ?? ''
    editableBook.authors = newBook.authors ?? ''
    editableBook.originalPublicationYear = newBook.originalPublicationYear ?? null
    editableBook.imageUrl = newBook.imageUrl ?? ''
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
    imageUrl: editableBook.imageUrl ?? '',
  }
  emit('save', payload)
}

function onCancel() {
  emit('cancel')
}
</script>

<style scoped>
.book-card {
  background: var(--background);
  border: 1px solid var(--border);
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
  color: var(--foreground);
}

.book-card__body {
  display: grid;
  grid-template-columns: 1fr;
  gap: 0.75rem;
}

.book-card__cover-wrapper {
  display: flex;
  justify-content: center;
}

.book-card__cover {
  width: 10rem;
  aspect-ratio: 3 / 4;
  border-radius: 6px;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
  background: var(--surface-muted, #f3f4f6);
}

.book-card__cover-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  display: block;
}

.book-card__cover-fallback {
  width: 100%;
  height: 100%;
  background: linear-gradient(135deg, #4752C4 0%, #7A84F6 100%);
}

.form-group {
  display: grid;
  gap: 0.25rem;
}

label {
  font-size: 0.9rem;
  color: var(--foreground);
}

.form-control {
  display: block;
  width: 100%;
  border: 1px solid var(--border);
  border-radius: 6px;
  padding: 0.375rem 0.5rem;
  color: var(--foreground);
  background: var(--background);
  outline: none;
  transition: border-color 0.15s ease, box-shadow 0.15s ease;
}

.form-control:focus {
  border-color: var(--primary);
  box-shadow: 0 0 0 3px rgba(88, 101, 242, 0.15);
}

.book-card__footer {
  display: flex;
  justify-content: flex-end;
  gap: 0.5rem;
}
</style>
