export interface PostProps {
  id: string | number;
  author: string;
  title: string;
  location?: string;
  description: string;
  votes?: number;
  comments: CommentProps[];
}

export interface CommentProps {
  id: string | number;
  author: string;
  text: string;
  votes?: number;
  replies: CommentProps[];
}

export interface ReplyProps {
  id: string | number;
  author: string;
  text: string;
  votes?: number;
}
