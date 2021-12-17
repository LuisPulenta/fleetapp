using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fleet_App.Prism.ViewModels
{
    public class OrdersPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public OrdersPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Ordenes de Trabajo";
        }
    }
}
