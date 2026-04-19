<template>
  <teleport to="body">
    <transition name="fade">
      <div
        v-if="open && book"
        class="fixed inset-0 z-[9999] flex items-center justify-center bg-black/50 p-4 sm:p-6"
        role="dialog"
        aria-modal="true"
        :aria-label="book.title || 'Dettagli libro'"
        @click.self="close"
        @keydown.esc="close"
        tabindex="-1"
        ref="dialogRef"
      >
        <div
          class="relative w-full max-w-3xl max-h-[90vh] bg-surface rounded-xl shadow-2xl overflow-hidden flex flex-col"
        >
          <button
            type="button"
            class="absolute top-3 right-3 z-10 p-1.5 rounded-md text-content hover:bg-surface-muted transition-colors"
            aria-label="Chiudi"
            @click="close"
          >
            <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none"
              stroke="currentColor" stroke-width="2" stroke-linecap="square" stroke-linejoin="miter">
              <rect x="3" y="3" width="18" height="18" rx="1" />
              <line x1="7" y1="7" x2="17" y2="17" />
              <line x1="17" y1="7" x2="7" y2="17" />
            </svg>
          </button>

          <div class="flex-1 min-h-0 overflow-y-auto">
            <div class="grid grid-cols-1 sm:grid-cols-[auto_1fr] gap-6 p-6 sm:p-8">
              <div class="flex items-start justify-center sm:justify-start">
                <div class="w-40 sm:w-48 aspect-[3/4] bg-surface-muted rounded-md overflow-hidden shadow-md">
                  <img
                    v-if="book.imageUrl"
                    :src="book.imageUrl"
                    :alt="book.title || 'Cover libro'"
                    class="w-full h-full object-cover"
                  />
                  <div
                    v-else
                    class="w-full h-full"
                    :style="{ background: FALLBACK_GRADIENT }"
                    role="img"
                    :aria-label="book.title || 'Cover libro'"
                  ></div>
                </div>
              </div>

              <div class="flex flex-col min-w-0">
                <h2 class="font-display font-bold text-2xl sm:text-3xl text-content leading-tight">
                  {{ book.title || 'Titolo sconosciuto' }}
                </h2>
                <p class="mt-2 text-sm text-content-muted">
                  By <span class="font-semibold uppercase tracking-wide text-content">{{ book.authors || 'Autore sconosciuto' }}</span>
                </p>

                <div v-if="book.description" class="mt-5 text-content text-sm leading-relaxed">
                  <p :class="['whitespace-pre-line', { 'line-clamp-6': !isDescriptionExpanded }]">
                    {{ book.description }}
                  </p>
                  <button
                    v-if="isDescriptionTruncatable"
                    type="button"
                    class="mt-2 text-sm font-semibold underline text-content hover:text-primary transition-colors"
                    @click="isDescriptionExpanded = !isDescriptionExpanded"
                  >
                    {{ isDescriptionExpanded ? 'Read less' : 'Read more >' }}
                  </button>
                </div>
              </div>
            </div>
          </div>

          <div class="shrink-0 border-t border-border">
            <button
              type="button"
              class="w-full py-4 bg-black text-white text-sm font-semibold tracking-wide hover:bg-neutral-800 transition-colors"
              @click="onViewDetails"
            >
              View Full Details →
            </button>
          </div>
        </div>
      </div>
    </transition>
  </teleport>
</template>

<script setup>
import { computed, nextTick, ref, watch } from 'vue'

const props = defineProps({
  open: { type: Boolean, default: false },
  book: { type: Object, default: null }
})

const emit = defineEmits(['close', 'view-details'])

const FALLBACK_GRADIENT = 'linear-gradient(135deg, #4752C4 0%, #7A84F6 100%)'
const DESCRIPTION_TRUNCATE_LENGTH = 320

const isDescriptionExpanded = ref(false)
const dialogRef = ref(null)

const isDescriptionTruncatable = computed(() => {
  const text = props.book?.description ?? ''
  return text.length > DESCRIPTION_TRUNCATE_LENGTH
})

watch(
  () => props.open,
  async (isOpen) => {
    if (!isOpen) return
    isDescriptionExpanded.value = false
    await nextTick()
    dialogRef.value?.focus()
  }
)

const close = () => emit('close')
const onViewDetails = () => emit('view-details', props.book)
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.18s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
