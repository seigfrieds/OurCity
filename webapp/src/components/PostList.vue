<script setup lang="ts">
import PostItem from "./PostItem.vue";
import type { PostProps } from "@/types/interfaces";

defineProps<{ posts: PostProps[] }>();
</script>

<template>
  <div class="post-list">
    <router-link
      v-for="post in posts"
      :key="post.id"
      :to="`/posts/${post.id}`"
      style="text-decoration: none"
    >
      <PostItem
        :title="post.title"
        :location="post.location"
        :description="post.description"
        :imageUrl="post.imageUrl"
      />
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
