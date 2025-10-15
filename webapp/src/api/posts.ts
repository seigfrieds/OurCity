import api from "./axios";
import type { 
  PostResponseDto,
  PostCreateRequestDto,
  PostUpdateRequestDto,
  PostVoteRequestDto,
} from "@/types/posts.ts";

export async function getPosts(): Promise<PostResponseDto[]> {
  const response = await api.get<PostResponseDto[]>("/posts");
  return response.data;
}

export async function getPostById(id: number): Promise<PostResponseDto> {
  const response = await api.get<PostResponseDto>(`/posts/${id}`);
  return response.data;
}

export async function createPost(postData: PostCreateRequestDto): Promise<PostResponseDto> {
  const response = await api.post<PostResponseDto>("/posts", postData);
  return response.data;
}

export async function updatePost(
  id: number,
  postData: PostUpdateRequestDto
): Promise<PostResponseDto> {
  const response = await api.put<PostResponseDto>(`/posts/${id}`, postData);
  return response.data;
}

export async function deletePost(id: number): Promise<void> {
  await api.delete(`/posts/${id}`);
}

export async function votePost(
  postId: number,
  userId: number,
  voteType: 'Upvote' | 'Downvote'
): Promise<PostResponseDto> {
  const voteData: PostVoteRequestDto = { userId, voteType };
  const response = await api.post<PostResponseDto>(`/posts/${postId}/vote`, voteData);
  return response.data;
}