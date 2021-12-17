using Fleet_App.Prism.ViewModels;
using Xamarin.Forms;

namespace Fleet_App.Prism.Views
{
    public partial class DtvsPage : ContentPage
    {
        public DtvsPage()
        {
            InitializeComponent();
        }
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var dtvsViewModel = DtvsPageViewModel.GetInstance();
            dtvsViewModel.RefreshList();
        }
    }
}
