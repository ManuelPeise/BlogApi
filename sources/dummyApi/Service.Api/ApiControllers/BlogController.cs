using Logic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Service.Api.ApiControllers
{
    
    public class BlogController : ApiControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [ApiAuthentication]
        [HttpGet(Name = "GetBlogs")]
        public async Task<Response<List<BlogModel>>> GetBlogs([FromQuery] bool loadPrivate)
        {
            var blogs = await _blogService.GetBlogs(loadPrivate);

            return new Response<List<BlogModel>>
            {
                Success = true,
                Data = blogs
            };
        }

        [HttpGet(Name = "GetPublicBlogMetaDataCollection")]
        public async Task<Response<List<BlogMetaData>>> GetPublicBlogMetaDataCollection([FromQuery] bool loadPrivate, int page = 1)
        {
            var blogMetaData = await _blogService.GetPublicBlogMetaDataCollection(loadPrivate, page);

            return new Response<List<BlogMetaData>>
            {
                Success = true,
                Data = blogMetaData
            };
        }

        [HttpPost(Name = "CreateBlog")]
        public async Task<ResponseBase> CreateBlog([FromBody] BlogModel blogModel)
        {
            var isCreated = await _blogService.AddBlog(blogModel);

            return new ResponseBase
            {
                Success = isCreated,
            };
        }

        [HttpPost(Name = "UpdateBlog")]
        public async Task<ResponseBase> UpdateBlog([FromBody] BlogModel blogModel)
        {
            var isCreated = await _blogService.UpdateBlog(blogModel);

            return new ResponseBase
            {
                Success = isCreated,
            };
        }

        [HttpPost(Name = "MarkBlogAsDeleted")]
        public async Task<ResponseBase> MarkBlogAsDeleted([FromQuery] int blogId)
        {
            var isMarkedAsDeleted = await _blogService.MarkBlogAsDeleted(blogId);

            return new ResponseBase
            {
                Success = isMarkedAsDeleted,
            };
        }

        [HttpPost(Name = "RestoreBlog")]
        public async Task<ResponseBase> RestoreBlog([FromQuery] int blogId)
        {
            var isRestored = await _blogService.RestoreBlog(blogId);

            return new ResponseBase
            {
                Success = isRestored,
            };
        }
    }
}