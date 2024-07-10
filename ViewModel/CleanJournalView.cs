using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Hotel.ViewModel
{
    public class CleanJournalView
    {
        [Display(Name ="Комната №")]
        public int Room { get; set; }
        [Display(Name ="Комната №")]
        public string RoomView { get; set; }
        [Display(Name = "Комната №")]
        public IEnumerable<SelectListItem> ListRooms { get; set; }
        [Display(Name ="Смена №")]

        public int ChangeID { get; set; }

        [Display(Name ="Уборщик")]
        public string FullName { get; set; }

        public int Actor { get; set; }

        [Display(Name = "Время уборки")]
        [DataType(DataType.Time)]
        //[DataType(DataFormatString = "{0:hh\\:mm\\:ss}", ApplyFormatInEditMode =true)] 

        public System.DateTime Today { get; set; }


    }
}