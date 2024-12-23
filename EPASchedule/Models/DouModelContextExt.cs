using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPASchedule.Models.Deds;
using EPASchedule.Models.Epaemis_local;

namespace EPASchedule
{
    internal class DouModelContextExt
    {
    }

    internal class DedsModelContext : DbContext
    {
        public DedsModelContext() : base("name=DedsModelContext")
        {
            Database.SetInitializer<DedsModelContext>(null);
        }

        public virtual DbSet<AR4_newCarKind> AR4_newCarKind { get; set; }
        public virtual DbSet<AR5_newCarKind> AR5_newCarKind { get; set; }
    }

    internal class MyData : DbContext
    {
        public MyData() : base("name=MyData")
        {
            Database.SetInitializer<MyData>(null);
        }

        public virtual DbSet<z_AR4_newCarKind> z_AR4_newCarKind { get; set; }
        public virtual DbSet<z_AR5_newCarKind> z_AR5_newCarKind { get; set; }
    }
}
