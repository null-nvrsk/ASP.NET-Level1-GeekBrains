using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.ViewModels
{
    public class EmployeeViewModel : IValidatableObject
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; init; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Фамилия обязательна")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Длина фамилии должна быть от 2 до 15 символов")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Неверный формат фамилии")]
        public string LastName { get; init; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Длина имени должна быть от 2 до 15 символов")]
        
        public string Name { get; init; }

        [Display(Name = "Отчество")]
        public string MiddleName { get; init; }

        [Display(Name = "Возраст")]
        [Range(18, 90, ErrorMessage = "Возраст должен быть от 18 до 90 лет")]
        public int Age { get; init; }

        [Display(Name = "Должность")]
        public string Position { get; init; }

        [Display(Name = "Отдел")]
        public string Department { get; init; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ValidationResult.Success;
            //yield return new ValidationResult("Текст ошибки", new[] { typeof(Name) });
        }
    }
}
