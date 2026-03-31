export interface IChangePasswordRequest {
  userId: number;
  currentPassword: string;
  updatedPassword: string;
}
