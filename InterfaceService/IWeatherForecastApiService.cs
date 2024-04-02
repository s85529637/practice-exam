namespace takeanexam.InterfaceService
{
    public interface IWeatherForecastApiService
    {
         Task<List<WeatherForecast>> Get(string Name);

         Task<bool> Create(res res);

         Task<bool> Update(ById res);

         Task<bool> Delete(int id);
    }
}
