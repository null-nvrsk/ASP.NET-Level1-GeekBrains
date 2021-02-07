using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Controllers
{
    //[Controller]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public AccountController(UserManager<User> UserManager, RoleManager<Role> RoleManager)
        {
            userManager = UserManager;
            roleManager = RoleManager;
        }

    }
}
