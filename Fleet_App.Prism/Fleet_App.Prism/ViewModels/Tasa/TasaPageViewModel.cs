using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Fleet_App.Common.Services;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Fleet_App.Common.Codigos;

namespace Fleet_App.Prism.ViewModels
{
    public class TasaPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;


        private AsignacionesOT _tasa;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isEnabledParcial;
        private bool _isRefreshing;
        private bool _habilitado;
        private CodigoCierre _cCierre;
        
        private ObservableCollection<ModemItemViewModel> _controlTasas;
        private ObservableCollection<CodigoCierre> _codigosCierre;
        

        public byte[] ImageArray { get; set; }
        private MediaFile _file;
        private MediaFile _file2;
        private ImageSource _imageSource;
        private ImageSource _imageSource2;
        private Picture _dniPicture;
        private bool _hayFirma;
        private Stream _streamFirma;
        private byte[] _imageArrayFirma;
        private DelegateCommand _takeDNICommand;
        private DelegateCommand _takeFirmaCommand;
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
        public CodigoCierre CCierre
        {
            get => _cCierre;
            set => SetProperty(ref _cCierre, value);
        }

        
        public ObservableCollection<ModemItemViewModel> ControlTasas
        {
            get => _controlTasas;
            set => SetProperty(ref _controlTasas, value);
        }
        public ObservableCollection<CodigoCierre> CodigosCierre
        {
            get => _codigosCierre;
            set => SetProperty(ref _codigosCierre, value);
        }

