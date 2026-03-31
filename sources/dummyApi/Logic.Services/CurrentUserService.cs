using Data.Database.Entities;
using Data.Database.Interfaces;
using Logic.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Models;
using System.Security.Claims;

namespace Logic.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly HttpContext _httpContext;
        private readonly ILogger<CurrentUserService> _logger;
        private readonly IDbRepositoryBase<UserEntity> _userUnitOfWork;
        private readonly UserModel? _currentUser;

        public UserModel? CurrentUser => _currentUser;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IDbRepositoryBase<UserEntity> userUnitOfWork, ILogger<CurrentUserService> logger)
        {
            _httpContext = httpContextAccessor.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _userUnitOfWork = userUnitOfWork;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Task.Run(async () => await LoadCurrentUser()).Wait();
        }

        public async Task<UserModel?> GetCurrentUser()
        {
            return _currentUser ?? await LoadCurrentUser();
        }

        private async Task<UserModel?> LoadCurrentUser()
        {
            var claims = _httpContext.User.Claims;

            var emailAddress = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(emailAddress))
            {
                throw new UnauthorizedAccessException();
            }

            var userEntity = await _userUnitOfWork.QueryData(true, x => x.Email == emailAddress, x => x.Blog, x => x.Blog.Posts, x => x.Address, x => x.Address.City, x => x.Address.City.Country);

            if (userEntity == null)
            {
                _logger.LogWarning("User with email {Email} not found in database.", emailAddress);
                throw new UnauthorizedAccessException();
            }

            return new UserModel
            {
                Id = userEntity.Id,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Email = userEntity.Email,
                ProfileImage = userEntity.ProfileImage,
                DateOfBirth = userEntity.DateOfBirth,
                BlogId = userEntity.BlogId ?? null,
                Blog = userEntity.Blog != null ? new BlogModel
                {
                    Id = userEntity.Blog.Id,
                    Name = userEntity.Blog.Name,
                    IsPrivate = userEntity.Blog.IsPrivate,
                    CreatedAt = userEntity.Blog.CreatedAt,
                    CreatedBy = userEntity.Blog.CreatedBy,
                    UpdatedAt = userEntity.Blog.UpdatedAt,
                    UpdatedBy = userEntity.Blog.UpdatedBy,
                    Posts = userEntity.Blog.Posts.Select(p => new PostModel
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Image = p.Image,
                        Content = p.Content,
                        CreatedAt = p.CreatedAt,
                        CreatedBy = p.CreatedBy,
                        UpdatedAt = p.UpdatedAt,
                        UpdatedBy = p.UpdatedBy
                    }).ToList()
                } : null,
                AddressId = userEntity.AddressId ?? 0,
                Address = userEntity.Address != null ? new AddressModel
                {
                    AddressId = userEntity.Address.Id,
                    Street = userEntity.Address.Street,
                    HouseNumber = userEntity.Address.HouseNumber,
                    PostalCode = userEntity?.Address?.City?.PostalCode ?? string.Empty,
                    CityId = userEntity?.Address?.CityId ?? 0,
                    CityName = userEntity?.Address?.City?.Name ?? string.Empty,
                    CountryId = userEntity?.Address?.City?.Country?.Id ?? 0,
                    CountryName = userEntity?.Address?.City?.Country?.Name ?? string.Empty,
                } : null,
                CreatedAt = userEntity.CreatedAt,
                CreatedBy = userEntity.CreatedBy,
                UpdatedAt = userEntity.UpdatedAt,
                UpdatedBy = userEntity.UpdatedBy
            };
        }
    }
}
