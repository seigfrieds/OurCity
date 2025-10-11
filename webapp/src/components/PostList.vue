<script lang="ts">
import type { PropType } from "vue";
import Post from "./Post.vue";

interface PostItem {
  id: string | number;
  title: string;
  location: string;
  description: string;
  imageUrl: string;
  votes?: number;
}

export default {
  name: "PostList",
  components: {
    Post,
  },
  props: {
    posts: {
      type: Array as PropType<PostItem[]>,
      required: true,
    },
  },
};
</script>

<template>
  <div class="post-list">
    <Post
      v-for="post in posts"
      :key="post.id"
      :title="post.title"
      :location="post.location"
      :description="post.description"
      :imageUrl="post.imageUrl"
    />
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
