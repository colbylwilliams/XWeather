using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ServiceStack;

using XWeather.Domain;
using XWeather.Constants;

namespace XWeather.Clients
{
	public class WuClient
	{
		static WuClient _shared;

		public static WuClient Shared => _shared ?? (_shared = new WuClient ());


		JsonServiceClient _client;
		JsonServiceClient client => _client ?? (_client = new JsonServiceClient ());


		public event EventHandler LocationAdded;
		public event EventHandler LocationRemoved;
		public event EventHandler UpdatedSelected;


		WuLocation _selected;
		public WuLocation Selected {
			get { return _selected; }
			set {

				foreach (var location in Locations) location.Selected = false;

				_selected = value;

				_selected.Selected = true;

				UpdatedSelected?.Invoke (this, EventArgs.Empty);

				SettingsStudio.Settings.LocationsJson = Locations.GetLocationsJson ();
			}
		}


		public List<WuLocation> Locations { get; set; } = new List<WuLocation> ();

		public bool HasCurrent => Selected != null;


		public async Task AddLocation (string countryOrUsState, string city)
		{
			LocationAdded?.Invoke (this, EventArgs.Empty);

			var wuAcLocation = await lookupLocation (countryOrUsState, city);

			var wuLocation = await getWuLocation (wuAcLocation);

			AddLocation (wuLocation);
		}


		public async Task AddLocation (LocationCoordinates coordnates)
		{
			LocationAdded?.Invoke (this, EventArgs.Empty);

			var wuAcLocation = await lookupLocation (coordnates);

			var wuLocation = await getWuLocation (wuAcLocation);

			AddLocation (wuLocation);
		}


		public async Task AddLocation (WuAcLocation location)
		{
			LocationAdded?.Invoke (this, EventArgs.Empty);

			var wuLocation = await getWuLocation (location);

			AddLocation (wuLocation);
		}


		public void AddLocation (WuLocation location, bool preventNotification = false)
		{
			Locations.Add (location);

			Locations.Sort ();

			if (!preventNotification)
			{
				LocationAdded?.Invoke (this, EventArgs.Empty);

				SettingsStudio.Settings.LocationsJson = Locations.GetLocationsJson ();
			}
		}


		public void RemoveLocation (WuLocation location)
		{
			Locations.Remove (location);

			Locations.Sort ();

			LocationRemoved?.Invoke (this, EventArgs.Empty);

			SettingsStudio.Settings.LocationsJson = Locations.GetLocationsJson ();
		}


		async Task<WuAcLocation> lookupLocation (string countryOrUsState, string city)
		{
			if (!string.IsNullOrEmpty (city) && !string.IsNullOrEmpty (countryOrUsState))
			{
				var cityName = city.Replace (' ', '_');

				try
				{
					var location = await GetAsync<GeoLookup> ($"/q/{countryOrUsState}/{cityName}");

					return location?.ToWuAcLocation ();
				}
				catch (Exception)
				{
					return null;
				}
			}

			return null;
		}


		async Task<WuAcLocation> lookupLocation (LocationCoordinates coordnates)
		{
			var location = coordnates != null ? await GetAsync<GeoLookup> ($"/q/{coordnates.Latitude},{coordnates.Longitude}") : null;

			return location?.ToWuAcLocation ();
		}


		// "entity": "Paris, France"
		// "entity": "Fort Thomas, KY"
		public Task<WuLocation> GetLocation (string countryOrUsStateAndCity)
		{
			var locationArray = countryOrUsStateAndCity.Split (new [] { ", " }, StringSplitOptions.None); // [ "Fort Thomas", "KY" ]

			if (locationArray?.Length == 2)
			{
				var city = locationArray [0]; // "Fort Thomas"
				var countryOrUsState = locationArray [1]; // "KY"

				return GetLocation (countryOrUsState, city);
			}

			return null;
		}


		public async Task<WuLocation> GetLocation (string countryOrUsState, string city)
		{
			var wuAcLocation = await lookupLocation (countryOrUsState, city);

			return await getWuLocation (wuAcLocation);
		}


		public Task GetLocations (LocationCoordinates coordnates)
		{
			return getLocationsFromSettingsStore (coordnates);
		}


		async Task getLocationsFromSettingsStore (LocationCoordinates coordnates)
		{
			var locations = SettingsStudio.Settings.LocationsJson.GetLocations ();

			var oldCurrent = locations.FirstOrDefault (l => l.Current);

			if (oldCurrent != null) locations.Remove (oldCurrent);


			var newCurrent = await lookupLocation (coordnates);

			if (newCurrent != null)
			{
				locations.Add (newCurrent);

				// if the previous current was selected, or theres not one selected, select this one
				newCurrent.Selected |= oldCurrent?.Selected ?? false || !locations.Any (l => l.Selected);
			}

			await getLocations (locations);
		}


		async Task getLocations (List<WuAcLocation> locations)
		{
			// if there's none selected, assume the old current was selected and select this one
			var selected = locations.FirstOrDefault (l => l.Selected) ?? locations.FirstOrDefault (l => l.Current) ?? locations.FirstOrDefault ();

			Selected = await getWuLocation (selected);

			Locations = new List<WuLocation> { Selected };


			var tasks = locations.Where (l => l != selected).Select (l => AddLocation (l)).ToArray ();

			await Task.WhenAll (tasks);
		}


		async Task<WuLocation> getWuLocation (WuAcLocation acLocation)
		{
			var location = new WuLocation (acLocation);

			location.Weather = await GetAsync<WuWeather> (acLocation.l);

			//System.Diagnostics.Debug.WriteLine (location.Weather.ToJson ());

			location.Updated = DateTime.UtcNow;

			return location;
		}


		public Task<T> GetAsync<T> (string location)
			where T : WuObject, new()
		{
			try
			{
				var url = ApiKeys.WuApiKeyedQueryJsonFmt.Fmt (new T ().WuKey, location);

				//System.Diagnostics.Debug.WriteLine (url);

				return client.GetAsync<T> (url);
			}
			catch (WebServiceException webEx)
			{
				System.Diagnostics.Debug.WriteLine ($"Exception processing Weather Underground request for {typeof (T).Name}\n{webEx.Message}");
				throw;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine ($"Exception processing Weather Underground request for {typeof (T).Name}\n{ex.Message}");
				throw;
			}
		}


		public Task<byte []> GetRadarImageAsync (RadarBounds bounds)
		{
			try
			{
				var query = $"image.gif?maxlat={bounds.MaxLat}&maxlon={bounds.MaxLon}&minlat={bounds.MinLat}&minlon={bounds.MinLon}&width={bounds.Width}&height={bounds.Height}&rainsnow={1}&num={6}&delay={25}";

				var url = ApiKeys.WuApiKeyedQueryFmt.Fmt ("animatedradar", query);

				//System.Diagnostics.Debug.WriteLine (url);

				return client.GetAsync<byte []> (url);
			}
			catch (WebServiceException webEx)
			{
				System.Diagnostics.Debug.WriteLine ($"Exception processing Weather Underground request for Radar Image\n{webEx.Message}");
				throw;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine ($"Exception processing Weather Underground request for Radar Image\n{ex.Message}");
				throw;
			}
		}
	}
}