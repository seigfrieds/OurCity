import type { PostResponseDto } from "@/types/api";
import type { PostProps } from "@/types/interfaces";

export function mapPostDtoToProps(dto: PostResponseDto, authorName?: string): PostProps {
  return {
    id: dto.id,
    author: authorName ?? `User ${dto.authorId}`,
    title: dto.title,
    location: dto.location,
    description: dto.description,
    votes: dto.votes ?? 0,
    comments: [], // load separately if needed
  };
}
