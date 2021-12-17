using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;

namespace Fleet_App.Prism.ViewModels
{
    public class ModemItemViewModel : ControlTasa
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectModemCommand;
        private DelegateCommand _putSNCommand;
        



        public ModemItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public DelegateCommand SelectModemCommand => _selectModemCommand ?? (_selectModemCommand = new DelegateCommand(SelectModem));
        public DelegateCommand PutSNCommand => _putSNCommand ?? (_putSNCommand = new DelegateCommand(PutSN));


        private async void SelectModem()
        {
            Settings.Modem = JsonConvert.SerializeObject(this);
            
            await _navigationService.NavigateAsync("TasaScanCodePage");
        }

        private async void PutSN()
        {
            var tasaPageViewModel = TasaPageViewModel.GetInstance();
            foreach (var tt in tasaPageViewModel.ControlTasas)
            {
                if (tt.IDREGISTRO == this.IDREGISTRO)
                {
                    tt.ESTADO3 = "Sin Número de Serie";
                }
            }
            tasaPageViewModel.RefreshList2();
        }

      
    }
}