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

namespace Fleet_App.Prism.ViewModels
{
    public class GraphPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private DelegateCommand _refreshCommand;
        private bool _isRunning;
        private List<TrabajosResponse> _trabajos;
        private ObservableCollection<Grafico> _grafico;
        private Proyecto _proyectoElegido;
        private int _proyectoId;
        private ObservableCollection<Proyecto> _proyectos;
        private double? _ordenes;
        public double? Ordenes
        {
            get => _ordenes;
            set => SetProperty(ref _ordenes, value);
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

        public ObservableCollection<Grafico> Grafico
        {
            get => _grafico;
            set => SetProperty(ref _grafico, value);
        }

    public Proyecto proyectoElegido
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

        public GraphPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            this._apiService = apiService;
            Hoy = DateTime.Today;
            //StartDate = DateTime.Today.AddDays(-700);
            StartDate = DateTime.Today.AddDays(-DateTime.Today.Day + 1);
            EndDate = DateTime.Now;
            LoadProyectos();
            //LoadDataAsync();
            Title = "O.T. por Fecha Asignación";
        }

        



        private void LoadProyectos()
        {
            Proyectos = new ObservableCollection<Proyecto>();
            Proyectos.Add(new Proyecto { Id = 1, Name = "Recuperos Cablevisión", });
            Proyectos.Add(new Proyecto { Id = 2, Name = "Recuperos Tasa", });
            Proyectos.Add(new Proyecto { Id = 3, Name = "Controles Remotos", });
        }


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


            if (proyectoElegido==null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe seleccionar un Proyecto", "Aceptar");
                return;
            };



            string proyectoAConsultar = "";
            if (proyectoElegido.Id == 1) { proyectoAConsultar = "Dtv"; };
            if (proyectoElegido.Id == 2) { proyectoAConsultar = "Tasa"; };
            if (proyectoElegido.Id == 3) { proyectoAConsultar = "Otro"; };




            UserResponse user = JsonConvert.DeserializeObject<UserResponse>(Settings.User2);
            TrabajosRequest request = new TrabajosRequest
            {
                Hasta = EndDate.AddDays(1),
                Desde = StartDate,
                UserID = user.IDUser,
                Proyecto= proyectoAConsultar,
            };

            Response2 response = await _apiService.GetTrabajos(url, "api", "/AsignacionesOTs/GetTrabajos", request);

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
             Cant=t.Cant,
             EstadoGaos=t.EstadoGaos,
             ProyectoModulo=t.ProyectoModulo
            }).ToList();
            
            Ordenes = 0;
            foreach (var o in Trabajos)
            {
                Ordenes = Ordenes + o.Cant;
            }


            Grafico = new ObservableCollection<Grafico>();

            foreach (TrabajosResponse trabajo in trabajos)
            {

                Grafico.Add(new Grafico
                    {
                        Cantidad = trabajo.Cant,
                        Nombre=trabajo.EstadoGaos
                    }
                    ); ;
                
            }
            
        }
    }
}