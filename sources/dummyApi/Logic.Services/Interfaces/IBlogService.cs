using Shared.Models;

namespace Logic.Services.Interfaces
{
    public interface IBlogService
    {
        Task<List<BlogModel>> GetBlogs(bool loadPrivate);
        Task AddPost(PostModel postModel);
        Task UpdatePost(PostModel postModel);
        Task DeletePost(int postId);
    }
}
