import api from "./axios";
import type {
  CommentResponseDto,
  CommentCreateRequestDto,
  CommentUpdateRequestDto,
  CommentVoteRequestDto,
} from "@/types/comments.ts";

export async function getComments(postId: number): Promise<CommentResponseDto[]> {
  const response = await api.get<CommentResponseDto[]>(`Posts/${postId}/comments`);
  return response.data;
}

export async function getCommentById(postId: number, id: number): Promise<CommentResponseDto> {
  const response = await api.get<CommentResponseDto>(`Posts/${postId}/comments/${id}`);
  return response.data;
}

export async function createComment(
  postId: number,
  commentData: CommentCreateRequestDto,
): Promise<CommentResponseDto> {
  const response = await api.post<CommentResponseDto>(`Posts/${postId}/comments`, commentData);
  return response.data;
}

export async function updateComment(
  postId: number,
  id: number,
  commentData: CommentUpdateRequestDto,
): Promise<CommentResponseDto> {
  const response = await api.put<CommentResponseDto>(`Posts/${postId}/comments/${id}`, commentData);
  return response.data;
}

export async function deleteComment(postId: number, id: number): Promise<void> {
  await api.delete(`Posts/${postId}/comments/${id}`);
}

export async function voteComment(
  commentId: number,
  postId: number,
  userId: number,
  voteType: "Upvote" | "Downvote",
): Promise<CommentResponseDto> {
  const voteData: CommentVoteRequestDto = { userId, voteType };
  const response = await api.post<CommentResponseDto>(
    `Posts/${postId}/comments/${commentId}/vote`,
    voteData,
  );
  return response.data;
}
