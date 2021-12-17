using System;
using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

namespace Fleet_App.Prism.ViewModels
{
    public class CableItemViewModel : ReclamoCable
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectCableCommand;
        private DelegateCommand _citaCommand;

        public CableItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public DelegateCommand SelectCableCommand => _selectCableCommand ?? (_selectCableCommand = new DelegateCommand(SelectCable));
        public DelegateCommand CitaCommand => _citaCommand ?? (_citaCommand = new DelegateCommand(Cita));


        private async void SelectCable()
        {
            Settings.Cable = JsonConvert.SerializeObject(this);
            await _navigationService.NavigateAsync("CablePage");
        }

        private async void Cita()
        {
            Settings.Cable = JsonConvert.SerializeObject(this);
            await _navigationService.NavigateAsync("CitaPage");
        }

    }
}