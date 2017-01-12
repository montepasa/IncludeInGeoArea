using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MontePasa.Core.HelperModel
{
    public class GoogleDistance
    {
        // Verilen bir noktanın istenilen bölgenin içinde olup olmadığını kontrol eden algoritmadır
        // Google haritada üzerinde enlem ve boylamı verilen bir noktanın enlemi Lat1, 
        // boylamı Lon1 olsun, kontrol edilecek bölgenin enlemi lat2, boylamı Lon2 ve bölgenin yarıçapı radius olsun
        // Eğer noktamız bu bölgenin içindeyse fonksiyonumuz true dönecektir, değilse false dönecektir 
        public static bool Distance(double lat1, double lon1, double lat2, double lon2, int radius)
        {
            var R = 6371d;
            var dLat = (Math.PI / 180d) * (lat2 - lat1);
            var dLon = (Math.PI / 180d) * (lon2 - lon1);
            var a =
              Math.Sin(dLat / 2d) * Math.Sin(dLat / 2d) +
              Math.Cos((Math.PI / 180d) * (lat1)) * Math.Cos((Math.PI / 180d) * (lat2)) *
              Math.Sin(dLon / 2d) * Math.Sin(dLon / 2d);
            var c = 2d * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1d - a));
            var d = R * c;
            var mtr = d * 1000;
            if (mtr < radius)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       // Yarıçapı ve harita bilgileri verilen (enlem ve boylam) bölgenin içinden rastgele bir konum üreten fonksiyondur.
       // Bölgenin enlemi latitude, boylamı longitude, yarı çapı radius için 
       //Geri dönecek değerler { Latitude: "", Longitude: ""} formatında olacaktır 
        public static GeoLocation ReproduceLocation(string latitude, string longitude, int radius)
        {
            double x0, y0;
            double.TryParse(latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out y0);
            double.TryParse(longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out x0);
            Random random = new Random();
            GeoLocation geolocation = new GeoLocation();
            double radiusInDegrees = radius / 111000f;
            double u = random.NextDouble();
            double v = random.NextDouble();
            double w = radiusInDegrees * Math.Sqrt(u);
            double t = 2 * Math.PI * v;
            double x = w * Math.Cos(t);
            double y = w * Math.Sin(t);
            double new_x = x / Math.Cos(y0);
            double foundLongitude = new_x + x0;
            double foundLatitude = y + y0;
            geolocation.Latitude = foundLatitude.ToString().Replace(",", ".");
            geolocation.Longitude = foundLongitude.ToString().Replace(",", ".");
            return geolocation;
        }
    }
}
