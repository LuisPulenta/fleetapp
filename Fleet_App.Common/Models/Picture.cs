using System;
using System.Collections.Generic;
using System.Text;

namespace Fleet_App.Common.Models
{
    public class Picture
    {
        public int PictureId { get; set; }
        public string ImagePath { get; set; }
        public DateTime FechaFoto { get; set; }
        public string FullPicture
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePath))
                {
                    return "pictureavatar.png";
                }
                

                return string.Format("http://fleetsa.serveftp.net:88/FleetApiNew/{0}", ImagePath.Substring(1));

                //return string.Format("http://keypress.serveftp.net:88/FleetApiPrueba/{0}", ImagePath.Substring(1));
                //return string.Format("http://keypress.serveftp.net:88/FleetApiNew/{0}", ImagePath.Substring(1));
                //return string.Format("http://190.221.174.70/Fleetsa/{0}", ImagePath.Substring(1));

            }
        }
        public byte[] ImageArray { get; internal set; }
    }
}