        public byte[] ImageArrayFirma
        {
            get => _imageArrayFirma;
            set => SetProperty(ref _imageArrayFirma, value);
        }
        public MediaFile File
        {
            get => _file;
            set => SetProperty(ref _file, value);
        }
        public MediaFile File2
        {
            get => _file2;
            set => SetProperty(ref _file2, value);
        }
        public Stream StreamFirma
        {
            get => _streamFirma;
            set => SetProperty(ref _streamFirma, value);
        }
        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }
        public ImageSource ImageSource2
        {
            get => _imageSource2;
            set => SetProperty(ref _imageSource2, value);
        }

        public Picture DNIPicture
        {
            get => _dniPicture;
            set => SetProperty(ref _dniPicture, value);
        }
        public bool HayFirma
        {
            get => _hayFirma;
            set => SetProperty(ref _hayFirma, value);
        }

        public List<CodigoCierre> MyCodigosCierre { get; set; }
        public List<ControlTasa> MyControlTasas { get; set; }
        #endregion

        private DelegateCommand _cancelCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _elijeTodosCommand;
        private DelegateCommand _deselijeTodosCommand;
        private DelegateCommand _deselijeTodos2Command;
        
        private DelegateCommand _phoneCallCommand;
        private DelegateCommand _phoneCallCommand1;
        private DelegateCommand _phoneCallCommand2;
        private DelegateCommand _phoneCallCommand3;
        private DelegateCommand _phoneCallCommand4;

        private DelegateCommand _otroRecuperoCommand;
        public DelegateCommand OtroRecuperoCommand => _otroRecuperoCommand ?? (_otroRecuperoCommand = new DelegateCommand(OtroRecupero));
        

        public DelegateCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(Cancel));
        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(Save));
        public DelegateCommand PhoneCallCommand => _phoneCallCommand ?? (_phoneCallCommand = new DelegateCommand(PhoneCall));
        public DelegateCommand PhoneCallCommand1 => _phoneCallCommand1 ?? (_phoneCallCommand1 = new DelegateCommand(PhoneCall1));
        public DelegateCommand PhoneCallCommand2 => _phoneCallCommand2 ?? (_phoneCallCommand2 = new DelegateCommand(PhoneCall2));
        public DelegateCommand PhoneCallCommand3 => _phoneCallCommand3 ?? (_phoneCallCommand3 = new DelegateCommand(PhoneCall3));
        public DelegateCommand PhoneCallCommand4 => _phoneCallCommand4 ?? (_phoneCallCommand4 = new DelegateCommand(PhoneCall4));

        public DelegateCommand ElijeTodosCommand => _elijeTodosCommand ?? (_elijeTodosCommand = new DelegateCommand(ElijeTodos));
        public DelegateCommand DeselijeTodosCommand => _deselijeTodosCommand ?? (_deselijeTodosCommand = new DelegateCommand(DeselijeTodos));
        public DelegateCommand DeselijeTodos2Command => _deselijeTodos2Command ?? (_deselijeTodos2Command = new DelegateCommand(DeselijeTodos2));
        
        public DelegateCommand TasaMapCommand => _tasaMapCommand ?? (_tasaMapCommand = new DelegateCommand(TasaMap));
        public DelegateCommand TakeDNICommand => _takeDNICommand ?? (_takeDNICommand = new DelegateCommand(TakeDNI));
        public DelegateCommand TakeFirmaCommand => _takeFirmaCommand ?? (_takeFirmaCommand = new DelegateCommand(TakeFirma));
        

        public TasaPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;

            Title = "Recupero de Tasa";
            instance = this;
            Tasa = JsonConvert.DeserializeObject<ReclamoTasa>(Settings.Tasa);

            LoadControlTasas();
            LoadCodigosCierre();
            

            ImageSource = "nophoto.png";
            ImageSource2 = "firma.png";

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
            Habilitado = false;
        }

        #region Singleton

        private static TasaPageViewModel instance;
        public static TasaPageViewModel GetInstance()
        {
            return instance;
        }
        #endregion


        private async void LoadControlTasas()
        {
            IsRefreshing = true;
            Habilitado = false;

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


            //Buscar los autonumericos del tasa seleccionado
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
                UrlDni = p.UrlDni,
                UrlFirma = p.UrlFirma,
                ImageArrayDni = p.ImageArrayDni,
                ImageArrayFirma = p.ImageArrayDni,
                Activo=p.Activo,
                Habilita=p.Habilita,
                ESTADO3=p.ESTADO3,
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

        public void RefreshList2()
        {
            var myListControls = this.ControlTasas.Select(p => new ModemItemViewModel(_navigationService)
            {
                IDREGISTRO = p.IDREGISTRO,
                RECUPIDJOBCARD = p.RECUPIDJOBCARD,
                ReclamoTecnicoID = p.ReclamoTecnicoID,
                IDSuscripcion = p.IDSuscripcion,
                ESTADOGAOS = p.ESTADOGAOS,
                PROYECTOMODULO = p.PROYECTOMODULO,
                FECHACUMPLIDA = p.FECHACUMPLIDA,
                HsCumplidaTime = p.HsCumplidaTime,
                UrlDni = p.UrlDni,
                UrlFirma = p.UrlFirma,
                ImageArrayDni = p.ImageArrayDni,
                ImageArrayFirma = p.ImageArrayDni,
                Activo = true,
                Habilita = p.Habilita,
                ESTADO3 = p.ESTADO3,
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

        private void LoadCodigosCierre()
        {
            CodigosCierre = new ObservableCollection<CodigoCierre>();
            
            //CodigosCierre.Add(new CodigoCierre { Codigo = 13, Descripcion = "Acepta Retiro", });
        }

        


        private async void Save()
        {

            HayFirma = true; //<-- Esto es para simular que hay firma



            if (Tasa.ESTADOGAOS == "PEN")
            {
                await App.Current.MainPage.DisplayAlert("Error", "El Estado sigue 'PEN'. No tiene sentido guardar.", "Aceptar");
                return;
            }

            if (Tasa.ESTADOGAOS == "INC" && CCierre == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Si la Orden tiene un Estado 'INC', hay que cargar el Código Cierre.", "Aceptar");
                return;
            }
            if (Tasa.ESTADOGAOS == "PAR" && CCierre == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Si la Orden tiene un Estado 'PAR', hay que cargar el Código Cierre.", "Aceptar");
                return;
            }

            if (Tasa.ESTADOGAOS == "EJB" && !HayFirma)
            {
                await App.Current.MainPage.DisplayAlert("Error", "No puede guardar como 'EJB' sin cargar la Foto de la Firma.", "Aceptar");
                return;
            }
            if (Tasa.ESTADOGAOS == "PAR" && !HayFirma)
            {
                await App.Current.MainPage.DisplayAlert("Error", "No puede guardar como 'PAR' sin cargar la Foto de la Firma.", "Aceptar");
                return;
            }


            if ((Tasa.ESTADOGAOS == "EJB") || (Tasa.ESTADOGAOS == "PAR"))
            {
                foreach (var ttt in ControlTasas)
                {
                    if (string.IsNullOrEmpty(ttt.ESTADO3) && ttt.Elegir == 1)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Hay modems sin N° de Serie. Complete todos los N° de Serie.", "Aceptar");
                        return;
                    }
                }
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


            //****************************************************************************************************************
            IsRunning = true;
            IsEnabled = false;

            byte[] imageArrayDni = null;
            if (File != null)
            {
                imageArrayDni = FilesHelper.ReadFully(this.File.GetStream());
                File.Dispose();
            }

            int? CR = null;
            if (Tasa.ESTADOGAOS == "EJB")
            {
                CR = 60;
                Tasa.CodigoCierre = 60;
            }
            else
            {
                CR = CCierre.Codigo;
                Tasa.CodigoCierre = CCierre.Codigo;
            }
            //*********************************************************************************************************
            //Grabar 
            //*********************************************************************************************************

            if (Tasa.ESTADOGAOS != "PAR")
            {
                //***** Graba EJB o INC *****

                var E2 = "";
                if (Tasa.ESTADOGAOS == "EJB")
                {
                    E2 = "SI";
                }
                if (Tasa.ESTADOGAOS == "INC")
                {
                    E2 = "NO";
                }

                var ya = DateTime.Now;

                foreach (var cc in ControlTasas)
                {
                    var fec1 = Tasa.FechaEvento1;
                    var fec2 = Tasa.FechaEvento2;
                    var fec3 = Tasa.FechaEvento3;

                    var evento1 = Tasa.Evento1;
                    var evento2 = Tasa.Evento2;
                    var evento3 = Tasa.Evento3;

                    var DescCR = "";
                    //if (CR == 10) { DescCR = "VERIFICACIÓN ADMINISTRATIVA"; };
                    if (CR == 21) { DescCR = "CLIENTE CONTINUA CON EL SERVICIO"; };
                    //if (CR == 22) { DescCR = "CLIENTE FALLECIO"; };
                    if (CR == 23) { DescCR = "CLIENTE NO ACEPTA RETIRO"; };
                    //if (CR == 24) { DescCR = "CLIENTE NO POSEE LOS EQUIPOS"; };
                    if (CR == 25) { DescCR = "CLIENTE YA ENTREGO LOS EQUIPOS"; };
                    if (CR == 26) { DescCR = "DIRECCIÓN Y NÚMERO ERRÓNEO"; };
                    if (CR == 27) { DescCR = "CLIENTE SE MUDO"; };
                    //if (CR == 28) { DescCR = "REFERENCIA INCORRECTA"; };
                    if (CR == 41) { DescCR = "CLIENTE AUSENTE"; };
                    if (CR == 43) { DescCR = "NO ATIENDE EL TELEFONO"; };
                    if (CR == 45) { DescCR = "VISITA COORDINADA"; };
                    if (CR == 60) { DescCR = "RECUPERADO"; };

                    
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
                        ESTADO2 = E2,
                        ESTADO3 = cc.ESTADO3,
                        ImageArrayDni = imageArrayDni,
                        ImageArrayFirma = _imageArrayFirma,
                        UrlDni = cc.UrlDni,
                        UrlFirma = cc.UrlFirma,
                        ZONA = cc.ZONA,
                        ESTADOGAOS = Tasa.ESTADOGAOS,
                        FECHACUMPLIDA = DateTime.Now,
                        SUBCON = Tasa.SUBCON,
                        CAUSANTEC = Tasa.CAUSANTEC,
                        FechaAsignada = Tasa.FechaAsignada,
                        PROYECTOMODULO = cc.PROYECTOMODULO,
                        DECO1 = cc.DECO1,
                        CMODEM1 = cc.CMODEM1,
                        Observacion = Tasa.Observacion,
                        HsCumplida = cc.HsCumplida,
                        UserID = Tasa.UserID,
                        CodigoCierre = CR,
                        CantRem = Tasa.CantRem,
                        Autonumerico = cc.Autonumerico,
                        HsCumplidaTime = DateTime.Now,
                        ObservacionCaptura = Tasa.ObservacionCaptura,
                        Novedades = Tasa.Novedades,
                        ReclamoTecnicoID = cc.ReclamoTecnicoID,
                        MODELO = cc.MODELO,
                        Motivos = cc.Motivos,
                        IDSuscripcion = cc.IDSuscripcion,
                        Evento4 = evento3,
                        FechaEvento4 = fec3,
                        Evento3 = evento2,
                        FechaEvento3 = fec2,
                        Evento2 = evento1,
                        FechaEvento2 = fec1,
                        Evento1 = DescCR,
                        FechaEvento1 = ya,
                        FechaCita=Tasa.FechaCita,
                        MedioCita= Tasa.MedioCita,


                        TelefAlternativo1 = cc.TelefAlternativo1,
                        TelefAlternativo2 = cc.TelefAlternativo2,
                        TelefAlternativo3 = cc.TelefAlternativo3,
                        TelefAlternativo4 = cc.TelefAlternativo4
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
                if (Tasa.CantEnt > 0)
                {
                    Tasa.CantRem = Tasa.CantRem - Tasa.CantEnt;
                }
                var newTasa = Tasa;
                var tasasViewModel = TasasPageViewModel.GetInstance();

                var oldTasa = tasasViewModel.MyTasas.Where(o => o.ReclamoTecnicoID == this.Tasa.ReclamoTecnicoID).FirstOrDefault();
                if (oldTasa != null && newTasa.ESTADOGAOS == "EJB")
                {
                    tasasViewModel.MyTasas.Remove(oldTasa);
                    tasasViewModel.RefreshList();
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }
                if (oldTasa != null && newTasa.ESTADOGAOS == "INC" &&
                    (
                    newTasa.CodigoCierre == 21 ||
                    newTasa.CodigoCierre == 22 ||
                    newTasa.CodigoCierre == 23 ||
                    newTasa.CodigoCierre == 24 ||
                    newTasa.CodigoCierre == 25 ||
                    newTasa.CodigoCierre == 26 ||
                    newTasa.CodigoCierre == 27 ||
                    newTasa.CodigoCierre == 28 ||
                    newTasa.CodigoCierre == 42 ||
                    newTasa.CodigoCierre == 44 ||
                    newTasa.CodigoCierre == 45 ||
                    newTasa.CodigoCierre == 60

                    )
                    )
                {
                    tasasViewModel.MyTasas.Remove(oldTasa);
                    tasasViewModel.RefreshList();
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }
                else
                {
                    tasasViewModel.MyTasas.Remove(oldTasa);
                    tasasViewModel.MyTasas.Add(newTasa);
                    tasasViewModel.LoadUser();
                    tasasViewModel.RefreshList();
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    tasasViewModel.RefreshList();
                    return;
                }

                //********************************************
            }
            else
            {
                var ya = DateTime.Now;
                var fec1 = Tasa.FechaEvento1;
                var fec2 = Tasa.FechaEvento2;
                var fec3 = Tasa.FechaEvento3;

                var evento1 = Tasa.Evento1;
                var evento2 = Tasa.Evento2;
                var evento3 = Tasa.Evento3;

                var DescCR = "";
                //if (CR == 10) { DescCR = "VERIFICACIÓN ADMINISTRATIVA"; };
                if (CR == 21) { DescCR = "CLIENTE CONTINUA CON EL SERVICIO"; };
                //if (CR == 22) { DescCR = "CLIENTE FALLECIO"; };
                if (CR == 23) { DescCR = "CLIENTE NO ACEPTA RETIRO"; };
                //if (CR == 24) { DescCR = "CLIENTE NO POSEE LOS EQUIPOS"; };
                if (CR == 25) { DescCR = "CLIENTE YA ENTREGO LOS EQUIPOS"; };
                if (CR == 26) { DescCR = "DIRECCIÓN Y NÚMERO ERRÓNEO"; };
                if (CR == 27) { DescCR = "CLIENTE SE MUDO"; };
                //if (CR == 28) { DescCR = "REFERENCIA INCORRECTA"; };
                if (CR == 41) { DescCR = "CLIENTE AUSENTE"; };
                if (CR == 43) { DescCR = "NO ATIENDE EL TELEFONO"; };
                if (CR == 45) { DescCR = "VISITA COORDINADA"; };
                if (CR == 60) { DescCR = "RECUPERADO"; };


                foreach (var cc in ControlTasas)
                {
                    if (cc.Elegir == 1)
                    {
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
                            ZONA = cc.ZONA,
                            ESTADOGAOS = "EJB",
                            ESTADO2 = "SI",
                            ESTADO3 = cc.ESTADO3,
                            ImageArrayDni = imageArrayDni,
                            ImageArrayFirma = _imageArrayFirma,
                            UrlDni = cc.UrlDni,
                            UrlFirma = cc.UrlFirma,
                            SUBCON = Tasa.SUBCON,
                            CAUSANTEC = Tasa.CAUSANTEC,
                            FechaAsignada = Tasa.FechaAsignada,
                            PROYECTOMODULO = cc.PROYECTOMODULO,
                            FECHACUMPLIDA = DateTime.Now,
                            DECO1 = cc.DECO1,
                            CMODEM1 = cc.CMODEM1,
                            Observacion = cc.Observacion,
                            HsCumplida = cc.HsCumplida,
                            UserID = Tasa.UserID,
                            CodigoCierre = 60,
                            ObservacionCaptura = Tasa.ObservacionCaptura,
                            Novedades = Tasa.Novedades,
                            CantRem = Tasa.CantRem,
                            Autonumerico = cc.Autonumerico,
                            HsCumplidaTime = DateTime.Now,
                            ReclamoTecnicoID = cc.ReclamoTecnicoID,
                            MODELO = cc.MODELO,
                            Motivos = cc.Motivos,
                            IDSuscripcion = cc.IDSuscripcion,
                            Evento4 = evento3,
                            FechaEvento4 = fec3,
                            Evento3 = evento2,
                            FechaEvento3 = fec2,
                            Evento2 = evento1,
                            FechaEvento2 = fec1,
                            Evento1 = "RECUPERADO",
                            FechaEvento1 = ya,
                            FechaCita = Tasa.FechaCita,
                            MedioCita = Tasa.MedioCita,
                            TelefAlternativo1 = cc.TelefAlternativo1,
                            TelefAlternativo2 = cc.TelefAlternativo2,
                            TelefAlternativo3 = cc.TelefAlternativo3,
                            TelefAlternativo4 = cc.TelefAlternativo4
                        };

                        var response = await _apiService.PutAsync(
                        url,
                        "api",
                        "/AsignacionesOTs",
                        mycc,
                        mycc.IDREGISTRO);

                        IsRunning = false;
                        IsEnabled = true;

                    }

                    else
                    {
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
                            ESTADO2 = "NO",
                            ESTADO3 = cc.ESTADO3,
                            ImageArrayDni = imageArrayDni,
                            ImageArrayFirma = _imageArrayFirma,
                            UrlDni = cc.UrlDni,
                            UrlFirma = cc.UrlFirma,
                            ZONA = cc.ZONA,
                            ESTADOGAOS = "INC",
                            FECHACUMPLIDA = DateTime.Now,
                            SUBCON = Tasa.SUBCON,
                            CAUSANTEC = Tasa.CAUSANTEC,
                            FechaAsignada = Tasa.FechaAsignada,
                            PROYECTOMODULO = cc.PROYECTOMODULO,
                            DECO1 = cc.DECO1,
                            CMODEM1 = cc.CMODEM1,
                            Observacion = cc.Observacion,
                            HsCumplida = cc.HsCumplida,
                            UserID = Tasa.UserID,
                            CodigoCierre = CCierre.Codigo,
                            CantRem = Tasa.CantRem,
                            Autonumerico = cc.Autonumerico,
                            HsCumplidaTime = DateTime.Now,
                            ObservacionCaptura = Tasa.ObservacionCaptura,
                            Novedades = Tasa.Novedades,
                            ReclamoTecnicoID = cc.ReclamoTecnicoID,
                            MODELO = cc.MODELO,
                            Motivos = cc.Motivos,
                            IDSuscripcion = cc.IDSuscripcion,
                            Evento4 = evento3,
                            FechaEvento4 = fec3,
                            Evento3 = evento2,
                            FechaEvento3 = fec2,
                            Evento2 = evento1,
                            FechaEvento2 = fec1,
                            Evento1 = DescCR,
                            FechaEvento1 = ya,
                            FechaCita = Tasa.FechaCita,
                            MedioCita = Tasa.MedioCita,
                            TelefAlternativo1 = cc.TelefAlternativo1,
                            TelefAlternativo2 = cc.TelefAlternativo2,
                            TelefAlternativo3 = cc.TelefAlternativo3,
                            TelefAlternativo4 = cc.TelefAlternativo4
                        };

                        var response = await _apiService.PutAsync(
                        url,
                        "api",
                        "/AsignacionesOTs",
                        mycc,
                        mycc.IDREGISTRO);

                        IsRunning = false;
                        IsEnabled = true;

                    }
                }
                //***** Borrar de la lista de Reclamos *****
                if (Tasa.CantEnt > 0)
                {
                    Tasa.CantRem = Tasa.CantRem - Tasa.CantEnt;
                }
                Tasa.ESTADOGAOS = "INC";
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

            }
        }
        private async void PhoneCall()
        {
            if(!String.IsNullOrEmpty(Tasa.TELEFONO))
            {
                await Clipboard.SetTextAsync(Tasa.TELEFONO);
                PhoneDialer.Open(Tasa.TELEFONO);
            }
        }

        private async void PhoneCall1()
        {
            if (!String.IsNullOrEmpty(Tasa.TelefAlternativo1))
            {
                await Clipboard.SetTextAsync(Tasa.TelefAlternativo1);
                PhoneDialer.Open(Tasa.TelefAlternativo1);
            }            
        }

        private async void PhoneCall2()
        {
            if (!String.IsNullOrEmpty(Tasa.TelefAlternativo2))
            {
                await Clipboard.SetTextAsync(Tasa.TelefAlternativo2);
                PhoneDialer.Open(Tasa.TelefAlternativo2);
            }
        }

        private async void PhoneCall3()
        {
            if (!String.IsNullOrEmpty(Tasa.TelefAlternativo3))
            {
                await Clipboard.SetTextAsync(Tasa.TelefAlternativo3);
                PhoneDialer.Open(Tasa.TelefAlternativo3);
            }
        }

        private async void PhoneCall4()
        {
            if (!String.IsNullOrEmpty(Tasa.TelefAlternativo4))
            {
                await Clipboard.SetTextAsync(Tasa.TelefAlternativo4);
                PhoneDialer.Open(Tasa.TelefAlternativo4);
            }
        }

        private async void Cancel()
        {
            Tasa.ESTADOGAOS = "PEN";
            await _navigationService.GoBackAsync();
        }

        private async void ElijeTodos()
        {

            var myListControls = this.ControlTasas.Select(p => new ModemItemViewModel(_navigationService)
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
                ESTADO2 = "SI",
                ESTADO3 = p.ESTADO3,
                Activo= true,
                Habilita=false,
                ImageArrayDni = p.ImageArrayDni,
                ImageArrayFirma = p.ImageArrayFirma,
                UrlDni = p.UrlDni,
                UrlFirma = p.UrlFirma,
                Elegir = 1,
                ZONA = p.ZONA,
                HsCumplida = p.HsCumplida,
                Observacion = p.Observacion,
                MODELO = p.MODELO,
                MarcaModeloId = p.MarcaModeloId,
                Motivos = p.Motivos
            }); ;
            this.ControlTasas = new ObservableCollection<ModemItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
            Habilitado = false;
        }

        private async void DeselijeTodos()
        {

            var myListControls = this.ControlTasas.Select(p => new ModemItemViewModel(_navigationService)
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
                ESTADO2 = "NO",
                ESTADO3 = p.ESTADO3,
                Activo = false,
                Habilita = false,
                ImageArrayDni = p.ImageArrayDni,
                ImageArrayFirma = p.ImageArrayFirma,
                UrlDni = p.UrlDni,
                UrlFirma = p.UrlFirma,
                Elegir = 0,
                ZONA = p.ZONA,
                HsCumplida = p.HsCumplida,
                Observacion = p.Observacion,
                MODELO = p.MODELO,
                MarcaModeloId = p.MarcaModeloId,
                Motivos = p.Motivos
            }); ;
            this.ControlTasas = new ObservableCollection<ModemItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
            Habilitado = false;
        }

        private async void DeselijeTodos2()
        {

            var myListControls = this.ControlTasas.Select(p => new ModemItemViewModel(_navigationService)
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
                ESTADO2 = "NO",
                ESTADO3 = p.ESTADO3,
                Activo = true,
                Habilita = true,
                ImageArrayDni = p.ImageArrayDni,
                ImageArrayFirma = p.ImageArrayFirma,
                UrlDni = p.UrlDni,
                UrlFirma = p.UrlFirma,
                Elegir = 0,
                ZONA = p.ZONA,
                HsCumplida = p.HsCumplida,
                Observacion = p.Observacion,
                MODELO = p.MODELO,
                MarcaModeloId = p.MarcaModeloId,
                Motivos = p.Motivos
            }); ;
            this.ControlTasas = new ObservableCollection<ModemItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
            Habilitado = false;
        }

       

        private async void TakeDNI()
        {
            await _navigationService.NavigateAsync("DNIPicture2Page");
        }
        private async void TakeFirma()
        {
            await _navigationService.NavigateAsync("Firma2Page");
        }
        private async void TasaMap()
        {
            if (Tasa.GRXX.Length <= 5 && Tasa.GRYY.Length <= 5)
            {
                await App.Current.MainPage.DisplayAlert("Lo siento...", "El cliente NO tiene una GeoPosicion Correcta.", "Aceptar");
                return;
            }
            await _navigationService.NavigateAsync("TasaMapPage");
        }
        private async void OtroRecupero()
        {
            await _navigationService.NavigateAsync("TasaOtroRecuperoPage");
        }

    }
}
