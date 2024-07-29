using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TradesWomanBE.Models;

namespace TradesWomanBE.Services.Context
{
    public class DataContext : DbContext
    {
        public DbSet<ClientModel> ClientInfo { get; set; }
        public DbSet<RecruiterModel> RecruiterInfo { get; set;}

       public DataContext(DbContextOptions options) : base (options){}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}