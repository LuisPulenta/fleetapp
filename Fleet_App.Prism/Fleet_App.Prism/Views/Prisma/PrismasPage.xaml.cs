using Fleet_App.Prism.ViewModels;
using Xamarin.Forms;

namespace Fleet_App.Prism.Views
{
    public partial class PrismasPage : ContentPage
    {
        public PrismasPage()
        {
            InitializeComponent();
        }

        public void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var prismasViewModel = PrismasPageViewModel.GetInstance();
            prismasViewModel.RefreshList();

        }
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var prismasViewModel = PrismasPageViewModel.GetInstance();
            prismasViewModel.RefreshList();
        }
    }
}
