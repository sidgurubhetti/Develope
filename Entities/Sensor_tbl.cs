using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SensorProject.Entities
{
    public class Sensor_tbl
    {
        [Key]
        [JsonIgnore]
        public int SensorId { get; set; }

        [Required(ErrorMessage = "SensorName is required.")]
        public string? SensorName { get; set; }

        [Required(ErrorMessage = "Count is required.")]
        [Range(1, 10, ErrorMessage = "You can use 1 to 10 parameters.")]
        public int TotalParameter { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }


    }
}
