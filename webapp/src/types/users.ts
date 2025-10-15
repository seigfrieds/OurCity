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