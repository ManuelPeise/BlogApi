import { IBlog } from './IBlog';

export interface IUserModel {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  blogId: number | null;
  blog: IBlog | null;
  createdBy: string;
  createdAt: string;
  updatedBy: string;
  updatedAt: string;
}
