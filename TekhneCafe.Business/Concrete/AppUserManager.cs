using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TekhneCafe.Business.Abstract;
using TekhneCafe.Business.Helpers.FilterServices;
using TekhneCafe.Business.Helpers.HeaderServices;
using TekhneCafe.Business.Models;
using TekhneCafe.Core.DTOs.AppUser;
using TekhneCafe.Core.Exceptions;
using TekhneCafe.Core.Exceptions.AppUser;
using TekhneCafe.Core.Extensions;
using TekhneCafe.Core.Filters.AppUser;
using TekhneCafe.DataAccess.Abstract;
using TekhneCafe.Entity.Concrete;

namespace TekhneCafe.Business.Concrete
{
    public class AppUserManager : IAppUserService
    {
        private readonly IAppUserDal _userDal;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IImageService _imageService;
        private readonly IConfiguration _configuration;

        public AppUserManager(IAppUserDal userDal, IMapper mapper, IHttpContextAccessor httpContextAccessor, IImageService imageService, IConfiguration configuration)
        {
            _userDal = userDal;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _httpContext = httpContextAccessor.HttpContext;
            _imageService = imageService;
            _configuration = configuration;
        }

        public List<AppUserListDto> GetUserList(AppUserRequestFilter filters = null)
        {
            var users = _userDal.GetAll().AsNoTracking();
            var filteredResult = new AppUserFilterService().FilterAppUsers(users, filters);
            new HeaderService(_httpContextAccessor).AddToHeaders(filteredResult.Headers);
            return _mapper.Map<List<AppUserListDto>>(filteredResult.ResponseValue);
        }

        public async Task<AppUser> GetUserByLdapIdAsync(string id)
        {
            var user = await _userDal.GetAll(_ => _.LdapId == Guid.Parse(id)).FirstOrDefaultAsync();
            AddEndpointToUserImage(user);
            return user;
        }

        public async Task<AppUser> GetRawUserByIdAsync(string id)
        {
            AppUser user = await _userDal.GetByIdAsync(Guid.Parse(id));
            ThrowExceptionUserNotExists(user);
            return user;
        }

        public async Task<AppUserListDto> GetUserByIdAsync(string id)
        {
            AppUser appUser = await _userDal.GetByIdAsync(Guid.Parse(id));
            ThrowExceptionUserNotExists(appUser);
            return _mapper.Map<AppUserListDto>(appUser);
        }

        public async Task<AppUser> CreateUserAsync(AppUserAddDto userDto)
        {
            AppUser user = _mapper.Map<AppUser>(userDto);
            await _userDal.AddAsync(user);
            return user;
        }

        public async Task UpdateUserPhoneAsync(string phone)
        {
            Guid userId = Guid.Parse(_httpContext.User.ActiveUserId());
            AppUser user = await _userDal.GetByIdAsync(userId);
            user.Phone = phone;
            await _userDal.UpdateAsync(user);
        }

        public async Task<string> UpdateUserImageAsync(string bucketName)
        {
            UploadImageRequest request = new()
            {
                BucketName = bucketName,
                Image = _httpContext.Request.Form.Files.FirstOrDefault()
            };
            var user = await _userDal.GetByIdAsync(Guid.Parse(_httpContext.User.ActiveUserId()));
            string oldImagePath = user.ImagePath;
            string imagePath = await _imageService.UploadImageAsync(request);
            user.ImagePath = imagePath;
            try
            {
                await UpdateUserAsync(user);
            }
            catch
            {
                await RemoveUserImage(bucketName, imagePath);
                throw new InternalServerErrorException();
            }
            if (oldImagePath != null)
                await RemoveUserImage(bucketName, oldImagePath);
            return string.Concat(_configuration.GetValue<string>("Minio:Endpoint"), "/", imagePath);
        }


        public async Task UpdateUserAsync(AppUser user)
            => await _userDal.UpdateAsync(user);

        public void ThrowExceptionUserNotExists(AppUser user)
        {
            if (user is null)
                throw new AppUserNotFoundException();
        }

        private void AddEndpointToUserImage(AppUser user)
        {
            if (user != null)
                user.ImagePath = !string.IsNullOrEmpty(user.ImagePath) ? string.Concat(_configuration.GetValue<string>("Minio:Endpoint"), "/", user.ImagePath) : null;
        }

        private async Task RemoveUserImage(string bucketName, string path)
            => await _imageService.RemoveImageAsync(new RemoveImageRequest() { BucketName = bucketName, ObjectName = path.Replace(bucketName + "/", "") });
    }
}
