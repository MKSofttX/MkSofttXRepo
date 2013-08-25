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
                new RecyclingCenterLocation("Sra. Reyna María Arias Torres", 19.541612, -99.147879),
                new RecyclingCenterLocation("AMBI", 19.40268,-99.070771),
                new RecyclingCenterLocation("Sra. María Concepción Chávez", 19.342346,-99.03587),
                new RecyclingCenterLocation("Glass Internacional Recycling", 19.342245,-98.041992),
                new RecyclingCenterLocation("Wenceslao Alcalá", 19.342539,-99.052048),
                new RecyclingCenterLocation("Centro de acopio de pilas usadas",21.943046,-88.132324),
                new RecyclingCenterLocation("Ecolomóvil", 21.125498,-97.77832),
                new RecyclingCenterLocation("Coyoacán", 40.518715,-74.412095),
                new RecyclingCenterLocation("Edison",19.343744,-99.156188),
                new RecyclingCenterLocation("Centro De Acipio De Materiales", 25.744549,-100.17952),
                new RecyclingCenterLocation("Trans Vidrio S.A. de C.V.", 25.744549,-100.17952),
                new RecyclingCenterLocation("CORESA", 25.653635,-100.344787),
                new RecyclingCenterLocation("Fabricación de  papel San José S.A. de C.V.", 25.746714,-100.329895)
            };
        }
    }

    public class RecyclingCenterLocation
    {
        public RecyclingCenterLocation(string name, double x, double y)
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
        }

        public string Name { get; set; }
        public double X {get;set;}
        public double Y {get;set;}
    }
}
