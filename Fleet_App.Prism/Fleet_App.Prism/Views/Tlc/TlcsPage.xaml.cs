using Fleet_App.Prism.ViewModels;
using Xamarin.Forms;

namespace Fleet_App.Prism.Views.Tlc
{
    public partial class TlcsPage : ContentPage
    {
        public TlcsPage()
        {
            InitializeComponent();
        }
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tlcsViewModel = TlcsPageViewModel.GetInstance();
            tlcsViewModel.RefreshList();
        }
    }
}
