using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Fleet_App.Prism.ViewModels
{
    public class ContactPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        
        public ContactPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Contacto";
        }

    }
}
