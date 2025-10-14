import http from './http';

export interface ImageDto {
  url: string;
}

export interface PostCreateRequestDto {
  title: string;
  description: string;
  location: string | null;
  images: ImageDto[];
}

export interface PostUpdateRequstDto {
  title?: string;
  description?: string;
  location?: string | null;
  images?: ImageDto[];
}

export interface PostResponseDto {
  id: number;
  title: string;
  description: string;
  location?: string;
  images: ImageDto[];
  createdAt: string;
  updatedAt: string;
}

class PostApi {
  async createPost(data: PostCreateRequestDto): Promise<PostResponseDto> {
    return http.post<PostResponseDto, PostCreateRequestDto>(`/posts`, 
      {...data}
    );
  }

  async getAllPosts(): Promise<PostResponseDto[]> {
    return http.get<PostResponseDto[]>(`/posts`);
  }

  async getPostById(postId: number): Promise<PostResponseDto> {
    return http.get<PostResponseDto>(`/posts/${postId}`);
  }

  async updatePost(token: string | null, postId: number, data: PostUpdateRequstDto): Promise<PostResponseDto> {
    return http.put<PostResponseDto, PostUpdateRequstDto>(`/posts/${postId}`, 
      {...data}
    );
  }

  async deletePost(postId: number): Promise<void> {
    return http.delete<PostResponseDto>(`/posts/${postId}`);
  }
}

export default new PostApi();
