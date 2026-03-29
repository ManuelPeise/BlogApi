import { IBlogPost } from './IBlogPost';

export interface IBlog {
  id: number;
  name: string;
  description: string;
  createdBy: string;
  createdAt: string;
  updatedBy: string;
  updatedAt: string;
  posts: IBlogPost[];
}
