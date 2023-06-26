using SensorProject.Entities;
using SensorProject.Models.Dto;

namespace SensorProject.IRepository
{
    public interface ISensorRepository
    {
        Task<ResponseDetails> AddSensor(Sensor_tbl sensor_Tbl);
        Task<ResponseDetails> AddSensorParameter(List<SensorParameter_tbl> sensorParaObj);
        Task<ResponseDetails> AddEditParameterHistory(ParameterHistoryDto parameterHistoryObj);

        
    }
}
