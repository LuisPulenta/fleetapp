using System;
using System.Collections.Generic;
using System.Text;
using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

namespace Fleet_App.Prism.ViewModels.Cable
{
    public class AutoCableItemViewModel : ControlTasa
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _scanSnCommand;
        private DelegateCommand _putSNCommand;

        public AutoCableItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand ScanSnCommand => _scanSnCommand ?? (_scanSnCommand = new DelegateCommand(ScanSn));
        public DelegateCommand PutSNCommand => _putSNCommand ?? (_putSNCommand = new DelegateCommand(PutSN));
        private async void ScanSn()
        {
            Settings.Cable = JsonConvert.SerializeObject(this);

            await _navigationService.NavigateAsync("CableScanCodePage");
        }

        private async void PutSN()
        {
            var pageViewModel = CablePageViewModel.GetInstance();
            foreach (var tt in pageViewModel.ControlCables)
            {
                if (tt.IDREGISTRO == this.IDREGISTRO)
                {
                    tt.ESTADO3 = "Sin Número de Serie";
                }
            }
            pageViewModel.RefreshListFromControlCables();
        }
    }
}
