using Fleet_App.Common.Helpers;
using Fleet_App.Prism.ViewModels;
using Prism.Navigation;
using SignaturePad.Forms;
using System;
using System.IO;
using Xamarin.Forms;

namespace Fleet_App.Prism.Views
{
    public partial class Firma2Page : ContentPage
    {
        private readonly INavigationService _navigationService;



        public Firma2Page(INavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;
        }

        public async void Save(object sender, EventArgs eventArgs)
        {
            Stream stream = await signatureSample.GetImageStreamAsync(SignatureImageFormat.Png);
            Stream stream2 = await signatureSample.GetImageStreamAsync(SignatureImageFormat.Png);

            var image = await signatureSample.GetImageStreamAsync(
                    SignatureImageFormat.Png,
                    strokeColor: Color.Black,
                    fillColor: Color.White);

            if (image != null)
            {
                var tasaViewModel = TasaPageViewModel.GetInstance();
                tasaViewModel.HayFirma = true;

                tasaViewModel.ImageSource2 = ImageSource.FromStream(() =>
                {
                    var streamFirma = stream;
                    return streamFirma;
                });
                var tasaViewModel2 = TasaPageViewModel.GetInstance();
                tasaViewModel2.StreamFirma = image;
                tasaViewModel2.ImageArrayFirma = FilesHelper.ReadFully(tasaViewModel2.StreamFirma);


            }
            var rvm = Firma2PageViewModel.GetInstance();
            rvm.CancelCommand.Execute();
        }

    }
}
