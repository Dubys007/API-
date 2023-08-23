using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sq016FirstApi.Data.Entities;
using Sq016FirstApi.Data.Repositories;
using Sq016FirstApi.DTOs;

namespace Sq016FirstApi.Controllers
{
    //[ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
         };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastRepository _repo;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastRepository weatherForecastRepository)
        {
            _logger = logger;
            _repo = weatherForecastRepository;
        }

        
        [Authorize(Roles ="admin, editor")]  // you must be logged in, you musst be an editor or an admin
        [Authorize(Policy = "CanAdd")]
        [HttpPost("add")] /// Http verbs
        public IActionResult AddNewWeatherForecast([FromBody]AddWeatherForecastDto model)
        {
            // validate your model
            if(ModelState.IsValid)
            {
                if(model.Summary == "string" || model.Summary == "")
                    return BadRequest("Invalid entry!");

                var recordToAdd = new WeatherForecast
                {
                    Date = DateTime.Now,
                    Summary = model.Summary,
                    TemperatureC = Random.Shared.Next(-20, 55)
                };

                if (_repo.Add(recordToAdd))
                {
                    return Ok("Record added successfully!");
                }

                return BadRequest("Add record failed!");
            }
            return BadRequest(ModelState);
        }


        [AllowAnonymous]
        [HttpGet("get-all")] /// Http verbs
        public IActionResult GetAll()
        {
            var forecasts = _repo.WeatherForecastListAll();

            var result = forecasts.Select(x => new ReturnWeatherForecastDto
            {
                Id = x.Id,
                Date = x.Date,
                Summary = x.Summary,
                TemperatureC = x.TemperatureC,
                TemperatureF = x.TemperatureF
            });
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("single/{id}")]
        public IActionResult GetSingle(int id)
        {
            var forecast = _repo.WeatherForecastGet(id);
            if(forecast.Id > 0)
            {
                var result = new ReturnWeatherForecastDto
                {
                    Id = forecast.Id,
                    Date = forecast.Date,
                    Summary = forecast.Summary,
                    TemperatureC = forecast.TemperatureC,
                    TemperatureF = forecast.TemperatureF
                };
                return Ok(result);
            }

            return NotFound($"No result found for record with id: {id}");
        }


        [Authorize(Roles = "admin, editor")]  // you must be logged in, you musst be an editor or an admin
        [Authorize(Policy = "CanEdit")]
        [HttpPut("update/{id}")]
        public IActionResult UpdateWeatherForecast(int id, [FromBody]UpdateWeatherForecastDto model)
        {
            var forecast = _repo.WeatherForecastGet(id);
            if(forecast != null)
            {
                forecast.TemperatureC = model.TemperatureC;
                forecast.Summary = model.Summary;

                if(_repo.Update(forecast))
                {
                    return Ok("Updated successfully");
                }

                return BadRequest("Update failed");
            }

            return BadRequest($"Update failed: Could not get forecast with id {id}");

        }


        [Authorize(Roles = "admin")]  // you must be logged in, you musst be an editor or an admin
        [Authorize(Policy = "CanDelete")]
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteWeatherForecast(int id)
        {
            var forecast = _repo.WeatherForecastGet(id);
            if (forecast != null)
            {
                if (_repo.Delete(forecast))
                {
                    return Ok("Deleted successfully");
                }

                return BadRequest("Deleted failed");
            }

            return BadRequest($"Delete failed: Could not get forecast with id {id}");

        }

        /*
         HttpGet  - Used when you want to get a result from an api
         HttpPost - Used when you want to add a record to an api
         HttpPut  - Used when you want to update a full record in an api
         HttpPatch - Used when you want to update a part of a record in an api
         HttpDelete - Used to delete a record from an api
         */
    }
}