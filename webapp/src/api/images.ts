import api from "axios";
import type { ImageResponseDto } from "@/types/api";

export async function uploadImage(postId: number, files: File[]): Promise<ImageResponseDto> {
  const formData = new FormData();

  files.forEach((file) => {
    formData.append("files", file);
  });

  const response = await api.post<ImageResponseDto>(`/images/upload/${postId}`, formData);
  return response.data;
}
