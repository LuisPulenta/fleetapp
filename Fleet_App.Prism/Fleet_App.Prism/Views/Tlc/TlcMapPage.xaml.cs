using Fleet_App.Common.Services;
using Fleet_App.Prism.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Fleet_App.Prism.Views.Tlc
{
    public partial class TlcMapPage : ContentPage
    {
        private readonly IGeolocatorService _geolocatorService;
        public TlcMapPage(IGeolocatorService geolocatorService)
        {
            InitializeComponent();
            _geolocatorService = geolocatorService;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MyMap.IsVisible = false;
            MoveMapToCurrentPositionAsync();
            MyMap.IsVisible = true;
            ShowPinsAsync();
        }

        private async void ShowPinsAsync()
        {
            var tlcPageViewModel = TlcPageViewModel.GetInstance();
            
            double latitude = Convert.ToDouble(tlcPageViewModel.Tlc.GRXX);
            double longitude = Convert.ToDouble(tlcPageViewModel.Tlc.GRYY);
            
            var position = new Position(latitude, longitude);
            MyMap.Pins.Add(new Pin
            {
                Address = tlcPageViewModel.Tlc.DOMICILIO,
                Label = $"{tlcPageViewModel.Tlc.CLIENTE}-{tlcPageViewModel.Tlc.NOMBRE}",
                Position = position,
                Type = PinType.Place
            });

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