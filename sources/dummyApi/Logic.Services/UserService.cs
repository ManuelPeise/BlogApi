using Data.Database.Entities;
using Logic.Services.Interfaces;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Models;
using System.Linq.Expressions;

namespace Logic.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;

            _logger = logger;
        }

        public async Task<List<UserModel>> GetUsers(bool includeBlog)
        {
            var userList = new List<UserModel>();

            try
            {
                var userEntities = _unitOfWork.UserTable.GetAll(false, includeBlog ? new Expression<Func<UserEntity, object>>[] { e => e.Blog } : null);

                userList = userEntities.Select(e => new UserModel
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    ProfileImage = e.ProfileImage,
                    DateOfBirth = e.DateOfBirth,
                    AddressId = e.AddressId ?? 0,
                    BlogId = e.BlogId,
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
                var userEntity = await _unitOfWork.UserTable.GetByIdAsync(false, id, includeBlog ? new Expression<Func<UserEntity, object>>[] { e => e.Blog.Posts } : null);

                if (userEntity == null)
                {
                    return null;
                }

                var userModel = new UserModel
                {
                    Id = userEntity.Id,
                    FirstName = userEntity.FirstName,
                    LastName = userEntity.LastName,
                    Email = userEntity.Email,
                    ProfileImage = userEntity.ProfileImage,
                    DateOfBirth = userEntity.DateOfBirth,
                    CreatedBy = userEntity.CreatedBy,
                    CreatedAt = userEntity.CreatedAt,
                    AddressId = userEntity.AddressId ?? 0,
                    BlogId = userEntity.BlogId ?? 0,
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
                    ProfileImage = new byte[0],
                    DateOfBirth = null,
                    Blog = null,
                    Address = null,
                    Credentials = new UserCredentialsEntity
                    {
                        PasswordHash = PasswordHasher.HashPassword(signupModel.Password),
                        CreatedAt = timeStamp,
                        CreatedBy = "System"
                    },
                    CreatedBy = "System",
                    CreatedAt = timeStamp
                };

                await _unitOfWork.UserTable.AddAsync(userEntity);

                await _unitOfWork.SaveChanges();

                return true;

            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while creating a new user.");

                return false;
            }
        }

        public async Task<Response<UserModel>> UpdateUser(UserModel userModel)
        {
            try
            {
                var userEntity = await _unitOfWork.UserTable.GetByIdAsync(false, userModel.Id, x => x.Credentials, x => x.Address.City.Country, x => x.Blog) ?? null;

                if (userEntity == null)
                {
                    return new Response<UserModel>
                    {
                        Success = false,
                        Data = null
                    };
                }

                userEntity.FirstName = userModel.FirstName;
                userEntity.LastName = userModel.LastName;
                userEntity.ProfileImage = userModel.ProfileImage;
                userEntity.DateOfBirth = userModel.DateOfBirth;

                await UpdateAddress(userEntity, userModel);

                if (!string.IsNullOrEmpty(userModel?.Credentials?.PasswordHash))
                {
                    userEntity.Credentials.PasswordHash = PasswordHasher.HashPassword(userModel.Credentials.PasswordHash);
                }

                await _unitOfWork.SaveChanges();

                return new Response<UserModel>
                {
                    Success = true,
                    Data = ToUserModel(userEntity)
                };

            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while updating the user.");

                return new Response<UserModel>
                {
                    Success = false,
                    Data = null
                };
            }
        }

        public async Task DeleteUser(int id)
        {
            try
            {
                await _unitOfWork.UserTable.DeleteAsync(id);
                await _unitOfWork.SaveChanges();
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while deleting the user.");
            }
        }

        private async Task UpdateAddress(UserEntity userEntity, UserModel userModel)
        {
            if (userModel.Address == null)
            {
                return;
            }

            userEntity.Address = new AddressEntity();
            userEntity.Address.Street = userModel.Address.Street;
            userEntity.Address.HouseNumber = userModel.Address.HouseNumber;

            if (!string.IsNullOrEmpty(userModel.Address.CityName))
            {
                var cityEntity = await _unitOfWork.CityTable.QueryData(false, x => x.Name.ToLower() == userModel.Address.CityName);

                if (cityEntity == null)
                {
                    var countryEntity = await _unitOfWork.CountryTable.GetByIdAsync(false, userModel.Address.CountryId);

                    userEntity.Address.City = new CityEntity
                    {
                        Name = userModel.Address.CityName,
                        PostalCode = userModel.Address.PostalCode,
                        CountryId = countryEntity?.Id,
                        Country = countryEntity != null ? null : new CountryEntity
                        {
                            Name = userModel.Address.CountryName
                        }
                    };
                }
                else
                {
                    userEntity.Address.CityId = cityEntity.Id;
                }

            }
        }

        private UserModel ToUserModel(UserEntity userEntity)
        {
            return new UserModel
            {
                Id = userEntity.Id,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Email = userEntity.Email,
                DateOfBirth = userEntity.DateOfBirth,
                ProfileImage = userEntity.ProfileImage,
                AddressId = userEntity.AddressId ?? 0,
                Address = userEntity.Address != null ? new AddressModel
                {
                    AddressId = userEntity.Address.Id,
                    Street = userEntity.Address.Street,
                    HouseNumber = userEntity.Address.HouseNumber,
                    CityId = userEntity.Address.CityId ?? 0,
                    PostalCode = userEntity?.Address?.City?.PostalCode ?? string.Empty,
                    CityName = userEntity?.Address?.City?.Name ?? string.Empty,
                    CountryId = userEntity?.Address?.City?.CountryId ?? 0,
                    CountryName = userEntity?.Address.City?.Country?.Name ?? string.Empty
                } : null,
                BlogId = userEntity?.BlogId ?? 0,
                Blog = userEntity?.Blog != null ? new BlogModel
                {
                    Id = userEntity.Blog.Id,
                    Name = userEntity.Blog.Name,
                    IsPrivate = userEntity.Blog.IsPrivate,
                    CreatedBy = userEntity.Blog.CreatedBy,
                    CreatedAt = userEntity.Blog.CreatedAt
                } : null,

                CreatedBy = userEntity?.CreatedBy,
                CreatedAt = userEntity?.CreatedAt
            };
        }
            
    }
}
