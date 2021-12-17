using System;
using System.Collections.Generic;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Widget;
using CustomRenderer.Droid;
using Fleet_App.Prism;
using Fleet_App.Prism.Droid;
using Fleet_App.Prism.ViewModels;
using Fleet_App.Prism.ViewModels.Teco;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace CustomRenderer.Droid
{
    public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
    {
        private int opc;
        List<CustomPin> customPins;

        public CustomMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.InfoWindowClick -= OnInfoWindowClick;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                customPins = formsMap.CustomPins;
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            NativeMap.InfoWindowClick += OnInfoWindowClick;
            NativeMap.SetInfoWindowAdapter(this);
        }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);

            if (pin.ClassId == "Dtv")
            {
                opc = 1;
                //marker.SetAlpha (1);
            }
            if (pin.ClassId == "Remote")
            {
                opc = 2;
                //marker.SetAlpha(2);
            }
            if (pin.ClassId == "Tasa")
            {
                opc = 3;
                //marker.SetAlpha(3);
            }
            if (pin.ClassId == "DTV")
            {
                opc = 4;
                //marker.SetAlpha(3);
            }
            if (pin.ClassId == "Teco")
            {
                opc = 6;
                //marker.SetAlpha(3);
            }
            
            

            if (pin.StyleId == "ConCitaOtroDia")
            {
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin2azul));
            }
            if (pin.StyleId == "SinCita")
            {
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin2rojo));
            }

            if (pin.StyleId == "ConCitaHoy")
            {
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin2verde));
            }
            return marker;
        }

        void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            //var customPin = GetCustomPin(e.Marker);
            //if (customPin == null)
            //{
            //    throw new Exception("Custom pin not found");
            //}

            //if (!string.IsNullOrWhiteSpace(customPin.Url))
            //{
            //    var url = Android.Net.Uri.Parse(customPin.Url);
            //    var intent = new Intent(Intent.ActionView, url);
            //    intent.AddFlags(ActivityFlags.NewTask);
            //    Android.App.Application.Context.StartActivity(intent);
            //}

            
            int lugar = e.Marker.Title.IndexOf(" Cita: ");

            if (lugar == -1) return;

            var txtBuscar = e.Marker.Title.Substring(0, lugar);


            if (opc == 1)
            {
                CablesPageViewModel.GetInstance().Filter = txtBuscar;
                CablesMapPageViewModel.GetInstance().CerrarMapa();
            }

            if (opc == 2)
            {
                RemotesPageViewModel.GetInstance().Filter = txtBuscar;
                RemotesMapPageViewModel.GetInstance().CerrarMapa();
            }

            if (opc == 3)
            {
                TasasPageViewModel.GetInstance().Filter = txtBuscar;
                TasasMapPageViewModel.GetInstance().CerrarMapa();
            }
            if (opc == 4)
            {
                DtvsPageViewModel.GetInstance().Filter = txtBuscar;
                throw new NotImplementedException();
                //DtvsMapPageViewModel.GetInstance().CerrarMapa();
            }
            if (opc == 6)
            {
                TecosPageViewModel.GetInstance().Filter = txtBuscar;
                TecosMapPageViewModel.GetInstance().CerrarMapa();
            }


        }

        public Android.Views.View GetInfoContents(Marker marker)
        {
            var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
            if (inflater != null)
            {
                Android.Views.View view;

                //var customPin = GetCustomPin(marker);
                //if (customPin == null)
                //{
                //    throw new Exception("Custom pin not found");
                //}

                //if (customPin.Name.Equals("Xamarin"))
                //{
                //    //view = inflater.Inflate(Resource.Layout.XamarinMapInfoWindow, null);
                //}
                //else
                //{
                //    //view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);
                //}

                //var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                //var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);

                //if (infoTitle != null)
                //{
                //    infoTitle.Text = marker.Title;
                //}
                //if (infoSubtitle != null)
                //{
                //    infoSubtitle.Text = marker.Snippet;
                //}

                return null;// view;
            }
            return null;
        }

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }

        CustomPin GetCustomPin(Marker annotation)
        {
            var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
            foreach (var pin in customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }
    }
}
