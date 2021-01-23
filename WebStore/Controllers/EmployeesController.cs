using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private List<Employee> _employees;

        public EmployeesController()
        {
            _employees = TestData.employees;
        }

        public IActionResult Index()
        {
            return View(_employees);
        }
    }
}
