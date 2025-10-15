import api from "./axios";
import type { UserResponseDto, UserCreateRequestDto, UserUpdateRequestDto } from "@/types/users.ts";

export async function getUsers(): Promise<UserResponseDto[]> {
  const response = await api.get<UserResponseDto[]>("/users");
  return response.data;
}

export async function getUserById(id: number): Promise<UserResponseDto> {
  const response = await api.get<UserResponseDto>(`/users/${id}`);
  return response.data;
}

export async function createUser(userData: UserCreateRequestDto): Promise<UserResponseDto> {
  const response = await api.post<UserResponseDto>("/users", userData);
  return response.data;
}

export async function updateUser(
  id: number,
  userData: UserUpdateRequestDto,
): Promise<UserResponseDto> {
  const response = await api.put<UserResponseDto>(`/users/${id}`, userData);
  return response.data;
}

export async function deleteUser(id: number): Promise<void> {
  await api.delete(`/users/${id}`);
}
