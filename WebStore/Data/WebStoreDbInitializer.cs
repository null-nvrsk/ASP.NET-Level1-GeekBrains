using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;

namespace WebStore.Data
{
    public class WebStoreDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<WebStoreDbInitializer> _Logger;

        public WebStoreDbInitializer(WebStoreDB db, ILogger<WebStoreDbInitializer> Logger)
        {
            _db = db;
            _Logger = Logger;
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

    }
}
