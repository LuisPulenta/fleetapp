using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Fleet_App.Common.Services;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;

namespace Fleet_App.Prism.ViewModels
{
    public class MenuItemViewModel : Menu
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private DelegateCommand _selectMenuCommand;
        private Ingreso NroIngreso;

        public MenuItemViewModel(INavigationService navigationService, IApiService apiService) 
        {
            _navigationService = navigationService;
            _apiService = apiService;
        }

        public DelegateCommand SelectMenuCommand => _selectMenuCommand ?? (_selectMenuCommand = new DelegateCommand(SelectMenu));

        private async void SelectMenu()
        {
            if (PageName.Equals("LoginPage"))
            {
                //var ing = JsonConvert.DeserializeObject<Ingreso>(Settings.Ingreso);


                
                //var webSesion = new WebSesionRequest
                //{
                //    ID_WS = ing.ID_WS,
                //    CONECTAVERAGE = ing.CONECTAVERAGE,
                //    IP = ing.IP,
                //    LOGINDATE = ing.LOGINDATE,
                //    LOGINTIME = ing.LOGINTIME,
                //    LOGOUTDATE = DateTime.Now,
                //    LOGOUTTIME = Convert.ToInt32(DateTime.Now.ToString("hhmmss")),
                //    MODULO = ing.MODULO,
                //    NROCONEXION = ing.NROCONEXION,
                //    USUARIO = ing.USUARIO,
                //};

                //var url = App.Current.Resources["UrlAPI"].ToString();
                //var response3 = await _apiService.PutAsync(
                //    url,
                //    "api",
                //    "/WebSesions",
                //    webSesion,
                //    ing.NROCONEXION);
                Settings.IsRemembered = false;
                await _navigationService.NavigateAsync("/NavigationPage/LoginPage");
                return;
            }

            await _navigationService.NavigateAsync($"/FleetMasterDetailPage/NavigationPage/{PageName}");

        }
    }
}