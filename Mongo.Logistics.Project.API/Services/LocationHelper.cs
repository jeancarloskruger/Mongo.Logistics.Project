using System.Globalization;

namespace Mongo.Logistics.Project.API.Services
{
    public static class LocationHelper
    {
        public static double[] GetLocation(this string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                return null;
            }
            if (location.Split(",").Length != 2)
            {
                return null;
            }

            if (double.TryParse(location.Split(",")[0], out _) && double.TryParse(location.Split(",")[1], out _))
            {
                return new double[] { double.Parse(location.Split(",")[0], CultureInfo.InvariantCulture), double.Parse(location.Split(",")[1], CultureInfo.InvariantCulture) };
            }
            return null;
        }
    }
}
