using Fleet_App.Common.Services;
using Fleet_App.Prism.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fleet_App.Prism.ViewModels.Teco;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Fleet_App.Prism.Views.Teco
{
    public partial class TecosMapPage : ContentPage
    {
        private readonly IGeolocatorService _geolocatorService;

        public TecosMapPage(IGeolocatorService geolocatorService)
        {
            InitializeComponent();
            _geolocatorService = geolocatorService;
            MyMap.MapType = MapType.Street;
            MyMap.IsVisible = false;
            MoveMapToCurrentPositionAsync();
            MyMap.IsVisible = true;
            ShowPinsAsync();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MyMap.IsVisible = false;
            MoveMapToCurrentPositionAsync();
            MyMap.IsVisible = true;
        }

        private async Task<List<CustomPin>> ShowPinsAsync()
        {

            CustomPin pin = new CustomPin
            {
                Type = PinType.Place,
                Position = new Position(-0, 0),

                Label = " ",
                Address = " ",
                Name = " ",
                StyleId = "",
                Url = ""
            };

            var pins = new List<CustomPin> { pin };
            TecosPageViewModel tecosPageViewModel = TecosPageViewModel.GetInstance();
            foreach (TecoItemViewModel teco in tecosPageViewModel.Remotes.ToList())
            {
                if (!string.IsNullOrEmpty(teco.GRXX) && !string.IsNullOrEmpty(teco.GRYY))
                {
                    if (teco.GRXX.Length > 5 && teco.GRYY.Length > 5)
                    {
                        Position position = new Position(Convert.ToDouble(teco.GRXX), Convert.ToDouble(teco.GRYY));
                        var HayCita = "";
                        if (teco.FechaCita == null)
                        {
                            HayCita = "SinCita";
                        }
                        else
                        {
                            if (teco.FechaCita.Value.Date == DateTime.Today)
                            {
                                HayCita = "ConCitaHoy";
                            }
                            else
                            {
                                HayCita = "ConCitaOtroDia";
                            }
                        }

                        MyMap.Pins.Add(new CustomPin
                        {
                            Label = teco.NOMBRE + " Cita: " + teco.FechaCita.ToString(),
                            Address = teco.DOMICILIO,
                            Position = position,
                            Type = PinType.Place,
                            StyleId = HayCita,
                            ClassId = "Teco",


                        });

                    }
                }
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