using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPASchedule.Models.Deds;

namespace EPASchedule.Models
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
}
