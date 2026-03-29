using Logic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Service.Api.ApiControllers
{
    [ApiAuthentication]
    public class BlogController : ApiControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet(Name = "GetBlogs")]
        public async Task<List<BlogModel>> GetBlogs([FromQuery] bool loadPrivate)
        {
            return await _blogService.GetBlogs(loadPrivate);
        }
    }
}