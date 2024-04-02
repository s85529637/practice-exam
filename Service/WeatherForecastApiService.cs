using Dapper;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using takeanexam.InterfaceService;

namespace takeanexam.Service
{
    public class WeatherForecastApiService : IWeatherForecastApiService
    {
        private readonly string _connectionString;
        public WeatherForecastApiService(IOptions<DBConnection> dbConfig)
        {
            _connectionString = dbConfig.Value.ConnectionString;

        }


        public async Task<List<WeatherForecast>> Get(string Name)
        {

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {

            };

            string sql = "SELECT * FROM YourTableName";
            if (!string.IsNullOrEmpty(Name))
            {
                sql += " WHERE Name = @Name";
                parameters.Add("@Name", Name);
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var books = await connection.QueryAsync<WeatherForecast>(sql, parameters);
                return books.ToList();
            }


        }

        public async Task<bool> Create(res res)
        {

            string sql = "INSERT INTO YourTableName ( [Name], [CreateDateTime], [UpdateTime]) VALUES (@Name, @CreateDateTime,@UpdateTime)";

            Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@Name",res.Name},
                    {"@CreateDateTime",DateTime.Now},
                    {"@UpdateTime",DateTime.Now}

                };

            using (var connection = new SqlConnection(_connectionString))
            {
                var affectedRows = await connection.ExecuteAsync(sql, parameters);
                return affectedRows > 0;
            }

        }

        public async Task<bool> Update(ById res)
        {

            string sql = "UPDATE YourTableName SET Name = @Name,UpdateTime=@UpdateTime where Id=@Id";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    {"@Name",res.Name},
                    {"@Id",res.Id},
                    {"@UpdateTime",DateTime.Now}

                };


            using (var connection = new SqlConnection(_connectionString))
            {
                var affectedRows = await connection.ExecuteAsync(sql, parameters);
                return affectedRows >= 0;
            }

        }

        public async Task<bool> Delete(int id)
        {
            string sql = "Delete YourTableName where Id=@Id";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
                {

                    {"@Id",id},
                };


            using (var connection = new SqlConnection(_connectionString))
            {
                var affectedRows = await connection.ExecuteAsync(sql, parameters);
                return affectedRows >= 0;
            }
        }
    }
}
