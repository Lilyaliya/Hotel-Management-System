using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModel
{
    public class BookingViewModel
    {
        [Display(Name ="Помещение")]
        
        public int Room { get; set; }
        public IEnumerable<SelectListItem> ListRooms { get; set; }
        [Display(Name ="Сотрудник смены")]
        public int Agent { get; set; }
        public int ClientID { get; set; }
        [Display(Name ="Посетитель")]
        public string Client { get; set; }

        public string FullName { get; set; }
        public string Contacts { get; set; }

        public IEnumerable<SelectListItem> ListClients { get; set; }
        public System.DateTime CreatedAt { get; set; }
        [Display(Name = "Дата заселения")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd-MM-yyyy}",ApplyFormatInEditMode =true)]
        [Required(ErrorMessage ="Заполните дату заселения!")]
        public System.DateTime Arrival { get; set; }
        [Display(Name = "Дата выселения")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true )]
        [Required(ErrorMessage = "Заполните дату заселения!")]
        public System.DateTime Departure { get; set; }
        [Display(Name = "Итого к оплате")]
        public decimal Bill { get; set; }

        [Display(Name ="Продолжительность")]// сколько дней живет
        public  int Days { get; set; }
        public Nullable<int> Receipt { get; set; }
        public string C_Contract { get; set; }
    }
}