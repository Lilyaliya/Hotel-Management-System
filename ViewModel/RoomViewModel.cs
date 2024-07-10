using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModel
{
    public class RoomViewModel
    {
        [Display(Name ="Номер помещения")]
        [Required(ErrorMessage ="Введите номер помещения")]
        public int RoomNum { get; set; }
        [Display(Name = "Количество мест")]
        [Required(ErrorMessage = "Введите количество мест")]
        [Range(1, 300, ErrorMessage ="В номере может проживать максимум 8 человек")]
        public Nullable<int> Places { get; set; }
        [Display(Name = "Класс номера")]
        //[Required(ErrorMessage = "выберите класс номера")]
        public List<SelectListItem> ListCategoryClass { get; set; }
        [Display(Name = "Вид из окна")]
        public List<SelectListItem> ListCategoryWindow { get; set; }
        [Display(Name = "Питание")]
        public List<SelectListItem> ListCategoryEating { get; set; }
        [Display(Name = "Нуждается в очистке")]
        public List<SelectListItem> ListDirty { get; set; }

        public string CategoryClass { get; set; }
        public string CategoryWindow { get; set; }
        public string CategoryEating { get; set; }
        [Display(Name = "Нуждается в очистке")]
        public string dirty { get; set; }

        [Display(Name = "Этаж №")]
        [Required(ErrorMessage = "Введите этаж для комнаты")]
        public int C_Floor { get; set; }
        [Display(Name = "Стоимость номера")]
        [Required(ErrorMessage = "Укажите стоимость номера")]
        [Range(1000f, 1000000f, ErrorMessage ="Минимальная стоимость услуги - 1000 р")]
        public decimal cost { get; set; }
        
        
        [Display(Name = "Фото номера")]
        public HttpPostedFileBase Image { get; set; }

        public string RoomImage { get; set; }

    }
}