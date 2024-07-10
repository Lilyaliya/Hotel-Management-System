using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hotel.Models;

namespace Hotel.ViewModel
{
    public class ChangeViewModel
    {
        public int ID { get; set; }
        public Nullable<int> Actors { get; set; }
        public DateTime ChangeDate { get; set; }

        public Actor CurrActor { get; set; }
        public List<Actor> ActorsList { get; set; }
        
    }
}