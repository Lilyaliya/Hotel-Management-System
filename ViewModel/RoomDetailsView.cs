using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotel.ViewModel
{
    public class RoomDetailsView
    {
        public int RoomNum { get; set; }
        public int C_Floor { get; set; }
        public Nullable<int> Places { get; set; }
        public String RoomImage { get; set; }
        public decimal cost { get; set; }
        public string dirty { get; set; }
        public string CategoryClass { get; set; }
        public string CategoryWindow { get; set; }
        public string CategoryEating { get; set; }


    }
}