using Fleet_App.Common.Helpers;
using Fleet_App.Prism.ViewModels;
using Prism.Navigation;
using SignaturePad.Forms;
using System;
using System.IO;
using Xamarin.Forms;

namespace Fleet_App.Prism.Views
{
    public partial class FirmaPage : ContentPage
    {
        private readonly INavigationService _navigationService;


      
        public FirmaPage(INavigationService navigationService)
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
                var remoteViewModel = RemotePageViewModel.GetInstance();
                remoteViewModel.HayFirma = true;

                remoteViewModel.ImageSource2 = ImageSource.FromStream(() =>
                {
                    var streamFirma = stream;
                    return streamFirma;
                });
                var remoteViewModel2 = RemotePageViewModel.GetInstance();
                remoteViewModel2.StreamFirma = image;
                remoteViewModel2.ImageArrayFirma = FilesHelper.ReadFully(remoteViewModel2.StreamFirma);


            }
            var rvm = FirmaPageViewModel.GetInstance();
            rvm.CancelCommand.Execute(); 
        }
        
    }
}
