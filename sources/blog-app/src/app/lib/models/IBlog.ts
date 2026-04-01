import { IBlogPost } from './IBlogPost';

export interface IBlog {
  id: number;
  name: string;
  description: string;
  image: Blob | null;
  isPrivate: boolean;
  isMarkedAsDeleted: boolean;
  isMarkedAsDeletedAt: string | null;
  createdBy: string;
  createdAt: string;
  updatedBy: string;
  updatedAt: string;
  posts: IBlogPost[];
}
