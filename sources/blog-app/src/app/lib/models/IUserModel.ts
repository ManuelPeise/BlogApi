import { IAddress } from './IAddress';
import { IBlog } from './IBlog';

export interface IUserModel {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  profileImage: Blob | null;
  dateOfBirth: string;
  isMarkedAsDeleted: boolean;
  isMarkedAsDeletedAt: string | null;
  blogs: IBlog[];
  addressId: number | null;
  address: IAddress | null;
  createdBy: string;
  createdAt: string;
  updatedBy: string;
  updatedAt: string;
}
