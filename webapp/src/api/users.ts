import api from "./axios";
import type { UserResponseDto } from "@/types/api";

export async function getUsers(): Promise<UserResponseDto[]> {
  const response = await api.get<UserResponseDto[]>("/users");
  return response.data;
}

export async function getUserById(id: number): Promise<UserResponseDto> {
  const response = await api.get<UserResponseDto>(`/users/${id}`);
  return response.data;
}

export async function createUser(userData: {
  username: string;
  displayName?: string;
}): Promise<UserResponseDto> {
  const response = await api.post<UserResponseDto>("/users", userData);
  return response.data;
}

export async function updateUser(id: number, userData: {
  username?: string;
  displayName?: string;
}): Promise<UserResponseDto> {
  const response = await api.put<UserResponseDto>(`/users/${id}`, userData);
  return response.data;
}

export async function deleteUser(id: number): Promise<UserResponseDto> {
  const response = await api.delete<UserResponseDto>(`/users/${id}`);
  return response.data;
}