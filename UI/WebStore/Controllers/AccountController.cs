﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels;

namespace WebStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;
        private readonly ILogger<AccountController> _Logger;

        public AccountController(UserManager<User> UserManager, SignInManager<User> SignInManager, ILogger<AccountController> Logger)
        {
            _UserManager = UserManager;
            _SignInManager = SignInManager;
            _Logger = Logger;
        }

        #region Register

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);

            _Logger.LogInformation($"Регистрация пользователя {Model.UserName}");

            var user = new User
            {
                UserName = Model.UserName
            };

            var registration_result = await _UserManager.CreateAsync(user, Model.Password);
            if (registration_result.Succeeded)
            {
                _Logger.LogInformation($"Пользователь {Model.UserName} успешно зарегистрирован");

                await _UserManager.AddToRoleAsync(user, Role.Users);
                _Logger.LogInformation($"Пользователь {Model.UserName} наделен ролью {Role.Users}");

                await _SignInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");
            }

            _Logger.LogWarning("В процессе регистрации пользователя {0} возникли ошибки {1}",
                Model.UserName,
                string.Join(',', registration_result.Errors.Select(e => e.Description)));

            foreach (var error in registration_result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(Model);
        }

        #endregion

        #region Login

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl) => View(new LoginViewModel
        {
            ReturnUrl = ReturnUrl
        });

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);

            var login_result = await _SignInManager.PasswordSignInAsync(
                Model.UserName,
                Model.Password,
                Model.RememberMe,
#if DEBUG
                false
#else
                true
#endif
                );

            if (login_result.Succeeded)
            {
                return LocalRedirect(Model.ReturnUrl ?? "/");
            }


            ModelState.AddModelError("", "Неверное имя пользователя или пароль!");

            return View(Model);
        }

        #endregion

        public async Task<IActionResult> Logout()
        {
            await _SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
    }
}
