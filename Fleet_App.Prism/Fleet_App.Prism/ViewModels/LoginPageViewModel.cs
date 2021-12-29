using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Fleet_App.Common.Services;
using Newtonsoft.Json;
using Plugin.DeviceInfo;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;

namespace Fleet_App.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private string _password;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isRemembered;
        private DelegateCommand _loginCommand;
        private WebSesionRequest nroIngreso;
        

        public LoginPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            Title = "Login";
            IsEnabled = true;
            IsRemembered = false;
            _navigationService = navigationService;
            _apiService = apiService;
            AppVersion = $"V.{App.Current.Resources["AppVersion"].ToString()}";
            //Email = "TEST";
            //Password = "TEST";
            
        }

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(Login));

        public string Email { get; set; }

        public string AppVersion { get; set; }

        public List<ModuleResponse> MyModules { get; set; }
        public WebSesionRequest NroIngreso
        {
            get => nroIngreso;
            set => SetProperty(ref nroIngreso, value);
        }
        
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public bool IsRemembered
        {
            get => _isRemembered;
            set => SetProperty(ref _isRemembered, value);
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe ingresar un Usuario", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe ingresar un Password.", "Aceptar");
                return;
            }
            IsRunning = true;
            IsEnabled = false;


            //Verificar conectividad
            var url = App.Current.Resources["UrlAPI"].ToString();
            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsEnabled = true;
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "Chequee su conexión a Internet.", "Aceptar");
                return;
            }



            //Verificar Usuario
            var response = await _apiService.GetUserByEmailAsync(url, "api", "/Account/GetUserByEmail", Email,Password);
            
            if (!response.IsSuccess)
            {
            IsEnabled = true;
            IsRunning = false;
            await App.Current.MainPage.DisplayAlert("Error", "Usuario o password incorrecto.", "Aceptar");
            Password = string.Empty;
            return;
            }

            //Verificar Password
            if (!(response.Result.USRCONTRASENA.ToLower() == Password.ToLower()))
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Usuario o password incorrecto.", "Aceptar");
                return;
            }
            //Verificar Usuario Habilitado
            if (response.Result.HabilitadoWeb == 0)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Usuario no habilitado.", "Aceptar");
                return;
            }


            //***************************************************************************************************
            //********** AGREGA INGRESO A WEBSESION **********
            var IdUserActual = $"{response.Result.IDUser}";


            //var webSesion = new WebSesionRequest
            //{
            //    CONECTAVERAGE = 0,
            //    IP = $"{CrossDeviceInfo.Current.Id}",             //   // Poner IP o IMEI     {CrossDeviceInfo.Current.DeviceName}
            //    LOGINDATE = DateTime.Now,
            //    LOGINTIME = Convert.ToInt32(DateTime.Now.ToString("hhmmss")),
            //    LOGOUTDATE = null,
            //    LOGOUTTIME = 0,
            //    MODULO = "App",
            //    NROCONEXION = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")) + Convert.ToInt32(DateTime.Now.ToString("hhmmss")),
            //    USUARIO = IdUserActual,
            //};

            //var url2 = App.Current.Resources["UrlAPI"].ToString();
            //var response3 = await _apiService.PostAsync(
            //   url2,
            //    "api",
            //    "/WebSesions",
            //    webSesion);
            //if (!response3.IsSuccess)
            //{
            //    await App.Current.MainPage.DisplayAlert("Error", "No se pudo registrar Ingreso del Usuario.", "Aceptar");
            //    IsRunning = false;
            //    IsEnabled = true;
            //    return;
            //}

            //var response4 = await _apiService.GetLastWebSesion(
            //             url,
            //             "api",
            //             "/WebSesions/GetLastWebSesion",
            //             webSesion.NROCONEXION);

            //if (response4.IsSuccess)
            //{
            //    NroIngreso = (WebSesionRequest)response4.Result;
            //}


            


            //********** CONTROLA NUMERO DE VERSION **********
            var response2 = await _apiService.GetList2Async<ModuleResponse>(
                url,
                "api",
                "/Modules");

            this.MyModules = (List<ModuleResponse>)response2.Result;



            if (response2.IsSuccess)
            {
            var bandera = 0;
            var version = App.Current.Resources["AppVersion"].ToString();
                foreach (var cc in MyModules)
            {
                if (cc.NroVersion != version && cc.ActualizOblig == 0 && cc.IdModulo==1)
                {
                    bandera = 1;
                }

                if (cc.NroVersion != version && cc.ActualizOblig == 1 && cc.IdModulo == 1)
                {
                    bandera = 2; 
                }
            }

            if (bandera==1)
                {
                    IsRunning = false;
                    IsEnabled = true;
                    Settings.IsRemembered = false;
                    await _navigationService.NavigateAsync("/FleetMasterDetailPage/NavigationPage/AvisoPage");
                    return;
                }

                if (bandera == 2)
                {
                    IsRunning = false;
                    IsEnabled = true;
                    Settings.IsRemembered = false;
                    await _navigationService.NavigateAsync("/FleetMasterDetailPage/NavigationPage/Aviso2Page");
                    return;
                }
            }
            //***************************************************************************************************


            Email = null;
            Password = null;

            IsEnabled = true;
            IsRunning = false;

            Settings.IsRemembered = IsRemembered;
            Settings.User2 = JsonConvert.SerializeObject(response.Result);
            Settings.Ingreso = JsonConvert.SerializeObject(NroIngreso);

            await _navigationService.NavigateAsync("/FleetMasterDetailPage/NavigationPage/HomePage");
        }
    }
}
