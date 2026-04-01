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
                var userEntities = _unitOfWork.UserTable.GetAll(false, includeBlog ? new Expression<Func<UserEntity, object>>[] { e => e.Blogs } : null);

                userList = userEntities.Select(ToUserModel).ToList();

                return await Task.FromResult(userList);
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while retrieving users.");

                return await Task.FromResult(userList);
            }
        }

        public async Task<UserModel?> GetUserById(int id, bool includeBlog)
        {
            try
            {
                var userEntity = await _unitOfWork.UserTable.GetByIdAsync(
                    false,
                    id,
                    includeBlog
                        ? new Expression<Func<UserEntity, object>>[] { e => e.Blogs, e => e.Blogs.Select(b => b.Posts) }
                        : null);

                if (userEntity == null)
                {
                    return null;
                }

                var userModel = ToUserModel(userEntity);
                return userModel;
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, exception, "An error occurred while retrieving user by ID {UserId}.", id);
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
                    Blogs = new List<BlogEntity>(),
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
                _logger.Log(LogLevel.Error, exception, "An error occurred while creating a new user with email {Email}.", signupModel.Email);

                return false;
            }
        }

        public async Task<Response<UserModel>> UpdateUser(UserModel userModel)
        {
            try
            {
                var userEntity = await _unitOfWork.UserTable.GetByIdAsync(false, userModel.Id, x => x.Credentials, x => x.Address.City.Country);

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
                _logger.Log(LogLevel.Error, exception, "An error occurred while updating the user {UserId}.", userModel.Id);

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
                _logger.Log(LogLevel.Error, exception, "An error occurred while deleting the user {UserId}.", id);
            }
        }

        private async Task UpdateAddress(UserEntity userEntity, UserModel userModel)
        {
            if (userModel.Address == null)
            {
                return;
            }

            if (userEntity.Address != null)
            {
                userEntity.Address.Street = userModel.Address.Street;
                userEntity.Address.HouseNumber = userModel.Address.HouseNumber;

                var normalizedCityName = userModel.Address.CityName?.ToLower();
                var cityEntity = await _unitOfWork.CityTable.QueryData(false, x => x.Name.ToLower() == normalizedCityName);


                if (cityEntity == null && !string.IsNullOrEmpty(userModel?.Address?.CityName))
                {
                    userEntity.Address.City = new CityEntity
                    {
                        Name = userModel.Address.CityName,
                        PostalCode = userModel.Address.PostalCode,
                    };
                }
                else
                {
                    userEntity.Address.CityId = cityEntity?.Id;
                }

                if (!string.IsNullOrEmpty(userModel?.Address?.CountryName))
                {
                    var countryEntity = await _unitOfWork.CountryTable.QueryData(false, x => x.Name == userModel.Address.CountryName);

                    if (userEntity.Address?.City != null && countryEntity != null)
                    {
                        userEntity.Address.City.CountryId = countryEntity.Id;

                    }

                    if (userEntity?.Address?.City != null && countryEntity == null)
                    {
                        userEntity.Address.City.Country = new CountryEntity
                        {
                            Name = userModel.Address.CountryName,
                        };
                    }
                }
            }
            else
            {
                userEntity.Address = new AddressEntity
                {
                    Street = userModel.Address.Street,
                    HouseNumber = userModel.Address.HouseNumber,
                };
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
                IsMarkedAsDeleted = userEntity.MarkedAsDeleted,
                MarkedAsDeletedAt = userEntity.MarkedAdDeletedAt,
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
                Blogs = userEntity?.Blogs != null ? userEntity.Blogs.Select(b => new BlogModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    Image = b.Image,
                    IsPrivate = b.IsPrivate,
                    IsMarkedAsDeleted = b.IsMarkedAsDeleted,
                    MarkedAsDeletedAt = b.MarkedAsDeletedAt,
                    Posts = b.Posts != null ? b.Posts.Select(p => new PostModel
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Content = p.Content,
                        BlogId = p.Blog.Id,
                        CreatedBy = p.CreatedBy,
                        CreatedAt = p.CreatedAt
                    }).ToList() : new List<PostModel>(),
                    CreatedBy = b.CreatedBy,
                    CreatedAt = b.CreatedAt
                }).ToList() : new List<BlogModel>(),
                CreatedBy = userEntity?.CreatedBy,
                CreatedAt = userEntity?.CreatedAt
            };
        }

    }
}
