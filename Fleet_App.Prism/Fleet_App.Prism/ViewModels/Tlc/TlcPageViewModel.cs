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
using Fleet_App.Common.Codigos;
using Xamarin.Essentials;

namespace Fleet_App.Prism.ViewModels
{
    public class TlcPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;


        private AsignacionesOT _tlc;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isEnabledParcial;
        private bool _isRefreshing;
        private bool _habilitado;
        private string _nroSeriesExtras;
        private CodigoCierre _cCierre;
        private ObservableCollection<ModemItemViewModel> _controlCables;
        private ObservableCollection<CodigoCierre> _codigosCierre;
        private DelegateCommand _TlcMapCommand;
        #region Properties
        public ReclamoCable Tlc { get; set; }
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
        public ObservableCollection<ModemItemViewModel> ControlCables
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
       

        public DelegateCommand CableMapCommand => _TlcMapCommand ?? (_TlcMapCommand = new DelegateCommand(TlcMap));
        public DelegateCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(Cancel));
        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(Save));
        public DelegateCommand PhoneCallCommand => _phoneCallCommand ?? (_phoneCallCommand = new DelegateCommand(PhoneCall));
        public DelegateCommand PhoneCallCommand1 => _phoneCallCommand1 ?? (_phoneCallCommand1 = new DelegateCommand(PhoneCall1));
        public DelegateCommand PhoneCallCommand2 => _phoneCallCommand2 ?? (_phoneCallCommand2 = new DelegateCommand(PhoneCall2));
        public DelegateCommand PhoneCallCommand3 => _phoneCallCommand3 ?? (_phoneCallCommand3 = new DelegateCommand(PhoneCall3));


        public DelegateCommand ElijeTodosCommand => _elijeTodosCommand ?? (_elijeTodosCommand = new DelegateCommand(ElijeTodos));
        public DelegateCommand DeselijeTodosCommand => _deselijeTodosCommand ?? (_deselijeTodosCommand = new DelegateCommand(DeselijeTodos));

        

        public TlcPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            
            Title = "Recupero de Tlc";
            instance = this;
            Tlc = JsonConvert.DeserializeObject<ReclamoCable>(Settings.Tlc);
                        
            LoadControlCables();
            LoadCodigosCierre();

            IsEnabled = true;
            IsRefreshing = false;
            
            if (Tlc.CantRec == 1)
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

        private static TlcPageViewModel instance;
        public static TlcPageViewModel GetInstance()
        {
            return instance;
        }
        #endregion


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
            var controller = string.Format("/AsignacionesTlc/GetAutonumericos", Tlc.ReclamoTecnicoID, Tlc.UserID);


            var response = await _apiService.GetList3Async<ControlCable>(
                 url,
                "api",
                controller,
                Tlc.ReclamoTecnicoID,
                Tlc.UserID);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
            }
            MyControlCables = (List<ControlCable>)response.Result;
            RefreshList();
            IsRefreshing = false;
        }

        public void RefreshList()
        {
            var myListControls = this.MyControlCables.Select(p => new ModemItemViewModel(_navigationService)
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
                TelefAlternativo1 = p.TelefAlternativo1,
                TelefAlternativo2 = p.TelefAlternativo2,
                TelefAlternativo3 = p.TelefAlternativo3,
                TelefAlternativo4 = p.TelefAlternativo4
            }); ;
            this.ControlCables = new ObservableCollection<ModemItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
        }



        private void LoadCodigosCierre()
        {
            CodigosCierre = new ObservableCollection<CodigoCierre>();

            var codigosMostrar = new int[] { 0,  22,  23,  24,  25,  27,  28,  41,  42,  43,  44,  45 };

            foreach (int cr in codigosMostrar)
            {
                CodigosCierre.Add(new CodigoCierre { Codigo = cr, Descripcion = CodigosTlc.GetCodigoDescription(cr), });
            }
        }





        private async void Save()
        {
            if (Tlc.ESTADOGAOS == "PEN")
            {
                await App.Current.MainPage.DisplayAlert("Error", "El Estado sigue 'PEN'. No tiene sentido guardar.", "Aceptar");
                return;
            }

            if (Tlc.ESTADOGAOS == "INC" && CCierre == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Si la Orden tiene un Estado 'INC', hay que cargar el Código Cierre.", "Aceptar");
                return;
            }
            if (Tlc.ESTADOGAOS == "PAR" && CCierre == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Si la Orden tiene un Estado 'PAR', hay que cargar el Código Cierre.", "Aceptar");
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

            //****************************************************************************************************************
            IsRunning = true;
            IsEnabled = false;

            int? CR = null;
            if (Tlc.ESTADOGAOS == "EJB")
            {
                CR = 70;
                Tlc.CodigoCierre = 70;
            }
            else
            {
                CR = CCierre.Codigo;
                Tlc.CodigoCierre = CCierre.Codigo;
            }
            //*********************************************************************************************************
            //Grabar 
            //*********************************************************************************************************

            if (Tlc.ESTADOGAOS != "PAR")
            {
                //***** Graba EJB o INC *****

                var E2 = "";
                if (Tlc.ESTADOGAOS == "EJB")
                {
                    E2 = "SI";
                }
                if (Tlc.ESTADOGAOS == "INC")
                {
                    E2 = "NO";
                }

                var ya = DateTime.Now;

                foreach (var cc in ControlCables)
                {
                    var fec1 = Tlc.FechaEvento1;
                    var fec2 = Tlc.FechaEvento2;
                    var fec3 = Tlc.FechaEvento3;

                    var evento1 = Tlc.Evento1;
                    var evento2 = Tlc.Evento2;
                    var evento3 = Tlc.Evento3;

                    var DescCR = CodigosTlc.GetCodigoDescription(CR);

                    var mycc = new AsignacionesOT
                    {
                        IDREGISTRO = cc.IDREGISTRO,
                        RECUPIDJOBCARD = cc.RECUPIDJOBCARD,
                        CLIENTE = Tlc.CLIENTE,
                        NOMBRE = Tlc.NOMBRE,
                        DOMICILIO = Tlc.DOMICILIO,
                        ENTRECALLE1 = Tlc.ENTRECALLE1,
                        ENTRECALLE2 = Tlc.ENTRECALLE2,
                        CP = Tlc.CP,
                        LOCALIDAD = Tlc.LOCALIDAD,
                        PROVINCIA = Tlc.PROVINCIA,
                        TELEFONO = Tlc.TELEFONO,
                        GRXX = Tlc.GRXX,
                        GRYY = Tlc.GRYY,
                        ESTADO = cc.ESTADO,
                        ESTADO2 = E2,
                        ESTADO3 = cc.ESTADO3,
                        ZONA = cc.ZONA,
                        ESTADOGAOS = Tlc.ESTADOGAOS,
                        FECHACUMPLIDA = ya,
                        SUBCON = Tlc.SUBCON,
                        CAUSANTEC = Tlc.CAUSANTEC,
                        FechaAsignada = Tlc.FechaAsignada,
                        PROYECTOMODULO = cc.PROYECTOMODULO,
                        DECO1 = cc.DECO1,
                        CMODEM1 = cc.CMODEM1,
                        Observacion = cc.Observacion,
                        HsCumplida = cc.HsCumplida,
                        UserID = Tlc.UserID,
                        CodigoCierre = CR,
                        CantRem = Tlc.CantRem,
                        Autonumerico = cc.Autonumerico,
                        HsCumplidaTime = ya,
                        ObservacionCaptura = Tlc.ObservacionCaptura,
                        Novedades = Tlc.Novedades,
                        ReclamoTecnicoID = cc.ReclamoTecnicoID,
                        MODELO = cc.MODELO,
                        Motivos = cc.Motivos,
                        IDSuscripcion = cc.IDSuscripcion,
                        FechaCita=Tlc.FechaCita,
                        MedioCita=Tlc.MedioCita,
                        NroSeriesExtras= NroSeriesExtras,
                        Evento4 = evento3,
                        FechaEvento4 = fec3,
                        Evento3 = evento2,
                        FechaEvento3 = fec2,
                        Evento2 = evento1,
                        FechaEvento2 = fec1,
                        Evento1 = DescCR,
                        FechaEvento1 = ya,
                        TelefAlternativo1 = Tlc.TelefAlternativo1,
                        TelefAlternativo2 = Tlc.TelefAlternativo2,
                        TelefAlternativo3 = Tlc.TelefAlternativo3,
                        TelefAlternativo4 = Tlc.TelefAlternativo4
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
                //***** Borrar de la lista de Cables *****
                if (Tlc.CantEnt > 0)
                {
                    Tlc.CantRem = Tlc.CantRem - Tlc.CantEnt;
                }
                var newCable = Tlc;
                var tlcsPageViewModel = TlcsPageViewModel.GetInstance();

                var oldCable = tlcsPageViewModel.MyTlcs.FirstOrDefault(o => o.ReclamoTecnicoID == this.Tlc.ReclamoTecnicoID);
                if (oldCable != null && newCable.ESTADOGAOS == "EJB")
                {
                    tlcsPageViewModel.MyTlcs.Remove(oldCable);
                    tlcsPageViewModel.RefreshList();
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }
                if (oldCable != null && newCable.ESTADOGAOS == "INC" &&
                    (
                        CodigosTlc.OcultarCodigo(newCable.CodigoCierre, newCable.ESTADOGAOS)
                    )
                    )
                {
                    tlcsPageViewModel.MyTlcs.Remove(oldCable);
                    
                    tlcsPageViewModel.LoadUser();
                    tlcsPageViewModel.RefreshList();
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }
                else
                {
                    tlcsPageViewModel.MyTlcs.Remove(oldCable);
                    tlcsPageViewModel.MyTlcs.Add(newCable);
                    tlcsPageViewModel.LoadUser();
                    tlcsPageViewModel.RefreshList();
                    
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }

                //********************************************
            }
            else
            {

                var ya = DateTime.Now;

                foreach (var cc in ControlCables)
                {
                    var fec1 = Tlc.FechaEvento1;
                    var fec2 = Tlc.FechaEvento2;
                    var fec3 = Tlc.FechaEvento3;

                    var evento1 = Tlc.Evento1;
                    var evento2 = Tlc.Evento2;
                    var evento3 = Tlc.Evento3;

                    var DescCR = CodigosTlc.GetCodigoDescription(CR);

                    if (cc.Elegir==1)
                    {

                        

                        var mycc = new AsignacionesOT
                        {
                            IDREGISTRO = cc.IDREGISTRO,
                            RECUPIDJOBCARD = cc.RECUPIDJOBCARD,
                            CLIENTE = Tlc.CLIENTE,
                            NOMBRE = Tlc.NOMBRE,
                            DOMICILIO = Tlc.DOMICILIO,
                            ENTRECALLE1 = Tlc.ENTRECALLE1,
                            ENTRECALLE2 = Tlc.ENTRECALLE2,
                            CP = Tlc.CP,
                            LOCALIDAD = Tlc.LOCALIDAD,
                            PROVINCIA = Tlc.PROVINCIA,
                            TELEFONO = Tlc.TELEFONO,
                            GRXX = Tlc.GRXX,
                            GRYY = Tlc.GRYY,
                            ESTADO = cc.ESTADO,
                            ZONA = cc.ZONA,
                            ESTADOGAOS = "EJB",
                            ESTADO2="SI",
                            ESTADO3=cc.ESTADO3,
                            SUBCON = Tlc.SUBCON,
                            CAUSANTEC = Tlc.CAUSANTEC,
                            FechaAsignada = Tlc.FechaAsignada,
                            PROYECTOMODULO = cc.PROYECTOMODULO,
                            FECHACUMPLIDA = ya,
                            DECO1 = cc.DECO1,
                            CMODEM1 = cc.CMODEM1,
                            Observacion = cc.Observacion,
                            HsCumplida = cc.HsCumplida,
                            UserID = Tlc.UserID,
                            CodigoCierre = 13,
                            ObservacionCaptura = Tlc.ObservacionCaptura,
                            Novedades = Tlc.Novedades,
                            CantRem = Tlc.CantRem,
                            Autonumerico = cc.Autonumerico,
                            HsCumplidaTime = ya,
                            ReclamoTecnicoID = cc.ReclamoTecnicoID,
                            MODELO = cc.MODELO,
                            Motivos = cc.Motivos,
                            IDSuscripcion = cc.IDSuscripcion,
                            FechaCita = Tlc.FechaCita,
                            MedioCita = Tlc.MedioCita,
                            NroSeriesExtras = NroSeriesExtras,
                            Evento4 = evento3,
                            FechaEvento4 = fec3,
                            Evento3 = evento2,
                            FechaEvento3 = fec2,
                            Evento2 = evento1,
                            FechaEvento2 = fec1,
                            Evento1 = DescCR,
                            FechaEvento1 = ya,
                            TelefAlternativo1 = Tlc.TelefAlternativo1,
                            TelefAlternativo2 = Tlc.TelefAlternativo2,
                            TelefAlternativo3 = Tlc.TelefAlternativo3,
                            TelefAlternativo4 = Tlc.TelefAlternativo4
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
                            CLIENTE = Tlc.CLIENTE,
                            NOMBRE = Tlc.NOMBRE,
                            DOMICILIO = Tlc.DOMICILIO,
                            ENTRECALLE1 = Tlc.ENTRECALLE1,
                            ENTRECALLE2 = Tlc.ENTRECALLE2,
                            CP = Tlc.CP,
                            LOCALIDAD = Tlc.LOCALIDAD,
                            PROVINCIA = Tlc.PROVINCIA,
                            TELEFONO = Tlc.TELEFONO,
                            GRXX = Tlc.GRXX,
                            GRYY = Tlc.GRYY,
                            ESTADO = cc.ESTADO,
                            ESTADO2 = "NO",
                            ESTADO3 = cc.ESTADO3,
                            ZONA = cc.ZONA,
                            ESTADOGAOS = "INC",
                            FECHACUMPLIDA = ya,
                            SUBCON = Tlc.SUBCON,
                            CAUSANTEC = Tlc.CAUSANTEC,
                            FechaAsignada = Tlc.FechaAsignada,
                            PROYECTOMODULO = cc.PROYECTOMODULO,
                            DECO1 = cc.DECO1,
                            CMODEM1 = cc.CMODEM1,
                            Observacion = cc.Observacion,
                            HsCumplida = cc.HsCumplida,
                            UserID = Tlc.UserID,
                            CodigoCierre = CCierre.Codigo,
                            CantRem = Tlc.CantRem,
                            Autonumerico = cc.Autonumerico,
                            HsCumplidaTime = ya,
                            ObservacionCaptura = Tlc.ObservacionCaptura,
                            Novedades = Tlc.Novedades,
                            ReclamoTecnicoID = cc.ReclamoTecnicoID,
                            MODELO = cc.MODELO,
                            Motivos = cc.Motivos,
                            IDSuscripcion = cc.IDSuscripcion,
                            FechaCita = Tlc.FechaCita,
                            MedioCita = Tlc.MedioCita,
                            NroSeriesExtras = NroSeriesExtras,
                            Evento4 = evento3,
                            FechaEvento4 = fec3,
                            Evento3 = evento2,
                            FechaEvento3 = fec2,
                            Evento2 = evento1,
                            FechaEvento2 = fec1,
                            Evento1 = DescCR,
                            FechaEvento1 = ya,
                            TelefAlternativo1 = Tlc.TelefAlternativo1,
                            TelefAlternativo2 = Tlc.TelefAlternativo2,
                            TelefAlternativo3 = Tlc.TelefAlternativo3,
                            TelefAlternativo4 = Tlc.TelefAlternativo4
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
                if (Tlc.CantEnt > 0)
                {
                    Tlc.CantRem = Tlc.CantRem - Tlc.CantEnt;
                }
                Tlc.ESTADOGAOS = "INC";
                var newCable = Tlc;
                var tlcsPageViewModel = TlcsPageViewModel.GetInstance();


                tlcsPageViewModel.LoadUser();



                var oldCable = tlcsPageViewModel.MyTlcs.FirstOrDefault(o => o.ReclamoTecnicoID == this.Tlc.ReclamoTecnicoID);

                //if (
                //    newCable.CodigoCierre == 1 ||
                //    newCable.CodigoCierre == 2 ||
                //    newCable.CodigoCierre == 8 ||
                //    newCable.CodigoCierre == 9 ||
                //    newCable.CodigoCierre == 14 ||
                //    newCable.CodigoCierre == 16 

                //    )
                //    {
                //    cablesViewModel.MyTlcs.Remove(oldCable);
                //    }
                
                //cablesViewModel.RefreshList();
                await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                tlcsPageViewModel.LoadUser();
                tlcsPageViewModel.RefreshList();
                await _navigationService.GoBackAsync();
                return;

            }
        }
        private async void PhoneCall()
        {
            await Clipboard.SetTextAsync(Tlc.TELEFONO);
            PhoneDialer.Open(Tlc.TELEFONO);
        }

        private async void PhoneCall1()
        {
            await Clipboard.SetTextAsync(Tlc.TelefAlternativo1);
            PhoneDialer.Open(Tlc.TelefAlternativo1);
        }

        private async void PhoneCall2()
        {
            await Clipboard.SetTextAsync(Tlc.TelefAlternativo2);
            PhoneDialer.Open(Tlc.TelefAlternativo2);
        }

        private async void PhoneCall3()
        {
            await Clipboard.SetTextAsync(Tlc.TelefAlternativo3);
            PhoneDialer.Open(Tlc.TelefAlternativo3);
        }

        private async void Cancel()
        {
            Tlc.ESTADOGAOS = "PEN";
            await _navigationService.GoBackAsync();
        }

        private async void ElijeTodos()
        {
            
            var myListControls = this.MyControlCables.Select(p => new ModemItemViewModel(_navigationService)
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
            this.ControlCables = new ObservableCollection<ModemItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
            Habilitado = false;
        }

        private async void DeselijeTodos()
        {
           
            var myListControls = this.MyControlCables.Select(p => new ModemItemViewModel(_navigationService)
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
            this.ControlCables = new ObservableCollection<ModemItemViewModel>(myListControls.OrderBy(p => p.Autonumerico));
            Habilitado = false;
        }

        private async void TlcMap()
        {
            if (Tlc.GRXX.Length <= 5 && Tlc.GRYY.Length <= 5)
            {
                await App.Current.MainPage.DisplayAlert("Lo siento...", "El cliente NO tiene una GeoPosicion Correcta.", "Aceptar");
                return;
            }
            await _navigationService.NavigateAsync("TlcMapPage");
        }
    }
}
