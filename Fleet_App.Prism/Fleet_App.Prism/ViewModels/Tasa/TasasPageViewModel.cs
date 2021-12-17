using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Fleet_App.Common.Services;
using Fleet_App.Prism.ViewModels;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Fleet_App.Prism.ViewModels
{
    public class TasasPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private UserResponse _user;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isChecked;
        private bool _isRefreshing;
        private string _descCR;
        private string _entreCalles;
        private ObservableCollection<TasaItemViewModel> _tasas;
        private static TasasPageViewModel _instance;
        private int _cantTasas;
        private string _filter;
        private DelegateCommand _searchCommand;
        private DelegateCommand _refreshCommand;
        private DelegateCommand _tasasMapCommand;
        private DelegateCommand _ponerHoyCommand;

        public List<ModuleResponse> MyModules { get; set; }
        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(Search));
        public DelegateCommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new DelegateCommand(Refresh));
        public DelegateCommand TasasMapCommand => _tasasMapCommand ?? (_tasasMapCommand = new DelegateCommand(TasasMap));
        public DelegateCommand PonerHoyCommand => _ponerHoyCommand ?? (_ponerHoyCommand = new DelegateCommand(PonerHoy));


        public string Filter
        {
            get => _filter;
            set => SetProperty(ref _filter, value);
        }
        public string DescCR
        {
            get => _descCR;
            set => SetProperty(ref _descCR, value);
        }
        public int CantTasas
        {
            get => _cantTasas;
            set => SetProperty(ref _cantTasas, value);
        }
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsChecked
        {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }

        public string EntreCalles
        {
            get => _entreCalles;
            set => SetProperty(ref _entreCalles, value);
        }
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
        public ObservableCollection<TasaItemViewModel> Tasas
        {
            get => _tasas;
            set => SetProperty(ref _tasas, value);
        }

        public List<ReclamoTasa> MyTasas { get; set; }

        public static TasasPageViewModel GetInstance()
        {
            return _instance;
        }


        public TasasPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
            LoadUser();
            Tasas = new ObservableCollection<TasaItemViewModel>();
            Title = "Recuperos de Tasa";
            _instance = this;
        }


        public async void LoadUser()
        {


            //********** CONTROLA NUMERO DE VERSION **********
            var url = App.Current.Resources["UrlAPI"].ToString();
            var response2 = await _apiService.GetList2Async<ModuleResponse>(
                url,
                "api",
                "/Modules");

            this.MyModules = (List<ModuleResponse>)response2.Result;



            if (response2.IsSuccess)
            {
                var bandera = 0;
                var version = App.Current.Resources["AppVersion"].ToString();
                foreach (var cc in MyModules)
                {
                    if (cc.NroVersion != version && cc.ActualizOblig == 0 && cc.IdModulo == 1)
                    {
                        bandera = 1;
                    }

                    if (cc.NroVersion != version && cc.ActualizOblig == 1 && cc.IdModulo == 1)
                    {
                        bandera = 2;
                    }
                }

                if (bandera == 1)
                {
                    IsRunning = false;
                    IsEnabled = true;
                    Settings.IsRemembered = false;
                    await _navigationService.NavigateAsync("/FleetMasterDetailPage/NavigationPage/AvisoPage");
                    return;
                }

                if (bandera == 2)
                {
                    IsRunning = true;
                    IsEnabled = true;
                    Settings.IsRemembered = false;
                    await _navigationService.NavigateAsync("/FleetMasterDetailPage/NavigationPage/Aviso2Page");
                    return;
                }
            }



            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User2);
            
            var controller = string.Format("/AsignacionesOTs/GetTasas/{0}", _user.IDUser);
            var response = await _apiService.GetTasasForUser(
                url,
                "api",
                controller,
                _user.IDUser);
            IsRefreshing = false;
            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Problema para recuperar datos.", "Aceptar");
                return;
            }
            MyTasas = (List<ReclamoTasa>)response.Result;
            RefreshList();
            IsRefreshing = false;

            //Remotes = new ObservableCollection<RemoteItemViewModel>(MyRemotes.Select(a => new RemoteItemViewModel(_navigationService)
            //{
            //}).ToList());

        }

        public void RefreshList()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                if (_isChecked == true)
                {
                    var myListTasaItemViewModel = MyTasas.Select(a => new TasaItemViewModel(_navigationService)
                    {
                        CantRem = a.CantRem,
                        CAUSANTEC = a.CAUSANTEC,
                        CLIENTE = a.CLIENTE,
                        CodigoCierre = a.CodigoCierre,
                        DescCR = DescCierre(a.CodigoCierre),
                        CP = a.CP,
                        Descripcion = a.Descripcion,
                        DOMICILIO = a.DOMICILIO,
                        ENTRECALLE1 = a.ENTRECALLE1,
                        ENTRECALLE2 = a.ENTRECALLE2,
                        ESTADOGAOS = a.ESTADOGAOS,
                        GRXX = a.GRXX,
                        GRYY = a.GRYY,
                        FechaAsignada = a.FechaAsignada,
                        Novedades = a.Novedades,
                        LOCALIDAD = a.LOCALIDAD,
                        NOMBRE = a.NOMBRE,
                        ObservacionCaptura = a.ObservacionCaptura,
                        PROYECTOMODULO = a.PROYECTOMODULO,
                        RECUPIDJOBCARD = a.RECUPIDJOBCARD,
                        SUBCON = a.SUBCON,
                        TELEFONO = a.TELEFONO,
                        UserID = a.UserID,
                        PROVINCIA = a.PROVINCIA,
                        ReclamoTecnicoID = a.ReclamoTecnicoID,
                        MOTIVOS = a.MOTIVOS,
                        IDSuscripcion = a.IDSuscripcion,
                        FechaCita = a.FechaCita,
                        MedioCita = a.MedioCita,
                        FechaEvento1 = a.FechaEvento1,
                        Evento1 = a.Evento1,
                        FechaEvento2 = a.FechaEvento2,
                        Evento2 = a.Evento2,
                        FechaEvento3 = a.FechaEvento3,
                        Evento3 = a.Evento3,
                        FechaEvento4 = a.FechaEvento4,
                        Evento4 = a.Evento4,
                        TelefAlternativo1 = a.TelefAlternativo1,
                        TelefAlternativo2 = a.TelefAlternativo2,
                        TelefAlternativo3 = a.TelefAlternativo3,
                        TelefAlternativo4 = a.TelefAlternativo4
                    });
                    Tasas = new ObservableCollection<TasaItemViewModel>(myListTasaItemViewModel
                        /*.OrderBy(o => o.FechaCita + o.NOMBRE + o.FechaAsignada));*/
                        .Where(o => o.CodigoCierre != 26
                      && o.CodigoCierre != 27
                      && o.CodigoCierre != 28
                      && o.CodigoCierre != 44
                      && o.CodigoCierre == 45
                        )
                        .OrderBy(o => o.FechaAsignada));
                    CantTasas = Tasas.Count();
                }
               if(_isChecked == false)
                {
                    var myListTasaItemViewModel = MyTasas.Select(a => new TasaItemViewModel(_navigationService)
                    {
                        CantRem = a.CantRem,
                        CAUSANTEC = a.CAUSANTEC,
                        CLIENTE = a.CLIENTE,
                        CodigoCierre = a.CodigoCierre,
                        DescCR = DescCierre(a.CodigoCierre),
                        CP = a.CP,
                        Descripcion = a.Descripcion,
                        DOMICILIO = a.DOMICILIO,
                        ENTRECALLE1 = a.ENTRECALLE1,
                        ENTRECALLE2 = a.ENTRECALLE2,
                        ESTADOGAOS = a.ESTADOGAOS,
                        GRXX = a.GRXX,
                        GRYY = a.GRYY,
                        FechaAsignada = a.FechaAsignada,
                        Novedades = a.Novedades,
                        LOCALIDAD = a.LOCALIDAD,
                        NOMBRE = a.NOMBRE,
                        ObservacionCaptura = a.ObservacionCaptura,
                        PROYECTOMODULO = a.PROYECTOMODULO,
                        RECUPIDJOBCARD = a.RECUPIDJOBCARD,
                        SUBCON = a.SUBCON,
                        TELEFONO = a.TELEFONO,
                        UserID = a.UserID,
                        PROVINCIA = a.PROVINCIA,
                        ReclamoTecnicoID = a.ReclamoTecnicoID,
                        MOTIVOS = a.MOTIVOS,
                        IDSuscripcion = a.IDSuscripcion,
                        FechaCita = a.FechaCita,
                        MedioCita = a.MedioCita,
                        FechaEvento1 = a.FechaEvento1,
                        Evento1 = a.Evento1,
                        FechaEvento2 = a.FechaEvento2,
                        Evento2 = a.Evento2,
                        FechaEvento3 = a.FechaEvento3,
                        Evento3 = a.Evento3,
                        FechaEvento4 = a.FechaEvento4,
                        Evento4 = a.Evento4,
                        TelefAlternativo1 = a.TelefAlternativo1,
                        TelefAlternativo2 = a.TelefAlternativo2,
                        TelefAlternativo3 = a.TelefAlternativo3,
                        TelefAlternativo4 = a.TelefAlternativo4
                    });
                    Tasas = new ObservableCollection<TasaItemViewModel>(myListTasaItemViewModel
                        /*.OrderBy(o => o.FechaCita + o.NOMBRE + o.FechaAsignada));*/
                        .Where(o => o.CodigoCierre != 26
                      && o.CodigoCierre != 27
                      && o.CodigoCierre != 28
                      && o.CodigoCierre != 44
                        )
                        .OrderBy(o => o.FechaAsignada));
                    CantTasas = Tasas.Count();
                }
            }
            else
            {
                var myListTasaItemViewModel = MyTasas.Select(a => new TasaItemViewModel(_navigationService)
                {
                    CantRem = a.CantRem,
                    CAUSANTEC = a.CAUSANTEC,
                    CLIENTE = a.CLIENTE,
                    CodigoCierre = a.CodigoCierre,
                    DescCR = DescCierre(a.CodigoCierre),
                    CP = a.CP,
                    Descripcion = a.Descripcion,
                    DOMICILIO = a.DOMICILIO,
                    ENTRECALLE1 = a.ENTRECALLE1,
                    ENTRECALLE2 = a.ENTRECALLE2,
                    ESTADOGAOS = a.ESTADOGAOS,
                    GRXX = a.GRXX,
                    GRYY = a.GRYY,
                    FechaAsignada = a.FechaAsignada,
                    Novedades = a.Novedades,
                    LOCALIDAD = a.LOCALIDAD,
                    NOMBRE = a.NOMBRE,
                    ObservacionCaptura = a.ObservacionCaptura,
                    PROYECTOMODULO = a.PROYECTOMODULO,
                    RECUPIDJOBCARD = a.RECUPIDJOBCARD,
                    SUBCON = a.SUBCON,
                    TELEFONO = a.TELEFONO,
                    UserID = a.UserID,
                    PROVINCIA = a.PROVINCIA,
                    ReclamoTecnicoID = a.ReclamoTecnicoID,
                    MOTIVOS = a.MOTIVOS,
                    IDSuscripcion = a.IDSuscripcion,
                    FechaCita = a.FechaCita,
                    MedioCita = a.MedioCita,
                    FechaEvento1 = a.FechaEvento1,
                    Evento1 = a.Evento1,
                    FechaEvento2 = a.FechaEvento2,
                    Evento2 = a.Evento2,
                    FechaEvento3 = a.FechaEvento3,
                    Evento3 = a.Evento3,
                    FechaEvento4 = a.FechaEvento4,
                    Evento4 = a.Evento4,
                    TelefAlternativo1 = a.TelefAlternativo1,
                    TelefAlternativo2 = a.TelefAlternativo2,
                    TelefAlternativo3 = a.TelefAlternativo3,
                    TelefAlternativo4 = a.TelefAlternativo4

                });
                

                    if (_isChecked == true)
                    {
                    Tasas = new ObservableCollection<TasaItemViewModel>(myListTasaItemViewModel
                    /*.OrderBy(o => o.FechaCita + o.NOMBRE + o.FechaAsignada)*/
                      .Where(
                            o => (o.NOMBRE.ToLower().Contains(this.Filter.ToLower()))
                            ||
                                (o.CLIENTE.ToLower().Contains(this.Filter.ToLower()))
                            ||
                                FecCita(Convert.ToDateTime(o.FechaCita)).Contains(this.Filter.ToLower())
                            && o.CodigoCierre != 26
                            && o.CodigoCierre != 27
                            && o.CodigoCierre != 28
                            && o.CodigoCierre != 44
                            && o.CodigoCierre == 45

                                //|| 
                                //    (o.Novedades.ToLower().Contains(this.Filter.ToLower()))
                                //||
                                //   o.NoMostrarAPP.Value == 0
                                )
                     .OrderBy(o => o.FechaAsignada));

                    }

                if (_isChecked == false)
                {
                    Tasas = new ObservableCollection<TasaItemViewModel>(myListTasaItemViewModel
                    /*.OrderBy(o => o.FechaCita + o.NOMBRE + o.FechaAsignada)*/
                      .Where(
                            o => (o.NOMBRE.ToLower().Contains(this.Filter.ToLower()))
                            ||
                                (o.CLIENTE.ToLower().Contains(this.Filter.ToLower()))
                            ||
                                FecCita(Convert.ToDateTime(o.FechaCita)).Contains(this.Filter.ToLower())
                            && o.CodigoCierre != 26
                            && o.CodigoCierre != 27
                            && o.CodigoCierre != 28
                            && o.CodigoCierre != 44

                                //|| 
                                //    (o.Novedades.ToLower().Contains(this.Filter.ToLower()))
                                //||
                                //   o.NoMostrarAPP.Value == 0
                                )
                     .OrderBy(o => o.FechaAsignada));

                }



                CantTasas = Tasas.Count();
            }
        }
          




        private async void TasasMap()
        {
            await _navigationService.NavigateAsync("TasasMapPage");
        }

        private string DescCierre(int? CR)
        {
            
            if (CR == 21) { return "CLIENTE CONTINUA CON EL SERVICIO"; };
            if (CR == 22) { return "CLIENTE FALLECIO"; };
            if (CR == 23) { return "CLIENTE NO ACEPTA RETIRO"; };
            if (CR == 24) { return "CLIENTE NO POSEE LOS EQUIPOS"; };
            if (CR == 25) { return "CLIENTE YA ENTREGO LOS EQUIPOS"; };
            //if (CR == 26) { return "CANCELADO. NO SE PUDO CONTACTAR AL CLIENTE"; };
            if (CR == 41) { return "CLIENTE AUSENTE"; };
            //if (CR == 42) { return "CLIENTE SE MUDO"; };
            if (CR == 43) { return "NO ATIENDE EL TELEFONO"; };
            //if (CR == 44) { return "REFERENCIA INCORRECTA"; };
            if (CR == 45) { return "VISITA COORDINADA"; };
            if (CR == 60) { return "RECUPERADO"; };

            return "";
        }


        private async void Search()
        {
            RefreshList();
        }

        private async void PonerHoy()
        {
            Filter = FecCita(DateTime.Now);
            RefreshList();
        }


        private async void Refresh()
        {
            LoadUser();
        }

        private string FecCita(DateTime FecCit)
        {
            var Mes = Convert.ToString(FecCit.Month);
            var Dia = Convert.ToString(FecCit.Day);
            var Año = Convert.ToString(FecCit.Year);
            if (Mes.Length == 1)
            {
                Mes = $"0{Mes}";
            };
            if (Dia.Length == 1)
            {
                Dia = $"0{Dia}";
            };

            return $"{Dia}/{Mes}/{Año}";
        }
    }
}