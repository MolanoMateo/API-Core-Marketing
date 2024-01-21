using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using COREAPI.Models;

namespace COREAPI.Data
{
    public class COREAPIContext : DbContext
    {
        public COREAPIContext (DbContextOptions<COREAPIContext> options)
            : base(options)
        {
        }

        public DbSet<COREAPI.Models.Usuario> Usuario { get; set; } = default!;
        public DbSet<COREAPI.Models.KPIXCampana> KPIXCampana { get; set; }

        public DbSet<COREAPI.Models.KPI> KPI { get; set; }

        public DbSet<COREAPI.Models.Campana> Campana { get; set; }

        public DbSet<COREAPI.Models.Buyer> Buyer { get; set; }
    }
}
