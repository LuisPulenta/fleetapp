using Prism;
using Prism.Ioc;
using Fleet_App.Prism.ViewModels;
using Fleet_App.Prism.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Fleet_App.Common.Services;
using Fleet_App.Common.Helpers;
using Fleet_App.Prism.ViewModels.Teco;
using Fleet_App.Prism.Views.Cable;
using Fleet_App.Prism.Views.Teco;
using Fleet_App.Prism.Views.Prisma;
using Fleet_App.Prism.Views.Tlc;
using Fleet_App.Prism.Views.Dtv;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Fleet_App.Prism
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjUyNDM5QDMxMzgyZTMxMmUzMGRSL2lEQVZDTTBuN3VCV09EQkFwZmV2S3A4QlF3c0FuR2JreTlKLy9UNVU9");

//            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTY2MzIyQDMxMzcyZTMzMmUzMFVnNW5KSnM2dTZmRDljWm1RYTduQXFwRmNKSzVPWk1lT1JGSFRySXZCUTA9");
            

            if (Settings.IsRemembered)
            {
                await NavigationService.NavigateAsync("/FleetMasterDetailPage/NavigationPage/HomePage");
            }
            else
            {
                await NavigationService.NavigateAsync("/NavigationPage/LoginPage");
            }

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.Register<IGeolocatorService, GeolocatorService>();

            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<FleetMasterDetailPage, FleetMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<RemotesPage, RemotesPageViewModel>();
            containerRegistry.RegisterForNavigation<CablesPage, CablesPageViewModel>();
            containerRegistry.RegisterForNavigation<DtvsPage, DtvsPageViewModel>();
            containerRegistry.RegisterForNavigation<DtvPage, DtvPageViewModel>();
            containerRegistry.RegisterForNavigation<DtvMapPage, DtvMapPageViewModel>();
            containerRegistry.RegisterForNavigation<TlcsPage, TlcsPageViewModel>();
            containerRegistry.RegisterForNavigation<TlcPage, TlcPageViewModel>();
            containerRegistry.RegisterForNavigation<TlcMapPage, TlcMapPageViewModel>();
            containerRegistry.RegisterForNavigation<PrismasPage, PrismasPageViewModel>();
            containerRegistry.RegisterForNavigation<PrismaPage, PrismaPageViewModel>();
            containerRegistry.RegisterForNavigation<PrismaMapPage, PrismaMapPageViewModel>();
      ;
            containerRegistry.RegisterForNavigation<DtvMapPage, DtvMapPageViewModel>();

            containerRegistry.RegisterForNavigation<OrdersPage, OrdersPageViewModel>();
            containerRegistry.RegisterForNavigation<RemotePage, RemotePageViewModel>();
            containerRegistry.RegisterForNavigation<DNIPicturePage, DNIPicturePageViewModel>();
            containerRegistry.RegisterForNavigation<FirmaPage, FirmaPageViewModel>();
            containerRegistry.RegisterForNavigation<RemoteMapPage, RemoteMapPageViewModel>();
            containerRegistry.RegisterForNavigation<AvisoPage, AvisoPageViewModel>();
            containerRegistry.RegisterForNavigation<Aviso2Page, Aviso2PageViewModel>();
            containerRegistry.RegisterForNavigation<RemotesMapPage, RemotesMapPageViewModel>();
            containerRegistry.RegisterForNavigation<CablePage, CablePageViewModel>();
            containerRegistry.RegisterForNavigation<CableScanCode2Page, CableScanCode2PageViewModel>();
            containerRegistry.RegisterForNavigation<CableOtroRecuperoPage, CableOtroRecuperoPageViewModel>();
            containerRegistry.RegisterForNavigation<TasasPage, TasasPageViewModel>();
            containerRegistry.RegisterForNavigation<TasaPage, TasaPageViewModel>();
            containerRegistry.RegisterForNavigation<Firma2Page, Firma2PageViewModel>();
            containerRegistry.RegisterForNavigation<DNIPicture2Page, DNIPicture2PageViewModel>();
            containerRegistry.RegisterForNavigation<TasasMapPage, TasasMapPageViewModel>();
            containerRegistry.RegisterForNavigation<TasaMapPage, TasaMapPageViewModel>();
            containerRegistry.RegisterForNavigation<MapsPage, MapsPageViewModel>();
            containerRegistry.RegisterForNavigation<CablesMapPage, CablesMapPageViewModel>();
            containerRegistry.RegisterForNavigation<CableMapPage, CableMapPageViewModel>();
            containerRegistry.RegisterForNavigation<CitaPage, CitaPageViewModel>();
            containerRegistry.RegisterForNavigation<TasaCitaPage, TasaCitaPageViewModel>();
            containerRegistry.RegisterForNavigation<PrismaCitaPage, PrismaCitaPageViewModel>();
            containerRegistry.RegisterForNavigation<DtvCitaPage, DtvCitaPageViewModel>();

            containerRegistry.RegisterForNavigation<TlcCitaPage, TlcCitaPageViewModel>();
            containerRegistry.RegisterForNavigation<TlcMapPage, TlcMapPageViewModel>();
          

            containerRegistry.RegisterForNavigation<RemoteCitaPage, RemoteCitaPageViewModel>();
            containerRegistry.RegisterForNavigation<EstadisticasPage, EstadisticasPageViewModel>();
            containerRegistry.RegisterForNavigation<GraphPage, GraphPageViewModel>();
            containerRegistry.RegisterForNavigation<Graph2Page, Graph2PageViewModel>();
            containerRegistry.RegisterForNavigation<Graph3Page, Graph3PageViewModel>();
            containerRegistry.RegisterForNavigation<TasaScanCodePage, ViewModels.CableScanCodePageViewModel>();
            containerRegistry.RegisterForNavigation<DevolucionesPage, DevolucionesPageViewModel>();
            containerRegistry.RegisterForNavigation<TasaOtroRecuperoPage, TasaOtroRecuperoPageViewModel>();
            containerRegistry.RegisterForNavigation<TasaScanCode2Page, TasaScanCode2PageViewModel>();
            containerRegistry.RegisterForNavigation<Graph4Page, Graph4PageViewModel>();
            containerRegistry.RegisterForNavigation<CableScanCodePage, ViewModels.Cable.CableScanCodePageViewModel>();
            containerRegistry.RegisterForNavigation<TecoCitaPage, TecoCitaPageViewModel>();
            containerRegistry.RegisterForNavigation<TecoMapPage, TecoMapPageViewModel>();
            containerRegistry.RegisterForNavigation<TecoPage, TecoPageViewModel>();
            containerRegistry.RegisterForNavigation<TecosMapPage, TecosMapPageViewModel>();
            containerRegistry.RegisterForNavigation<TecosPage, TecosPageViewModel>();
            containerRegistry.RegisterForNavigation<TecoDniPicturePage, TecoDniPicturePageViewModel>();
            containerRegistry.RegisterForNavigation<TecoOtherPicturePage, TecoOtherPicturePageViewModel>();
            containerRegistry.RegisterForNavigation<ContactPage, ContactPageViewModel>();
            containerRegistry.RegisterForNavigation<DtvOtroRecuperoPage, DtvOtroRecuperoPageViewModel>();
            containerRegistry.RegisterForNavigation<DtvScanCode2Page, DtvScanCode2PageViewModel>();

        }
    }
}
