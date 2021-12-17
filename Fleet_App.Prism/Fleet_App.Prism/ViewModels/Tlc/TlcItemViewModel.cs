using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

namespace Fleet_App.Prism.ViewModels
{
    public class TlcItemViewModel : ReclamoCable
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectCableCommand;
        private DelegateCommand _tlccitaCommand;
        public TlcItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public DelegateCommand SelectCableCommand => _selectCableCommand ?? (_selectCableCommand = new DelegateCommand(SelectCable));
        public DelegateCommand TlcCitaCommand => _tlccitaCommand ?? (_tlccitaCommand = new DelegateCommand(TlcCita));


        private async void SelectCable()
        {
            Settings.Tlc = JsonConvert.SerializeObject(this);
            await _navigationService.NavigateAsync("TlcPage");
        }

        private async void TlcCita()
        {
            Settings.Dtv = JsonConvert.SerializeObject(this);
            await _navigationService.NavigateAsync("TlcCitaPage");
        }

    }
}