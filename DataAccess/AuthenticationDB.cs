using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.Entity;
using Hotel.Models;

namespace Hotel.DataAccess
{
    public class AuthenticationDB: DbContext
    {
        public AuthenticationDB(): base("name=hosteldat2Entities")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Actor>()
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Roles> Roles { get; set; }
    }
}