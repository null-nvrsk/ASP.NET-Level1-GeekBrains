﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Services.Data
{
    public class WebStoreDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<WebStoreDbInitializer> _Logger;
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;

        public WebStoreDbInitializer(
            WebStoreDB db, 
            ILogger<WebStoreDbInitializer> Logger,
            UserManager<User> UserManager,
            RoleManager<Role> RoleManager)
        {
            _db = db;
            _Logger = Logger;
            _UserManager = UserManager;
            _RoleManager = RoleManager;
        }
        public void Initialize()
        {
            var timer = Stopwatch.StartNew();

            _Logger.LogInformation("Инициализация базы данных...");
            //_db.Database.EnsureDeleted();
            //_db.Database.EnsureCreated();

            var db = _db.Database;

            if (db.GetPendingMigrations().Any())
            {
                _Logger.LogInformation("Выполнение миграций...");
                db.Migrate();
                _Logger.LogInformation("Миграций выполнены успешно");
            }
            else
                _Logger.LogInformation("БД находится в актуальном состоянии ({0:0.0###} с)", timer.Elapsed.TotalSeconds);            

            try
            {
                InitializeProducts();
                InitializeIdentityAsync().Wait();
            }
            catch (Exception error)
            {
                _Logger.LogError(error, "Ошибка при выполнении инициализации БД");
                throw;
            }

            _Logger.LogInformation("Иницализация БД выполнены успешно {0}", timer.Elapsed.TotalSeconds);
        }

        private void InitializeProducts()
        {
            var timer = Stopwatch.StartNew();

            if (_db.Products.Any())
            {
                _Logger.LogInformation("Иницализация БД товаров не требуется");
                return;
            }


            _Logger.LogInformation("Инициализация товаров...");

            _Logger.LogInformation("Добавлние секций...");
            using (_db.Database.BeginTransaction())
            {
                _db.Sections.AddRange(TestData.Sections);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] ON");
                _db.SaveChanges();
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] OFF");

                _db.Database.CommitTransaction();
                _Logger.LogInformation("Секции успешно добавлены в БД");
            }

            _Logger.LogInformation("Добавлние брендов...");
            using (_db.Database.BeginTransaction())
            {
                _db.Brands.AddRange(TestData.Brands);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] ON");
                _db.SaveChanges();
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] OFF");

                _db.Database.CommitTransaction();
                _Logger.LogInformation("Бренды успешно добавлены в БД");
            }

            _Logger.LogInformation("Добавлние товаров...");
            using (_db.Database.BeginTransaction())
            {
                _db.Products.AddRange(TestData.Products);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] ON");
                _db.SaveChanges();
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] OFF");

                _db.Database.CommitTransaction();
                _Logger.LogInformation("Товары успешно добавлены в БД");
            }

            _Logger.LogInformation("Инициализация товаров выполнена успешно ({0:0.0###} с)", timer.Elapsed.TotalSeconds);
        }

        private async Task InitializeIdentityAsync()
        {
            var timer = Stopwatch.StartNew();
            _Logger.LogInformation("Инициализация системы Identity...");


            async Task CheckRole(string RoleName)
            {
                if (!await _RoleManager.RoleExistsAsync(RoleName))
                {
                    _Logger.LogInformation($"Роль {RoleName} отсутвует. Создаю...");
                    await _RoleManager.CreateAsync(new Role { Name = RoleName });
                    _Logger.LogInformation($"Роль {RoleName} создана успешно");
                }
            }

            await CheckRole(Role.Administrator);
            await CheckRole(Role.Users);

            if (await _UserManager.FindByNameAsync(User.Administrator) is null)
            {
                _Logger.LogInformation("Отсутствует учетная запись администратора");
                var admin = new User
                {
                    UserName = User.Administrator
                };

                var creation_result = await _UserManager.CreateAsync(admin, User.DefaultAdminPassword);
                if (creation_result.Succeeded)
                {
                    _Logger.LogInformation("Учетная запись администратора создана успешно");
                    await _UserManager.AddToRoleAsync(admin, Role.Administrator);
                    _Logger.LogInformation("Учетная запись администратора наделена ролью {0}", Role.Administrator);
                }
                else
                {
                    var errors = creation_result.Errors.Select(e => e.Description);
                    throw new InvalidOperationException($"Ошибка при создании учетной записи администратора: {string.Join(",", errors)}");
                }
            }

            _Logger.LogInformation("Инициализация системы Identity завершена успешно за {0:0.0##} с", timer.Elapsed.Seconds);
        }
    }
}
