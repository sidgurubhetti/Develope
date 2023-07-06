using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        /*public async Task<ResponseDetails> AddEditParameterHistory(ParameterHistoryDto parameterHistoryObj)
        {
            List<ParameterDto> listParameter = null;
            ResponseDetails responseDetail = null;
            try
            {
                if (parameterHistoryObj.Id == 0)
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
                    if (UpdateParameterHistory != null)
                    {
                        listParameter = new List<ParameterDto>();
                        listParameter = JsonConvert.DeserializeObject<List<ParameterDto>>(UpdateParameterHistory.ParameterHistory);
                        listParameter.Add(parameterHistoryObj.parameterObj);
                        UpdateParameterHistory.ParameterHistory = JsonConvert.SerializeObject(listParameter);
                        UpdateParameterHistory.Id = parameterHistoryObj.Id;
                        UpdateParameterHistory.SensorId = parameterHistoryObj.SensorId;
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
*/

        public async Task<ResponseDetails> AddEditParameterHistory(ParameterHistoryDto parameterHistoryObj)
        {
            List<ParameterDto> listParameter = null;
            ResponseDetails responseDetail = null;
            try
            {
                if (parameterHistoryObj.Id == 0)
                {
                    // Check if the SensorId already exists in the database
                    var existingParameterHistory = await _context.tbl_ParameterHistory
                        .FirstOrDefaultAsync(x => x.SensorId == parameterHistoryObj.SensorId);

                    if (existingParameterHistory != null)
                    {
                        // Handle if a parameter history record with the same SensorId already exists
                        // You can throw an exception or return an appropriate response indicating the error
                        responseDetail = new ResponseDetails 
                        { 
                            Message = "Parameter history record with the same SensorId already exists!", 
                            Success = false 
                        };
                        return responseDetail;
                    }


                    // Check the maximum parameter limit for the given SensorId
                    var sensorId = parameterHistoryObj.SensorId;
                    var existingParameterCount = await _context.tbl_ParameterHistory
                        .CountAsync(x => x.SensorId == sensorId);

                    //var totalParameterLimit = sensor.TotalParameter; // Assuming `sensor` object represents the relevant sensor

                    /*if (existingParameterCount + parameterHistoryObj.Count > totalParameterLimit)
                    {
                        responseDetail = new ResponseDetails()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Only a maximum of " + totalParameterLimit + " parameters can be added for the given SensorId"
                        };
                        return responseDetail;
                    }*/

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
                    var updateParameterHistory = await _context.tbl_ParameterHistory.FirstOrDefaultAsync(x => x.Id == parameterHistoryObj.Id);
                    if (updateParameterHistory != null)
                    {
                        // Check if the SensorId has been changed
                        if (updateParameterHistory.SensorId != parameterHistoryObj.SensorId)
                        {
                            // Check if the new SensorId already exists in the database
                            var existingParameterHistory = await _context.tbl_ParameterHistory
                                .FirstOrDefaultAsync(x => x.SensorId == parameterHistoryObj.SensorId);

                            if (existingParameterHistory != null)
                            {
                                // Handle if a parameter history record with the same SensorId already exists
                                // You can throw an exception or return an appropriate response indicating the error
                                responseDetail = new ResponseDetails 
                                { 
                                    Message = "Parameter history record with the same SensorId already exists.", 
                                    Success = false 
                                };
                                return responseDetail;
                            }
                        }

                        listParameter = new List<ParameterDto>();
                        listParameter = JsonConvert.DeserializeObject<List<ParameterDto>>(updateParameterHistory.ParameterHistory);
                        listParameter.Add(parameterHistoryObj.parameterObj);
                        updateParameterHistory.ParameterHistory = JsonConvert.SerializeObject(listParameter);
                        updateParameterHistory.SensorId = parameterHistoryObj.SensorId;
                        _context.SaveChanges();
                    }
                    else
                    {
                        // Handle if the parameter history record doesn't exist
                        responseDetail = new ResponseDetails 
                        {
                            Message = "Parameter history record not found.", 
                            Success = false 
                        };
                        return responseDetail;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or log the error
                responseDetail = new ResponseDetails
                {
                    Message = "An error occurred while adding/editing parameter history.",
                    Success = false
                };
                return responseDetail;
            }
            // Handle success case
            responseDetail = new ResponseDetails
            {
                Message = "Parameter history added/edited successfully.",
                Success = true
            };
            return await Task.Run(() => responseDetail); // Return the responseDetail object with wrapping it in Task.Run
        }

        public async Task<List<ParameterHistoryDto>> GetAllParameterHistory()
        {
            try
            {
                var parameterHistoryList = await _context.tbl_ParameterHistory.ToListAsync();
                var parameterHistoryDtoList = new List<ParameterHistoryDto>();

                foreach (var parameterHistory in parameterHistoryList)
                {
                    var parameterDtoList = JsonConvert.DeserializeObject<List<ParameterDto>>(parameterHistory.ParameterHistory);

                    var parameterHistoryDto = new ParameterHistoryDto
                    {
                        Id = parameterHistory.Id,
                        SensorId = parameterHistory.SensorId,
                        parameterObj = parameterDtoList.LastOrDefault() // Assuming you want to retrieve the latest parameter object
                    };

                    parameterHistoryDtoList.Add(parameterHistoryDto);
                }

                return parameterHistoryDtoList;
            }
            catch (Exception ex)
            {
                // Handle exceptions or log the error
                throw;
            }
        }

        public async Task<List<ParameterHistoryDto>> GetParameterHistoryBySensorId(int sensorId)
        {
            try
            {
                var parameterHistoryList = await _context.tbl_ParameterHistory
                    .Where(p => p.SensorId == sensorId)
                    .ToListAsync();

                var parameterHistoryDtoList = new List<ParameterHistoryDto>();

                foreach (var parameterHistory in parameterHistoryList)
                {
                    var parameterDtoList = JsonConvert.DeserializeObject<List<ParameterDto>>(parameterHistory.ParameterHistory);

                    var parameterHistoryDto = new ParameterHistoryDto
                    {
                        Id = parameterHistory.Id,
                        SensorId = parameterHistory.SensorId,
                        parameterObj = parameterDtoList.LastOrDefault()
                    };

                    parameterHistoryDtoList.Add(parameterHistoryDto);
                }

                return parameterHistoryDtoList;
            }
            catch (Exception ex)
            {
                // Handle exceptions or log the error
                throw;
            }
        }


    }
}
