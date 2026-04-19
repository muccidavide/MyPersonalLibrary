<template>
  <teleport to="body">
    <transition name="fade">
      <div v-if="open"
        class="fixed inset-0 flex items-center justify-center bg-[rgba(6,6,7,0.6)] z-[9999] p-5"
        @click.self="$emit('cancel')" role="dialog" aria-modal="true">
        <div class="w-full max-w-[480px] bg-surface rounded-xl shadow-[0_10px_30px_rgba(0,0,0,0.25)] p-4 relative border border-border">
          <div class="px-2 pt-2 pb-4">
            <p class="m-0 text-content">{{ message }}</p>
          </div>
          <div class="flex justify-end gap-2">
            <button
              class="px-3 py-1 rounded text-sm cursor-pointer bg-surface-muted text-content border border-border hover:bg-border-strong transition-colors"
              @click="$emit('cancel')">{{ cancelText }}</button>
            <button
              class="px-3 py-1 rounded text-sm cursor-pointer bg-primary text-content-inverse border-none hover:bg-primary-dark transition-colors"
              @click="$emit('confirm')">{{ confirmText }}</button>
          </div>
        </div>
      </div>
    </transition>
  </teleport>
</template>

<script setup>
const emit = defineEmits(['confirm', 'cancel'])

defineProps({
  open: { type: Boolean, default: false },
  message: { type: String, required: true },
  confirmText: { type: String, default: 'Conferma' },
  cancelText: { type: String, default: 'Annulla' }
})
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity .18s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
