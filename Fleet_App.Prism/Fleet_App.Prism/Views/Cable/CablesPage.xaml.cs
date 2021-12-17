using Fleet_App.Prism.ViewModels;
using Xamarin.Forms;

namespace Fleet_App.Prism.Views
{
    public partial class CablesPage : ContentPage
    {
        public CablesPage()
        {
            InitializeComponent();
        }
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var cablesViewModel = CablesPageViewModel.GetInstance();
            cablesViewModel.RefreshList();
        }
    }
}
