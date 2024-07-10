using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hotel.Models;
using System.ComponentModel.DataAnnotations;
namespace Hotel.ViewModel
{
    public class LoginView
    {
        [Required]
        [Display(Name = "Логин")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]

        public string Password { get; set; }
        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }


    public class CustomSerializeModel
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public List<string> Role { get; set; }

    }

    


}