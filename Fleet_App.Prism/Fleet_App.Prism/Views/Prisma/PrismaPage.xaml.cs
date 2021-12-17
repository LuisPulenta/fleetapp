using Fleet_App.Common.Models;
using Fleet_App.Prism.ViewModels;
using System;
using Fleet_App.Common.Codigos;
using Xamarin.Forms;

namespace Fleet_App.Prism.Views
{
    public partial class PrismaPage : ContentPage
    {
        public PrismaPage()
        {
            InitializeComponent();
        }

        async void OnButtonClicked(object sender, EventArgs args)
        {
            label.Text = "EJB";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.Green;

            var prismaPageViewModel = PrismaPageViewModel.GetInstance();
            prismaPageViewModel.CodigosCierre.Clear();

            var codigosMostrar = new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10 };

            foreach (int cr in codigosMostrar)
            {
                prismaPageViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = cr, Descripcion = CodigosPrisma.GetCodigoDescription(cr), });
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

            var prismaPageViewModel = PrismaPageViewModel.GetInstance();
            prismaPageViewModel.CodigosCierre.Clear();
            var codigosMostrar = new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10 };

            foreach (int cr in codigosMostrar)
            {
                prismaPageViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = cr, Descripcion = CodigosPrisma.GetCodigoDescription(cr), });
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

            var prismaPageViewModel = PrismaPageViewModel.GetInstance();
            prismaPageViewModel.CodigosCierre.Clear();
            var codigosMostrar = new int[] { 1, 2, 3, 4, 6, 7, 8, 9, 10 };

            foreach (int cr in codigosMostrar)
            {
                prismaPageViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = cr, Descripcion = CodigosPrisma.GetCodigoDescription(cr), });
            }


            //label2.Text = (DateTime.Now).ToString("dd/MM/yyyy");
            //label3.Text = (DateTime.Now).ToString("HH:mm");
            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = true;
        }

    }
}
