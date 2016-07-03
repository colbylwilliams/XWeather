using System;

using Foundation;
using UIKit;

using XWeather.Domain;
using System.Collections.Generic;

namespace XWeather.iOS
{
	public partial class DetailsTvc : BaseTvc<DetailsTvCell>
	{

		List<ForecastDay> Forecasts => Location?.Forecasts;
		CurrentObservation Conditions => Location?.Conditions;


		public DetailsTvc (IntPtr handle) : base (handle) { }


		public override nint RowsInSection (UITableView tableView, nint section) => Labels.Count;


		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = DequeCell (tableView, indexPath);

			var label = Labels [indexPath.Row];

			cell.SetData (label, getDetail (indexPath.Row));

			return cell;
		}


		public override UIView GetViewForHeader (UITableView tableView, nint section)
		{
			tableHeader.SetData (Location);
			return tableHeader;
		}


		static List<string> Labels = new List<string> {
			"Sunrise",
			"Sunset",
			"Chance of Rain",
			"Humidity",
			"Wind",
			"Gust",
			"Feels Like",
			"Precipitation",
			"Pressure",
			"Visibility",
			"UV Index"
		};


		string getDetail (int row)
		{
			if (Forecasts == null || Conditions == null) return string.Empty;

			switch (row) {
				case 0:
					return Location.Sunrise.LocalDateTime.ToShortTimeString ();
				case 1:
					return Location.Sunset.LocalDateTime.ToShortTimeString ();
				case 2:
					return $"{Forecasts [0].pop}%";
				case 3:
					return Conditions.relative_humidity;
				case 4:
					return $"{Conditions.wind_dir} {Conditions.wind_mph} mph";
				case 5:
					return $"{Conditions.wind_dir} {Conditions.wind_gust_mph} mph";
				case 6:
					return Conditions.feelslike_f.WithDegreeSymbol ();
				case 7:
					return $"{Conditions.precip_today_in} in";
				case 8:
					return $"{Conditions.pressure_in} inHg";
				case 9:
					return $"{Conditions.visibility_mi} mi";
				case 10:
					return Conditions.UV.ToString ();
				default:
					return string.Empty;
			}
		}
	}
}