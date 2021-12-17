using Fleet_App.Common.Models;
using Fleet_App.Prism.ViewModels;
using System;
using Fleet_App.Common.Codigos;
using Xamarin.Forms;

namespace Fleet_App.Prism.Views.Tlc
{
    public partial class TlcPage : ContentPage
    {
        public TlcPage()
        {
            InitializeComponent();
        }

        async void OnButtonClicked(object sender, EventArgs args)
        {
            label.Text = "EJB";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.Green;

            var tlcPageViewModel = TlcPageViewModel.GetInstance();
            tlcPageViewModel.CodigosCierre.Clear();

            var codigosMostrar = new int[] { 0, 1, 4, 5, 7, 9, 10, 11, 12, 13, 18, 19, 25, 26, 27, 28 };

            foreach (int cr in codigosMostrar)
            {
                tlcPageViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = cr, Descripcion = CodigosTlc.GetCodigoDescription(cr), });
            }


            //label2.Text = (DateTime.Now).ToString("dd/MM/yyyy");
            //label3.Text = (DateTime.Now).ToString("HH:mm");
            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = false;



        }
        async void OnButtonClicked2(object sender, EventArgs args)
        {
            label.Text = "INC";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.OrangeRed;

            var tlcPageViewModel = TlcPageViewModel.GetInstance();
            tlcPageViewModel.CodigosCierre.Clear();
            var codigosMostrar = new int[] { 0, 1, 4, 5, 7, 9, 10, 11, 12, 13, 18, 19, 25, 26, 27, 28 };

            foreach (int cr in codigosMostrar)
            {
                tlcPageViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = cr, Descripcion = CodigosTlc.GetCodigoDescription(cr), });
            }


            //label2.Text = (DateTime.Now).ToString("dd/MM/yyyy");
            //label3.Text = (DateTime.Now).ToString("HH:mm");
            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = true;
        }
        async void OnButtonClicked3(object sender, EventArgs args)
        {
            label.Text = "PAR";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.YellowGreen;

            var tlcPageViewModel = TlcPageViewModel.GetInstance();
            tlcPageViewModel.CodigosCierre.Clear();
            var codigosMostrar = new int[] { 0, 1, 4, 5, 7, 9, 10, 11, 12, 13, 18, 19, 25, 26, 27, 28 };

            foreach (int cr in codigosMostrar)
            {
                tlcPageViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = cr, Descripcion = CodigosTlc.GetCodigoDescription(cr), });
            }


            //label2.Text = (DateTime.Now).ToString("dd/MM/yyyy");
            //label3.Text = (DateTime.Now).ToString("HH:mm");
            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = true;
        }

    }
}
