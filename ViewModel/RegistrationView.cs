using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace Hotel.ViewModel
{
    public class RegistrationView
    {
        [Required(ErrorMessage = "Укажите логин")]
        [Display(Name = "Логин")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Введите ФИО!")]
        [Display(Name = "Полное имя")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Укажите эл. почту")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Укажите контактный номер")]
        [Display(Name = "Контактный номер")]
        [Phone(ErrorMessage = "Некорректный номер!")]
        public string ContactPhone { get; set; }

        [Required(ErrorMessage = "Введите серию и номер паспорта")]
        [Display(Name = "Паспорт")]
        public string Passport { get; set; }

        [Required(ErrorMessage = "Задайте пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Вы не подтвердили пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Ошибка : пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }

    public class RegistrationViewEmp
    {
        [Required(ErrorMessage = "Укажите логин")]
        [Display(Name = "Логин")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Введите ФИО!")]
        [Display(Name = "Полное имя")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Укажите эл. почту")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        
        
        public List<SelectListItem> ListRoles { get; set; }

        [Display(Name = "Роль сотрудника")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Укажите контактный номер")]
        [Display(Name = "Контактный номер")]
        [Phone(ErrorMessage = "Некорректный номер!")]
        public string ContactPhone { get; set; }

        [Required(ErrorMessage = "Введите серию и номер паспорта")]
        [Display(Name = "Паспорт")]
        public string Passport { get; set; }

        [Required(ErrorMessage = "Задайте пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Вы не подтвердили пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Ошибка : пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}