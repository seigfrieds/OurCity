<script setup lang="ts">

import { ref, defineEmits } from "vue";
import "@/assets/styles/smooth-transition.css";

const commentText = ref("");
const emit = defineEmits(["submit comment"]);

function handleSubmit() {
  emit("submit comment", commentText.value);
}
</script>

<template>
  <div class="comment-input-wrapper">
    <textarea
      class="comment-input"
      placeholder="Add your thoughts..."
      v-model="commentText"
    ></textarea>
    <transition name="smooth-transition">
      <div v-if="commentText.trim().length > 0">
        <button
          class="comment-arrow-btn"
          @click.prevent="handleSubmit"
        >
        <i class="pi pi-send"></i>
        </button>
      </div>
    </transition>
  </div>
</template>

<style scoped>
.comment-input-wrapper {
  position: relative;
}
.comment-input {
  width: 100%;
  border-radius: 3rem;
  border: 1px solid var(--border-color);
  padding: 0.75rem 3rem 0.5rem 1.5rem;
  font-size: 1rem;
  min-height: 4rem;
  resize: vertical;
}
.comment-arrow-btn {
  position: absolute;
  bottom: 1rem;
  right: 1rem;
  width: 2rem;
  height: 2rem;
  background: var(--secondary-color);
  border: none;
  border-radius: 0.5rem;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: background 0.2s;
}
.comment-arrow-btn:hover {
  background: #005fa3;
}
</style>
