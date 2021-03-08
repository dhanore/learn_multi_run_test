using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using yoyo_web_app.Models;
using yoyo_web_app.Services;

namespace learn_multi_run_test.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class FitnessController : ControllerBase
    {
        readonly IFitnessService _fitnessService;
        public FitnessController(IFitnessService fitnessService)
        {
            _fitnessService = fitnessService;
        }

        [HttpGet]
        public List<FitnessRatingShuttle> GetAllShuttle()
        {
            var item = _fitnessService.GetAllShuttleRecord();
            return item;
        }

        [HttpGet("{id}")]
        public List<FitnessRatingShuttle> GetAllShuttle(int id)
        {
            var item = _fitnessService.GetShuttlerRecordById(id);
            return item;
        }

        [HttpGet("{id}/result")]
        public AthletesResult GetAthletesResult(int id)
        {
            var item = _fitnessService.GetAthletesResult(id);
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<AthletesResult>> Post(AthletesDto athletes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var isSaved = await _fitnessService.SaveRecordAsync(athletes);

            if (!isSaved)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
