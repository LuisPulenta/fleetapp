using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Fleet_App.Common.Services;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Fleet_App.Prism.ViewModels.Teco
{
    public class TecoPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;

        private Reclamo _remote;

        private bool _isRunning;
        private bool _isEnabled;
        private bool _isEnabledParcial;
        private int _cantFotos;
        public byte[] ImageArray { get; set; }

        private MediaFile _file;
        private MediaFile _file2;
        private ImageSource _imageSource;
        private ImageSource _imageSource2;
        private Picture _dniPicture;
        private bool _hayFirma;
        private bool _isRefreshing;
        private CodigoCierre _cCierre;
        private ObservableCollection<CodigoCierre> _codigosCierre;
        private ObservableCollection<Control> _controls;
        private Stream _streamFirma;
        private byte[] _imageArrayFirma;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _takeDNICommand;
        private DelegateCommand _takeFirmaCommand;
        private DelegateCommand _remoteMapCommand;
        private DelegateCommand _phoneCallCommand;

        public DelegateCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new DelegateCommand(Cancel));
        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(Save));
        public DelegateCommand TakeDNICommand => _takeDNICommand ?? (_takeDNICommand = new DelegateCommand(TakeDNI));
        public DelegateCommand TakeFirmaCommand => _takeFirmaCommand ?? (_takeFirmaCommand = new DelegateCommand(TakeFirma));
        public DelegateCommand RemoteMapCommand => _remoteMapCommand ?? (_remoteMapCommand = new DelegateCommand(RemoteMap));
        public DelegateCommand PhoneCallCommand => _phoneCallCommand ?? (_phoneCallCommand = new DelegateCommand(PhoneCall));







        public Reclamo Remote { get; set; }
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }
        public byte[] ImageArrayFirma
        {
            get => _imageArrayFirma;
            set => SetProperty(ref _imageArrayFirma, value);
        }
        public CodigoCierre CCierre
        {
            get => _cCierre;
            set => SetProperty(ref _cCierre, value);
        }

        public List<CodigoCierre> MyCodigosCierre { get; set; }
        public ObservableCollection<CodigoCierre> CodigosCierre
        {
            get => _codigosCierre;
            set => SetProperty(ref _codigosCierre, value);
        }
        public ObservableCollection<Control> Controls
        {
            get => _controls;
            set => SetProperty(ref _controls, value);
        }
        public int CantFotos
        {
            get => _cantFotos;
            set => SetProperty(ref _cantFotos, value);
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
        public bool IsEnabledParcial
        {
            get => _isEnabledParcial;
            set => SetProperty(ref _isEnabledParcial, value);
        }
        public bool HayFirma
        {
            get => _hayFirma;
            set => SetProperty(ref _hayFirma, value);
        }

        public List<Control> MyControls { get; set; }


        public TecoPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Remote = JsonConvert.DeserializeObject<Reclamo>(Settings.Remote);
            Title = "Recuperos Teco";
            ImageSource = "nophoto.png";
            ImageSource2 = "nophoto.png";
            instance = this;
            IsEnabled = true;
            LoadControls();
            LoadCodigosCierre();
            HayFirma = false;
            IsRefreshing = false;
            if (Remote.CantRem == 1)
            {
                IsEnabledParcial = false;
            }
            else
            {
                IsEnabledParcial = true;
            }



        }

        #region Singleton

        private static TecoPageViewModel instance;
        public static TecoPageViewModel GetInstance()
        {
            return instance;
        }
        #endregion


        private async void LoadControls()
        {
            IsRefreshing = true;
            IsEnabled = false;
            IsRunning = true;
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

            //Buscar los autonumericos del remote seleccionado
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
                return;
            }
            
            MyControls = (List<Control>)response.Result;
            RefreshList();
            IsRefreshing = false;
            IsEnabled = true;
            IsRunning = false;
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
                    CODIGOEQUIVALENCIA = p.ControlesEquivalencia?.CODIGOEQUIVALENCIA,
                    DECO1 = p.ControlesEquivalencia?.DECO1,
                    DESCRIPCION = p.ControlesEquivalencia?.DESCRIPCION,
                    ID = p.ControlesEquivalencia?.ID ?? 0
                }


            });
            
            Controls = new ObservableCollection<Control>(myListControls.OrderBy(p => p.Autonumerico));
        }
        
        private void LoadCodigosCierre()
        {
            CodigosCierre = new ObservableCollection<CodigoCierre>();
            CodigosCierre.Add(new CodigoCierre { Codigo = 1, Descripcion = "Sin respuesta/ Llamado telefónico", });
            CodigosCierre.Add(new CodigoCierre { Codigo = 3, Descripcion = "Se coordinó visita/ Llamada telefónica", });
            CodigosCierre.Add(new CodigoCierre { Codigo = 6, Descripcion = "Se coordinó visita/ Envío correo", });
            CodigosCierre.Add(new CodigoCierre { Codigo = 9, Descripcion = "Se coordinó visita/ Envío SMS", });
            CodigosCierre.Add(new CodigoCierre { Codigo = 10, Descripcion = "Ausente/ Visita en domicilio", });
            CodigosCierre.Add(new CodigoCierre { Codigo = 11, Descripcion = "Menor en domicilio/ Visita en domicilio", });
            CodigosCierre.Add(new CodigoCierre { Codigo = 12, Descripcion = "Sin stock de unidad/ Visita en domicilio", });
            CodigosCierre.Add(new CodigoCierre { Codigo = 14, Descripcion = "Entrega rechazada/ Visita en domicilio", });
            CodigosCierre.Add(new CodigoCierre { Codigo = 15, Descripcion = "Se desestima pedido/ Visita en domicilio", });
            //CodigosCierre.Add(new CodigoCierre { Codigo = 16, Descripcion = "En gestión/ Visita en domicilio", });
        }

        private async void Cancel()
        {
            Remote.ESTADOGAOS = "PEN";
            await _navigationService.GoBackAsync();
        }
        private async void Save()
        {
            if (Remote.ESTADOGAOS == "PEN")
            {
                await App.Current.MainPage.DisplayAlert("Error", "El Estado sigue 'PEN'. No tiene sentido guardar.", "Aceptar");
                return;
            }

            if (Remote.ESTADOGAOS == "EJB" && _file == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "No puede guardar como 'EJB' sin cargar la Foto del DNI.", "Aceptar");
                return;
            }
            if (Remote.ESTADOGAOS == "PAR" && _file == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "No puede guardar como 'PAR' sin cargar la Foto del DNI.", "Aceptar");
                return;
            }

            if (Remote.ESTADOGAOS == "EJB" && !HayFirma)
            {
                await App.Current.MainPage.DisplayAlert("Error", "No puede guardar como 'EJB' sin cargar la Foto de la Firma.", "Aceptar");
                return;
            }
            if (Remote.ESTADOGAOS == "PAR" && !HayFirma)
            {
                await App.Current.MainPage.DisplayAlert("Error", "No puede guardar como 'PAR' sin cargar la Foto de la Firma.", "Aceptar");
                return;
            }

            if (Remote.ESTADOGAOS == "INC" && CCierre == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Si la Orden tiene un Estado 'INC', hay que cargar el Código Cierre.", "Aceptar");
                return;
            }
            if (Remote.ESTADOGAOS == "PAR" && CCierre == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Si la Orden tiene un Estado 'PAR', hay que cargar el Código Cierre.", "Aceptar");
                return;
            }

            if (Remote.ESTADOGAOS == "PAR" && (Remote.CantEnt == 0 || Remote.CantEnt == null))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Si la Orden tiene un Estado 'PAR', hay que cargar la Cantidad Entregada.", "Aceptar");
                return;
            }

            if (Remote.ESTADOGAOS == "PAR" && Remote.CantEnt == Remote.CantRem)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Si va a entregar la Cantidad Total use la opción 'Sí a todo'.", "Aceptar");
                return;
            }

            if (Remote.ESTADOGAOS == "PAR" && Remote.CantEnt > Remote.CantRem)
            {
                await App.Current.MainPage.DisplayAlert("Error", "No puede entregar mayor Cantidad que la solicitada.", "Aceptar");
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
            
            byte[] imageArrayDni = null;
            if (File != null)
            {
                imageArrayDni = FilesHelper.ReadFully(this.File.GetStream());
                File.Dispose();
            }
            int? CR = null;
            if (Remote.ESTADOGAOS == "EJB")
            {
                CR = 13;
                Remote.CodigoCierre = 13;
            }
            else
            {
                CR = CCierre.Codigo;
                Remote.CodigoCierre = CCierre.Codigo;
            }
            //*********************************************************************************************************
            //Grabar 
            //*********************************************************************************************************

            if (Remote.ESTADOGAOS != "PAR")
            {
                //***** Graba EJB o INC *****
                var ya = DateTime.Now;

                foreach (var cc in Controls)
                
                
                {
                    var fec1 = Remote.FechaEvento1;
                    var fec2 = Remote.FechaEvento2;
                    var fec3 = Remote.FechaEvento3;

                    var evento1 = Remote.Evento1;
                    var evento2 = Remote.Evento2;
                    var evento3 = Remote.Evento3;

                    var DescCR = "";

                    if (CR == 1) { DescCR = "Sin respuesta/ Llamado telefónico"; };
                    if (CR == 3) { DescCR = "Se coordinó visita/ Llamada telefónica"; };
                    if (CR == 6) { DescCR = "Se coordinó visita/ Envío correo"; };
                    if (CR == 9) { DescCR = "Se coordinó visita/ Envío SMS"; };
                    if (CR == 10) { DescCR = "Ausente/ Visita en domicilio"; };
                    if (CR == 11) { DescCR = "Menor en domicilio/ Visita en domicilio"; };
                    if (CR == 12) { DescCR = "Sin stock de unidad/ Visita en domicilio"; };
                    if (CR == 14) { DescCR = "Entrega rechazada/ Visita en domicilio"; };
                    if (CR == 15) { DescCR = "Se desestima pedido/ Visita en domicilio"; };
                    if (CR == 13) { DescCR = "Recuperado"; };





                    var mycc = new AsignacionesOT
                    {
                        IDREGISTRO = cc.IDREGISTRO,
                        RECUPIDJOBCARD = cc.RECUPIDJOBCARD,
                        ESTADOGAOS = Remote.ESTADOGAOS,
                        PROYECTOMODULO = cc.PROYECTOMODULO,
                        FECHACUMPLIDA = DateTime.Now,
                        HsCumplidaTime = DateTime.Now,
                        CodigoCierre = CR,
                        Autonumerico = cc.Autonumerico,
                        ImageArrayDni = imageArrayDni,
                        ImageArrayFirma = _imageArrayFirma,
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
                        Evento4 = evento3,
                        FechaEvento4 = fec3,
                        Evento3 = evento2,
                        FechaEvento3 = fec2,
                        Evento2 = evento1,
                        FechaEvento2 = fec1,
                        Evento1 = DescCR,
                        FechaEvento1 = ya,
                        FechaCita = Remote.FechaCita,
                        MedioCita = Remote.MedioCita
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
                //***** Borrar de la lista de Reclamos *****
                if (Remote.CantEnt>0)
                {
                    Remote.CantRem = Remote.CantRem - Remote.CantEnt;
                }
                var newRemote = Remote;
                var tecoPageViewModel = TecosPageViewModel.GetInstance();

                var oldRemote = tecoPageViewModel.MyRemotes.FirstOrDefault(o => o.RECUPIDJOBCARD == this.Remote.RECUPIDJOBCARD);
                if (oldRemote != null && newRemote.ESTADOGAOS == "EJB")
                {
                    tecoPageViewModel.MyRemotes.Remove(oldRemote);
                    tecoPageViewModel.RefreshList();
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }
                if (oldRemote != null && newRemote.ESTADOGAOS == "INC" && (newRemote.CodigoCierre == 14 || newRemote.CodigoCierre == 15))
                {
                    tecoPageViewModel.MyRemotes.Remove(oldRemote);
                    tecoPageViewModel.RefreshList();
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }
                else
                {
                    tecoPageViewModel.MyRemotes.Remove(oldRemote);
                    tecoPageViewModel.MyRemotes.Add(newRemote);
                    tecoPageViewModel.RefreshList();
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }

                //********************************************



            }
            else
            {
                var cont = 0;
                var ya = DateTime.Now;
                foreach (var cc in Controls)
                {
                    var fec1 = Remote.FechaEvento1;
                    var fec2 = Remote.FechaEvento2;
                    var fec3 = Remote.FechaEvento3;

                    var evento1 = Remote.Evento1;
                    var evento2 = Remote.Evento2;
                    var evento3 = Remote.Evento3;

                    var DescCR = "";

                    if (CR == 1) { DescCR = "Sin respuesta/ Llamado telefónico"; };
                    if (CR == 3) { DescCR = "Se coordinó visita/ Llamada telefónica"; };
                    if (CR == 6) { DescCR = "Se coordinó visita/ Envío correo"; };
                    if (CR == 9) { DescCR = "Se coordinó visita/ Envío SMS"; };
                    if (CR == 10) { DescCR = "Ausente/ Visita en domicilio"; };
                    if (CR == 11) { DescCR = "Menor en domicilio/ Visita en domicilio"; };
                    if (CR == 12) { DescCR = "Sin stock de unidad/ Visita en domicilio"; };
                    if (CR == 14) { DescCR = "Entrega rechazada/ Visita en domicilio"; };
                    if (CR == 15) { DescCR = "Se desestima pedido/ Visita en domicilio"; };
                    if (CR == 13) { DescCR = "Recuperado"; };


                    if (cont < Remote.CantEnt)
                    {
                       

                        var mycc = new AsignacionesOT
                        {
                            IDREGISTRO = cc.IDREGISTRO,
                            RECUPIDJOBCARD = cc.RECUPIDJOBCARD,
                            ESTADOGAOS = "EJB",
                            PROYECTOMODULO = cc.PROYECTOMODULO,
                            FECHACUMPLIDA = DateTime.Now,
                            HsCumplidaTime = DateTime.Now,
                            CodigoCierre = 13,
                            Autonumerico = cc.Autonumerico,
                            ImageArrayDni = imageArrayDni,
                            ImageArrayFirma = _imageArrayFirma,
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
                            Evento4 = evento3,
                            FechaEvento4 = fec3,
                            Evento3 = evento2,
                            FechaEvento3 = fec2,
                            Evento2 = evento1,
                            FechaEvento2 = fec1,
                            Evento1 = DescCR,
                            FechaEvento1 = ya,
                            FechaCita = Remote.FechaCita,
                            MedioCita = Remote.MedioCita
                        };

                        var response = await _apiService.PutAsync(
                        url,
                        "api",
                        "/AsignacionesOTs",
                        mycc,
                        mycc.IDREGISTRO);

                        IsRunning = false;
                        IsEnabled = true;
                        cont = cont + 1;
                    }

                    else
                    {
                        var mycc = new AsignacionesOT
                        {
                            IDREGISTRO = cc.IDREGISTRO,
                            RECUPIDJOBCARD = cc.RECUPIDJOBCARD,
                            ESTADOGAOS = "INC",
                            PROYECTOMODULO = cc.PROYECTOMODULO,
                            FECHACUMPLIDA = DateTime.Now,
                            HsCumplidaTime = DateTime.Now,
                            CodigoCierre = CCierre.Codigo,
                            Autonumerico = cc.Autonumerico,
                            ImageArrayDni = null,
                            ImageArrayFirma = null,
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
                            Evento4 = evento3,
                            FechaEvento4 = fec3,
                            Evento3 = evento2,
                            FechaEvento3 = fec2,
                            Evento2 = evento1,
                            FechaEvento2 = fec1,
                            Evento1 = DescCR,
                            FechaEvento1 = ya,
                            FechaCita = Remote.FechaCita,
                            MedioCita = Remote.MedioCita
                        };

                        var response = await _apiService.PutAsync(
                        url,
                        "api",
                        "/AsignacionesOTs",
                        mycc,
                        mycc.IDREGISTRO);

                        IsRunning = false;
                        IsEnabled = true;
                        cont = cont + 1;
                    }
                }
                //***** Borrar de la lista de Reclamos *****
                if (Remote.CantEnt > 0)
                {
                    Remote.CantRem = Remote.CantRem - Remote.CantEnt;
                }
                Remote.ESTADOGAOS = "INC";
                var newRemote = Remote;
                var remotesViewModel = TecosPageViewModel.GetInstance();
                var oldRemote = remotesViewModel.MyRemotes.FirstOrDefault(o => o.RECUPIDJOBCARD == this.Remote.RECUPIDJOBCARD);

                if (oldRemote != null && (newRemote.CodigoCierre == 14 || newRemote.CodigoCierre == 15))
                {
                    remotesViewModel.MyRemotes.Remove(oldRemote);
                    remotesViewModel.RefreshList();
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }
                else
                {

                    remotesViewModel.MyRemotes.Remove(oldRemote);
                    remotesViewModel.MyRemotes.Add(newRemote);
                    remotesViewModel.RefreshList();
                    await App.Current.MainPage.DisplayAlert("Ok", "Guardado con éxito!!", "Aceptar");
                    await _navigationService.GoBackAsync();
                    return;
                }
            }
        }
        private async void TakeDNI()
        {
            await _navigationService.NavigateAsync("TecoDniPicturePage");
        }
        private async void TakeFirma()
        {
            await _navigationService.NavigateAsync("TecoOtherPicturePage");
        }
        private async void RemoteMap()
        {
            if (Remote.GRXX.Length <= 5 && Remote.GRYY.Length <= 5)
            {
                await App.Current.MainPage.DisplayAlert("Lo siento...", "Esta Solicitud no tiene cargadas las Coordenadas para el mapa.", "Aceptar");
                return;
            }
            await _navigationService.NavigateAsync("TecoMapPage");
        }
      
        private async void PhoneCall()
        {
            await Clipboard.SetTextAsync(Remote.TELEFONO);
            PhoneDialer.Open(Remote.TELEFONO);
        }

        
    }
}
