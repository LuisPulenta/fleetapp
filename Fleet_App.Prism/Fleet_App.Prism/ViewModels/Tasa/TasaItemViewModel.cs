using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

namespace Fleet_App.Prism.ViewModels
{
    public class TasaItemViewModel : ReclamoTasa
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectTasaCommand;
        private DelegateCommand _citaCommand;
        public TasaItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public DelegateCommand SelectTasaCommand => _selectTasaCommand ?? (_selectTasaCommand = new DelegateCommand(SelectTasa));
        public DelegateCommand CitaCommand => _citaCommand ?? (_citaCommand = new DelegateCommand(Cita));

        private async void SelectTasa()
        {
            Settings.Tasa = JsonConvert.SerializeObject(this);
            await _navigationService.NavigateAsync("TasaPage");
        }

        private async void Cita()
        {
            Settings.Tasa = JsonConvert.SerializeObject(this);
            await _navigationService.NavigateAsync("TasaCitaPage");
        }
    }
}