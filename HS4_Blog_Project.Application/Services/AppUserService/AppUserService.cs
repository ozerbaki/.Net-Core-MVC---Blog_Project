﻿using AutoMapper;
using HS4_Blog_Project.Application.Models.DTOs;
using HS4_Blog_Project.Application.Models.VMs;
using HS4_Blog_Project.Domain.Entities;
using HS4_Blog_Project.Domain.Enum;
using HS4_Blog_Project.Domain.Repositories;
using HS4_Blog_Project.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS4_Blog_Project.Application.Services.AppUserService
{
    public class AppUserService : IAppUserService
    {
        private readonly IMapper _mapper; //field
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAppUserRepository _appUserRepository;

        public AppUserService(IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAppUserRepository appUserRepository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _appUserRepository = appUserRepository;
        }

        public async Task<UpdateProfileDTO> GetByUserName(string userName)
        {
            var result = await _appUserRepository.GetFilteredFirstOrDefoult(
                 select: x => new UpdateProfileDTO
                 {
                     Id = x.Id,
                     UserName = x.UserName,
                     Password = x.PasswordHash,
                     Email = x.Email,
                     ImagePath = x.ImagePath
                 },
                 where: x => x.UserName == userName);

            return result;
        }

        public async Task<SignInResult> Login(LoginDTO model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            return result;

        }

        public async Task<IdentityResult> Register(RegisterDTO model)
        {
            //Map'leme yapmalıyız
            var user = _mapper.Map<AppUser>(model);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            return result;
        }


        public async Task UpdateUser(UpdateProfileDTO model)
        {
            // Update işlemlerinde önce id ile ilgili nesne ram'a çekilir. Dışarıdan gelen güncel bilgilerle değişiklikleri yaparım. En son SaveChanges ile veritabanına güncellemeleri gönderirim.

            var user = await _appUserRepository.GetDefault(x => x.Id == model.Id);

            if (model.UploadPath != null)
            {
                using var image = Image.Load(model.UploadPath.OpenReadStream());

                image.Mutate(x => x.Resize(600, 560));
                Guid guid = Guid.NewGuid();
                image.Save($"wwwroot/images/{guid}.jpg");
                user.ImagePath = $"/images/{guid}.jpg";

                await _appUserRepository.Update(user);
            }

            if (model.Password != null)
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);

                await _userManager.UpdateAsync(user);
            }
            //else
            //{
            //    throw Exception("Şifre daha önce kullanılmış!");
            //}

            if (model.UserName != null)
            {
                var isUserNameExists = await _userManager.FindByNameAsync(model.UserName);

                if (isUserNameExists == null)
                {
                    await _userManager.SetUserNameAsync(user, model.UserName);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
            }

            if (model.Email != null)
            {
                var isUserEmailExists = await _userManager.FindByNameAsync(model.Email);

                if (isUserEmailExists == null)
                {
                    await _userManager.SetEmailAsync(user, model.Email);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
            }
        }
    }
}
