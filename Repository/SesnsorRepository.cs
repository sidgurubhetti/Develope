using SensorProject.Context;
using SensorProject.Entities;
using SensorProject.IRepository;
using SensorProject.Models.Dto;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Net.WebSockets;

namespace SensorProject.Repository
{
     
    public class SesnsorRepository : ISensorRepository
    {
        private readonly AppDbContext _context;
        public SesnsorRepository(AppDbContext context)
        {
            this._context = context; 
        }

        public async Task<ResponseDetails> AddEditParameterHistory(ParameterHistoryDto parameterHistoryObj)
        {
            List<ParameterDto> listParameter = null;
            ResponseDetails responseDetail = null;
            try
            {
                if(parameterHistoryObj.Id == 0)
                {

                 listParameter = new List<ParameterDto>();
                 listParameter.Add(parameterHistoryObj.parameterObj);
                    var parameterHistory = new tbl_ParameterHistory()
                    {
                        SensorId = parameterHistoryObj.SensorId,
                        ParameterHistory = JsonConvert.SerializeObject(listParameter)
                    };
                    _context.tbl_ParameterHistory.Add(parameterHistory);
                    _context.SaveChanges();

                }
                else
                {
                    var UpdateParameterHistory = _context.tbl_ParameterHistory.FirstOrDefault(x => x.Id == parameterHistoryObj.Id);
                    if(UpdateParameterHistory != null)
                    {
                        listParameter = new List<ParameterDto>();
                        listParameter = JsonConvert.DeserializeObject<List<ParameterDto>>(UpdateParameterHistory.ParameterHistory);
                        listParameter.Add(parameterHistoryObj.parameterObj);
                        UpdateParameterHistory.ParameterHistory = JsonConvert.SerializeObject(listParameter);
                        UpdateParameterHistory.Id= parameterHistoryObj.Id;
                        UpdateParameterHistory.SensorId= parameterHistoryObj.SensorId;
                        _context.SaveChanges();

                    }
                    else
                    {

                    }

                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return await Task.Run(() => responseDetail);
        }

        public async Task<ResponseDetails> AddSensor(Sensor_tbl sensor_Tbl)
        {
            ResponseDetails responseDetails = null;
            try
            {
                if(sensor_Tbl is null)
                {
                    responseDetails = new ResponseDetails()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Please enter valid inputs"
                    };
                    return await Task.Run(() => responseDetails);
                }
                 sensor_Tbl.CreatedBy = "Siddhu";
                 sensor_Tbl.CreatedOn = DateTime.Now;
                 sensor_Tbl.IsActive = true;
                _context.Sensor_tbl.Add(sensor_Tbl);
                _context.SaveChanges();

                responseDetails = new ResponseDetails()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Data save successFully",
                    data = sensor_Tbl
                };

            }
            catch (Exception ex)
            {

                throw;
            }
            return await Task.Run(() => responseDetails);
        }

        public async Task<ResponseDetails> AddSensorParameter(List<SensorParameter_tbl> sensorParaObj)
        {
            ResponseDetails responseDetails = null;
            var listSensorObj = new List<SensorParameter_tbl>();
            SensorParameter_tbl sensorParam = null;
            try
            {
                if (!sensorParaObj.Any())
                {
                    responseDetails = new ResponseDetails()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Please enter valid inputs"
                    };
                    return await Task.Run(() => responseDetails);
                }
                var sensorId = sensorParaObj.FirstOrDefault()?.SensorId;
                var existingParameterCount = _context.SensorParameter_tbl.Count(sp => sp.SensorId == sensorId);

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

                if (existingParameterCount + sensorParaObj.Count > sensor.TotalParameter)
                {
                    responseDetails = new ResponseDetails()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Only a maximum of "+ sensor.TotalParameter + " parameters can be added for the given SensorId"
                    };
                    return responseDetails;
                }

                sensorParaObj.ForEach(item =>
                {
                    sensorParam = new SensorParameter_tbl()
                    {
                        SensorId = item.SensorId,
                        SensorParameterName = item.SensorParameterName,
                        CreatedBy="Siddu",
                        CreatedOn = DateTime.Now,
                        IsActive= true
                    };
                    listSensorObj.Add(sensorParam);
                });

                _context.SensorParameter_tbl.AddRange(listSensorObj);
                await _context.SaveChangesAsync();

                responseDetails = new ResponseDetails()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Data save successFully"
                };
            }
            catch (Exception ex)
            {
                throw;
            }
            return await Task.Run(() => responseDetails);
        }
    }
}
