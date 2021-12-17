using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Fleet_App.Common.Services;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Fleet_App.Prism.ViewModels
{
    public class RemoteCitaPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private DateTime _fechaDeCita;
        private TimeSpan _horaDeCita;
        private DateTime _hoy;


        private AsignacionesOT _remote;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isEnabledParcial;
        private bool _isRefreshing;
        private bool _habilitado;
        private MedioCita _medioCita;
        private String _medioDeCita;
        private ObservableCollection<Control> _controls;
        private ObservableCollection<RemoteItemViewModel> _controlRemotes;
        private ObservableCollection<MedioCita> _mediosCita;
        private DelegateCommand _remoteMapCommand;
        #region Properties
        public Reclamo Remote { get; set; }
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        //public bool Habilitado { get => _habilitado; set => _habilitado = value; }
        public bool Habilitado
        {
            get => _habilitado;
            set => SetProperty(ref _habilitado, value);
        }
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public ObservableCollection<Control> Controls
        {
            get => _controls;
            set => SetProperty(ref _controls, value);
        }

        public DateTime FechaDeCita
        {
            get => _fechaDeCita;
            set => SetProperty(ref _fechaDeCita, value);
        }

        public DateTime Hoy
        {
            get => _hoy;
            set => SetProperty(ref _hoy, value);
        }


        public TimeSpan HoraDeCita
        {
            get => _horaDeCita;
            set => SetProperty(ref _horaDeCita, value);
        }

        public bool IsEnabledParcial
        {
            get => _isEnabledParcial;
            set => SetProperty(ref _isEnabledParcial, value);
        }
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }
        public MedioCita MedioCita
        {
            get => _medioCita;
            set => SetProperty(ref _medioCita, value);
        }

        public string MedioDeCita
        {
            get => _medioDeCita;
            set => SetProperty(ref _medioDeCita, value);
        }
        public ObservableCollection<RemoteItemViewModel> ControlRemotes
        {
            get => _controlRemotes;
            set => SetProperty(ref _controlRemotes, value);
        }
        public ObservableCollection<MedioCita> MediosCita
        {
            get => _mediosCita;
            set => SetProperty(ref _mediosCita, value);
        }
        public List<CodigoCierre> MyCodigosCierre { get; set; }
        public List<Control> MyControls { get; set; }
        #endregion

        private DelegateCommand _cancelCommand;
        private DelegateCommand _saveCommand;

        private DelegateCommand _phoneCallCommand;


        public DelegateCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(Cancel));
        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(Save));
        public DelegateCommand PhoneCallCommand => _phoneCallCommand ?? (_phoneCallCommand = new DelegateCommand(PhoneCall));




        public RemoteCitaPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;

            FechaDeCita = DateTime.Today;  //.AddDays(0)
            Hoy = DateTime.Now;


            Title = "Registrar Cita";
            instance = this;
            Remote = JsonConvert.DeserializeObject<Reclamo>(Settings.Remote);

            LoadControls();
            LoadMediosCita();


            IsEnabled = true;
            IsRefreshing = false;

            if (Remote.CantRec == 1)
            {
                IsEnabledParcial = false;
            }
            else
            {
                IsEnabledParcial = true;
            }
            this.Habilitado = false;
        }

        #region Singleton

        private static RemoteCitaPageViewModel instance;
        public static RemoteCitaPageViewModel GetInstance()
        {
            return instance;
        }
        #endregion








        private void LoadMediosCita()
        {
            MediosCita = new ObservableCollection<MedioCita>();
            MediosCita.Add(new MedioCita { Codigo = 1, Descripcion = "Teléfono", });
            MediosCita.Add(new MedioCita { Codigo = 2, Descripcion = "WhatsApp", });
            MediosCita.Add(new MedioCita { Codigo = 3, Descripcion = "SMS", });
            MediosCita.Add(new MedioCita { Codigo = 4, Descripcion = "Visita directa", });


            //CodigosCierre.Add(new CodigoCierre { Codigo = 13, Descripcion = "Acepta Retiro", });
        }





        private async void Save()
        {


            //if (string.IsNullOrEmpty(MedioDeCita))
            if (MedioCita == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe seleccionar un medio de cita.", "Aceptar");
                return;
            }


            //Verificar conectividad
            var url = App.Current.Resources["UrlAPI"].ToString();
            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsEnabled = true;
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "Chequee su conexión a Internet.", "Aceptar");
                return;
            }
            IsRunning = true;
            IsEnabled = false;

            MedioDeCita = MedioCita.Descripcion;
            var ya = DateTime.Now;

            foreach (var cc in Controls)
            {
                var fec1 = Remote.FechaEvento1;
                var fec2 = Remote.FechaEvento2;
                var fec3 = Remote.FechaEvento3;

                var evento1 = Remote.Evento1;
                var evento2 = Remote.Evento2;
                var evento3 = Remote.Evento3;


                var mycc = new AsignacionesOT
                {
                    IDREGISTRO = cc.IDREGISTRO,
                    RECUPIDJOBCARD = cc.RECUPIDJOBCARD,
                    ESTADOGAOS = Remote.ESTADOGAOS,
                    PROYECTOMODULO = cc.PROYECTOMODULO,
                    FECHACUMPLIDA = cc.FECHACUMPLIDA,
                    HsCumplidaTime = cc.HsCumplidaTime,
                    CodigoCierre = cc.CodigoCierre,
                    Autonumerico = cc.Autonumerico,
                    ImageArrayDni = cc.ImageArrayDni,
                    ImageArrayFirma = cc.ImageArrayFirma,
                    CLIENTE = Remote.CLIENTE,
                    NOMBRE = Remote.NOMBRE,
                    DOMICILIO = Remote.DOMICILIO,
                    ENTRECALLE1 = Remote.ENTRECALLE1,
                    ENTRECALLE2 = Remote.ENTRECALLE2,


                    CP = Remote.CP,
                    DECO1 = cc.DECO1,
                    CMODEM1 = cc.CMODEM1,
                    ESTADO = cc.ESTADO,
                    ZONA = cc.ZONA,
                    HsCumplida = cc.HsCumplida,
                    Observacion = cc.Observacion,
                    UrlDni = cc.UrlDni,
                    UrlFirma = cc.UrlFirma,
                    ObservacionCaptura = Remote.ObservacionCaptura,
                    Novedades = Remote.Novedades,


                    LOCALIDAD = Remote.LOCALIDAD,
                    TELEFONO = Remote.TELEFONO,

                    GRXX = Remote.GRXX,
                    GRYY = Remote.GRYY,
                    //EntreCalles = Remote.EntreCalles,
                    UserID = Remote.UserID,
                    CAUSANTEC = Remote.CAUSANTEC,
                    SUBCON = Remote.SUBCON,
                    FechaAsignada = Remote.FechaAsignada,
                    ReclamoTecnicoID = cc.ReclamoTecnicoID,

                    CantRem = Remote.CantRem,

                    FechaCita = FechaDeCita.Add(HoraDeCita),
                    MedioCita = MedioDeCita,
                    Evento4 = evento3,
                    FechaEvento4 = fec3,
                    Evento3 = evento2,
                    FechaEvento3 = fec2,
                    Evento2 = evento1,
                    FechaEvento2 = fec1,
                    Evento1 = $"Cita por {MedioDeCita} para el {FechaDeCita.Add(HoraDeCita)}",
                    FechaEvento1 = ya,
                };

                var response = await _apiService.PutAsync(
                url,
                "api",
                "/AsignacionesOTs",
                mycc,
                mycc.IDREGISTRO);

                IsRunning = false;
                IsEnabled = true;

                if (!response.IsSuccess)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Ocurrió un error al guardar la Orden, intente más tarde.", "Aceptar");
                    return;
                }
            }
            //***** Borrar de la lista de Remotes *****

            Remote.FechaCita = FechaDeCita.Add(HoraDeCita);
            Remote.MedioCita = MedioDeCita;

            var newRemote = Remote;
            var RemotesViewModel = RemotesPageViewModel.GetInstance();




            var oldRemote = RemotesViewModel.MyRemotes.FirstOrDefault(o => o.ReclamoTecnicoID == this.Remote.ReclamoTecnicoID);


            RemotesViewModel.MyRemotes.Remove(oldRemote);
            RemotesViewModel.MyRemotes.Add(newRemote);

            RemotesViewModel.LoadUser();
            RemotesViewModel.RefreshList();

            await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
            await _navigationService.GoBackAsync();
            return;


            //********************************************
        }


        private async void PhoneCall()
        {
            await Clipboard.SetTextAsync(Remote.TELEFONO);
            PhoneDialer.Open(Remote.TELEFONO);
        }

        private async void Cancel()
        {
            Remote.ESTADOGAOS = "PEN";
            await _navigationService.GoBackAsync();
        }

        private async void LoadControls()
        {
            this.IsRefreshing = true;
            this.Habilitado = false;

            //Verificar conectividad
            var url = App.Current.Resources["UrlAPI"].ToString();
            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsEnabled = true;
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert("Error", "Chequee su conexión a Internet.", "Aceptar");
                return;
            }


            //Buscar los autonumericos del Remote seleccionado
            var controller = string.Format("/Controls/GetAutonumericos", Remote.RECUPIDJOBCARD, Remote.UserID);


            var response = await _apiService.GetListAsync<Control>(
                 url,
                "api",
                controller,
                Remote.RECUPIDJOBCARD,
                Remote.UserID);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
            }
            MyControls = (List<Control>)response.Result;
            RefreshList();
            IsRefreshing = false;
        }


        public void RefreshList()
        {
            var myListControls = MyControls.Select(p => new Control
            {
                IDREGISTRO = p.IDREGISTRO,
                Autonumerico = p.Autonumerico,
                CodigoCierre = p.CodigoCierre,
                RECUPIDJOBCARD = p.RECUPIDJOBCARD,
                ESTADOGAOS = p.ESTADOGAOS,
                PROYECTOMODULO = p.PROYECTOMODULO,
                FECHACUMPLIDA = p.FECHACUMPLIDA,
                HsCumplidaTime = p.HsCumplidaTime,
                UrlDni = p.UrlDni,
                UrlFirma = p.UrlFirma,
                ImageArrayDni = p.ImageArrayDni,
                ImageArrayFirma = p.ImageArrayDni,
                DECO1 = p.DECO1,
                CMODEM1 = p.CMODEM1,
                ESTADO = p.ESTADO,
                ZONA = p.ZONA,
                HsCumplida = p.HsCumplida,
                Observacion = p.Observacion,
                ReclamoTecnicoID = p.ReclamoTecnicoID,
                ControlesEquivalencia = new ControlesEquivalencia
                {
                    CODIGOEQUIVALENCIA = p.ControlesEquivalencia.CODIGOEQUIVALENCIA,
                    DECO1 = p.ControlesEquivalencia.DECO1,
                    DESCRIPCION = p.ControlesEquivalencia.DESCRIPCION,
                    ID = p.ControlesEquivalencia.ID
                }


            }); ;
            Controls = new ObservableCollection<Control>(myListControls.OrderBy(p => p.Autonumerico));
        }


    }
}
