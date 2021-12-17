using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Fleet_App.Common.Services;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace Fleet_App.Prism.ViewModels
{
    public class CableScanCode2PageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ControlTasa _modem;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _buttonCommand;
        public DelegateCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(Cancel));
        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(Save));
        public DelegateCommand ButtonCommand => _buttonCommand ?? (_buttonCommand = new DelegateCommand(Button));
        public ControlTasa Modem { get => _modem; set => _modem = value; }

        private string _result;

        public string Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }




        public CableScanCode2PageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Modem = JsonConvert.DeserializeObject<ControlTasa>(Settings.Modem);
            Title = "Scanear Código QR";
        }

        private async void Cancel()
        {
            await _navigationService.GoBackAsync();
        }

        private async void Save()
        {


            CableOtroRecuperoPageViewModel cableOtroRecuperoPageViewModel = CableOtroRecuperoPageViewModel.GetInstance();

            
            cableOtroRecuperoPageViewModel.NROSERIEEXTRA = Result;
            
            
            await _navigationService.GoBackAsync();
        }


        private async void Button()
        {
            var options = new MobileBarcodeScanningOptions();
            options.PossibleFormats = new List<BarcodeFormat>
            {
                BarcodeFormat.QR_CODE,
                BarcodeFormat.CODE_128,
                BarcodeFormat.EAN_13,
                
                BarcodeFormat.CODABAR,
                BarcodeFormat.UPC_A,
                BarcodeFormat.UPC_E,
                BarcodeFormat.EAN_8,
                BarcodeFormat.CODE_39,
                BarcodeFormat.CODE_93,
                BarcodeFormat.ITF
            };
            var page = new ZXingScannerPage(options) { Title = "Scanner" };
            var closeItem = new ToolbarItem { Text = "Cerrar" };
            closeItem.Clicked += (object sender, EventArgs e) =>
            {
                page.IsScanning = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.Navigation.PopModalAsync();
                });
            };
            page.ToolbarItems.Add(closeItem);
            page.OnScanResult += (result) =>
            {
                page.IsScanning = false;

                Device.BeginInvokeOnMainThread(() => {
                    Application.Current.MainPage.Navigation.PopModalAsync();
                    if (string.IsNullOrEmpty(result.Text))
                    {
                        Result = "El código escaneado no es válido";
                    }
                    else
                    {
                        Result = result.Text;
                    }
                });
            };
            Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(page) { BarTextColor = Color.White, BarBackgroundColor = Color.CadetBlue }, true);
        }
    }
}

