using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

namespace Fleet_App.Prism.ViewModels.Teco
{
    public class TecoItemViewModel : Reclamo
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectRemoteCommand;
        private DelegateCommand _citaCommand;

        public TecoItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public DelegateCommand SelectRemoteCommand => _selectRemoteCommand ?? (_selectRemoteCommand = new DelegateCommand(SelectRemote));
        public DelegateCommand CitaCommand => _citaCommand ?? (_citaCommand = new DelegateCommand(Cita));

        private async void SelectRemote()
        {

            Settings.Remote = JsonConvert.SerializeObject(this);
            await _navigationService.NavigateAsync("TecoPage");
        }

        private async void Cita()
        {
            Settings.Remote = JsonConvert.SerializeObject(this);
            await _navigationService.NavigateAsync("TecoCitaPage");
        }

    }
}