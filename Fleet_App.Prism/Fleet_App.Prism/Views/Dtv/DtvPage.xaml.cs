using Fleet_App.Common.Models;
using Fleet_App.Prism.ViewModels;
using System;
using Fleet_App.Common.Codigos;
using Xamarin.Forms;

namespace Fleet_App.Prism.Views
{
    public partial class DtvPage : ContentPage
    {
        public DtvPage()
        {
            InitializeComponent();
        }

        async void OnButtonClicked(object sender, EventArgs args)
        {
            label.Text = "EJB";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.Green;

            var dtvPageViewModel = DtvPageViewModel.GetInstance();
            dtvPageViewModel.CodigosCierre.Clear();

            var codigosMostrar = new int[] { 0, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 70 };

            foreach (int cr in codigosMostrar)
            {
                dtvPageViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = cr, Descripcion = CodigosDtv.GetCodigoDescription(cr), });
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

            var dtvPageViewModel = DtvPageViewModel.GetInstance();
            dtvPageViewModel.CodigosCierre.Clear();
            var codigosMostrar = new int[] { 0, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 70 };

            foreach (int cr in codigosMostrar)
            {
                dtvPageViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = cr, Descripcion = CodigosDtv.GetCodigoDescription(cr), });
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

            var dtvPageViewModel = DtvPageViewModel.GetInstance();
            dtvPageViewModel.CodigosCierre.Clear();
            var codigosMostrar = new int[] { 0, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 70 };

            foreach (int cr in codigosMostrar)
            {
                dtvPageViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = cr, Descripcion = CodigosDtv.GetCodigoDescription(cr), });
            }


            //label2.Text = (DateTime.Now).ToString("dd/MM/yyyy");
            //label3.Text = (DateTime.Now).ToString("HH:mm");
            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = true;
        }

    }
}
