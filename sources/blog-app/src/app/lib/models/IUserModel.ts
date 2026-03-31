import { IAddress } from './IAddress';
import { IBlog } from './IBlog';

export interface IUserModel {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  profileImage: Blob | null;
  dateOfBirth: string;
  blogId: number | null;
  blog: IBlog | null;
  addressId: number | null;
  address: IAddress | null;
  createdBy: string;
  createdAt: string;
  updatedBy: string;
  updatedAt: string;
}
