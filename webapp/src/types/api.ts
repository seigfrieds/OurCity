export type ApiResponse<T> = {
  data: T;
  message: string;
  isSuccess: boolean;
}

export type ImageDto = {
  url: string;
}

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

export type PostResponseDto = {
  id: number;
  authorId: number;
  title: string;
  description: string;
  location: string;
  votes: number;
  createdAt: string;
  updatedAt: string; 
  images?: ImageDto[];
}

export type UserResponseDto = {
  id: number;
  username: string,
  displayName?: string,
  postIds: number[];
  createdAt: string;
  updatedAt: string;
  isDeleted: boolean;
}

export type UserCreateRequestDto = {
  username: string;
  displayName?: string;
}

export type UserUpdateRequestDto = {
  username?: string;
  displayName?: string;
}