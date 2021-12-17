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
    public class TasaCitaPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private DateTime _fechaDeCita;
        private TimeSpan _horaDeCita;
        private DateTime _hoy;


        private AsignacionesOT _tasa;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isEnabledParcial;
        private bool _isRefreshing;
        private bool _habilitado;
        private MedioCita _medioCita;
        private String _medioDeCita;
        private ObservableCollection<ModemItemViewModel> _controlTasas;
        private ObservableCollection<MedioCita> _mediosCita;
        private DelegateCommand _tasaMapCommand;
        #region Properties
        public ReclamoTasa Tasa { get; set; }
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
        public ObservableCollection<ModemItemViewModel> ControlTasas
        {
            get => _controlTasas;
            set => SetProperty(ref _controlTasas, value);
        }
        public ObservableCollection<MedioCita> MediosCita
        {
            get => _mediosCita;
            set => SetProperty(ref _mediosCita, value);
        }
        public List<CodigoCierre> MyCodigosCierre { get; set; }
        public List<ControlTasa> MyControlTasas { get; set; }
        #endregion

        private DelegateCommand _cancelCommand;
        private DelegateCommand _saveCommand;

        private DelegateCommand _phoneCallCommand;


        public DelegateCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(Cancel));
        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(Save));
        public DelegateCommand PhoneCallCommand => _phoneCallCommand ?? (_phoneCallCommand = new DelegateCommand(PhoneCall));




        public TasaCitaPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;

            FechaDeCita = DateTime.Today;  //.AddDays(0)
            Hoy = DateTime.Now;


            Title = "Registrar Cita";
            instance = this;
            Tasa = JsonConvert.DeserializeObject<ReclamoTasa>(Settings.Tasa);

            LoadControlTasas();
            LoadMediosCita();


            IsEnabled = true;
            IsRefreshing = false;

            if (Tasa.CantRec == 1)
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

        private static TasaCitaPageViewModel instance;
        public static TasaCitaPageViewModel GetInstance()
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


            //await App.Current.MainPage.DisplayAlert("Hora", HoraDeCita.Hours.ToString(), "Aceptar");
            //await App.Current.MainPage.DisplayAlert("Hora", HoraDeCita.Minutes.ToString(), "Aceptar");

            //*********************************************************************************************************
            //Grabar 
            //*********************************************************************************************************

            var ya = DateTime.Now;


            foreach (var cc in ControlTasas)
            {
                var fec1 = Tasa.FechaEvento1;
                var fec2 = Tasa.FechaEvento2;
                var fec3 = Tasa.FechaEvento3;

                var evento1 = Tasa.Evento1;
                var evento2 = Tasa.Evento2;
                var evento3 = Tasa.Evento3;


                var mycc = new AsignacionesOT
                {
                    IDREGISTRO = cc.IDREGISTRO,
                    RECUPIDJOBCARD = cc.RECUPIDJOBCARD,
                    CLIENTE = Tasa.CLIENTE,
                    NOMBRE = Tasa.NOMBRE,
                    DOMICILIO = Tasa.DOMICILIO,
                    ENTRECALLE1 = Tasa.ENTRECALLE1,
                    ENTRECALLE2 = Tasa.ENTRECALLE2,
                    CP = Tasa.CP,
                    LOCALIDAD = Tasa.LOCALIDAD,
                    PROVINCIA = Tasa.PROVINCIA,
                    TELEFONO = Tasa.TELEFONO,
                    GRXX = Tasa.GRXX,
                    GRYY = Tasa.GRYY,
                    ESTADO = cc.ESTADO,
                    ESTADO2 = cc.ESTADO2,
                    ESTADO3 = cc.ESTADO3,
                    ImageArrayDni = cc.ImageArrayDni,
                    ImageArrayFirma = cc.ImageArrayFirma,
                    UrlDni = cc.UrlDni,
                    UrlFirma = cc.UrlFirma,
                    ZONA = cc.ZONA,
                    ESTADOGAOS = Tasa.ESTADOGAOS,
                    FECHACUMPLIDA = cc.FECHACUMPLIDA,
                    SUBCON = Tasa.SUBCON,
                    CAUSANTEC = Tasa.CAUSANTEC,
                    FechaAsignada = Tasa.FechaAsignada,
                    PROYECTOMODULO = cc.PROYECTOMODULO,
                    DECO1 = cc.DECO1,
                    CMODEM1 = cc.CMODEM1,
                    Observacion = Tasa.Observacion,
                    HsCumplida = cc.HsCumplida,
                    UserID = Tasa.UserID,
                    CodigoCierre = cc.CodigoCierre,
                    CantRem = Tasa.CantRem,
                    Autonumerico = cc.Autonumerico,
                    HsCumplidaTime = cc.HsCumplidaTime,
                    ObservacionCaptura = Tasa.ObservacionCaptura,
                    Novedades = Tasa.Novedades,
                    ReclamoTecnicoID = cc.ReclamoTecnicoID,
                    MODELO = cc.MODELO,
                    Motivos = cc.Motivos,
                    IDSuscripcion = cc.IDSuscripcion,
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
            //***** Borrar de la lista de Tasas *****

            Tasa.FechaCita = FechaDeCita.Add(HoraDeCita);
            Tasa.MedioCita = MedioDeCita;

            var newTasa = Tasa;
            var tasasViewModel = TasasPageViewModel.GetInstance();




            var oldTasa = tasasViewModel.MyTasas.Where(o => o.ReclamoTecnicoID == this.Tasa.ReclamoTecnicoID).FirstOrDefault();


            tasasViewModel.MyTasas.Remove(oldTasa);
            tasasViewModel.MyTasas.Add(newTasa);

            tasasViewModel.LoadUser();
            tasasViewModel.RefreshList();

            await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
            await _navigationService.GoBackAsync();
            return;


            //********************************************
        }


        private async void PhoneCall()
        {
            await Clipboard.SetTextAsync(Tasa.TELEFONO);
            PhoneDialer.Open(Tasa.TELEFONO);
        }

        private async void Cancel()
        {
            Tasa.ESTADOGAOS = "PEN";
            await _navigationService.GoBackAsync();
        }

        private async void LoadControlTasas()
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


            //Buscar los autonumericos del Tasa seleccionado
            var controller = string.Format("/ControlTasas/GetAutonumericos", Tasa.ReclamoTecnicoID, Tasa.UserID);


            var response = await _apiService.GetList3Async<ControlTasa>(
                 url,
                "api",
                controller,
                Tasa.ReclamoTecnicoID,
                Tasa.UserID);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
            }
            MyControlTasas = (List<ControlTasa>)response.Result;
            RefreshList();
            IsRefreshing = false;
        }


        public void RefreshList()
        {
            var myListControls = this.MyControlTasas.Select(p => new ModemItemViewModel(_navigationService)
            {
                IDREGISTRO = p.IDREGISTRO,
                RECUPIDJOBCARD = p.RECUPIDJOBCARD,
                ReclamoTecnicoID = p.ReclamoTecnicoID,
                IDSuscripcion = p.IDSuscripcion,
                ESTADOGAOS = p.ESTADOGAOS,
                PROYECTOMODULO = p.PROYECTOMODULO,
                FECHACUMPLIDA = p.FECHACUMPLIDA,
                HsCumplidaTime = p.HsCumplidaTime,
                CodigoCierre = p.CodigoCierre,
                Autonumerico = p.Autonumerico,
                DECO1 = p.DECO1,
                CMODEM1 = p.CMODEM1,
                ESTADO = p.ESTADO,
                ZONA = p.ZONA,
                HsCumplida = p.HsCumplida,
                Observacion = p.Observacion,
                MODELO = p.MODELO,
                MarcaModeloId = p.MarcaModeloId,
                Motivos = p.Motivos,
                Elegir = p.Elegir,
               
            }); ;
            this.ControlTasas = new ObservableCollection<ModemItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
        }


    }
}
