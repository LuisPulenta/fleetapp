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
    public class PrismaPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;


        private AsignacionesOT _Prisma;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isEnabledParcial;
        private bool _isRefreshing;
        private bool _habilitado;
        private CodigoCierre _cCierre;
        
        private ObservableCollection<ModemItemViewModel> _controlPrismas;
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
        private DelegateCommand _PrismaMapCommand;
        

        

        #region Properties
        public ReclamoPrisma Prisma { get; set; }
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

        
        public ObservableCollection<ModemItemViewModel> ControlPrismas
        {
            get => _controlPrismas;
            set => SetProperty(ref _controlPrismas, value);
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
        public List<ControlPrisma> MyControlPrismas { get; set; }
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
        
        public DelegateCommand PrismaMapCommand => _PrismaMapCommand ?? (_PrismaMapCommand = new DelegateCommand(PrismaMap));
        public DelegateCommand TakeDNICommand => _takeDNICommand ?? (_takeDNICommand = new DelegateCommand(TakeDNI));
        public DelegateCommand TakeFirmaCommand => _takeFirmaCommand ?? (_takeFirmaCommand = new DelegateCommand(TakeFirma));
        

        public PrismaPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;

            Title = "Recupero de Prisma";
            instance = this;
            Prisma = JsonConvert.DeserializeObject<ReclamoPrisma>(Settings.Prisma);

            LoadControlPrismas();
            LoadCodigosCierre();
            

            ImageSource = "nophoto.png";
            ImageSource2 = "firma.png";

            IsEnabled = true;
            IsRefreshing = false;

            if (Prisma.CantRec == 1)
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

        private static PrismaPageViewModel instance;
        public static PrismaPageViewModel GetInstance()
        {
            return instance;
        }
        #endregion


        private async void LoadControlPrismas()
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


            //Buscar los autonumericos del Prisma seleccionado
            var controller = string.Format("/ControlPrismas/GetAutonumericos", Prisma.ReclamoTecnicoID, Prisma.UserID);


            var response = await _apiService.GetList3Async<ControlPrisma>(
                 url,
                "api",
                controller,
                Prisma.ReclamoTecnicoID,
                Prisma.UserID);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
            }
            MyControlPrismas = (List<ControlPrisma>)response.Result;
            RefreshList();
            IsRefreshing = false;
        }

        public void RefreshList()
        {
            var myListControls = this.MyControlPrismas.Select(p => new ModemItemViewModel(_navigationService)
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
            this.ControlPrismas = new ObservableCollection<ModemItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
            
        }

        public void RefreshList2()
        {
            var myListControls = this.ControlPrismas.Select(p => new ModemItemViewModel(_navigationService)
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
            this.ControlPrismas = new ObservableCollection<ModemItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));

        }

        private void LoadCodigosCierre()
        {
            CodigosCierre = new ObservableCollection<CodigoCierre>();
            
            //CodigosCierre.Add(new CodigoCierre { Codigo = 13, Descripcion = "Acepta Retiro", });
        }

        


        private async void Save()
        {

            HayFirma = true; //<-- Esto es para simular que hay firma



            if (Prisma.ESTADOGAOS == "PEN")
            {
                await App.Current.MainPage.DisplayAlert("Error", "El Estado sigue 'PEN'. No tiene sentido guardar.", "Aceptar");
                return;
            }

            if (Prisma.ESTADOGAOS == "INC" && CCierre == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Si la Orden tiene un Estado 'INC', hay que cargar el Código Cierre.", "Aceptar");
                return;
            }
            if (Prisma.ESTADOGAOS == "PAR" && CCierre == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Si la Orden tiene un Estado 'PAR', hay que cargar el Código Cierre.", "Aceptar");
                return;
            }

            if (Prisma.ESTADOGAOS == "EJB" && !HayFirma)
            {
                await App.Current.MainPage.DisplayAlert("Error", "No puede guardar como 'EJB' sin cargar la Foto de la Firma.", "Aceptar");
                return;
            }
            if (Prisma.ESTADOGAOS == "PAR" && !HayFirma)
            {
                await App.Current.MainPage.DisplayAlert("Error", "No puede guardar como 'PAR' sin cargar la Foto de la Firma.", "Aceptar");
                return;
            }


            if ((Prisma.ESTADOGAOS == "EJB") || (Prisma.ESTADOGAOS == "PAR"))
            {
                foreach (var ttt in ControlPrismas)
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
            if (Prisma.ESTADOGAOS == "EJB")
            {
                CR = 10;
                Prisma.CodigoCierre = 10;
            }
            else
            {
                CR = CCierre.Codigo;
                Prisma.CodigoCierre = CCierre.Codigo;
            }
            //*********************************************************************************************************
            //Grabar 
            //*********************************************************************************************************

            if (Prisma.ESTADOGAOS != "PAR")
            {
                //***** Graba EJB o INC *****

                var E2 = "";
                if (Prisma.ESTADOGAOS == "EJB")
                {
                    E2 = "SI";
                }
                if (Prisma.ESTADOGAOS == "INC")
                {
                    E2 = "NO";
                }

                var ya = DateTime.Now;

                foreach (var cc in ControlPrismas)
                {
                    var fec1 = Prisma.FechaEvento1;
                    var fec2 = Prisma.FechaEvento2;
                    var fec3 = Prisma.FechaEvento3;

                    var evento1 = Prisma.Evento1;
                    var evento2 = Prisma.Evento2;
                    var evento3 = Prisma.Evento3;

                    var DescCR = "";
                    ////if (CR == 10) { DescCR = "VERIFICACIÓN ADMINISTRATIVA"; };
                    //if (CR == 21) { DescCR = "CLIENTE CONTINUA CON EL SERVICIO"; };
                    ////if (CR == 22) { DescCR = "CLIENTE FALLECIO"; };
                    //if (CR == 23) { DescCR = "CLIENTE NO ACEPTA RETIRO"; };
                    ////if (CR == 24) { DescCR = "CLIENTE NO POSEE LOS EQUIPOS"; };
                    //if (CR == 25) { DescCR = "CLIENTE YA ENTREGO LOS EQUIPOS"; };
                    //if (CR == 26) { DescCR = "DIRECCIÓN Y NÚMERO ERRÓNEO"; };
                    //if (CR == 27) { DescCR = "CLIENTE SE MUDO"; };
                    ////if (CR == 28) { DescCR = "REFERENCIA INCORRECTA"; };
                    //if (CR == 41) { DescCR = "CLIENTE AUSENTE"; };
                    //if (CR == 43) { DescCR = "NO ATIENDE EL TELEFONO"; };
                    //if (CR == 45) { DescCR = "VISITA COORDINADA"; };
                    //if (CR == 60) { DescCR = "RECUPERADO"; };
                    if (CR == 1) { DescCR = "A PEDIDO DE PRISMA"; };
                    if (CR == 2) { DescCR = "COMERCIO CERRADO"; };
                    if (CR == 3) { DescCR = "DATOS INCORRECTOS"; };
                    if (CR == 4) { DescCR = "DESEA MANTENER LA TERMINAL"; };
                    if (CR == 5) { DescCR = "VERIFICACIÓN ADMINISTRATIVA"; };
                    if (CR == 6) { DescCR = "VERIFICACIÓN ADMINISTRATIVA"; };
                    if (CR == 7) { DescCR = "VERIFICACIÓN ADMINISTRATIVA"; };
                    if (CR == 8) { DescCR = "VERIFICACIÓN ADMINISTRATIVA"; };
                    if (CR == 9) { DescCR = "VERIFICACIÓN ADMINISTRATIVA"; };
                    if (CR == 10) { DescCR = "VERIFICACIÓN ADMINISTRATIVA"; };


                    var mycc = new AsignacionesOT
                    {
                        IDREGISTRO = cc.IDREGISTRO,
                        RECUPIDJOBCARD = cc.RECUPIDJOBCARD,
                        CLIENTE = Prisma.CLIENTE,
                        NOMBRE = Prisma.NOMBRE,
                        DOMICILIO = Prisma.DOMICILIO,
                        ENTRECALLE1 = Prisma.ENTRECALLE1,
                        ENTRECALLE2 = Prisma.ENTRECALLE2,
                        CP = Prisma.CP,
                        LOCALIDAD = Prisma.LOCALIDAD,
                        PROVINCIA = Prisma.PROVINCIA,
                        TELEFONO = Prisma.TELEFONO,
                        GRXX = Prisma.GRXX,
                        GRYY = Prisma.GRYY,
                        ESTADO = cc.ESTADO,
                        ESTADO2 = E2,
                        ESTADO3 = cc.ESTADO3,
                        ImageArrayDni = imageArrayDni,
                        ImageArrayFirma = _imageArrayFirma,
                        UrlDni = cc.UrlDni,
                        UrlFirma = cc.UrlFirma,
                        ZONA = cc.ZONA,
                        ESTADOGAOS = Prisma.ESTADOGAOS,
                        FECHACUMPLIDA = DateTime.Now,
                        SUBCON = Prisma.SUBCON,
                        CAUSANTEC = Prisma.CAUSANTEC,
                        FechaAsignada = Prisma.FechaAsignada,
                        PROYECTOMODULO = cc.PROYECTOMODULO,
                        DECO1 = cc.DECO1,
                        CMODEM1 = cc.CMODEM1,
                        Observacion = Prisma.Observacion,
                        HsCumplida = cc.HsCumplida,
                        UserID = Prisma.UserID,
                        CodigoCierre = CR,
                        CantRem = Prisma.CantRem,
                        Autonumerico = cc.Autonumerico,
                        HsCumplidaTime = DateTime.Now,
                        ObservacionCaptura = Prisma.ObservacionCaptura,
                        Novedades = Prisma.Novedades,
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
                        FechaCita=Prisma.FechaCita,
                        MedioCita= Prisma.MedioCita,


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
                //***** Borrar de la lista de Prismas *****
                if (Prisma.CantEnt > 0)
                {
                    Prisma.CantRem = Prisma.CantRem - Prisma.CantEnt;
                }
                var newPrisma = Prisma;
                var PrismasViewModel = PrismasPageViewModel.GetInstance();

                var oldPrisma = PrismasViewModel.MyPrismas.Where(o => o.ReclamoTecnicoID == this.Prisma.ReclamoTecnicoID).FirstOrDefault();
                if (oldPrisma != null && newPrisma.ESTADOGAOS == "EJB")
                {
                    PrismasViewModel.MyPrismas.Remove(oldPrisma);
                    PrismasViewModel.RefreshList();
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }
                if (oldPrisma != null && newPrisma.ESTADOGAOS == "INC" &&
                    (
                    newPrisma.CodigoCierre == 21 ||
                    newPrisma.CodigoCierre == 22 ||
                    newPrisma.CodigoCierre == 23 ||
                    newPrisma.CodigoCierre == 24 ||
                    newPrisma.CodigoCierre == 25 ||
                    newPrisma.CodigoCierre == 26 ||
                    newPrisma.CodigoCierre == 27 ||
                    newPrisma.CodigoCierre == 28 ||
                    newPrisma.CodigoCierre == 42 ||
                    newPrisma.CodigoCierre == 44 ||
                    newPrisma.CodigoCierre == 45 ||
                    newPrisma.CodigoCierre == 60

                    )
                    )
                {
                    PrismasViewModel.MyPrismas.Remove(oldPrisma);
                    PrismasViewModel.RefreshList();
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }
                else
                {
                    PrismasViewModel.MyPrismas.Remove(oldPrisma);
                    PrismasViewModel.MyPrismas.Add(newPrisma);
                    PrismasViewModel.LoadUser();
                    PrismasViewModel.RefreshList();
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    PrismasViewModel.RefreshList();
                    return;
                }

                //********************************************
            }
            else
            {
                var ya = DateTime.Now;
                var fec1 = Prisma.FechaEvento1;
                var fec2 = Prisma.FechaEvento2;
                var fec3 = Prisma.FechaEvento3;

                var evento1 = Prisma.Evento1;
                var evento2 = Prisma.Evento2;
                var evento3 = Prisma.Evento3;

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


                foreach (var cc in ControlPrismas)
                {
                    if (cc.Elegir == 1)
                    {
                        var mycc = new AsignacionesOT
                        {
                            IDREGISTRO = cc.IDREGISTRO,
                            RECUPIDJOBCARD = cc.RECUPIDJOBCARD,
                            CLIENTE = Prisma.CLIENTE,
                            NOMBRE = Prisma.NOMBRE,
                            DOMICILIO = Prisma.DOMICILIO,
                            ENTRECALLE1 = Prisma.ENTRECALLE1,
                            ENTRECALLE2 = Prisma.ENTRECALLE2,
                            CP = Prisma.CP,
                            LOCALIDAD = Prisma.LOCALIDAD,
                            PROVINCIA = Prisma.PROVINCIA,
                            TELEFONO = Prisma.TELEFONO,
                            GRXX = Prisma.GRXX,
                            GRYY = Prisma.GRYY,
                            ESTADO = cc.ESTADO,
                            ZONA = cc.ZONA,
                            ESTADOGAOS = "EJB",
                            ESTADO2 = "SI",
                            ESTADO3 = cc.ESTADO3,
                            ImageArrayDni = imageArrayDni,
                            ImageArrayFirma = _imageArrayFirma,
                            UrlDni = cc.UrlDni,
                            UrlFirma = cc.UrlFirma,
                            SUBCON = Prisma.SUBCON,
                            CAUSANTEC = Prisma.CAUSANTEC,
                            FechaAsignada = Prisma.FechaAsignada,
                            PROYECTOMODULO = cc.PROYECTOMODULO,
                            FECHACUMPLIDA = DateTime.Now,
                            DECO1 = cc.DECO1,
                            CMODEM1 = cc.CMODEM1,
                            Observacion = cc.Observacion,
                            HsCumplida = cc.HsCumplida,
                            UserID = Prisma.UserID,
                            CodigoCierre = 10,
                            ObservacionCaptura = Prisma.ObservacionCaptura,
                            Novedades = Prisma.Novedades,
                            CantRem = Prisma.CantRem,
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
                            FechaCita = Prisma.FechaCita,
                            MedioCita = Prisma.MedioCita,
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
                            CLIENTE = Prisma.CLIENTE,
                            NOMBRE = Prisma.NOMBRE,
                            DOMICILIO = Prisma.DOMICILIO,
                            ENTRECALLE1 = Prisma.ENTRECALLE1,
                            ENTRECALLE2 = Prisma.ENTRECALLE2,
                            CP = Prisma.CP,
                            LOCALIDAD = Prisma.LOCALIDAD,
                            PROVINCIA = Prisma.PROVINCIA,
                            TELEFONO = Prisma.TELEFONO,
                            GRXX = Prisma.GRXX,
                            GRYY = Prisma.GRYY,
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
                            SUBCON = Prisma.SUBCON,
                            CAUSANTEC = Prisma.CAUSANTEC,
                            FechaAsignada = Prisma.FechaAsignada,
                            PROYECTOMODULO = cc.PROYECTOMODULO,
                            DECO1 = cc.DECO1,
                            CMODEM1 = cc.CMODEM1,
                            Observacion = cc.Observacion,
                            HsCumplida = cc.HsCumplida,
                            UserID = Prisma.UserID,
                            CodigoCierre = CCierre.Codigo,
                            CantRem = Prisma.CantRem,
                            Autonumerico = cc.Autonumerico,
                            HsCumplidaTime = DateTime.Now,
                            ObservacionCaptura = Prisma.ObservacionCaptura,
                            Novedades = Prisma.Novedades,
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
                            FechaCita = Prisma.FechaCita,
                            MedioCita = Prisma.MedioCita,
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
                if (Prisma.CantEnt > 0)
                {
                    Prisma.CantRem = Prisma.CantRem - Prisma.CantEnt;
                }
                Prisma.ESTADOGAOS = "INC";
                var newPrisma = Prisma;
                var PrismasViewModel = PrismasPageViewModel.GetInstance();
                var oldPrisma = PrismasViewModel.MyPrismas.Where(o => o.ReclamoTecnicoID == this.Prisma.ReclamoTecnicoID).FirstOrDefault();


                PrismasViewModel.MyPrismas.Remove(oldPrisma);
                PrismasViewModel.MyPrismas.Add(newPrisma);
                PrismasViewModel.LoadUser();
                PrismasViewModel.RefreshList();
                await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                await _navigationService.GoBackAsync();
                return;

            }
        }
        private async void PhoneCall()
        {
            await Clipboard.SetTextAsync(Prisma.TELEFONO);
            PhoneDialer.Open(Prisma.TELEFONO);
        }

        private async void PhoneCall1()
        {
            await Clipboard.SetTextAsync(Prisma.TelefAlternativo1);
            PhoneDialer.Open(Prisma.TelefAlternativo1);
        }

        private async void PhoneCall2()
        {
            await Clipboard.SetTextAsync(Prisma.TelefAlternativo2);
            PhoneDialer.Open(Prisma.TelefAlternativo2);
        }

        private async void PhoneCall3()
        {
            await Clipboard.SetTextAsync(Prisma.TelefAlternativo3);
            PhoneDialer.Open(Prisma.TelefAlternativo3);
        }

        private async void PhoneCall4()
        {
            await Clipboard.SetTextAsync(Prisma.TelefAlternativo4);
            PhoneDialer.Open(Prisma.TelefAlternativo4);
        }

        private async void Cancel()
        {
            Prisma.ESTADOGAOS = "PEN";
            await _navigationService.GoBackAsync();
        }

        private async void ElijeTodos()
        {

            var myListControls = this.ControlPrismas.Select(p => new ModemItemViewModel(_navigationService)
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
            this.ControlPrismas = new ObservableCollection<ModemItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
            Habilitado = false;
        }

        private async void DeselijeTodos()
        {

            var myListControls = this.ControlPrismas.Select(p => new ModemItemViewModel(_navigationService)
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
            this.ControlPrismas = new ObservableCollection<ModemItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
            Habilitado = false;
        }

        private async void DeselijeTodos2()
        {

            var myListControls = this.ControlPrismas.Select(p => new ModemItemViewModel(_navigationService)
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
            this.ControlPrismas = new ObservableCollection<ModemItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
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
        private async void PrismaMap()
        {
            if (Prisma.GRXX.Length <= 5 && Prisma.GRYY.Length <= 5)
            {
                await App.Current.MainPage.DisplayAlert("Lo siento...", "El cliente NO tiene una GeoPosicion Correcta.", "Aceptar");
                return;
            }
            await _navigationService.NavigateAsync("PrismaMapPage");
        }
        private async void OtroRecupero()
        {
            await _navigationService.NavigateAsync("PrismaOtroRecuperoPage");
        }

    }
}
