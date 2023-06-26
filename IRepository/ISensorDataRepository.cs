using SensorProject.Entities;
using SensorProject.Models.Dto;

namespace SensorProject.IRepository
{
    public interface ISensorDataRepository
    {
        //Task<ResponseDetails> AddSensorData(SensorData_tbl sensorData_tbl);

        Task<List<SensorData_tbl>> GetSensorData();
        //Task<List<SensorData_tbl>> GetAll();

    }
}
