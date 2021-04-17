using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Models;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _EmployeesData;

        public EmployeesController(IEmployeesData employeesData)
        {
            _EmployeesData = employeesData;
        }

        public IActionResult Index()
        {
            return View(_EmployeesData.Get());
        }

        public IActionResult Details(int id)
        {
            var employee = _EmployeesData.Get(id);
            if (employee is not null)
                return View(employee);
            return NotFound();
        }

        public IActionResult Create()
        {
            return View("Edit", new EmployeeViewModel());
        }

        #region Edit            

        [HttpGet]
        [Authorize(Roles = Role.Administrator)]
        public IActionResult Edit(int id)
        {
            if (id <= 0) return BadRequest();

            var employee = _EmployeesData.Get(id);

            if (employee is null) return NotFound();

            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                LastName = employee.LastName,
                Name = employee.FirstName,
                MiddleName = employee.Patronymic,
                Age = employee.Age,
                Position = employee.Position,
                Department = employee.Department
            });
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            if (model.Name == "Усама" && model.MiddleName == "бен" && model.LastName == "Ладен")
                ModelState.AddModelError("", "Вы уже умерли!");

            if (!ModelState.IsValid) return View(model);

            var employee = new Employee
            {
                Id = model.Id,
                LastName = model.LastName,
                FirstName = model.Name,
                Patronymic = model.MiddleName,
                Age = model.Age,
                Position = model.Position,
                Department = model.Department
            };

            if (employee.Id == 0)
                _EmployeesData.Add(employee);
            else
                _EmployeesData.Update(employee);

            return RedirectToAction("Index");
        }

        #endregion

        #region Delete

        [HttpGet]
        [Authorize(Roles = Role.Administrator)]
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest();

            var employee = _EmployeesData.Get(id);

            if (employee is null) return NotFound();

            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                LastName = employee.LastName,
                Name = employee.FirstName,
                MiddleName = employee.Patronymic,
                Age = employee.Age,
                Position = employee.Position,
                Department = employee.Department
            });
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public IActionResult DeleteConfermed(int id)
        {
            _EmployeesData.Delete(id);
            return RedirectToAction("Index");
        }

        #endregion
    }
}
