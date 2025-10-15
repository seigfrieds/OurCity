import api from "./axios";
import type { CommentResponseDto } from "@/types/comments";

export async function getComments(): Promise<CommentResponseDto[]> {
  const response = await api.get<CommentResponseDto[]>("/comments");
  return response.data;
}

export async function getCommentById(id: number): Promise<CommentResponseDto> {
  const response = await api.get<CommentResponseDto>(`/comments/${id}`);
  return response.data;
}

export async function createComment(commentData: {
  authorId: number;
  content: string;
  postId: number;
}): Promise<CommentResponseDto> {
  const response = await api.post<CommentResponseDto>("/comments", commentData);
  return response.data;
}

export async function updateComment(id: number, commentData: {
  content?: string;
}): Promise<CommentResponseDto> {
  const response = await api.put<CommentResponseDto>(`/comments/${id}`, commentData);
  return response.data;
}

export async function deleteComment(id: number): Promise<CommentResponseDto> {
  const response = await api.delete<CommentResponseDto>(`/comments/${id}`);
  return response.data;
}

export async function voteComment(id: number, upvote: boolean): Promise<CommentResponseDto> {
  const response = await api.post<CommentResponseDto>(`/comments/${id}/vote`, { upvote });
  return response.data;
}