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
using Fleet_App.Prism.ViewModels.Cable;

namespace Fleet_App.Prism.ViewModels
{
    public class CablesPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private UserResponse _user;
        private CableResponse _cab;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isRefreshing;
        private string _entreCalles;
        private ObservableCollection<CableItemViewModel> _cables;
        private static CablesPageViewModel _instance;
        private int _cantCables;
        private string _filter;
        private string _descCR;
        private DelegateCommand _searchCommand;
        private DelegateCommand _refreshCommand;
        private DelegateCommand _cablesMapCommand;
        private DelegateCommand _ponerHoyCommand;

        public DelegateCommand CablesMapCommand => _cablesMapCommand ?? (_cablesMapCommand = new DelegateCommand(CablesMap));
        public DelegateCommand PonerHoyCommand => _ponerHoyCommand ?? (_ponerHoyCommand = new DelegateCommand(PonerHoy));
        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(Search));
        public DelegateCommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new DelegateCommand(Refresh));

        public List<ModuleResponse> MyModules { get; set; }
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

        public int CantCables
        {
            get => _cantCables;
            set => SetProperty(ref _cantCables, value);
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
        public ObservableCollection<CableItemViewModel> Cables
        {
            get => _cables;
            set => SetProperty(ref _cables, value);
        }

        public List<ReclamoCable> MyCables { get; set; }

        public static CablesPageViewModel GetInstance()
        {
            return _instance;
        }


        public CablesPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
            LoadUser();
            Cables = new ObservableCollection<CableItemViewModel>();
            Title = "Recuperos de Cablevisión";
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
                    IsRunning = false;
                    IsEnabled = true;
                    Settings.IsRemembered = false;
                    await _navigationService.NavigateAsync("/FleetMasterDetailPage/NavigationPage/Aviso2Page");
                    return;
                }
            }

            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User2);
            
            var controller = string.Format("/AsignacionesOTs/GetCables/{0}", _user.IDUser);
            var response = await _apiService.GetCablesForUser(
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
            MyCables = (List<ReclamoCable>)response.Result;
            RefreshList();
            IsRefreshing = false;
            
        }


        /// <summary>
        /// 
        /// </summary>
        public void RefreshList()
        {
            var cablesMostrar = MyCables.Select(a => new CableItemViewModel(_navigationService)
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
                NroSeriesExtras = a.NroSeriesExtras,
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


            if (!string.IsNullOrEmpty(this.Filter))
            {
                cablesMostrar = cablesMostrar.Where(
                    o => (o.NOMBRE.ToLower().Contains(this.Filter.ToLower()))
                         ||
                         (o.CLIENTE.ToLower().Contains(this.Filter.ToLower()))
                         ||
                         FecCita(Convert.ToDateTime(o.FechaCita)).Contains(this.Filter.ToLower())
                         ||
                         (o.MOTIVOS.ToLower().Contains(this.Filter.ToLower()))
                         );
            }
            
            Cables = new ObservableCollection<CableItemViewModel>(cablesMostrar
                .OrderBy(o => o.FechaCita + o.NOMBRE + o.FechaAsignada)
                .Where(x => !CodigosCable.OcultarCodigo(x.CodigoCierre, x.ESTADOGAOS)));

            CantCables = Cables.Count();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="FecCit"></param>
        /// <returns></returns>
        private string FecCita (DateTime FecCit)
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

        private string DescCierre(int? CR)
        {
            if (CR == null) return String.Empty;
            return CodigosCable.GetCodigoDescription(CR.Value);
             
        }


        private async void Search()
        {
            RefreshList();
        }

        private async void CablesMap()
        {

            await _navigationService.NavigateAsync("CablesMapPage");
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
    }
}