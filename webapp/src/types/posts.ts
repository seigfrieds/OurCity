import type { ImageDto } from "./images";

export type PostCreateRequestDto = {
  authorId: number;
  title: string;
  description: string;
  location: string;
}

export type PostUpdateRequestDto = {
  title?: string;
  description?: string;
  location?: string;
}

export type PostVoteRequestDto = {
  userId: number;
  voteType: 'Upvote' | 'Downvote';
}

export type PostResponseDto = {
  id: number;
  authorId: number;
  title: string;
  description: string;
  location: string;
  votes: number;
  commentIds?: number[];
  images?: ImageDto[];
}

export type PostUpvoteResponseDto = {
  id: number;
  authorId: number;
  upvoted: boolean;
}

export type PostDownvoteResponseDto = {
  id: number;
  authorId: number;
  downvoted: boolean;
}
