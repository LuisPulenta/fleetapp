using Fleet_App.Common.Helpers;
using Fleet_App.Common.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fleet_App.Prism.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private UserResponse _user2;


        public UserResponse User2
        {
            get => _user2;
            set => SetProperty(ref _user2, value);
        }

        public HomePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            User2 = JsonConvert.DeserializeObject<UserResponse>(Settings.User2);
            _navigationService = navigationService;
            Title = "Fleet";

        }
    }
}
