using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Fleet_App.Common.Services;
using Fleet_App.Prism.ViewModels;
using Newtonsoft.Json;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Prism.Commands;



namespace Fleet_App.Prism.Views
{
    public partial class MapsPage : ContentPage
    {
        private readonly IGeolocatorService _geolocatorService;
        
        private readonly IApiService _apiService;
        private readonly INavigationService _navigationService;
        private UserResponse _user;
        private ObservableCollection<RemoteItemViewModel> _remotes;


        public List<Reclamo> MyRemotes { get; set; }

        public ObservableCollection<RemoteItemViewModel> Remotes { get; set; }
        


        public MapsPage(IGeolocatorService geolocatorService,  IApiService apiService, INavigationService navigationService)
        {

            InitializeComponent();

            _geolocatorService = geolocatorService;

            _apiService = apiService;
           _navigationService = navigationService;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MyMap.IsVisible = false;
            MoveMapToCurrentPositionAsync();
            MyMap.IsVisible = true;
            ShowPinsAsync();




        }

        private async Task<List<Pin>> ShowPinsAsync()
        {
            var pins = new List<Pin>();
            var mapsViewModel = MapsPageViewModel.GetInstance();



            //*********************************************************************

            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User2);
            string url = App.Current.Resources["UrlAPI"].ToString();
            string controller = string.Format("/AsignacionesOTs/GetAlls/{0}", _user.IDUser);
            Response<object> response = await _apiService.GetRemotesForUser(
                url,
                "api",
                controller,
                _user.IDUser);
            
            if (!response.IsSuccess)
            {
            
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Problema para recuperar datos.", "Aceptar");
                
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
            Remotes = new ObservableCollection<RemoteItemViewModel>
                (myListRemoteItemViewModel
                .Where(o => 
                (o.ESTADOGAOS == "PEN")
                && (o.GRXX != "''")
                && (o.GRYY != "''")
                && (o.GRXX != "0")
                && (o.GRYY != "0"))         
                .OrderBy(o => o.FechaAsignada + o.NOMBRE));
            


            //**********************************************************************
                                                                              
            foreach (var remote in mapsViewModel.Remotes.ToList())
            {
               
                if (!string.IsNullOrEmpty(remote.GRXX) && !string.IsNullOrEmpty(remote.GRYY))
                {
                    if (remote.GRXX.Length > 5 && remote.GRYY.Length > 5)
                    {
                        var position = new Position(Convert.ToDouble(remote.GRXX), Convert.ToDouble(remote.GRYY));
                        var tipopin = new PinType();
                        tipopin = PinType.Place;
                        var tipo = string.Empty;

                        if (remote.PROYECTOMODULO=="Otro")
                        {
                            tipo = "CONTROL REMOTO";
                        }
                        if (remote.PROYECTOMODULO == "Dtv")
                        {
                            tipo = "RECUPERO CABLEVISION";
                        }
                        if (remote.PROYECTOMODULO == "Tasa")
                        {
                            tipo = "RECUPERO TELEFONICA";
                        }
                        if (remote.PROYECTOMODULO == "DTV")
                        {
                            tipo = "RECUPERO DTV";
                        }
                        if (remote.PROYECTOMODULO == "Teco")
                        {
                            tipo = "RECUPERO TELECOM";
                        }
                        if (remote.PROYECTOMODULO == "TLC")
                        {
                            tipo = "RECUPERO TELECENTRO";
                        }

                        pins.Add(new Pin
                        {
                            Label = tipo + " - " + remote.NOMBRE,
                            Address = remote.DOMICILIO,
                            Position = position,
                            Type = tipopin,
                        });

                        //pins.Add(new CustomPin
                        //{
                        //    Label = tipo + " - " + remote.NOMBRE,
                        //    Address = remote.DOMICILIO,
                        //    Position = position,
                        //    Type = tipopin,
                        //    StyleId = HayCita,
                        //    ClassId = "Tasa",
                        //});
                    }
                }

            }

            foreach (var pin in pins)
            {
                MyMap.Pins.Add(pin);
            }

            return pins;
        }
        private async void MoveMapToCurrentPositionAsync()
        {
            bool isLocationPermision = await CheckLocationPermisionsAsync();

            if (isLocationPermision)
            {
                MyMap.IsShowingUser = true;

                await _geolocatorService.GetLocationAsync();
                if (_geolocatorService.Latitude != 0 && _geolocatorService.Longitude != 0)
                {
                    Position position = new Position(
                        _geolocatorService.Latitude,
                        _geolocatorService.Longitude);
                    MyMap.IsVisible = false;


                    MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                        position,
                        Distance.FromKilometers(.5)));
                    MyMap.IsVisible = true;

                }
            }
        }

        private async Task<bool> CheckLocationPermisionsAsync()
        {
            PermissionStatus permissionLocation = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            PermissionStatus permissionLocationAlways = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationAlways);
            PermissionStatus permissionLocationWhenInUse = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);
            bool isLocationEnabled = permissionLocation == PermissionStatus.Granted ||
                                     permissionLocationAlways == PermissionStatus.Granted ||
                                     permissionLocationWhenInUse == PermissionStatus.Granted;
            if (isLocationEnabled)
            {
                return true;
            }

            await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);

            permissionLocation = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            permissionLocationAlways = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationAlways);
            permissionLocationWhenInUse = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);
            return permissionLocation == PermissionStatus.Granted ||
                   permissionLocationAlways == PermissionStatus.Granted ||
                   permissionLocationWhenInUse == PermissionStatus.Granted;
        }

        private void MapStreetCommand(object sender, EventArgs eventArgs)
        {
            MyMap.MapType = MapType.Street;
        }
        private void MapSateliteCommand(object sender, EventArgs eventArgs)
        {
            MyMap.MapType = MapType.Satellite;
        }
        private void MapHybridCommand(object sender, EventArgs eventArgs)
        {
            MyMap.MapType = MapType.Hybrid;
        }

    }
}