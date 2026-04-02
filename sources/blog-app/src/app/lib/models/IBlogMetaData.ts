export interface IBlogMetaData {
  blogId: number;
  title: string;
  description: string;
  author: string;
  image?: string | null;
  postCount: number;
  lastPostingDate: string;
}
