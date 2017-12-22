using System.Threading.Tasks;

namespace BinanceAPI
{
    public interface IBinanceClient
    {
        Task<T> GetAsync<T>(string endpoint, string args = null);
        Task<T> GetSignedAsync<T>(string endpoint, string args = null);
        Task<T> PostSignedAsync<T>(string endpoint, string args = null);
        Task<T> DeleteSignedAsync<T>(string endpoint, string args = null);
    }
}