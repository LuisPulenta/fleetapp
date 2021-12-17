using Fleet_App.Common.Services;
using Prism.Navigation;

namespace Fleet_App.Prism.ViewModels
{
    public class RemotesMapPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private static RemotesMapPageViewModel _instance;

        public static RemotesMapPageViewModel GetInstance()
        {
            return _instance;
        }

        public RemotesMapPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _instance = this;
            Title = "Mapa de Controles Remotos";
        }

        public async void CerrarMapa()
        {
            await _navigationService.GoBackAsync();
        }
    }
}