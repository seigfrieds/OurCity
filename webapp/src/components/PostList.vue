<script lang="ts">
import type { PropType } from "vue";
import PostItem from "./PostItem.vue";

interface PostProps {
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
    PostItem,
  },
  props: {
    posts: {
      type: Array as PropType<PostProps[]>,
      required: true,
    },
  },
};
</script>

<template>
  <div class="post-list">
    <PostItem
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
