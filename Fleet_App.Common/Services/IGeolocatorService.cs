using System.Threading.Tasks;
namespace Fleet_App.Common.Services
{
    public interface IGeolocatorService
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
        Task GetLocationAsync();
    }
}