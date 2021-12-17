using Xamarin.Forms;
using Fleet_App.Prism.ViewModels;
using System;
using Fleet_App.Common.Codigos;
using Fleet_App.Common.Models;

namespace Fleet_App.Prism.Views
{
    public partial class RemotePage : ContentPage
    {
        public RemotePage()
        {
            InitializeComponent();
        }
        async void OnButtonClicked(object sender, EventArgs args)
        {
            label.Text = "EJB";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.Green;

            var remoteViewModel = RemotePageViewModel.GetInstance();
            remoteViewModel.CodigosCierre.Clear();

            var codigosMostrar = new int[] {             
                13,14,15,16,17,18,19,20,21,22,23,24,25,26
            };

            foreach (int cr in codigosMostrar)
            {
                remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = cr, Descripcion = CodigosRemotes.GetCodigoDescription(cr), });
            }


            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = false;



        }
        async void OnButtonClicked2(object sender, EventArgs args)
        {
            label.Text = "INC";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.OrangeRed;

            var remoteViewModel = RemotePageViewModel.GetInstance();
            remoteViewModel.CodigosCierre.Clear();

            var codigosMostrar = new int[] {
                1,2,3,4,5,6,7,8,9,10,11,12,14,15,16,17,18,19,20,21,22,23,24,25,26
            };

            foreach (int cr in codigosMostrar)
            {
                remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = cr, Descripcion = CodigosRemotes.GetCodigoDescription(cr), });
            }

            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = true;
        }
        async void OnButtonClicked3(object sender, EventArgs args)
        {
            label.Text = "PAR";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.YellowGreen;
            var remoteViewModel = RemotePageViewModel.GetInstance();
            remoteViewModel.CodigosCierre.Clear();
            var codigosMostrar = new int[] {
                1,2,3,4,5,6,7,8,9,10,11,12,15,16,17,18,19,20,21,22,23,24,25,26
            };

            foreach (int cr in codigosMostrar)
            {
                remoteViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = cr, Descripcion = CodigosRemotes.GetCodigoDescription(cr), });
            }
            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = true;
        }
    }
}