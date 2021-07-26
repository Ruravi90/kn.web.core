using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace kn.web.core.Socket
{
    

    public class ParseData
    {
        private string url = "https://nominatim.openstreetmap.org/reverse?lat=#Lat#&lon=#Lon#&format=geocodejson";
        public Models.Event parse(string data) {
            Console.WriteLine(data);
  
            Models.Event _event;

            string serial = data.Substring(3, data.IndexOf(";NA") - 3);
            string msn = data.Substring(data.LastIndexOf("#D#"));

            var msnPartes = msn.Split(";");

            if (msnPartes.Count() < 16)
            {
                return null;
            }

            string lat = msnPartes[2];

            if (lat == "NA") {
                return null;
            }

            string lon = msnPartes[4];
            string speed = msnPartes[6];
            string course = msnPartes[7];
            string height = msnPartes[8];
            string stats = msnPartes[9];
            string hdop = msnPartes[10];
            int inputs = int.Parse(msnPartes[11]);
            string outputs = msnPartes[12];
            string adc = msnPartes[13];
            string ibutton = msnPartes[14];
            string msgFinal = msnPartes[15];

            int enc = 0;

            if (inputs != 0) 
            {
                enc = 1;
            }

            string FIRSTLAT = lat.Substring(0, 2);
            string resultadoLAT = lat.Substring(2);
            float RELAT = float.Parse(resultadoLAT);
            float PRLAT = RELAT / 60;
            string FIXLAT = PRLAT.ToString("N8");
            string FINLAT = FIXLAT.ToString().Substring(FIXLAT.IndexOf("."));

            float latFinal = float.Parse(FIRSTLAT + FINLAT);

            string FIRSTLON = lon.Substring(0, 3);
            string resultadoLON = lon.Substring(3);
            float RELON = float.Parse(resultadoLON);
            float PRLON = RELON / 60;
            string FIXLON = PRLON.ToString("N8");
            string FINLON = FIXLON.ToString().Substring(FIXLON.IndexOf("."));

            float lonFinal = float.Parse("-" + FIRSTLON + FINLON);


            var y = new ReverseGeocoder();

            var r2 = y.ReverseGeocode(new ReverseGeocodeRequest
            {
                Longitude = lonFinal,
                Latitude = latFinal,

                BreakdownAddressElements = true,
                ShowExtraTags = true,
                ShowAlternativeNames = true,
                ShowGeoJSON = true
            });
            r2.Wait();

            _event = new Models.Event
            {
                antena = serial,
                plataforma = "ALKKON",
                fecha_evento = DateTime.Now,
                fecha_recepcion = DateTime.Now,
                posicion = r2.Result.DisplayName,
                latitud = lonFinal,
                longitud = lonFinal,
                tipo_evento = 1,
                info_adicional = "",
                ignicion = enc
            };

            return _event;
        }
    }
}
