using System.Threading.Tasks;
using Fleet_App.Common.Models;
namespace Fleet_App.Common.Services
{
    public interface IApiService
    {
        Task<Response<UserResponse>> GetUserByEmailAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            string email,
            string password);
       
        Task<bool> CheckConnectionAsync(string url);

        Task<Response<object>> GetRemotesForUser(
            string urlBase,
            string servicePrefix,
            string controller,
            int id);

        Task<Response<object>> GetCablesForUser(
            string urlBase,
            string servicePrefix,
            string controller,
            int id);
        Task<Response<object>> GetPrismasForUser(
            string urlBase,
            string servicePrefix,
            string controller,
            int id);

        Task<Response<object>> GetTasasForUser(
           string urlBase,
           string servicePrefix,
           string controller,
           int id);
        Task<Response<object>> GetDtvsForUser(
           string urlBase,
           string servicePrefix,
           string controller,
           int id);

        Task<Response<object>> GetDevolucionesForUser(
           string urlBase,
           string servicePrefix,
           string controller,
           int id);

        Task<Response<object>> GetAllsForUser(
           string urlBase,
           string servicePrefix,
           string controller,
           int id);

        Task<Response<object>> GetListAsync<T>(
        string urlBase,
        string servicePrefix,
        string controller,
        string id,
        int? userid);

        Task<Response<object>> GetList2Async<T>(
        string urlBase,
        string servicePrefix,
        string controller);

        Task<Response<object>> GetList3Async<T>(
        string urlBase,
        string servicePrefix,
        string controller,
        int? id, 
        int? userid);

        Task<Response<object>> PutAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model,
            int id);

        Task<Response<object>> PostAsync<T>(
           string urlBase,
           string servicePrefix,
           string controller,
           T model);

        Task<Response<object>> GetLastWebSesion(
            string urlBase,
            string servicePrefix,
            string controller,
            int id);
        
        Task<Response2> GetTrabajos(string urlBase, string servicePrefix, string controller, TrabajosRequest model);
        Task<Response2> GetTrabajos2(string urlBase, string servicePrefix, string controller, TrabajosRequest model);
        Task<Response2> GetModelos(string urlBase, string servicePrefix, string controller, TrabajosRequest model);

    }
}
