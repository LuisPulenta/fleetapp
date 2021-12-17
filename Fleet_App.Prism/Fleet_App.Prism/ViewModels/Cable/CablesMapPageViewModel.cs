using Fleet_App.Common.Services;
using Prism.Navigation;

namespace Fleet_App.Prism.ViewModels
{
    public class CablesMapPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private static CablesMapPageViewModel _instance;

        public static CablesMapPageViewModel GetInstance()
        {
            return _instance;
        }

        public CablesMapPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _instance = this;
            Title = "Mapa de Recuperos Cablevisión";
        }

        public async void CerrarMapa()
        {
            await _navigationService.GoBackAsync();
        }

    }
}