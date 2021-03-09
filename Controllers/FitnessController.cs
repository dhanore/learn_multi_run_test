using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using yoyo.web.BL.Models;
using yoyo.web.BL.Services;

namespace yoyo_web_app.Controllers
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

        /// <summary>
        /// Get Top 2 Shuttle Information
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<FitnessRatingShuttle> GetAllShuttle()
        {
            var item = _fitnessService.GetAllShuttleRecord();
            return item;
        }

        /// <summary>
        /// Get Next two Shuttle Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public List<FitnessRatingShuttle> GetAllShuttle(int id)
        {
            var item = _fitnessService.GetShuttlerRecordById(id);
            return item;
        }


        /// <summary>
        /// Save Atheletes Shuttle Information
        /// </summary>
        /// <param name="athletes"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get Atheletes Shuttle Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/result")]
        public AthletesResult GetAthletesResult(int id)
        {
            var item = _fitnessService.GetAthletesResult(id);
            return item;
        }
    }
}
