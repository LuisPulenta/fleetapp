using Fleet_App.Common.Services;
using Prism.Navigation;

namespace Fleet_App.Prism.ViewModels.Teco
{
    public class TecosMapPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private static TecosMapPageViewModel _instance;

        public static TecosMapPageViewModel GetInstance()
        {
            return _instance;
        }

        public TecosMapPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _instance = this;
            Title = "Mapa de recuperos Teco";
        }

        public async void CerrarMapa()
        {
            await _navigationService.GoBackAsync();
        }
    }
}