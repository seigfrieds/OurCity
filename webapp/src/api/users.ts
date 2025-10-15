import type { UserResponseDto } from "@/types/api";
import api from "axios";

export async function getUserByUsername(username: string): Promise<UserResponseDto> {
    const response = await api.get<UserResponseDto>(`/users/name/${username}`);
    return response.data;
}