using Fleet_App.Common.Services;
using Prism.Navigation;

namespace Fleet_App.Prism.ViewModels
{
    public class RemoteMapPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;

        public RemoteMapPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Mapa de Control Remoto";
        }
    }
}