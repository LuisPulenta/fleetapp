using Prism.Navigation;
using Fleet_App.Prism.Models;
using System.Collections.Generic;
using Prism.Commands;
using System;
using Fleet_App.Common.Services;
using Newtonsoft.Json;
using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.QrCode.Internal;

namespace Fleet_App.Prism.ViewModels
{
    public class Graph4PageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private DelegateCommand _refreshCommand;
        private bool _isRunning;
        private List<TrabajosResponse> _trabajos;
        private List<TrabajosResponse2> _trabajos2;
        private ObservableCollection<Grafico> _grafico;
        private Proyecto _proyectoElegido;
        private int _proyectoId;
        private ObservableCollection<Proyecto> _proyectos;
        private double? _ordenes;
        private double? _ordenes1;
        private double? _ordenes2;
        private double? _ordenes3;
        private double? _ordenes4;
        private double? _ordenes5;
        private string _titulo;
        private string _titulo1;
        private string _titulo2;
        private string _titulo3;
        private string _titulo4;
        private string _titulo5;
        private string _tituloModelos;
        private string _cantidadModelos;
        private string _detalleModelos;
        private ObservableCollection<ModelosResponse> _modelos;

        public ObservableCollection<ModelosResponse> Modelos
        {
            get => _modelos;
            set => SetProperty(ref _modelos, value);
        }

        public string TituloModelos
        {
            get => _tituloModelos;
            set => SetProperty(ref _tituloModelos, value);
        }

        public string CantidadModelos
        {
            get => _cantidadModelos;
            set => SetProperty(ref _cantidadModelos, value);
        }

        public string DetalleModelos
        {
            get => _detalleModelos;
            set => SetProperty(ref _detalleModelos, value);
        }

        public double? Ordenes
        {
            get => _ordenes;
            set => SetProperty(ref _ordenes, value);
        }
        public double? Ordenes1
        {
            get => _ordenes1;
            set => SetProperty(ref _ordenes1, value);
        }
        public double? Ordenes2
        {
            get => _ordenes2;
            set => SetProperty(ref _ordenes2, value);
        }
        public double? Ordenes3
        {
            get => _ordenes3;
            set => SetProperty(ref _ordenes3, value);
        }
        public double? Ordenes4
        {
            get => _ordenes4;
            set => SetProperty(ref _ordenes4, value);
        }
        public double? Ordenes5
        {
            get => _ordenes5;
            set => SetProperty(ref _ordenes5, value);
        }
        public string Titulo
        {
            get => _titulo;
            set => SetProperty(ref _titulo, value);
        }
        public string Titulo1
        {
            get => _titulo1;
            set => SetProperty(ref _titulo1, value);
        }
        public string Titulo2
        {
            get => _titulo2;
            set => SetProperty(ref _titulo2, value);
        }
        public string Titulo3
        {
            get => _titulo3;
            set => SetProperty(ref _titulo3, value);
        }
        public string Titulo4
        {
            get => _titulo4;
            set => SetProperty(ref _titulo4, value);
        }
        public string Titulo5
        {
            get => _titulo5;
            set => SetProperty(ref _titulo5, value);
        }

