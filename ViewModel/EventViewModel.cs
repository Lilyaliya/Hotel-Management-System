using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModel
{
    public class EventViewModel
    {
        public int Agent { get; set; }
        [Display(Name = "Посетитель")]
        //здесь хранится ID клиента
        public string Client { get; set; }
        public int ClientID { get; set; }
        //имя клиента
        public string FullName { get; set; }
        public string ContactPhone { get; set; }
        [Display(Name = "Название мероприятия")]
        [Required(ErrorMessage = "Укажите название мероприятия!")]
        public string Name { get; set; }
        [Display(Name = "Краткое описание мероприятия")]
        [Required(ErrorMessage = "Добавьте описание!")]
        public string Description { get; set; }
        [Display(Name = "Дата мероприятия")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Заполните дату мероприятия!")]
        public System.DateTime C_Date { get; set; }
        [Display(Name = "Количество гостей")]
        [Required(ErrorMessage = "Укажите количество гостей!")]
        public int Visitors { get; set; }
        public Nullable<int> ReceiptID { get; set; }
        [Display(Name = "Помещение")]
        public int selectedRoom { get; set; }
        // это строковый вариант выбора номера (через selectListItem)
        public string selected { get; set; }
        public IEnumerable<SelectListItem> ListRooms { get; set; }

       
        public List<SelectListItem> ListCategoryClass { get; set; }

        public IEnumerable<SelectListItem> ListClients { get; set; }
        [Display(Name = "Класс помещения")]
        public string CategoryClass { get; set; }

        public ICollection<ItemRooms> Rooms { get; set; }

        [Display(Name = "Итого к оплате")]
        public decimal Bill { get; set; }

    }

    public class ItemRooms
    {
        public int Room { get; set; }
        public string CategoryClass { get; set; }
    }
}