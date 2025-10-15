import type { PostResponseDto } from "@/types/posts";
import type { PostProps } from "@/types/interfaces";

export function mapPostDtoToProps(dto: PostResponseDto, authorName?: string): PostProps {
  return {
    id: dto.id,
    author: authorName ?? (dto as any).authorId ? `User ${(dto as any).authorId}` : `User`,
    title: dto.title,
    location: dto.location,
    description: dto.description,
    votes: dto.votes ?? 0,
    // Map backend images to UI-friendly array of URLs
    // If API doesn't return images, default to empty array
    // The UI checks post.imageUrls for gallery
    imageUrls: dto.images?.map(img => img.url) ?? [],
    comments: [], // load separately if needed
  };
}
