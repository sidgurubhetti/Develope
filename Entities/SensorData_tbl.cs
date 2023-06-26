    using System;
    using System.ComponentModel.DataAnnotations;

    namespace SensorProject.Entities
    {
        public class SensorData_tbl
        {
            [Key]
            public int SenDataId { get; set; }
            public int SensorId { get; set; }
            public int TotalParameter { get; set; }
            public DateTime DateTime { get; set; }
            public int Parameter1 { get; set; }
            public int Parameter2 { get; set; }
            public int Parameter3 { get; set; }
            public int Parameter4 { get; set; }
            public int Parameter5 { get; set; }
            public int Parameter6 { get; set; }
            public int Parameter7 { get; set; }
            public int Parameter8 { get; set; }
            public int Parameter9 { get; set; } 
            public int Parameter10 { get; set; }
        }
    }
