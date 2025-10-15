import api from "./axios";
import type { PostResponseDto } from "@/types/api";

export async function getPosts(): Promise<PostResponseDto[]> {
  const response = await api.get<PostResponseDto[]>("/posts");
  return response.data;
}

export async function getPostById(id: number): Promise<PostResponseDto> {
  const response = await api.get<PostResponseDto>(`/posts/${id}`);
  return response.data;
}

export async function createPost(postData: {
  authorId: number;
  title: string;
  description: string;
  location: string;
}): Promise<PostResponseDto> {
  const response = await api.post<PostResponseDto>("/posts", postData);
  return response.data;
}

export async function updatePost(id: number, postData: {
  title?: string;
  description?: string;
  location?: string;
}): Promise<PostResponseDto> {
  const response = await api.put<PostResponseDto>(`/posts/${id}`, postData);
  return response.data;
}

export async function deletePost(id: number): Promise<PostResponseDto> {
  const response = await api.delete<PostResponseDto>(`/posts/${id}`);
  return response.data;
}

export async function votePost(id: number, upvote: boolean): Promise<PostResponseDto> {
  const response = await api.post<PostResponseDto>(`/posts/${id}/vote`, { upvote });
  return response.data;
}