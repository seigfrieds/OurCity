export type CommentCreateRequestDto = {
  authorId: number;
  content: string;
}

export type CommentUpdateRequestDto = {
  content?: string;
}

export type CommentVoteRequestDto = {
  userId: number;
  voteType: 'Upvote' | 'Downvote';
}

export type CommentResponseDto = {
  id: number;
  authorId: number;
  postId: number;
  content: string;
  votes: number;
  isDeleted: boolean;
  createdAt: string;
  updatedAt: string; 
}

export type CommentUpvoteResponseDto = {
  id: number;
  postId: number;
  authorId: number;
  upvoted: boolean;
}

export type CommentDownvoteResponseDto = {
  id: number;
  postId: number;
  authorId: number;
  downvoted: boolean;
}