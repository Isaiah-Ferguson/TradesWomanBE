using Microsoft.EntityFrameworkCore;
using TradesWomanBE.Models;

namespace TradesWomanBE.Services.Context
{
    public class DataContext : DbContext
    {
        public DbSet<ClientModel> ClientInfo { get; set; }
        public DbSet<RecruiterModel> RecruiterInfo { get; set;}
        public DbSet<ProgramModel> Programs { get; set; }
        public DbSet<MeetingsModel> Meetings { get; set; }
        public DbSet<MeetingNotesModel> MeetingNotes { get; set; }
        public DbSet<CTWIStipendsModel> Stipends { get; set; }
       public DataContext(DbContextOptions options) : base (options){}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}