export type ApiResponse<T> = {
  data: T;
  message: string;
  isSuccess: boolean;
};

export type PostCreateRequestDto = {
  authorId: number;
  title: string;
  description: string;
  location: string;
};

export type PostUpdateRequestDto = {
  title?: string;
  description?: string;
  location?: string;
};

export type PostResponseDto = {
  id: number;
  authorId: number;
  title: string;
  description: string;
  location: string;
  votes: number;
  createdAt: string;
  updatedAt: string;
};

export type ImageResponseDto = {
  url: string;
};

export type UserResponseDto = {
  id: number;
  username: string;
  displayName?: string;
  postIds: number[];
  createdAt: Date;
  updatedAt: Date;
  isDeleted: boolean;
};
