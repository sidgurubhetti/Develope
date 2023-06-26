using Microsoft.EntityFrameworkCore;
using SensorProject.Context;
using SensorProject.Entities;
using SensorProject.IRepository;
using SensorProject.Models.Dto;

namespace SensorProject.Repository
{
    public class SensorDataRepository : ISensorDataRepository
    {
        private readonly AppDbContext _context;
        public SensorDataRepository(AppDbContext context)
        {
            _context = context;
        }
        /*public async Task<ResponseDetails> AddSensorData(SensorData_tbl sensorData_tbl)
        {
            ResponseDetails responseDetails = null;
            try
            {
                if (sensorData_tbl == null)
                {
                    responseDetails = new ResponseDetails()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Please provide valid sensor data."
                    };
                    return await Task.Run(() => responseDetails);
                }

                var sensorId = sensorData_tbl.SensorId;
                var sensor = await _context.Sensor_tbl.FindAsync(sensorId);

                if (sensor == null)
                {
                    responseDetails = new ResponseDetails()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Invalid SensorId"
                    };
                    return responseDetails;
                }

                var totalParameterCount = sensor.TotalParameter;

                // Get the property names starting with "P"
                var parameterProperties = typeof(SensorData_tbl).GetProperties()
                    .Where(p => p.Name.StartsWith("P") && p.PropertyType == typeof(string))
                    .ToList();

                // Validate the parameter count
                var parameterCount = parameterProperties
                    .Select(p => (string)p.GetValue(sensorData_tbl))
                    .Count(value => !string.IsNullOrEmpty(value));

                if (parameterCount > totalParameterCount)
                {
                    responseDetails = new ResponseDetails()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = $"Only a maximum of {totalParameterCount} parameters can be added for the given SensorId"
                    };
                    return responseDetails;
                }

                // Assign current date/time to the sensor data
                sensorData_tbl.DateTime = DateTime.Now;

                // Fill the parameter values
                for (int i = 0; i < totalParameterCount; i++)
                {
                    var parameterProperty = parameterProperties.ElementAtOrDefault(i);
                    if (parameterProperty != null)
                    {
                        var parameterValue = (string)parameterProperty.GetValue(sensorData_tbl);
                        parameterValue = string.IsNullOrEmpty(parameterValue) ? $"P{i + 1} value" : parameterValue;
                        parameterProperty.SetValue(sensorData_tbl, parameterValue);
                    }
                }

                _context.SensorData_tbl.Add(sensorData_tbl);
                await _context.SaveChangesAsync();

                responseDetails = new ResponseDetails()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Sensor data saved successfully.",
                    data = sensorData_tbl
                };
            }
            catch (Exception ex)
            {
                responseDetails = new ResponseDetails()
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Message = "An error occurred while saving the sensor data."
                };
            }

            return responseDetails;
        }*/



        public async Task<List<SensorData_tbl>> GetSensorData()
        {
            try
            {
                var sensorData = await _context.SensorData_tbl.ToListAsync();
                return sensorData;
            }
            catch (Exception ex)
            {
                // Handle exception appropriately
                throw;
            }
        }
    }
}
