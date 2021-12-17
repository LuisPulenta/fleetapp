using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Fleet_App.Common.Services;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;

namespace Fleet_App.Prism.ViewModels
{
    public class CableOtroRecuperoPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private UserResponse _user;

        private string _cliente;
        public string Cliente
        {
            get => _cliente;
            set => SetProperty(ref _cliente, value);
        }

        private string _cOddeco1;
        public string CODDECO1
        {
            get => _cOddeco1;
            set => SetProperty(ref _cOddeco1, value);
        }

        private string _nRoserieextra;
        public string NROSERIEEXTRA
        {
            get => _nRoserieextra;
            set => SetProperty(ref _nRoserieextra, value);
        }

        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }
        
        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private DelegateCommand _putSnCommand;
        //public DelegateCommand PutSNCommand => _putSnCommand ?? (_putSnCommand = new DelegateCommand(PutSn));

        private DelegateCommand _selectModemCommand;
        public DelegateCommand SelectModemCommand => _selectModemCommand ?? (_selectModemCommand = new DelegateCommand(SelectModem));

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(Cancel));

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(Save));


        /// <summary>
        /// 
        /// </summary>
        /// <param name="navigationService"></param>
        /// <param name="apiService"></param>
        public CableOtroRecuperoPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _instance = this;
            Title = "Scanea código Cablevisión";
        }

        #region Singleton

        private static CableOtroRecuperoPageViewModel _instance;
        public static CableOtroRecuperoPageViewModel GetInstance()
        {
            return _instance;
        }
        #endregion

        //private async void PutSn()
        //{
        //    NROSERIEEXTRA = "Sin Número de Serie";
        //}

        private async void Cancel()
        {
           await _navigationService.GoBackAsync();
        }

        private async void Save()
        {

            //if (CODDECO1 == null)
            //{
            //    await App.Current.MainPage.DisplayAlert("Error", "Debe elegir un Catálogo.", "Aceptar");
            //    return;
            //}

            if (NROSERIEEXTRA == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe poner un N° de Serie.", "Aceptar");
                return;
            }


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

            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User2);
            Cliente = CablePageViewModel.GetInstance().Cable.CLIENTE;



            IsRunning = true;
            IsEnabled = false;
                       
            var nuevoRecupero = new AsignacionesOtsEquiposExtraRequest
            {
                CODDECO1= CODDECO1,
                FECHACARGA=DateTime.Today,
                IDGAOS=null,
                IDTECNICO=_user.IDUser,
                NROCLIENTE= Cliente,
                NROSERIEEXTRA= NROSERIEEXTRA,
                ProyectoModulo = "Cable"
            };

            var response = await _apiService.PostAsync(
            url,
            "api",
            "/AsignacionesOtsEquiposExtras",
              
            nuevoRecupero);
            
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Ocurrió un error al guardar el Recupero, intente más tarde.", "Aceptar");
                return;
            }
                
                
              
            await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
            await _navigationService.GoBackAsync();
            return;

        }

        private async void SelectModem()
        {
            Settings.Modem = JsonConvert.SerializeObject(this);

            await _navigationService.NavigateAsync("CableScanCode2Page");
        }
    }
}
