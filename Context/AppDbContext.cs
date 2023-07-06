using Microsoft.EntityFrameworkCore;
using SensorProject.Entities;
using System.Reflection.Emit;


namespace SensorProject.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<SensorParameter_tbl> SensorParameter_tbl { get; set; }
        public DbSet<Sensor_tbl> Sensor_tbl { get; set; }
        public DbSet<tbl_ParameterHistory> tbl_ParameterHistory { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("users");

        }
    }
}
