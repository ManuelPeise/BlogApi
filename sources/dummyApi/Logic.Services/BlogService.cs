using Data.Database.Entities;
using Data.Database.Interfaces;
using Logic.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Models;
using System.Linq.Expressions;

namespace Logic.Services
{
    public class BlogService : IBlogService
    {
        private readonly IDbRepositoryBase<BlogEntity> _blogRepository;
        private readonly IDbRepositoryBase<PostEntity> _postRepository;
        private readonly ILogger<BlogService> _logger;

        public BlogService(
            IDbRepositoryBase<BlogEntity> blogRepository,
            IDbRepositoryBase<PostEntity> postRepository,
            ILogger<BlogService> logger)
        {
            _blogRepository = blogRepository;
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task<List<BlogModel>> GetBlogs(bool loadPrivate)
        {
            try
            {
                var entities = _blogRepository.GetAll(true, new Expression<Func<BlogEntity, object>>[] { e => e.Posts });

                var filteredEntities = loadPrivate ? entities : entities.Where(e => !e.IsPrivate);

                return filteredEntities.Select(e => new BlogModel
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Image = e.Image,
                    IsPrivate = e.IsPrivate,
                    CreatedAt = e.CreatedAt,
                    CreatedBy = e.CreatedBy,
                    Posts = e.Posts.Select(p => new PostModel
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Content = p.Content,
                        Image = p.Image,
                        BlogId = p.BlogId,
                        CreatedAt = p.CreatedAt,
                        CreatedBy = p.CreatedBy
                    }).ToList()
                }).ToList();
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while retrieving blogs.");
                return new List<BlogModel>();
            }
        }

        public async Task AddPost(PostModel postModel)
        {
            try
            {
                var entity = new PostEntity
                {
                    Title = postModel.Title,
                    Content = postModel.Content,
                    Image = postModel.Image ?? new byte[0],
                    BlogId = postModel.BlogId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                };

                await _postRepository.AddAsync(entity);
                await _postRepository.SaveChanges();

            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while retrieving blogs.");

            }
        }

        public async Task UpdatePost(PostModel postModel)
        {
            try
            {
                var entity = await _postRepository.GetByIdAsync(false, postModel.Id);

                if (entity == null)
                {
                    _logger.Log(LogLevel.Warning, "Post with ID {PostId} not found for update.", postModel.Id);
                    return;
                }
                entity.Title = postModel.Title;
                entity.Content = postModel.Content;
                entity.Image = postModel.Image;

                await _postRepository.UpdateAsync(false, entity);
                await _postRepository.SaveChanges();
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while retrieving blogs.");
            }
        }

        public async Task DeletePost(int postId)
        {
            try
            {
                await _postRepository.DeleteAsync(postId);
                await _postRepository.SaveChanges();
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while retrieving blogs.");

            }
        }
    }
}
