using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("AddSensorData")]
        /*public async Task<IActionResult> AddSensorData(SensorData_tbl sensorData_tbl)
        {
            if (!ModelState.IsValid)
            {
                var errorResponse = new ResponseDetails()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Invalid sensor data.",
                    data = ModelState
                };
                return BadRequest(errorResponse);
            }
            var responseDetails = await _sensorDataRepository.AddSensorData(sensorData_tbl);
            return Ok(responseDetails);
        }*/
        [HttpGet]
        public async Task<IActionResult> GetSensorData()
        {
            try
            {
                var sensorData = await _sensorDataRepository.GetSensorData();

                foreach (var data in sensorData)
                {
                    var parameterValues = new List<int>();

                    for (int i = 1; i <= data.TotalParameter; i++)
                    {
                        var parameterValue = (int)data.GetType().GetProperty($"Parameter{i}").GetValue(data);
                        parameterValues.Add(parameterValue);
                    }

                    Console.WriteLine($"SensorDataId: {data.SenDataId}, SensorId: {data.SensorId}, DateTime: {data.DateTime}, Parameters: [{string.Join(", ", parameterValues)}]");
                }

                return Ok(sensorData);
            }
            catch (Exception ex)
            {
                // Log the exception and return an appropriate error response
                return StatusCode(500, "An error occurred while retrieving sensor data.");
            }
        }


    }
}

/*{
  "sensorId": 3,
  "totalParameter": 5,
  "dateTime": "2023-06-26T11:30:41.193Z",
  "p1": 55,
  "p2": 22,
  "p3": 42,
  "p4": 23,
  "p5": 35
}*/
