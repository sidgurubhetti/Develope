using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorProject.Entities;
using SensorProject.IRepository;
using SensorProject.Models.Dto;

namespace SensorProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly ISensorRepository _IsensorRepo;
        public SensorController(ISensorRepository IsensorRepo)
        {
            this._IsensorRepo = IsensorRepo;  
        }

        [HttpPost("AddSensorName")]
        public async Task<IActionResult> AddSensor(Sensor_tbl sensor_Tbl)
        {            
            var responseDetails=await _IsensorRepo.AddSensor(sensor_Tbl);
            return Ok(responseDetails);
        }

        [HttpPost("AddSensorParameter")]
        public async Task<IActionResult> AddSensorParameter(List<SensorParameter_tbl> sensorParaObj)
        {
            var responseDetails = await _IsensorRepo.AddSensorParameter(sensorParaObj);
            return Ok(responseDetails);
        }

    }
}
