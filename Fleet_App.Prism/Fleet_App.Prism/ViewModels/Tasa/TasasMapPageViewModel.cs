using Fleet_App.Common.Services;
using Prism.Navigation;

namespace Fleet_App.Prism.ViewModels
{
    public class TasasMapPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private static TasasMapPageViewModel _instance;

        public static TasasMapPageViewModel GetInstance()
        {
            return _instance;
        }

        public TasasMapPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _instance = this;
            Title = "Mapa de Recuperos Tasa";
        }

        public async void CerrarMapa()
        {
            await _navigationService.GoBackAsync();
        }
    }
}