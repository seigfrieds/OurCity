<!-- Generative AI - CoPilot was used to assist in the creation of this file.
  CoPilot was asked to create a post list component that displays a list of post items. -->
<script setup lang="ts">
import PostItem from "./PostItem.vue";
import posts from "@/data/mockPosts";
</script>

<template>
  <div class="post-list">
    <router-link
      v-for="post in posts"
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
