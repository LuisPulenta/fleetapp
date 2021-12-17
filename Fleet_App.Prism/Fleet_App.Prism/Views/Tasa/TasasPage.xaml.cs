using Fleet_App.Prism.ViewModels;
using Xamarin.Forms;

namespace Fleet_App.Prism.Views
{
    public partial class TasasPage : ContentPage
    {
        public TasasPage()
        {
            InitializeComponent();
        }

        public void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var tasasViewModel = TasasPageViewModel.GetInstance();
            tasasViewModel.RefreshList();

        }
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tasasViewModel = TasasPageViewModel.GetInstance();
            tasasViewModel.RefreshList();
        }


    }
}
