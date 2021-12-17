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
    public class DevolucionesPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private UserResponse _user;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isRefreshing;
        private ObservableCollection<Devolucion> _devoluciones;

        public ObservableCollection<Devolucion> Devoluciones
        {
            get => _devoluciones;
            set => SetProperty(ref _devoluciones, value);
        }

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

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public List<Devolucion> MyDevol { get; set; }

        public DevolucionesPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
            LoadUser();
            Title = "Devoluciones";
        }




        public async void LoadUser()
        {
            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User2);
            var url = App.Current.Resources["UrlAPI"].ToString();
            var controller = string.Format("/AsignacionesOTs/GetDevoluciones/{0}", _user.IDUser);
            var response = await _apiService.GetDevolucionesForUser(
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
            MyDevol = (List<Devolucion>)response.Result;
            RefreshList();
            IsRefreshing = false;
        }


        public void RefreshList()
        {
                var myListDevolucion = MyDevol.Select(a => new Devolucion()
                {
                    PROYECTOMODULO = a.PROYECTOMODULO,
                    Cantidad=a.Cantidad,
                    
                });
            Devoluciones = new ObservableCollection<Devolucion>(myListDevolucion
                    .OrderBy(o => o.PROYECTOMODULO));
        }

    }
}