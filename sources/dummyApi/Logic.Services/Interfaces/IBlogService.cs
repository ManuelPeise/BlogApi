using Shared.Models;

namespace Logic.Services.Interfaces
{
    public interface IBlogService
    {
        Task<List<BlogModel>> GetBlogs(bool loadPrivate);
        Task<List<BlogMetaData>> GetPublicBlogMetaDataCollection(bool loadPrivate, int pageSize = 10);
        Task<bool> AddBlog(BlogModel blogModel);
        Task<bool> UpdateBlog(BlogModel blogModel);
        Task<bool> MarkBlogAsDeleted(int blogId);
        Task<bool> DeleteBlog(int blogId);
        Task<bool> RestoreBlog(int id);
        Task<bool> DeleteBlogs();
        Task<bool> AddPostToBlog(PostModel postModel);
        Task<bool> UpdatePost(PostModel postModel);
        Task<bool> DeletePost(int postId);
    }
}
