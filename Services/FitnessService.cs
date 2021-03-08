using yoyo_web_app.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using yoyo_web_app.Repository;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace yoyo_web_app.Services
{
    public interface IFitnessService
    {
        List<FitnessRatingShuttle> GetAllShuttleRecord();
        AthletesResult GetAthletesResult(int id);
        List<FitnessRatingShuttle> GetShuttlerRecordById(int id);
        Task<bool> SaveRecordAsync(AthletesDto athletes);
    }

    public class FitnessService : IFitnessService
    {
        private static List<FitnessRatingShuttle> _jsonData;

        readonly string jsonFile = Path.GetFullPath(@"models\fitnessrating_beeptest.json");
        ILogger<AthletesResult> _logger;
        readonly IRepository<AthletesResult> _athletesRepo;

        public FitnessService(IRepository<AthletesResult> athletesRepo, ILogger<AthletesResult> logger)
        {
            _logger = logger;
            _jsonData = JsonConvert.DeserializeObject<List<FitnessRatingShuttle>>(System.IO.File.ReadAllText(jsonFile));
            _athletesRepo = athletesRepo;
        }

        public AthletesResult GetAthletesResult(int id)
        {
            var item = _athletesRepo.FindBy(o => o.Id == id).FirstOrDefault();
            return item;
        }

        public List<FitnessRatingShuttle> GetAllShuttleRecord()
        {
            var item = _jsonData.OrderBy(o => o.AccumulatedShuttleDistance).Take(2).ToList();
            return item;
        }

        public List<FitnessRatingShuttle> GetShuttlerRecordById(int id)
        {
            var item = _jsonData.Where(o => o.AccumulatedShuttleDistance > id).OrderBy(o => o.AccumulatedShuttleDistance).Take(2).ToList();
            return item;
        }

        public async Task<bool> SaveRecordAsync(AthletesDto athletes)
        {
            try
            {
                var dataExist = _athletesRepo.FindBy(o => o.Id == athletes.Id).FirstOrDefault();

                var item = _jsonData.LastOrDefault(g => g.AccumulatedShuttleDistance < athletes.AccumulatedShuttleDistance);

                var _athletesResult = new AthletesResult
                {
                    Id = athletes.Id,
                    AthleteName = athletes.AthleteName,
                    ShuttleNo = item?.ShuttleNo.ToString(),
                    SpeedLevel = item?.SpeedLevel.ToString(),
                    IsWarned = dataExist?.IsWarned ?? athletes.IsWarned
                };

                _athletesRepo.Create(_athletesResult);

                if (dataExist != null)
                {
                    _athletesRepo.Edit(_athletesResult);
                }

                await _athletesRepo.SaveAsync();

                _logger.LogInformation("FitnessService-SaveRecordAsync", "SaveRecordAsync");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, "SaveRecordAsync Exception");
                return false;
            }
        }
    }
}
