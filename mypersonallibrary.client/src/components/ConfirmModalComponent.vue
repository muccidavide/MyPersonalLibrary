<template>
    <teleport to="body">
        <transition name="fade">
            <div v-if="open" class="modal-overlay" @click.self="$emit('cancel')" role="dialog" aria-modal="true">
                <div class="modal-card">
                    <div class="modal-body">
                        <p class="modal-message">{{ message }}</p>
                    </div>
                    <div class="modal-actions">
                        <button class="btn btn-sm btn-secondary" @click="$emit('cancel')">{{ cancelText }}</button>
                        <button class="btn btn-sm btn-primary" @click="$emit('confirm')">{{ confirmText }}</button>
                    </div>
                </div>
            </div>
        </transition>
    </teleport>
    
</template>

<script setup>
defineProps({
    open: { type: Boolean, default: false },
    message: { type: String, required: true },
    confirmText: { type: String, default: 'Conferma' },
    cancelText: { type: String, default: 'Annulla' }
})
</script>

<style scoped>
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
    max-width: 480px;
    background: #fff;
    border-radius: 12px;
    box-shadow: 0 10px 30px rgba(0, 0, 0, 0.25);
    padding: 1rem;
    position: relative;
    border: 4px solid rgba(188, 108, 37, 0.12);
}

.modal-body {
    padding: 0.5rem 0.5rem 1rem 0.5rem;
}

.modal-message {
    margin: 0;
}

.modal-actions {
    display: flex;
    justify-content: flex-end;
    gap: 0.5rem;
}

.btn {
    padding: 0.25rem 0.5rem;
    border-radius: 4px;
    font-size: 0.875rem;
    cursor: pointer;
}

.btn-primary {
    background-color: #007bff;
    color: white;
    border: none;
}

.btn-secondary {
    background-color: #e5e7eb;
    color: #111827;
    border: none;
}

.btn-sm {
    padding: 0.2rem 0.4rem;
}

.fade-enter-active,
.fade-leave-active {
    transition: opacity .18s ease;
}

.fade-enter-from,
.fade-leave-to {
    opacity: 0;
}
</style>


