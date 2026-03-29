export interface IBlogPost {
  id: number;
  title: string;
  content: string;
  image?: Uint8Array;
  blogId: number;
  createdBy: string;
  createdAt: string;
  updatedBy: string;
  updatedAt: string;
}
