using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.API.POS.Helper
{
    public static class LocationHepler
    {
        public static double DistanceTo(double lat1, double lon1, double lat2, double lon2)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515 * 1000;
            return dist;
        }

        public static bool DistanceIsValid(double lat1, double lon1, double lat2, double lon2, double distance)
        {
            if ((lat1 == lat2) && (lon1 == lon2))
            {
                return true;
            }

            var distanceTo = DistanceTo(lat1, lon1, lat2, lon2);
            if (distance > distanceTo)
                return true;
            return false;
        }

        private static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private static double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}
