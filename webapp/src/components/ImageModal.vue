<script setup lang="ts">
import "@/assets/styles/smooth-transition.css";

const props = defineProps<{ show: boolean; imageUrl?: string; title?: string }>();
const emit = defineEmits(["close"]);
</script>

<template>
  <transition name="smooth-transition">
    <div v-if="props.show" class="image-modal">
      <button class="close-modal-btn" @click="emit('close')">&times;</button>
      <div class="modal-content" @click.stop>
        <img :src="props.imageUrl" :alt="props.title" class="modal-image" />
      </div>
    </div>
  </transition>
</template>

<style scoped>
.image-modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background: rgba(0, 0, 0, 0.85);
  display: flex;
  align-items: center;
  justify-content: center;
}
.close-modal-btn {
  position: absolute;
  top: 2rem;
  right: 2rem;
  background: none;
  border: none;
  font-size: 4rem;
  cursor: pointer;
  color: var(--surface-color);
}
.close-modal-btn:hover {
  color: var(--danger-color, #d9534f);
}
.modal-image {
  max-width: 100vw;
  max-height: 100vh;
}
</style>
