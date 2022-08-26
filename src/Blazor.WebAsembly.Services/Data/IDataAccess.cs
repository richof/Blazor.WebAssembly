
namespace Blazor.WebAsembly.Services.Data
{
    public interface IDataAccess
    {
        Task CreateUpdate<T>(string sql, T parameters);
        Task<T> Get<T, U>(string sql, U parameters);
        Task<List<T>> GetAll<T, U>(string sql, U parameters);
    }
}