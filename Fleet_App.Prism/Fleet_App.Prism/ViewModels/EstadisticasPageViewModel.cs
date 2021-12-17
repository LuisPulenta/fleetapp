using Fleet_App.Common.Services;
using Prism.Commands;
using Prism.Navigation;

namespace Fleet_App.Prism.ViewModels
{
    public class EstadisticasPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private DelegateCommand _graphCommand;
        private DelegateCommand _graph2Command;
        private DelegateCommand _graph3Command;
        private DelegateCommand _graph4Command;

        public DelegateCommand GraphCommand => _graphCommand ?? (_graphCommand = new DelegateCommand(Graph));
        public DelegateCommand Graph2Command => _graph2Command ?? (_graph2Command = new DelegateCommand(Graph2));
        public DelegateCommand Graph3Command => _graph3Command ?? (_graph3Command = new DelegateCommand(Graph3));
        public DelegateCommand Graph4Command => _graph4Command ?? (_graph4Command = new DelegateCommand(Graph4));

        public EstadisticasPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
            Title = "Estadísticas";

        }

        private async void Graph()
        {
            await _navigationService.NavigateAsync("GraphPage");
        }

        private async void Graph2()
        {
            await _navigationService.NavigateAsync("Graph2Page");
        }

        private async void Graph3()
        {
            await _navigationService.NavigateAsync("Graph3Page");
        }

        private async void Graph4()
        {
            await _navigationService.NavigateAsync("Graph4Page");
        }
    }
}