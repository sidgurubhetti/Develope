//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SensorProject.Entities
{
    public class WeatherData
    {
        [Key]
        public string City { get; set; }
        public decimal Temperature { get; set; }
        public decimal Precipitation { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Humidity { get; set; }
    }
}
