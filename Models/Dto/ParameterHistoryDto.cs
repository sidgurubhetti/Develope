namespace SensorProject.Models.Dto
{
    public class ParameterHistoryDto
    {
        public int Id { get; set; }
        public int? SensorId { get; set; }
        public ParameterDto parameterObj { get; set; }
    }
}
