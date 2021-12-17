using Fleet_App.Prism.ViewModels;
using Xamarin.Forms;

namespace Fleet_App.Prism.Views
{
    public partial class RemotesPage : ContentPage
    {
        public RemotesPage()
        {
            InitializeComponent();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var remotesViewModel = RemotesPageViewModel.GetInstance();
            remotesViewModel.RefreshList();
        }
    }
}
