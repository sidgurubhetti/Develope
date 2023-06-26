using System.ComponentModel.DataAnnotations;

namespace SensorProject.Entities
{
    public class SensorParametersData_tbl
    {
        [Key]
        public int SensorDataId { get; set; }
        public int SensorId { get; set; }
        public DateTime DateTime { get; set; }
        public float P1 { get; set; }
        public float P2 { get; set; }
        public float P3 { get; set; }
        public float P4 { get; set; }
        public float P5 { get; set; }
        public float P6 { get; set; }
        public float P7 { get; set; }
        public float P8 { get; set; }
        public float P9 { get; set; }
        public float P10 { get; set; }
    }
}
