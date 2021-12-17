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
using Fleet_App.Prism.ViewModels.Cable;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace Fleet_App.Prism.ViewModels
{
    public class CablePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;


        private AsignacionesOT _cable;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isEnabledParcial;
        private bool _isRefreshing;
        private bool _habilitado;
        private string _nroSeriesExtras;
        private CodigoCierre _cCierre;
        private ObservableCollection<AutoCableItemViewModel> _controlCables;
        private ObservableCollection<CodigoCierre> _codigosCierre;
        private DelegateCommand _cableMapCommand;
        #region Properties
        public ReclamoCable Cable { get; set; }
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

        
        public string NroSeriesExtras
        {
            get => _nroSeriesExtras;
            set => SetProperty(ref _nroSeriesExtras, value);
        }


        public CodigoCierre CCierre
        {
            get => _cCierre;
            set => SetProperty(ref _cCierre, value);
        }
        public ObservableCollection<AutoCableItemViewModel> ControlCables
        {
            get => _controlCables;
            set => SetProperty(ref _controlCables, value);
        }
        public ObservableCollection<CodigoCierre> CodigosCierre
        {
            get => _codigosCierre;
            set => SetProperty(ref _codigosCierre, value);
        }
        public List<CodigoCierre> MyCodigosCierre { get; set; }
        public List<ControlCable> MyControlCables { get; set; }
        #endregion

        private DelegateCommand _cancelCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _elijeTodosCommand;
        private DelegateCommand _deselijeTodosCommand;
        private DelegateCommand _phoneCallCommand;
        private DelegateCommand _phoneCallCommand1;
        private DelegateCommand _phoneCallCommand2;
        private DelegateCommand _phoneCallCommand3;
        private DelegateCommand _phoneCallCommand4;
        private DelegateCommand _otroRecuperoCommand;

        public DelegateCommand CableMapCommand => _cableMapCommand ?? (_cableMapCommand = new DelegateCommand(CableMap));
        public DelegateCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(Cancel));
        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(Save));
        public DelegateCommand PhoneCallCommand => _phoneCallCommand ?? (_phoneCallCommand = new DelegateCommand(PhoneCall));
        public DelegateCommand PhoneCallCommand1 => _phoneCallCommand1 ?? (_phoneCallCommand1 = new DelegateCommand(PhoneCall1));
        public DelegateCommand PhoneCallCommand2 => _phoneCallCommand2 ?? (_phoneCallCommand2 = new DelegateCommand(PhoneCall2));
        public DelegateCommand PhoneCallCommand3 => _phoneCallCommand3 ?? (_phoneCallCommand3 = new DelegateCommand(PhoneCall3));
        public DelegateCommand PhoneCallCommand4 => _phoneCallCommand4 ?? (_phoneCallCommand4 = new DelegateCommand(PhoneCall4));
        public DelegateCommand ElijeTodosCommand => _elijeTodosCommand ?? (_elijeTodosCommand = new DelegateCommand(ElijeTodos));
        public DelegateCommand DeselijeTodosCommand => _deselijeTodosCommand ?? (_deselijeTodosCommand = new DelegateCommand(DeselijeTodos));
        public DelegateCommand OtroRecuperoCommand => _otroRecuperoCommand ?? (_otroRecuperoCommand = new DelegateCommand(OtroRecupero));


        public CablePageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            
            Title = "Recupero de Cablevisión";
            instance = this;
            Cable = JsonConvert.DeserializeObject<ReclamoCable>(Settings.Cable);
                        
            LoadControlCables();
            LoadCodigosCierre();

            IsEnabled = true;
            IsRefreshing = false;
            
            if (Cable.CantRec == 1)
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

        private static CablePageViewModel instance;
        public static CablePageViewModel GetInstance()
        {
            return instance;
        }
        #endregion
        

        /// <summary>
        /// 
        /// </summary>
        private async void LoadControlCables()
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

            //Buscar los autonumericos del cable seleccionado
            var controller = string.Format("/ControlCables/GetAutonumericos", Cable.ReclamoTecnicoID, Cable.UserID);

            var response = await _apiService.GetList3Async<ControlCable>(
                 url,
                "api",
                controller,
                Cable.ReclamoTecnicoID,
                Cable.UserID);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
            }
            
            MyControlCables = (List<ControlCable>)response.Result;

            // Filtro autonuméricos que están terminados.
            MyControlCables = MyControlCables.Where(x => !CodigosCable.OcultarCodigo(x.CodigoCierre, x.ESTADOGAOS)).ToList();

            // Filtrar por OT
            MyControlCables = MyControlCables.Where(x => x.RECUPIDJOBCARD == Cable.RECUPIDJOBCARD).ToList();

            RefreshList();
            IsRefreshing = false;
        }


        /// <summary>
        /// 
        /// </summary>
        public void RefreshList()
        {
            var myListControls = this.MyControlCables.Select(p => new AutoCableItemViewModel(_navigationService)
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
                Elegir=p.Elegir
            }); ;
            this.ControlCables = new ObservableCollection<AutoCableItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
        }
        /// <summary>
        /// 
        /// </summary>
        public void RefreshListFromControlCables()
        {
            var myListControls = this.ControlCables.Select(p => new AutoCableItemViewModel(_navigationService)
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
                Elegir=p.Elegir,
                ESTADO3 = p.ESTADO3,
                // Activo = true,
            }); ;
            this.ControlCables = new ObservableCollection<AutoCableItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
        }



        private void LoadCodigosCierre()
        {
            CodigosCierre = new ObservableCollection<CodigoCierre>();

            var codigosMostrar = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 30 };

            foreach (int cr in codigosMostrar)
            {
                CodigosCierre.Add(new CodigoCierre { Codigo = cr, Descripcion = CodigosCable.GetCodigoDescription(cr), });
            }

        }




        /// <summary>
        /// 
        /// </summary>
        private async void Save()
        {
            ///////////////////////
            // VALIDACIONES
            ///////////////////////
            if (Cable.ESTADOGAOS == "PEN")
            {
                await App.Current.MainPage.DisplayAlert("Error", "El Estado sigue 'PEN'. No tiene sentido guardar.", "Aceptar");
                return;
            }

            if (Cable.ESTADOGAOS == "INC" && CCierre == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Si la Orden tiene un Estado 'INC', hay que cargar el Código Cierre.", "Aceptar");
                return;
            }
            if (Cable.ESTADOGAOS == "PAR" && CCierre == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Si la Orden tiene un Estado 'PAR', hay que cargar el Código Cierre.", "Aceptar");
                return;
            }

            ////////////////////////////
            // VERIFICAR CONECTIVIDAD
            ////////////////////////////
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

            int? CR = null;
            if (Cable.ESTADOGAOS == "EJB")
            {
                CR = 30;
                Cable.CodigoCierre = 30;
            }
            else
            {
                CR = CCierre.Codigo;
                Cable.CodigoCierre = CCierre.Codigo;
            }

            //*********************************************************************************************************
            //Grabar 
            //*********************************************************************************************************

            if (Cable.ESTADOGAOS != "PAR")
            {
                //***** Graba EJB o INC *****

                var E2 = "";
                if (Cable.ESTADOGAOS == "EJB")
                {
                    E2 = "SI";
                }
                if (Cable.ESTADOGAOS == "INC")
                {
                    E2 = "NO";
                }

                var ya = DateTime.Now;

                foreach (var cc in ControlCables)
                {
                    var fec1 = Cable.FechaEvento1;
                    var fec2 = Cable.FechaEvento2;
                    var fec3 = Cable.FechaEvento3;

                    var evento1 = Cable.Evento1;
                    var evento2 = Cable.Evento2;
                    var evento3 = Cable.Evento3;

                    var DescCR = CodigosCable.GetCodigoDescription(CR);
                    
                    var mycc = new AsignacionesOT
                    {
                        IDREGISTRO = cc.IDREGISTRO,
                        RECUPIDJOBCARD = cc.RECUPIDJOBCARD,
                        CLIENTE = Cable.CLIENTE,
                        NOMBRE = Cable.NOMBRE,
                        DOMICILIO = Cable.DOMICILIO,
                        ENTRECALLE1 = Cable.ENTRECALLE1,
                        ENTRECALLE2 = Cable.ENTRECALLE2,
                        CP = Cable.CP,
                        LOCALIDAD = Cable.LOCALIDAD,
                        PROVINCIA = Cable.PROVINCIA,
                        TELEFONO = Cable.TELEFONO,
                        GRXX = Cable.GRXX,
                        GRYY = Cable.GRYY,
                        ESTADO = cc.ESTADO,
                        ESTADO2 = E2,
                        ESTADO3 = cc.ESTADO3,
                        ZONA = cc.ZONA,
                        ESTADOGAOS = Cable.ESTADOGAOS,
                        FECHACUMPLIDA = ya,
                        SUBCON = Cable.SUBCON,
                        CAUSANTEC = Cable.CAUSANTEC,
                        FechaAsignada = Cable.FechaAsignada,
                        PROYECTOMODULO = cc.PROYECTOMODULO,
                        DECO1 = cc.DECO1,
                        CMODEM1 = cc.CMODEM1,
                        Observacion = cc.Observacion,
                        HsCumplida = cc.HsCumplida,
                        UserID = Cable.UserID,
                        CodigoCierre = CR,
                        CantRem = Cable.CantRem,
                        Autonumerico = cc.Autonumerico,
                        HsCumplidaTime = ya,
                        ObservacionCaptura = Cable.ObservacionCaptura,
                        Novedades = Cable.Novedades,
                        ReclamoTecnicoID = cc.ReclamoTecnicoID,
                        MODELO = cc.MODELO,
                        Motivos = cc.Motivos,
                        IDSuscripcion = cc.IDSuscripcion,
                        FechaCita=Cable.FechaCita,
                        MedioCita=Cable.MedioCita,
                        NroSeriesExtras= NroSeriesExtras,
                        Evento4 = evento3,
                        FechaEvento4 = fec3,
                        Evento3 = evento2,
                        FechaEvento3 = fec2,
                        Evento2 = evento1,
                        FechaEvento2 = fec1,
                        Evento1 = DescCR,
                        FechaEvento1 = ya,
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
                        await App.Current.MainPage.DisplayAlert("Error", "Ocurrió un error de respuesta de servidor al guardar la Órden, intente más tarde.", "Aceptar");
                        return;
                    }
                }
                //***** Borrar de la lista de Cables *****
                if (Cable.CantEnt > 0)
                {
                    Cable.CantRem = Cable.CantRem - Cable.CantEnt;
                }

                var newCable = Cable;

                var cablesViewModel = CablesPageViewModel.GetInstance();

                var oldCable = cablesViewModel.MyCables.FirstOrDefault(o => o.ReclamoTecnicoID == this.Cable.ReclamoTecnicoID);

                if (oldCable != null && newCable.ESTADOGAOS == "EJB")
                {
                    cablesViewModel.MyCables.Remove(oldCable);
                    cablesViewModel.RefreshList();
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }
                if (oldCable != null && newCable.ESTADOGAOS == "INC" && CodigosCable.CodigosOcultar.Contains(newCable.CodigoCierre))
                {
                    cablesViewModel.MyCables.Remove(oldCable);
                    
                    cablesViewModel.LoadUser();
                    cablesViewModel.RefreshList();
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }
                else
                {
                    cablesViewModel.MyCables.Remove(oldCable);
                    cablesViewModel.MyCables.Add(newCable);
                    cablesViewModel.LoadUser();
                    cablesViewModel.RefreshList();
                    
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }

                //********************************************
            }
            else
            //********************************************
            // PARCIALES
            //********************************************
            {

                var ya = DateTime.Now;

                foreach (var cc in ControlCables)
                {
                    var fec1 = Cable.FechaEvento1;
                    var fec2 = Cable.FechaEvento2;
                    var fec3 = Cable.FechaEvento3;

                    var evento1 = Cable.Evento1;
                    var evento2 = Cable.Evento2;
                    var evento3 = Cable.Evento3;


                    var descCr = CR == null ? String.Empty : CodigosCable.GetCodigoDescription(CR.Value);


                    if (cc.Elegir==1)
                    {
                        //********************************************
                        // ESTO PASA SI ESTA SELECCIONADO
                        //********************************************
                        var mycc = new AsignacionesOT
                        {
                            IDREGISTRO = cc.IDREGISTRO,
                            RECUPIDJOBCARD = cc.RECUPIDJOBCARD,
                            CLIENTE = Cable.CLIENTE,
                            NOMBRE = Cable.NOMBRE,
                            DOMICILIO = Cable.DOMICILIO,
                            ENTRECALLE1 = Cable.ENTRECALLE1,
                            ENTRECALLE2 = Cable.ENTRECALLE2,
                            CP = Cable.CP,
                            LOCALIDAD = Cable.LOCALIDAD,
                            PROVINCIA = Cable.PROVINCIA,
                            TELEFONO = Cable.TELEFONO,
                            GRXX = Cable.GRXX,
                            GRYY = Cable.GRYY,
                            ESTADO = cc.ESTADO,
                            ZONA = cc.ZONA,
                            ESTADOGAOS = "EJB",
                            ESTADO2="SI",
                            ESTADO3=cc.ESTADO3,
                            SUBCON = Cable.SUBCON,
                            CAUSANTEC = Cable.CAUSANTEC,
                            FechaAsignada = Cable.FechaAsignada,
                            PROYECTOMODULO = cc.PROYECTOMODULO,
                            FECHACUMPLIDA = ya,
                            DECO1 = cc.DECO1,
                            CMODEM1 = cc.CMODEM1,
                            Observacion = cc.Observacion,
                            HsCumplida = cc.HsCumplida,
                            UserID = Cable.UserID,
                            CodigoCierre = 30,
                            ObservacionCaptura = Cable.ObservacionCaptura,
                            Novedades = Cable.Novedades,
                            CantRem = Cable.CantRem,
                            Autonumerico = cc.Autonumerico,
                            HsCumplidaTime = ya,
                            ReclamoTecnicoID = cc.ReclamoTecnicoID,
                            MODELO = cc.MODELO,
                            Motivos = cc.Motivos,
                            IDSuscripcion = cc.IDSuscripcion,
                            FechaCita = Cable.FechaCita,
                            MedioCita = Cable.MedioCita,
                            NroSeriesExtras = NroSeriesExtras,
                            Evento4 = evento3,
                            FechaEvento4 = fec3,
                            Evento3 = evento2,
                            FechaEvento3 = fec2,
                            Evento2 = evento1,
                            FechaEvento2 = fec1,
                            // Evento1 = descCr,
                            Evento1 = String.Empty,
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
                        
                    }

                    else
                    {
                        //********************************************
                        // ESTO PASA SI NO ESTA SELECCIONADO
                        //********************************************
                        var mycc = new AsignacionesOT
                        {
                            IDREGISTRO = cc.IDREGISTRO,
                            RECUPIDJOBCARD = cc.RECUPIDJOBCARD,
                            CLIENTE = Cable.CLIENTE,
                            NOMBRE = Cable.NOMBRE,
                            DOMICILIO = Cable.DOMICILIO,
                            ENTRECALLE1 = Cable.ENTRECALLE1,
                            ENTRECALLE2 = Cable.ENTRECALLE2,
                            CP = Cable.CP,
                            LOCALIDAD = Cable.LOCALIDAD,
                            PROVINCIA = Cable.PROVINCIA,
                            TELEFONO = Cable.TELEFONO,
                            GRXX = Cable.GRXX,
                            GRYY = Cable.GRYY,
                            ESTADO = cc.ESTADO,
                            ESTADO2 = "NO",
                            ESTADO3 = cc.ESTADO3,
                            ZONA = cc.ZONA,
                            // ESTADO GAOS
                            ESTADOGAOS = "INC",
                            FECHACUMPLIDA = ya,
                            SUBCON = Cable.SUBCON,
                            CAUSANTEC = Cable.CAUSANTEC,
                            FechaAsignada = Cable.FechaAsignada,
                            PROYECTOMODULO = cc.PROYECTOMODULO,
                            DECO1 = cc.DECO1,
                            CMODEM1 = cc.CMODEM1,
                            Observacion = cc.Observacion,
                            HsCumplida = cc.HsCumplida,
                            UserID = Cable.UserID,
                            // CODIGO CIERRE
                            // CodigoCierre = CCierre.Codigo,
                            CodigoCierre = CR,
                            CantRem = Cable.CantRem,
                            Autonumerico = cc.Autonumerico,
                            HsCumplidaTime = ya,
                            ObservacionCaptura = Cable.ObservacionCaptura,
                            Novedades = Cable.Novedades,
                            ReclamoTecnicoID = cc.ReclamoTecnicoID,
                            MODELO = cc.MODELO,
                            Motivos = cc.Motivos,
                            IDSuscripcion = cc.IDSuscripcion,
                            FechaCita = Cable.FechaCita,
                            MedioCita = Cable.MedioCita,
                            NroSeriesExtras = NroSeriesExtras,
                            Evento4 = evento3,
                            FechaEvento4 = fec3,
                            Evento3 = evento2,
                            FechaEvento3 = fec2,
                            Evento2 = evento1,
                            FechaEvento2 = fec1,
                            // EVENTO 1
                            Evento1 = descCr,
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
                        
                    }
                }

                //***** Borrar de la lista de Reclamos *****
                if (Cable.CantEnt > 0)
                {
                    Cable.CantRem = Cable.CantRem - Cable.CantEnt;
                }
                Cable.ESTADOGAOS = "INC";
                var newCable = Cable;
                var cablesViewModel = CablesPageViewModel.GetInstance();
                
                cablesViewModel.LoadUser();

                var oldCable = cablesViewModel.MyCables.FirstOrDefault(o => o.ReclamoTecnicoID == this.Cable.ReclamoTecnicoID);

                        if (CodigosCable.OcultarCodigo(newCable.CodigoCierre, newCable.ESTADOGAOS))
                            {
                            cablesViewModel.MyCables.Remove(oldCable);
                            }
                
                await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                cablesViewModel.LoadUser();
                cablesViewModel.RefreshList();
                await _navigationService.GoBackAsync();
                return;

            }
        }
        private async void PhoneCall()
        {
            if(!String.IsNullOrEmpty(Cable.TELEFONO))
            {
                await Clipboard.SetTextAsync(Cable.TELEFONO);
                PhoneDialer.Open(Cable.TELEFONO);
            }            
        }

        private async void PhoneCall1()
        {
            if (!String.IsNullOrEmpty(Cable.TelefAlternativo1))
            {
                await Clipboard.SetTextAsync(Cable.TelefAlternativo1);
                PhoneDialer.Open(Cable.TelefAlternativo1);
            }
        }

        private async void PhoneCall2()
        {
            if (!String.IsNullOrEmpty(Cable.TelefAlternativo2))
            {
                await Clipboard.SetTextAsync(Cable.TelefAlternativo2);
                PhoneDialer.Open(Cable.TelefAlternativo2);
            }
        }

        private async void PhoneCall3()
        {
            if (!String.IsNullOrEmpty(Cable.TelefAlternativo3))
            {
                await Clipboard.SetTextAsync(Cable.TelefAlternativo3);
                PhoneDialer.Open(Cable.TelefAlternativo3);
            }
        }

        private async void PhoneCall4()
        {
            if (!String.IsNullOrEmpty(Cable.TelefAlternativo4))
            {
                await Clipboard.SetTextAsync(Cable.TelefAlternativo4);
                PhoneDialer.Open(Cable.TelefAlternativo4);
            }
        }

        private async void Cancel()
        {
            Cable.ESTADOGAOS = "PEN";
            await _navigationService.GoBackAsync();
        }

        private async void ElijeTodos()
        {
            
            var myListControls = this.MyControlCables.Select(p => new AutoCableItemViewModel(_navigationService)
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
                ESTADO2="SI",
                ESTADO3= p.DECO1,
                Elegir=1,
                ZONA = p.ZONA,
                HsCumplida = p.HsCumplida,
                Observacion = p.Observacion,
                MODELO = p.MODELO,
                MarcaModeloId = p.MarcaModeloId,
                Motivos = p.Motivos
                
            }); ;
            this.ControlCables = new ObservableCollection<AutoCableItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
            Habilitado = false;
        }

        private async void DeselijeTodos()
        {
           
            var myListControls = this.MyControlCables.Select(p => new AutoCableItemViewModel(_navigationService)
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
                ESTADO3 = p.DECO1,
                Elegir = 0,
                ZONA = p.ZONA,
                HsCumplida = p.HsCumplida,
                Observacion = p.Observacion,
                MODELO = p.MODELO,
                MarcaModeloId = p.MarcaModeloId,
                Motivos = p.Motivos
            }); ;
            this.ControlCables = new ObservableCollection<AutoCableItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
            Habilitado = false;
        }

        private async void CableMap()
        {
            if (Cable.GRXX.Length <= 5 && Cable.GRYY.Length <= 5)
            {
                await App.Current.MainPage.DisplayAlert("Lo siento...", "El cliente NO tiene una GeoPosicion Correcta.", "Aceptar");
                return;
            }
            await _navigationService.NavigateAsync("CableMapPage");
        }

        private async void OtroRecupero()
        {
            await _navigationService.NavigateAsync("CableOtroRecuperoPage");
        }


   
    }
}
