using Fleet_App.Common.Models;
using Fleet_App.Prism.ViewModels;
using System;
using Xamarin.Forms;

namespace Fleet_App.Prism.Views
{
    public partial class TasaPage : ContentPage
    {
        public TasaPage()
        {
            InitializeComponent();
        }

        async void OnButtonClicked(object sender, EventArgs args)
        {
            label.Text = "EJB";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.Green;

            var tasaViewModel = TasaPageViewModel.GetInstance();
            tasaViewModel.CodigosCierre.Clear();
            tasaViewModel.Habilitado = true;
            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = false;
            


        }
        async void OnButtonClicked2(object sender, EventArgs args)
        {
            label.Text = "INC";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.OrangeRed;
            

            var tasaViewModel = TasaPageViewModel.GetInstance();

            tasaViewModel.Habilitado = true;
            tasaViewModel.CodigosCierre.Clear();
            //tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 10, Descripcion = "VERIFICACION ADMINISTRATIVA", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 21, Descripcion = "CLIENTE CONTINUA CON EL SERVICIO", });
            //tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 22, Descripcion = "CLIENTE FALLECIO", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 23, Descripcion = "CLIENTE NO ACEPTA RETIRO", });
            //tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 24, Descripcion = "CLIENTE NO POSEE LOS EQUIPOS", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 25, Descripcion = "CLIENTE YA ENTREGO LOS EQUIPOS", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 26, Descripcion = "DIRECCIÓN Y NÚMERO ERRÓNEO", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 27, Descripcion = "CLIENTE SE MUDÓ", });
            //tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 28, Descripcion = "REFERENCIA INCORRECTA", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 41, Descripcion = "CLIENTE AUSENTE", });
            //tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 42, Descripcion = "CLIENTE SE MUDÓ", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 43, Descripcion = "NO ATIENDE EL TELEFONO", });
            //tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 44, Descripcion = "REFERENCIA INCORRECTA", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 45, Descripcion = "VISITA COORDINADA", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 60, Descripcion = "RECUPERADO", });


            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = true;

        }
        async void OnButtonClicked3(object sender, EventArgs args)
        {
            label.Text = "PAR";
            label.TextColor = Xamarin.Forms.Color.White;
            label.BackgroundColor = Xamarin.Forms.Color.YellowGreen;
            

            var tasaViewModel = TasaPageViewModel.GetInstance();
            tasaViewModel.CodigosCierre.Clear();
            tasaViewModel.CodigosCierre.Clear();
            tasaViewModel.Habilitado = true;

            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 21, Descripcion = "CLIENTE CONTINUA CON EL SERVICIO", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 23, Descripcion = "CLIENTE NO ACEPTA RETIRO", });
            //tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 24, Descripcion = "CLIENTE NO POSEE LOS EQUIPOS", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 25, Descripcion = "CLIENTE YA ENTREGO LOS EQUIPOS", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 41, Descripcion = "VISITA CLIENTE AUSENTE", });
            tasaViewModel.CodigosCierre.Add(new CodigoCierre { Codigo = 45, Descripcion = "VISITA COORDINADA", });



            //label2.Text = (DateTime.Now).ToString("dd/MM/yyyy");
            //label3.Text = (DateTime.Now).ToString("HH:mm");
            CodCierre.SelectedItem = null;
            CodCierre.IsEnabled = true;
        }

        

    }
}
