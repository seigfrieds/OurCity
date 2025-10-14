<script setup lang="ts">
import Card from "primevue/card";
import VoteButton from "primevue/button";
import { withDefaults, defineProps, defineEmits } from "vue";

withDefaults(
  defineProps<{
    title: string;
    location?: string;
    description: string;
    imageUrl?: string;
    upvotes?: number;
    downvotes?: number;
    showVotes?: boolean;
  }>(),
  {
    upvotes: 0,
    downvotes: 0,
    showVotes: true,
  },
);

const emit = defineEmits<{
  (e: "upvote"): void;
  (e: "downvote"): void;
}>();

function upvote() {
  emit("upvote");
}

function downvote() {
  emit("downvote");
}
</script>

<template>
  <Card class="post-card">
    <template #content>
      <div class="post-content">
        <div v-if="showVotes" class="post-votes">
          <VoteButton icon="pi pi-chevron-up" class="vote-btn upvote" @click.prevent="upvote" />
          <div class="vote-count">{{ upvotes - downvotes }}</div>
          <VoteButton
            icon="pi pi-chevron-down"
            class="vote-btn downvote"
            @click.prevent="downvote"
          />
        </div>
        <div class="post-text-content">
          <h2 class="post-title">{{ title }}</h2>
          <div class="post-location">
            <i class="pi pi-map-marker location-icon"></i>
            <span>{{ location }}</span>
          </div>
          <p class="post-description">{{ description }}</p>
        </div>
        <div class="post-image-container">
          <img :src="imageUrl" :alt="title" class="post-image" />
        </div>
      </div>
    </template>
  </Card>
</template>

<style scoped>
.post-card {
  display: flex;
}

.post-content {
  display: flex;
  gap: 1.5rem;
}

.post-votes {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 56px;
  flex-shrink: 0;
  gap: 8px;
}

.vote-count {
  font-weight: 700;
  font-size: 1rem;
  color: var(--text-color);
}

.vote-btn {
  width: 28px;
  height: 28px;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: 0;
  border-radius: 4px;
  transition:
    background-color 150ms ease,
    color 150ms ease,
    transform 120ms ease;
}

.vote-btn.p-button {
  background: transparent !important;
  border: none !important;
  box-shadow: none !important;
  color: var(--subtle-text-color) !important;
}

.vote-btn.p-button .pi {
  font-size: 0.9rem;
}

.vote-btn.p-button:hover {
  background: rgba(0, 0, 0, 0.04) !important;
  color: var(--text-color) !important;
  transform: translateY(-1px);
}

.vote-btn.upvote.p-button:hover {
  color: var(--danger-color, #d9534f) !important;
}

.vote-btn.downvote.p-button:hover {
  color: var(--primary-color, #0b74de) !important;
}

.post-text-content {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.post-title {
  font-size: 1.5rem;
  color: var(--surface-color);
}

.post-location {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: var(--subtle-text-color);
}

.location-icon {
  font-size: 1rem;
  color: var(--subtle-text-color);
}

.post-description {
  line-height: 1.5;
  color: var(--text-color);
}

.post-image-container {
  width: 200px;
  flex-shrink: 0;
}

.post-image {
  width: 100%;
  height: 150px;
  object-fit: cover;
  border-radius: 4px;
}

/* Responsive design for smaller screens */
@media (max-width: 768px) {
  .post-content {
    flex-direction: column;
  }

  .post-image-container {
    width: 100%;
    order: -1;
    margin-bottom: 1rem;
  }
}
</style>
