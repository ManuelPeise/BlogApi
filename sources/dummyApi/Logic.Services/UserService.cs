using Data.Database.Entities;
using Data.Database.Interfaces;
using Logic.Services.Interfaces;
using Logic.Shared;
using Microsoft.Extensions.Logging;
using Shared.Models;
using System.Linq.Expressions;

namespace Logic.Services
{
    public class UserService : IUserService
    {
        private readonly IDbRepositoryBase<UserEntity> _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IDbRepositoryBase<UserEntity> userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<List<UserModel>> GetUsers(bool includeBlog)
        {
            var userList = new List<UserModel>();

            try
            {
                var userEntities = _userRepository.GetAll(false, includeBlog ? new Expression<Func<UserEntity, object>>[] { e => e.Blog } : null);

                userList = userEntities.Select(e => new UserModel
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    CreatedBy = e.CreatedBy,
                    CreatedAt = e.CreatedAt
                }).ToList();

                return userList;
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while retrieving users.");

                return userList;
            }
        }

        public async Task<UserModel?> GetUserById(int id, bool includeBlog)
        {
            try
            {
                var userEntity = await _userRepository.GetByIdAsync(false, id, includeBlog ? new Expression<Func<UserEntity, object>>[] { e => e.Blog.Posts } : null);

                if (userEntity == null)
                {
                    return null;
                }

                var userModel = new UserModel
                {
                    Id = userEntity.Id,
                    FirstName = userEntity.FirstName,
                    LastName = userEntity.LastName,
                    CreatedBy = userEntity.CreatedBy,
                    CreatedAt = userEntity.CreatedAt,
                    Blog = userEntity.Blog != null ? new BlogModel
                    {
                        Id = userEntity.Blog.Id,
                        Name = userEntity.Blog.Name,
                        IsPrivate = userEntity.Blog.IsPrivate,
                        CreatedBy = userEntity.Blog.CreatedBy,
                        CreatedAt = userEntity.Blog.CreatedAt,
                        Posts = userEntity.Blog.Posts != null ? userEntity.Blog.Posts.Select(p => new PostModel
                        {
                            Id = p.Id,
                            Title = p.Title,
                            Content = p.Content,
                            BlogId = p.Blog.Id,
                            CreatedBy = p.CreatedBy,
                            CreatedAt = p.CreatedAt
                        }).ToList() : new List<PostModel>()
                    } : null
                };
                return userModel;
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while retrieving user by ID.");
                return null;
            }
        }

        public async Task<bool> CreateUser(SignupModel signupModel)
        {
            try
            {
                var timeStamp = DateTime.UtcNow;

                var userEntity = new UserEntity
                {
                    FirstName = signupModel.FirstName,
                    LastName = signupModel.LastName,
                    Email = signupModel.Email,
                    Blog = null,
                    Credentials = new UserCredentialsEntity
                    {
                        PasswordHash = PasswordHasher.HashPassword(signupModel.Password),
                        CreatedAt = timeStamp,
                        CreatedBy = "System"
                    },
                    CreatedBy = "System",
                    CreatedAt = timeStamp
                };

                await _userRepository.AddAsync(userEntity);

                await _userRepository.SaveChanges();

                return true;

            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while creating a new user.");

                return false;
            }
        }

        public async Task<UserModel?> UpdateUser(UserModel userModel)
        {
            try
            {
                var userEntity = await _userRepository.GetByIdAsync(true, userModel.Id);
                if (userEntity == null)
                {
                    return null;
                }

                userEntity.FirstName = userModel.FirstName;
                userEntity.LastName = userModel.LastName;
                userEntity.CreatedBy = userModel.CreatedBy;
                userEntity.CreatedAt = DateTime.UtcNow;

                await _userRepository.UpdateAsync(false, userEntity);
                await _userRepository.SaveChanges();

                return new UserModel
                {
                    Id = userEntity.Id,
                    FirstName = userEntity.FirstName,
                    LastName = userEntity.LastName,
                    CreatedBy = userEntity.CreatedBy,
                    CreatedAt = userEntity.CreatedAt
                };

            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while updating the user.");
                return null;
            }
        }

        public async Task DeleteUser(int id)
        {
            try
            {
                await _userRepository.DeleteAsync(id);
                await _userRepository.SaveChanges();
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while deleting the user.");
            }
        }
    }
}
