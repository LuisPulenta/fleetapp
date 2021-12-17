using Fleet_App.Prism.ViewModels;
using Fleet_App.Prism.ViewModels.Teco;
using Xamarin.Forms;

namespace Fleet_App.Prism.Views.Teco
{
    public partial class TecosPage : ContentPage
    {
        public TecosPage()
        {
            InitializeComponent();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tecosPageViewModel = TecosPageViewModel.GetInstance();
            tecosPageViewModel.RefreshList();
        }
    }
}
