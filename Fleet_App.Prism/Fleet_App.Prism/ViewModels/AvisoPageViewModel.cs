using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Fleet_App.Prism.ViewModels
{
    public class AvisoPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        private DelegateCommand _continueCommand;
        private DelegateCommand _tiendaCommand;

        public DelegateCommand ContinueCommand => _continueCommand ?? (_continueCommand = new DelegateCommand(Continue));
        public DelegateCommand TiendaCommand => _tiendaCommand ?? (_tiendaCommand = new DelegateCommand(Tienda));

        public AvisoPageViewModel(INavigationService navigationService) : base(navigationService)
        {
           _navigationService = navigationService;
            Title = "Aviso!!";
        }

        private async void Continue()
        {
            await _navigationService.NavigateAsync("/FleetMasterDetailPage/NavigationPage/HomePage");
        }

        private async void Tienda()
        {
            await NavigationService.NavigateAsync("/NavigationPage/LoginPage");

            var url = "https://play.google.com/store/apps/details?id=com.ssc2.fleetapp";
            Device.OpenUri(new Uri(url));
            //var closer = DependencyService.Get<ICloseApplication>();
            //closer?.closeApplication();
        }
    }
}
