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
        //public DbSet<SensorParametersData_tbl>  SensorParametersData_Tbls { get; set; }
        public DbSet<WeatherData> WeatherData { get; set; }
        public DbSet<SensorData_tbl> SensorData_tbl { get; set; }
        public DbSet<SensorParameterHistory_tbl> SensorParameterHistory_tbl { get; set; }
        public DbSet<tbl_ParameterHistory> tbl_ParameterHistory { get; set; }






        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("users");

            //builder.Entity<SensorParametersData_tbl>().ToTable("SensorParametersData_tbl");

            builder.Entity<WeatherData>(entity =>
            {
                entity.Property(e => e.Temperature)
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Precipitation)
                    .HasColumnType("decimal(10, 2)");
            });

            builder.Entity<SensorData_tbl>().ToTable("SensorData_tbl");

            builder.Entity<SensorParameterHistory_tbl>().ToTable("SensorParameterHistory_tbl");

        }
    }
}
