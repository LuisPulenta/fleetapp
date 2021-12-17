using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

using Xamarin.Essentials;



namespace Fleet_App.Prism.ViewModels
{

    public class PrismaItemViewModel : ReclamoPrisma
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectPrismaCommand;
        private DelegateCommand _prismacitaCommand;


        public PrismaItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public DelegateCommand SelectPrismaCommand => _selectPrismaCommand ?? (_selectPrismaCommand = new DelegateCommand(SelectPrisma));
        public DelegateCommand PrismaCitaCommand => _prismacitaCommand ?? (_prismacitaCommand = new DelegateCommand(PrismaCita));

        private async void SelectPrisma()
        {
            Settings.Prisma = JsonConvert.SerializeObject(this);
            await _navigationService.NavigateAsync("PrismaPage");

        }

        private async void PrismaCita()
        {
            Settings.Prisma = JsonConvert.SerializeObject(this);
            await _navigationService.NavigateAsync("PrismaCitaPage");
        }
    }
}