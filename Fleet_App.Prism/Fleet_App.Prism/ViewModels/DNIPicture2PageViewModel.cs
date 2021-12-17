using Fleet_App.Common.Models;
using Fleet_App.Common.Services;
using Microsoft.AspNetCore.Http;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;


namespace Fleet_App.Prism.ViewModels
{
    public class DNIPicture2PageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;

        private Picture orderPicture;
        private Tasa _tasa;

        private bool _isRunning;
        private bool _isEnabled;
        private ImageSource _imageSource;
        private MediaFile _file;
        public Tasa Tasa
        {
            get => _tasa;
            set => SetProperty(ref _tasa, value);
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

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        private DelegateCommand _cancelCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _takeDNICommand;

        public DelegateCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(Cancel));
        public DelegateCommand SaveCommand => _saveCommand ?? (_cancelCommand = new DelegateCommand(Save));
        public DelegateCommand TakeDNICommand => _takeDNICommand ?? (_cancelCommand = new DelegateCommand(TakeDNI));

        public DNIPicture2PageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Tomar Fotografía DNI";
            IsEnabled = true;
            ImageSource = "nophoto";
        }

        private async void Cancel()
        {
            await _navigationService.GoBackAsync();
        }



        private async void TakeDNI()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                "De dónde quiere tomar la imagen?",
                "Cancelar",
                null,
                "Galería",
                "Nueva Foto");

            if (source == "Cancelar")
            {
                _file = null;
                return;
            }

            if (source == "Nueva Foto")
            {
                _file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                _file = await CrossMedia.Current.PickPhotoAsync();
            }
            if (_file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = _file.GetStream();
                    return stream;
                });
                var tasaViewModel = TasaPageViewModel.GetInstance();
                tasaViewModel.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = _file.GetStream();
                    return stream;
                });
                tasaViewModel.File = _file;
            }
            IsRunning = false;
        }


        private async void Save()
        {
            if (_file == null)
            {
                var tasaViewModel = TasaPageViewModel.GetInstance();
                tasaViewModel.ImageSource = "nophoto";
            }
            await _navigationService.GoBackAsync();
        }
    }
}