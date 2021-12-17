using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Fleet_App.Common.Services;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fleet_App.Prism.ViewModels
{
    public class MapsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private UserResponse _user;
        private readonly RemoteResponse _rem;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isRefreshing;
        private static MapsPageViewModel _instance;
        private string _entreCalles;
        private ObservableCollection<RemoteItemViewModel> _remotes;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

       

        #region Singleton

        private static MapsPageViewModel instance;
        public static MapsPageViewModel GetInstance()
        {
            return instance;
        }
        #endregion











        public string EntreCalles
        {
            get => _entreCalles;
            set => SetProperty(ref _entreCalles, value);
        }
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
        public ObservableCollection<RemoteItemViewModel> Remotes
        {
            get => _remotes;
            set => SetProperty(ref _remotes, value);
        }

        public List<Reclamo> MyRemotes { get; set; }



        public MapsPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Mapa";
            LoadUser();

            instance = this;
        }

        private async void LoadUser()
        {
            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User2);
            string url = App.Current.Resources["UrlAPI"].ToString();
            string controller = string.Format("/AsignacionesOTs/GetAlls/{0}", _user.IDUser);
            Response<object> response = await _apiService.GetRemotesForUser(
                url,
                "api",
                controller,
                _user.IDUser);
            IsRefreshing = false;
            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Problema para recuperar datos.", "Aceptar");
                return;
            }
            MyRemotes = (List<Reclamo>)response.Result;
            IEnumerable<RemoteItemViewModel> myListRemoteItemViewModel = MyRemotes.Select(a => new RemoteItemViewModel(_navigationService)
            {
                CantRem = a.CantRem,
                CAUSANTEC = a.CAUSANTEC,
                CLIENTE = a.CLIENTE,
                CodigoCierre = a.CodigoCierre,
                CP = a.CP,
                Descripcion = a.Descripcion,
                DOMICILIO = a.DOMICILIO,
                ENTRECALLE1 = a.ENTRECALLE1,
                ENTRECALLE2 = a.ENTRECALLE2,
                ESTADOGAOS = a.ESTADOGAOS,
                GRXX = a.GRXX,
                GRYY = a.GRYY,
                FechaAsignada = a.FechaAsignada,
                Novedades = a.Novedades,
                LOCALIDAD = a.LOCALIDAD,
                NOMBRE = a.NOMBRE,
                ObservacionCaptura = a.ObservacionCaptura,
                PROYECTOMODULO = a.PROYECTOMODULO,
                RECUPIDJOBCARD = a.RECUPIDJOBCARD,
                SUBCON = a.SUBCON,
                TELEFONO = a.TELEFONO,
                UserID = a.UserID,
            });
            Remotes = new ObservableCollection<RemoteItemViewModel>(myListRemoteItemViewModel.OrderBy(o => o.FechaAsignada + o.NOMBRE));
            IsRefreshing = false;
        }


                
 
    }
}