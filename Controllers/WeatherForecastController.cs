using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using takeanexam.InterfaceService;

namespace takeanexam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly RedisService _redisService;
        private readonly IWeatherForecastApiService _WeatherForecastApiService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, RedisService redisService, IWeatherForecastApiService WeatherForecastApiService)
        {
            _logger = logger;
            _redisService = redisService;
            _WeatherForecastApiService = WeatherForecastApiService;
        }

        [HttpGet]
        public async Task<List<WeatherForecast>> Get( string? Name)
        {
            return await _WeatherForecastApiService.Get(Name);
           
        }

        [HttpPost]
        public async Task<bool> Create([FromBody] res res)
        {
            return await _WeatherForecastApiService.Create(res);
        }

        [HttpPut]
        public async Task<bool> Update([FromBody] ById res)
        {
            return await _WeatherForecastApiService.Update(res);
        }

        [HttpDelete]
        public async Task<bool> Delete(int id)
        {
            return await _WeatherForecastApiService.Delete(id);
        }
    }
}
