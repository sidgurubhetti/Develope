using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SensorProject.Entities;
using SensorProject.IRepository;
using SensorProject.Models.Dto;

namespace SensorProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorDataController : ControllerBase
    {
        private readonly ISensorDataRepository _sensorDataRepository;
        public SensorDataController(ISensorDataRepository sensorDataRepository)
        {
            _sensorDataRepository = sensorDataRepository;
        }


        [HttpPost("AddEditParameterHistory")]
        public async Task<IActionResult> AddEditParameterHistory(ParameterHistoryDto sensorHistoryObj)
        {
            var responseDetails = await _sensorDataRepository.AddEditParameterHistory(sensorHistoryObj);
            return Ok(responseDetails);
        }

        [HttpGet("GetAllParameterHistory")]
        public async Task<IActionResult> GetAllParameterHistory()
        {
            var parameterHistoryList = await _sensorDataRepository.GetAllParameterHistory();
            return Ok(parameterHistoryList);
        }

        [HttpGet("GetParameterHistoryBySensorId/{sensorId}")]
        public async Task<IActionResult> GetParameterHistoryBySensorId(int sensorId)
        {
            var parameterHistoryList = await _sensorDataRepository.GetParameterHistoryBySensorId(sensorId);
            if (parameterHistoryList == null || parameterHistoryList.Count == 0)
            {
                return NotFound();
            }

            return Ok(parameterHistoryList);
        }
    }
}

