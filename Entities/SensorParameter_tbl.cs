using System.ComponentModel.DataAnnotations;

namespace SensorProject.Entities
{
    public class SensorParameter_tbl
    {
        [Key]
        public int SensorParameterId { get; set; }
        public string? SensorParameterName { get; set; }
        public  int? SensorId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
    }
}