        public DelegateCommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new DelegateCommand(LoadDataAsync));

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime Hoy { get; set; }

        public List<TrabajosResponse> Trabajos
        {
            get => _trabajos;
            set => SetProperty(ref _trabajos, value);
        }
        public List<TrabajosResponse2> Trabajos2
        {
            get => _trabajos2;
            set => SetProperty(ref _trabajos2, value);
        }
        public ObservableCollection<Grafico> Grafico
        {
            get => _grafico;
            set => SetProperty(ref _grafico, value);
        }

        public Proyecto ProyectoElegido
        {
            get => _proyectoElegido;
            set => SetProperty(ref _proyectoElegido, value);
        }

        public int ProyectoId
        {
            get => _proyectoId;
            set => SetProperty(ref _proyectoId, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public ObservableCollection<Proyecto> Proyectos
        {
            get => _proyectos;
            set => SetProperty(ref _proyectos, value);
        }

        public Graph4PageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            this._apiService = apiService;
            Hoy = DateTime.Today;
            Ordenes1 = null;
            Ordenes2 = null;
            Ordenes3 = null;
            Ordenes4 = null;
            Ordenes5 = null;
            //StartDate = DateTime.Today.AddDays(-700);
            StartDate = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
            EndDate = DateTime.Now;
            LoadProyectos();
            //LoadDataAsync();
            Title = "Mis Objetivos";
        }





        private void LoadProyectos()
        {
            Proyectos = new ObservableCollection<Proyecto>();
            //Proyectos.Add(new Proyecto { Id = 1, Name = "Recuperos Cablevisión", });
            Proyectos.Add(new Proyecto { Id = 2, Name = "Recuperos Tasa", });
            Proyectos.Add(new Proyecto { Id = 3, Name = "Controles Remotos", });
            // Proyectos.Add(new Proyecto { Id = 4, Name = "Adicionales", });
        }


        /// <summary>
        /// 
        /// </summary>
        private async void LoadDataAsync()
        {
            IsRunning = true;

            string url = App.Current.Resources["UrlAPI"].ToString();

            bool connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "Revise su conexión a Internet",
                    "Aceptar");
                return;
            }


            if (ProyectoElegido == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe seleccionar un Proyecto", "Aceptar");
                return;
            };

            TituloModelos = String.Empty;
            CantidadModelos = String.Empty;
            Modelos = new ObservableCollection<ModelosResponse>();


            string proyectoAConsultar = String.Empty;

            switch (ProyectoElegido.Id)
            {
                case 1:
                    proyectoAConsultar = "Dtv";
                    break;
                case 2:
                    proyectoAConsultar = "Tasa";
                    break;
                case 3:
                    proyectoAConsultar = "Otro";
                    break;
                case 4:
                    proyectoAConsultar = "Adicionales";
                    break;
            }
            
            UserResponse user = JsonConvert.DeserializeObject<UserResponse>(Settings.User2);
            TrabajosRequest request = new TrabajosRequest
            {
                Hasta = EndDate.AddDays(1),
                Desde = StartDate,
                UserID = user.IDUser,
                Proyecto = proyectoAConsultar,
            };
            
            switch (ProyectoElegido.Id)
            {
                case 1:
                    return;
                case 2:
                    await LoadProyectoTasa(url, request);
                    break;
                case 3:
                    await LoadProyectoOtros(url, request);
                    break;
                case 4:
                    await LoadProyectoAdicionales(url, request);
                    break;
            }

        }


        /// <summary>
        /// Proyecto Tasa
        /// </summary>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task LoadProyectoTasa(string url, TrabajosRequest request)
         {
            Response2 response = await _apiService.GetTrabajos(url, "api", "/AsignacionesOTs/GetTrabajos4", request);
            IsRunning = false;
            
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");
                return;
            }

            List<TrabajosResponse> trabajos = (List<TrabajosResponse>)response.Result;
            Trabajos = trabajos.Select(t => new TrabajosResponse()
            {
                Cant = t.Cant,
                EstadoGaos = t.EstadoGaos,
                ProyectoModulo = t.ProyectoModulo
            }).ToList();

            Ordenes = 0;
            Ordenes1 = null;
            Ordenes2 = null;
            Ordenes3 = null;
            Ordenes4 = null;
            Ordenes5 = null;
            foreach (var o in Trabajos)
            {
                Ordenes = Ordenes + o.Cant;
            }

            Titulo = "Total OT asignadas:";
            Titulo2 = "";
            Titulo3 = "60%:";
            Titulo4 = "65%:";
            Titulo5 = "70%:";
            Ordenes2 = null;
            Ordenes3 = Ordenes * 0.60;
            Ordenes4 = Ordenes * 0.65;
            Ordenes5 = Ordenes * 0.70;


            // TRABAJOS CON COD 60
            response = await _apiService.GetTrabajos(url, "api", "/AsignacionesOTs/GetTrabajos3", request);
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");
                return;
            }
            trabajos = (List<TrabajosResponse>)response.Result;

            //Titulo1 = "Cantidad con cod 60:";

            //Ordenes1 = trabajos.Where(x => x.CodigoCierre == "60").Sum(x => x.Cant);




            //var response2 = await _apiService.GetModelos(url, "api", "/AsignacionesOTs/GetModelosTotales", request);
            //IsRunning = false;

            // if (!response2.IsSuccess)
            //{
            //    await App.Current.MainPage.DisplayAlert(
            //        "Error",
            //        response2.Message,
            //        "Aceptar");
            //    return;
            //}

            //List<ModelosResponse> modelosResponse = (List<ModelosResponse>)response2.Result;

            //Modelos = new ObservableCollection<ModelosResponse>();
            //modelosResponse.ForEach(t => 
            //    Modelos.Add(new ModelosResponse()
            //    {
            //        Modelo = $"{t.Modelo}: ",
            //        Cant = t.Cant
            //    })
            //    );

            //TituloModelos = "Modelos Extras: ";
            //CantidadModelos = Modelos.Count.ToString();
            
         }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task LoadProyectoOtros(string url, TrabajosRequest request)
        {
            Response2 response = await _apiService.GetTrabajos2(url, "api", "/AsignacionesOTs/GetTrabajos5", request);
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");
                return;
            }

            List<TrabajosResponse2> trabajos = (List<TrabajosResponse2>)response.Result;
            Trabajos2 = trabajos.Select(t => new TrabajosResponse2()
            {
                FechaInicio = t.FechaInicio,
                FechaAsignada = t.FechaAsignada,
                FECHACUMPLIDA = t.FECHACUMPLIDA,
                Cant = t.Cant,
                ProyectoModulo = t.ProyectoModulo
            }).ToList();

            Ordenes = 0;
            Ordenes1 = 0;
            Ordenes2 = 0;
            Ordenes3 = 0;
            Ordenes4 = 0;
            Ordenes5 = 0;
            foreach (var o in Trabajos2)
            {
                Ordenes = Ordenes + o.Cant;
                int Dif = (o.FECHACUMPLIDA - o.FechaAsignada).Days;

                if (Dif >= 0 && Dif < 1)
                {
                    Ordenes1 = Ordenes1 + o.Cant;
                }
                if (Dif >= 1 && Dif < 2)
                {
                    Ordenes2 = Ordenes2 + o.Cant;
                }
                if (Dif >= 2 && Dif < 3)
                {
                    Ordenes3 = Ordenes3 + o.Cant;
                }
                if (Dif >= 3 && Dif < 4)
                {
                    Ordenes4 = Ordenes4 + o.Cant;
                }
                if (Dif >= 4)
                {
                    Ordenes5 = Ordenes5 + o.Cant;
                }
            }

            Titulo = "Total OT cumplidas:";
            Titulo1 = "0hs:";
            Titulo2 = "24hs:";
            Titulo3 = "48hs:";
            Titulo4 = "72hs:";
            Titulo5 = "+72hs:";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task LoadProyectoAdicionales(string url, TrabajosRequest request)
        {
            Response2 response = await _apiService.GetTrabajos2(url, "api", "/AsignacionesOTs/GetAdicionalesTotales", request);
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Aceptar");
                return;
            }

            List<ModelosResponse> modelosResponse = (List<ModelosResponse>)response.Result;

            Modelos = new ObservableCollection<ModelosResponse>();
            modelosResponse.ForEach(t =>
                Modelos.Add(new ModelosResponse()
                {
                    Modelo = $"{t.Modelo}: ",
                    Cant = t.Cant
                })
            );

            TituloModelos = "Adicionales: ";
            CantidadModelos = Modelos.Count.ToString();

        }
    }
}