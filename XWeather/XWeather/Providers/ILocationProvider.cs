using System.Threading.Tasks;

using XWeather.Domain;

namespace XWeather
{
	public interface ILocationProvider
	{
		Task<LocationCoordinates> GetCurrentLocationCoordnatesAsync ();
	}
}