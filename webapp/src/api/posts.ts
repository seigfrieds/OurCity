import api from "./axios";
import type { ApiResponse, PostResponseDto } from "@/types/api";

export async function getPosts(): Promise<ApiResponse<PostResponseDto[]>> {
  const response = await api.get<ApiResponse<PostResponseDto[]>>("/posts");
  return response.data;
}

export async function getPostById(id: number): Promise<ApiResponse<PostResponseDto>> {
  const response = await api.get<ApiResponse<PostResponseDto>>(`/posts/${id}`);
  return response.data;
}

export async function createPost(postData: {
  authorId: number;
  title: string;
  description: string;
  location: string;
}): Promise<ApiResponse<PostResponseDto>> {
  const response = await api.post<ApiResponse<PostResponseDto>>("/posts", postData);
  return response.data;
}

export async function updatePost(id: number, postData: {
  title?: string;
  description?: string;
  location?: string;
}): Promise<ApiResponse<PostResponseDto>> {
  const response = await api.put<ApiResponse<PostResponseDto>>(`/posts/${id}`, postData);
  return response.data;
}

export async function deletePost(id: number): Promise<ApiResponse<null>> {
  const response = await api.delete<ApiResponse<null>>(`/posts/${id}`);
  return response.data;
}

export async function upvotePost(id: number): Promise<ApiResponse<PostResponseDto>> {
  const response = await api.post<ApiResponse<PostResponseDto>>(`/posts/${id}/upvote`);
  return response.data;
}

export async function downvotePost(id: number): Promise<ApiResponse<PostResponseDto>> {
  const response = await api.post<ApiResponse<PostResponseDto>>(`/posts/${id}/downvote`);
  return response.data;
}
