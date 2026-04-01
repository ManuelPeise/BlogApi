using Logic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Service.Api.ApiControllers
{
    [ApiAuthentication]
    public class BlogPostController : ApiControllerBase
    {
        private readonly IBlogService _blogService;
        
        public BlogPostController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpPost(Name = "AddPostToBlog")]
        public async Task AddPostToBlog([FromBody] PostModel postModel)
        {
            await _blogService.AddPostToBlog(postModel);
        }

        [HttpPost(Name = "UpdatePost")]
        public async Task UpdatePost([FromBody] PostModel postModel)
        {
            await _blogService.UpdatePost(postModel);
        }

        [HttpDelete(Name = "DeletePost")]
        public async Task DeletePost([FromQuery] int postId)
        {
            await _blogService.DeletePost(postId);
        }
    }
}
