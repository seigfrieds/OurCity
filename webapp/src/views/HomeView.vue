<script setup lang="ts">
import { ref, onMounted } from 'vue'
import PageHeader from '@/components/PageHeader.vue'
import { getPosts } from '@/api/posts'
import type { PostResponseDto } from '@/types/api'
import PostList from '@/components/PostList.vue'

const posts = ref<PostResponseDto[]>([])

onMounted(async () => {
  try {
    const response = await getPosts()
    posts.value = response.data // Adjust if your API response shape is different
  } catch (error) {
    console.error('Failed to fetch posts:', error)
  }
})
</script>

<template>
  <div class="home-view">
    <PageHeader />
    <div class="content">
      <PostList :posts="posts" />
    </div>
  </div>
</template>
