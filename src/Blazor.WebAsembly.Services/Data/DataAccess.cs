using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Blazor.WebAsembly.Services.Data
{
    public class DataAccess : IDataAccess
    {
        private readonly IConfiguration _configuration;

        public DataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<T> Get<T, U>(string sql, U parameters)
        {
            string connectionString = _configuration.GetConnectionString("Default");
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var data = await connection.QueryAsync<T>(sql, parameters);
                return data.FirstOrDefault();
            }

        }


        public async Task<List<T>> GetAll<T, U>(string sql, U parameters)
        {
            string connectionString = _configuration.GetConnectionString("Default");
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var data = await connection.QueryAsync<T>(sql, parameters);
                return data.ToList();
            }

        }
        public async Task CreateUpdate<T>(string sql, T parameters)
        {
            string connectionString = _configuration.GetConnectionString("Default");
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.ExecuteAsync(sql, parameters);
                }
                catch (Exception e)
                {

                    throw;
                }
               
            }
        }
    }
}
