using Fleet_App.Common.Services;
using Prism.Commands;
using Prism.Navigation;

namespace Fleet_App.Prism.ViewModels
{
    public class Firma2PageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(Cancel));




        public Firma2PageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "FirmaPage";
            instance = this;
        }

        #region Singleton

        private static Firma2PageViewModel instance;
        public static Firma2PageViewModel GetInstance()
        {
            return instance;
        }
        #endregion


        private async void Cancel()
        {
            await _navigationService.GoBackAsync();
        }
    }
}