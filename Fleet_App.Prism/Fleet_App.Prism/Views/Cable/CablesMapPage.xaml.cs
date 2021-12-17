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
    public partial class CablesMapPage : ContentPage
    {
        private readonly IGeolocatorService _geolocatorService;

        public CablesMapPage(IGeolocatorService geolocatorService)
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
            CablesPageViewModel cablesViewModel = CablesPageViewModel.GetInstance();
            foreach (CableItemViewModel cable in cablesViewModel.Cables.ToList())
            {
                if (!string.IsNullOrEmpty(cable.GRXX) && !string.IsNullOrEmpty(cable.GRYY))
                {
                    if (cable.GRXX.Length > 5 && cable.GRYY.Length > 5)
                    {
                        Position position = new Position(Convert.ToDouble(cable.GRXX), Convert.ToDouble(cable.GRYY));
                        var HayCita = "";
                        if (cable.FechaCita == null)
                        {
                            HayCita = "SinCita";
                        }
                        else
                        {
                            if (cable.FechaCita.Value.Date == DateTime.Today)
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
                            Label = cable.NOMBRE + " Cita: "+cable.FechaCita.ToString(),
                            Address = cable.DOMICILIO,
                            Position = position,
                            Type = PinType.Place,
                            StyleId = HayCita,
                            ClassId= "Dtv",


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