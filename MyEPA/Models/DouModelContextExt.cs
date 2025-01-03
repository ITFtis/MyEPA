using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class DouModelContextExt
    {
    }

    internal class DedsModelContext : DbContext
    {
        public DedsModelContext() : base("name=DedsModelContext")
        {
            Database.SetInitializer<DedsModelContext>(null);
        }

        public virtual DbSet<MyEPA.Models.Deds.User> User { get; set; }
    }
}