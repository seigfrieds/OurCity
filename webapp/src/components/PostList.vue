<script setup lang="ts">
import PostItem from "./PostItem.vue";
import type { PostResponseDto } from '@/types/posts';

const props = defineProps<{
  posts: PostResponseDto[]
}>();
</script>

<template>
  <div class="post-list">
    <router-link
      v-for="post in props.posts"
      :key="post.id"
      :to="`/posts/${post.id}`"
      style="text-decoration: none"
    >
      <PostItem :post="post" />
    </router-link>
  </div>
</template>

<style scoped>
.post-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  padding: 1rem;
}
</style>