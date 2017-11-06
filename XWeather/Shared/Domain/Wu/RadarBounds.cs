namespace XWeather
{
	public class RadarBounds
	{
		public double Width { get; set; }

		public double Height { get; set; }

		public double MinLat { get; set; }

		public double MaxLat { get; set; }

		public double MinLon { get; set; }

		public double MaxLon { get; set; }

		public override string ToString ()
		{
			return string.Format ("[RadarBounds: Width={0}, Height={1}, MinLat={2}, MaxLat={3}, MinLon={4}, MaxLon={5}]", Width, Height, MinLat, MaxLat, MinLon, MaxLon);
		}
	}
}