using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; init; }
        public string LastName { get; init; }
        public string Name { get; init; }
        public string MiddleName { get; init; }
        public int Age { get; init; }
        public string Position { get; init; }
        public string Department { get; init; }
    }
}
