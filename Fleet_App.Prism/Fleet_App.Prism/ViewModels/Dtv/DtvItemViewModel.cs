using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

using Xamarin.Essentials;



namespace Fleet_App.Prism.ViewModels
{

    public class DtvItemViewModel : ReclamoDtv
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectDtvCommand;
        private DelegateCommand _dtvcitaCommand;


        public DtvItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public DelegateCommand SelectDtvCommand => _selectDtvCommand ?? (_selectDtvCommand = new DelegateCommand(SelectDtv));
        public DelegateCommand DtvCitaCommand => _dtvcitaCommand ?? (_dtvcitaCommand = new DelegateCommand(DtvCita));

        private async void SelectDtv()
        {
            Settings.Dtv = JsonConvert.SerializeObject(this);
            await _navigationService.NavigateAsync("DtvPage");

        }

        private async void DtvCita()
        {
            Settings.Dtv = JsonConvert.SerializeObject(this);
            await _navigationService.NavigateAsync("DtvCitaPage");
        }
    }
}