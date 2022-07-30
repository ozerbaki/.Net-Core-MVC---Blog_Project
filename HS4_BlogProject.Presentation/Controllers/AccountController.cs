using HS4_Blog_Project.Application.Models.DTOs;
using HS4_Blog_Project.Application.Services.AppUserService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HS4_BlogProject.Presentation.Controllers
{
    /*
        Core Identity
        Create, Update, Delete user accounts
        Account Configuration
        Auhentication & Authorization
        Password Recovery
        Two-Factor Authentication with sms
        microsoft, facebook, google login providers
    */
    public class AccountController : Controller
    {
        private readonly IAppUserService _appUserService;

        public AccountController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) //Eğer kullanıcı hali hazırda sisteme Authenticate olmuşsa
            {
                return RedirectToAction("Index", ""); // Areas
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            //Service
            //AppUserService.Create(registerDTO)

            if (ModelState.IsValid)
            {
                var result = await _appUserService.Register(registerDTO);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "");

                }
                //Identity'nin içerisinde gömülü olarak bulunan Errors listesinin içerisinde dolaşıyoruz. result error ile dolarsa hataları yazdırıyoruz.
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                    TempData["Error"] = "Something went wrong";
                }
            }
            return View();

        }


        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", ""); // Areas
            }
            return View();

        }

        [HttpPost]
        public IActionResult Login(LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                _appUserService.Login(model);
            }
            return RedirectToAction("Index", ""); // Areas
        }

        /*
        Jira, Azure gibi platformlar üzerinden iş parçalarımız bulunmaktadır.
        Jira, Efor, Kaynak seçimi, Süre, Task
        Profil güncelleme işini üzerime aldığımızı düşünelim
        Wireframescathers, Balsamic
            1- Controller'a gelip Action oluşturalım
            2- View sayfasını geliştirelim
            3- UpdateProfileDTO class'ını oluşturalım
            4- GetByUserNamemetodunu Application katmanında Service'in içine yazarız. Interface ve Conrete olarak
            5- Service içinde AppUserRepository'i inject etmem gerekiyor
            6- Service içinde GetByUserName metodunu duldurduk
            7- Repository üzerinden GetFiltredFirstOrDefaoult metodunu çağırdım
            8- Task, async, await keyworlerini yazdım. Asenkron hale getirdim.
        */

        public async Task<IActionResult> Edit(string username)
        {
            //Kullanıcı bilgilerini edit edeceğiz.
            var user = await _appUserService.GetByUserName(username);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProfileDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _appUserService.UpdateUser(model);
                }
                catch (Exception)
                {

                    TempData["Error"] = "Your profile hasn't been updated";
                }
                //Success mesajı yazılabilir.
                return RedirectToAction("Index", ""); //Areas

            }
            else
            {
                TempData["Error"] = "Your profile hasn't been updated";
                return View(model);
            }
        }
    }
}
