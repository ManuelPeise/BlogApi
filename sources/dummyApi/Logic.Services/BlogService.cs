using Data.Database.Entities;
using Data.Database.Interfaces;
using Logic.Services.Interfaces;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Models;

namespace Logic.Services
{
    public class BlogService : ALogicBase, IBlogService
    {
        private readonly IDbRepositoryBase<BlogEntity> _blogRepository;
        private readonly IDbRepositoryBase<PostEntity> _postRepository;
        private readonly ILogger<BlogService> _logger;

        public BlogService(
            ICurrentUserService currentUserService,
            IDbRepositoryBase<BlogEntity> blogRepository,
            IDbRepositoryBase<PostEntity> postRepository,
            ILogger<BlogService> logger) : base(currentUserService)
        {
            _blogRepository = blogRepository;
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task<List<BlogModel>> GetBlogs(bool loadPrivate)
        {
            try
            {
                var entities = _blogRepository.GetAll(true, e => e.Posts);

                var filteredEntities = loadPrivate ? entities : entities.Where(e => !e.IsMarkedAsDeleted && !e.IsPrivate || e.UserId == CurrentUser.Id);

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

        public async Task<bool> AddBlog(BlogModel blogModel)
        {
            try
            {
                var entity = new BlogEntity
                {
                    Title = blogModel.Title,
                    Description = blogModel.Description,
                    Image = blogModel.Image ?? new byte[0],
                    IsPrivate = blogModel.IsPrivate,
                    UserId = CurrentUser.Id,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                };
                await _blogRepository.AddAsync(entity);
                await _blogRepository.SaveChanges();

                return true;
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while retrieving blogs.");
            }

            return false;
        }

        public async Task<bool> UpdateBlog(BlogModel blogModel)
        {
            try
            {
                var entity = await _blogRepository.GetByIdAsync(false, blogModel.Id);
                if (entity == null)
                {
                    _logger.Log(LogLevel.Warning, "Blog with ID {BlogId} not found for update.", blogModel.Id);
                    return false;
                }

                entity.Title = blogModel.Title;
                entity.Description = blogModel.Description;
                entity.Image = blogModel.Image;
                entity.IsPrivate = blogModel.IsPrivate;

                await _blogRepository.SaveChanges();

                return true;
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while retrieving blogs.");
            }
            return false;
        }

        public async Task<bool> MarkBlogAsDeleted(int blogId)
        {
            try
            {
                var entity = await _blogRepository.GetByIdAsync(false, blogId);
                if (entity == null)
                {
                    _logger.Log(LogLevel.Warning, "Blog with ID {BlogId} not found for deletion.", blogId);
                    return false;
                }
                entity.IsMarkedAsDeleted = true;
                entity.MarkedAsDeletedAt = DateTime.UtcNow;

                await _blogRepository.UpdateAsync(false, entity);
                await _blogRepository.SaveChanges();
                return true;
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while retrieving blogs.");
            }
            return false;
        }

        public async Task<bool> DeleteBlog(int id)
        {
            try
            {
                await _blogRepository.DeleteAsync(id);
                await _blogRepository.SaveChanges();

                return true;
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while retrieving blogs.");
            }

            return false;
        }

        public async Task<bool> RestoreBlog(int id)
        {
            try
            {
                var entity = await _blogRepository.GetByIdAsync(false, id);
                
                if (entity == null)
                {
                    _logger.Log(LogLevel.Warning, "Blog with ID {BlogId} not found for restoration.", id);
                    return false;
                }

                entity.IsMarkedAsDeleted = false;
                entity.MarkedAsDeletedAt = null;
                
                await _blogRepository.SaveChanges();

                return true;
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while retrieving blogs.");
            }

            return false;
        }

        public async Task<bool> DeleteBlogs()
        {
            try
            {
                var entities = _blogRepository.GetAll(false, e => e.IsMarkedAsDeleted);

                if (entities == null || !entities.Any())
                {
                    _logger.Log(LogLevel.Warning, "No blogs found for restoration.");
                    return false;
                }
                await _blogRepository.DeleteRangeAsync(entities.Select(e => e.Id));

                await _blogRepository.SaveChanges();

                return true;
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while retrieving blogs.");
            }
            return false;
        }

        public async Task<bool> AddPostToBlog(PostModel postModel)
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

                return true;
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while retrieving blogs.");

            }

            return false;
        }

        public async Task<bool> UpdatePost(PostModel postModel)
        {
            try
            {
                var entity = await _postRepository.GetByIdAsync(false, postModel.Id);

                if (entity == null)
                {
                    _logger.Log(LogLevel.Warning, "Post with ID {PostId} not found for update.", postModel.Id);
                    return false;
                }

                entity.Title = postModel.Title;
                entity.Content = postModel.Content;
                entity.Image = postModel.Image;

                await _postRepository.SaveChanges();

                return true;
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while retrieving blogs.");
            }
            return false;
        }

        public async Task<bool> DeletePost(int postId)
        {
            try
            {
                await _postRepository.DeleteAsync(postId);
                await _postRepository.SaveChanges();
                return true;
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while retrieving blogs.");
            }
            return false;
        }
    }
}
