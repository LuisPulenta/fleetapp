using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Fleet_App.Common.Services;
using Newtonsoft.Json;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fleet_App.Prism.ViewModels
{
    public class FleetMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private UserResponse _user2;


        public UserResponse User2
        {
            get => _user2;
            set => SetProperty(ref _user2, value);
        }

        
        


            public FleetMasterDetailPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            User2 = JsonConvert.DeserializeObject<UserResponse>(Settings.User2);
            LoadMenus();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        private void LoadMenus()
        {
            var menus = new List<Menu>
            {

                new Menu
                {
                    Icon = "ic_action_settings_remote.png",
                    PageName = "RemotesPage",
                    Title = "Controles Remotos"
                },
                //new Menu
                //{
                //    Icon = "ic_action_teco.png",
                //    PageName = "TecosPage",
                //    Title = "Recuperos Teco"
                //},
                new Menu
                {
                    Icon = "ic_action_router.png",
                    PageName = "PrismasPage",
                    Title = "Recuperos Prisma"
                },
                new Menu
                {
                    Icon = "ic_action_router.png",
                    PageName = "CablesPage",
                    Title = "Recuperos Cablevisión"
                },
                new Menu
                {
                    Icon = "ic_action_dtv.png",
                    PageName = "DtvsPage",
                    Title = "Recuperos DTV"
                },

                 new Menu
                {
                    Icon = "ic_action_settings_input_antenna.png",
                    PageName = "TasasPage",
                    Title = "Recuperos Tasa"
                },
                 
                new Menu
                {
                    Icon = "ic_action_tlc.png",
                    PageName = "TlcsPage",
                    Title = "Recuperos TLC"
                },

                 new Menu
                {
                    Icon = "ic_action_mapmenu.png",
                    PageName = "MapsPage",
                    Title = "Mapa"
                },

                 new Menu
                {
                    Icon = "ic_equalizer.png",
                    PageName = "EstadisticasPage",
                    Title = "Estadísticas"
                },

                 new Menu
                 {
                     Icon = "ic_action_settings",
                     PageName = "ContactPage",
                     Title = "Contacto"
                 },

                // new Menu
                //{
                //    Icon = "ic_action_restore_page.png",
                //    PageName = "DevolucionesPage",
                //    Title = "Devoluciones"
                //},

                //new Menu
                //{
                //    Icon = "",
                //    PageName = "",
                //    Title = ""
                //},

                new Menu
                {
                    Icon = "ic_action_exit_to_app.png",
                    PageName = "LoginPage",
                    Title = "Cerrar Sesión"
                },

                
               
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService, _apiService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title
                }).ToList());
        }
    }
}