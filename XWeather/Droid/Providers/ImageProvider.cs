namespace XWeather.Droid
{
	public static class ImageProvider
	{
		public static int ResourceForString (string icon)
		{
			switch (icon) {
				case ConditionStrings.chanceflurries:
					return Resource.Drawable.chanceflurries;
				case ConditionStrings.chancerain:
					return Resource.Drawable.chancerain;
				case ConditionStrings.chancesleet:
					return Resource.Drawable.chancesleet;
				case ConditionStrings.chancesnow:
					return Resource.Drawable.chancesnow;
				case ConditionStrings.chancetstorms:
					return Resource.Drawable.chancetstorms;
				case ConditionStrings.clear:
					return Resource.Drawable.clear;
				case ConditionStrings.cloudy:
					return Resource.Drawable.cloudy;
				case ConditionStrings.flurries:
					return Resource.Drawable.flurries;
				case ConditionStrings.fog:
					return Resource.Drawable.fog;
				case ConditionStrings.hazy:
					return Resource.Drawable.hazy;
				case ConditionStrings.mostlycloudy:
					return Resource.Drawable.mostlycloudy;
				case ConditionStrings.mostlysunny:
					return Resource.Drawable.mostlysunny;
				case ConditionStrings.nt_chanceflurries:
					return Resource.Drawable.nt_chanceflurries;
				case ConditionStrings.nt_chancerain:
					return Resource.Drawable.nt_chancerain;
				case ConditionStrings.nt_chancesleet:
					return Resource.Drawable.nt_chancesleet;
				case ConditionStrings.nt_chancesnow:
					return Resource.Drawable.nt_chancesnow;
				case ConditionStrings.nt_chancetstorms:
					return Resource.Drawable.nt_chancetstorms;
				case ConditionStrings.nt_clear:
					return Resource.Drawable.nt_clear;
				case ConditionStrings.nt_cloudy:
					return Resource.Drawable.nt_cloudy;
				case ConditionStrings.nt_flurries:
					return Resource.Drawable.nt_flurries;
				case ConditionStrings.nt_fog:
					return Resource.Drawable.nt_fog;
				case ConditionStrings.nt_hazy:
					return Resource.Drawable.nt_hazy;
				case ConditionStrings.nt_mostlycloudy:
					return Resource.Drawable.nt_mostlycloudy;
				case ConditionStrings.nt_mostlysunny:
					return Resource.Drawable.nt_mostlysunny;
				case ConditionStrings.nt_partlycloudy:
					return Resource.Drawable.nt_partlycloudy;
				case ConditionStrings.nt_partlysunny:
					return Resource.Drawable.nt_partlysunny;
				case ConditionStrings.nt_rain:
					return Resource.Drawable.nt_rain;
				case ConditionStrings.nt_sleet:
					return Resource.Drawable.nt_sleet;
				case ConditionStrings.nt_snow:
					return Resource.Drawable.nt_snow;
				case ConditionStrings.nt_sunny:
					return Resource.Drawable.nt_sunny;
				case ConditionStrings.nt_tstorms:
					return Resource.Drawable.nt_tstorms;
				case ConditionStrings.partlycloudy:
					return Resource.Drawable.partlycloudy;
				case ConditionStrings.partlysunny:
					return Resource.Drawable.partlysunny;
				case ConditionStrings.rain:
					return Resource.Drawable.rain;
				case ConditionStrings.sleet:
					return Resource.Drawable.sleet;
				case ConditionStrings.snow:
					return Resource.Drawable.snow;
				case ConditionStrings.sunny:
					return Resource.Drawable.sunny;
				case ConditionStrings.tstorms:
					return Resource.Drawable.tstorms;
				default: return Resource.Drawable.unknown;
			}
		}
	}
}

