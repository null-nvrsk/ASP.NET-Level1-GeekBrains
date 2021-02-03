using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Data
{
    public static class TestData
    {
        public static List<Employee> employees { get; } = new()
        {
            new Employee { Id = 1, FirstName = "Михаил", LastName = "Родионов", Patronymic = "Елисеевич", Age = 54, Position = "Генеральный секретарь", Department = "Администрация" },
            new Employee { Id = 2, FirstName = "Сергей", LastName = "Белов", Patronymic = "Даниилович", Age = 33, Position = "Бухгалтер", Department = "Бухгалтерия" },
            new Employee { Id = 3, FirstName = "Дарья", LastName = "Фомина", Patronymic = "Степановна", Age = 24, Position = "Оператор call-центра", Department = "Коммерческий отдел" },
        };

    }
}
