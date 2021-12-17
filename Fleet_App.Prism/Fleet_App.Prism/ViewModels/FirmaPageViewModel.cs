using Fleet_App.Common.Services;
using Prism.Commands;
using Prism.Navigation;

namespace Fleet_App.Prism.ViewModels
{
    public class FirmaPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(Cancel));

        


        public FirmaPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "FirmaPage";
            instance = this;
        }

        #region Singleton

        private static FirmaPageViewModel instance;
        public static FirmaPageViewModel GetInstance()
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