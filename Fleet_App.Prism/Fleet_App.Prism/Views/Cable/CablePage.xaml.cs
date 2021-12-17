using Fleet_App.Common.Models;
using Fleet_App.Prism.ViewModels;
using System;
using Fleet_App.Prism.ViewModels.Cable;
using Xamarin.Forms;

namespace Fleet_App.Prism.Views
{
    public partial class CablePage : ContentPage
    {
        public CablePage()
        {
            InitializeComponent();
        }


        /// <summary>
        /// BOTON SI A TO DO
        /// Este no deja abrir el selector de cod de cierre.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void OnButtonClicked(object sender, EventArgs args)
        {
            label.Text = "EJB";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.Green;

            var cableViewModel = CablePageViewModel.GetInstance();
            cableViewModel.CodigosCierre.Clear();

            var codigosMostrar = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 30 };

            foreach (int cr in codigosMostrar)
            {
                cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = cr, Descripcion = CodigosCable.GetCodigoDescription(cr), });
            }

            CodCierre.SelectedItem = 30;
            CodCierre.IsEnabled = false;

        }


        /// <summary>
        /// BOTON NO A TO DO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void OnButtonClicked2(object sender, EventArgs args)
        {
            label.Text = "INC";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.OrangeRed;

            var cableViewModel = CablePageViewModel.GetInstance();
            cableViewModel.CodigosCierre.Clear();

            var codigosMostrar = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

            foreach (int cr in codigosMostrar)
            {
                cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = cr, Descripcion = CodigosCable.GetCodigoDescription(cr), });
            }

            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = true;
        }


        /// <summary>
        /// BOTON PARCIAL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void OnButtonClicked3(object sender, EventArgs args)
        {
            label.Text = "PAR";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.YellowGreen;

            var cableViewModel = CablePageViewModel.GetInstance();
            cableViewModel.CodigosCierre.Clear();
            cableViewModel.CodigosCierre.Clear();

            var codigosMostrar = new int[] { 2, 3, 5, 8, 15 };

            foreach (int cr in codigosMostrar)
            {
                cableViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = cr, Descripcion = CodigosCable.GetCodigoDescription(cr), });
            }


            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = true;
        }

    }
}
