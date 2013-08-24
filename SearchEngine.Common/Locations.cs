using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Common
{
    public class Locations
    {
        public static List<RecyclingCenterLocation> GetRecyclingCenterLocations()
        {
            return new List<RecyclingCenterLocation>()
            {
                new RecyclingCenterLocation(19.541612, -99.147879),
                new RecyclingCenterLocation(19.40268,-99.070771),
            };
        }
    }

    public class RecyclingCenterLocation
    {
        public RecyclingCenterLocation(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double X {get;set;}
        public double Y {get;set;}
    }
}
