using Foundation;

using XWeather;

namespace SettingsStudio
{
	public static partial class Settings
	{
		public static void SetUomDefaults (bool firstLaunch)
		{
			if (!DidSetUomDefaults && firstLaunch)
			{
				var local = NSLocale.CurrentLocale;
				var isMetric = local?.UsesMetricSystem ?? true;

				UomTemperature = isMetric ? TemperatureUnits.Celsius : TemperatureUnits.Fahrenheit;
				UomDistance = isMetric ? DistanceUnits.Kilometers : DistanceUnits.Miles;
				UomPressure = isMetric ? PressureUnits.Millibars : PressureUnits.InchesOfMercury;
				UomLength = isMetric ? LengthUnits.Millimeters : LengthUnits.Inches;
				UomSpeed = isMetric ? SpeedUnits.KilometersPerHour : SpeedUnits.MilesPerHour;
			}

			DidSetUomDefaults = true;
		}
	}
}
