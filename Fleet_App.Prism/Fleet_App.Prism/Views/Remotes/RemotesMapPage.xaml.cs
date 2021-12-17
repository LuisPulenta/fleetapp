using Fleet_App.Common.Services;
using Fleet_App.Prism.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Fleet_App.Prism.Views
{
    public partial class RemotesMapPage : ContentPage
    {
        private readonly IGeolocatorService _geolocatorService;

        public RemotesMapPage(IGeolocatorService geolocatorService)
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
            RemotesPageViewModel RemotesViewModel = RemotesPageViewModel.GetInstance();
            foreach (RemoteItemViewModel Remote in RemotesViewModel.Remotes.ToList())
            {
                if (!string.IsNullOrEmpty(Remote.GRXX) && !string.IsNullOrEmpty(Remote.GRYY))
                {
                    if (Remote.GRXX.Length > 5 && Remote.GRYY.Length > 5)
                    {
                        Position position = new Position(Convert.ToDouble(Remote.GRXX), Convert.ToDouble(Remote.GRYY));
                        var HayCita = "";
                        if (Remote.FechaCita == null)
                        {
                            HayCita = "SinCita";
                        }
                        else
                        {
                            if (Remote.FechaCita.Value.Date == DateTime.Today)
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
                            Label = Remote.NOMBRE + " Cita: " + Remote.FechaCita.ToString(),
                            Address = Remote.DOMICILIO,
                            Position = position,
                            Type = PinType.Place,
                            StyleId = HayCita,
                            ClassId = "Remote",


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