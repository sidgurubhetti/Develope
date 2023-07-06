using SensorProject.Entities;
using SensorProject.Models.Dto;

namespace SensorProject.IRepository
{
    public interface ISensorDataRepository
    {
        Task<List<ParameterHistoryDto>> GetAllParameterHistory();
        Task<ResponseDetails> AddEditParameterHistory(ParameterHistoryDto parameterHistoryObj);
        Task<List<ParameterHistoryDto>> GetParameterHistoryBySensorId(int sensorId);


    }
}
