using System;
using System.Collections.Generic;
using System.Linq;

using Android.Content;
using Android.Graphics;
using Android.Text;
using Android.Util;
using Android.Views;

using SettingsStudio;

using XWeather.Clients;
using XWeather.Domain;

namespace XWeather.Droid
{
	public class DailyGraphView : View
	{
		List<ForecastDay> Forecasts => WuClient.Shared?.Selected?.Forecasts ?? new List<ForecastDay> ();

		List<HourlyForecast> Hourly => WuClient.Shared?.Selected?.HourlyForecasts ?? new List<HourlyForecast> ();


		List<double> _highTemps;
		List<double> HighTemps => _highTemps ?? (_highTemps = Forecasts.Select (f => f.HighTemp (Settings.UomTemperature)).ToList ());


		List<double> _lowTemps;
		List<double> LowTemps => _lowTemps ?? (_lowTemps = Forecasts.Select (f => f.LowTemp (Settings.UomTemperature)).ToList ());


		List<double> _hourlyTemps;
		List<double> HourlyTemps => _hourlyTemps ?? (_hourlyTemps = Hourly.Select (h => h.Temp (Settings.UomTemperature)).ToList ());


		float padding = 30;

		float lineWidth = 2;

		float fontSize = 11;


		int dividerCount = 8;

		int scalePadding = 4;


		bool hourly => !Settings.HighLowGraph;

		RectF graphRect;

		float scaleHigh, scaleLow, scaleRange, scaleX, scaleY, inset, density;


		public DailyGraphView (Context context) : base (context) { }

		public DailyGraphView (Context context, IAttributeSet attrs) : base (context, attrs) { }

		public DailyGraphView (Context context, IAttributeSet attrs, int defStyleAttr) : base (context, attrs, defStyleAttr) { }


		bool scaled;

		public override void Draw (Canvas canvas)
		{
			base.Draw (canvas);

			if (!scaled) {

				density = Resources.DisplayMetrics.Density;

				padding *= density;

				lineWidth *= density;

				fontSize *= density;

				scaled = true;
			}


			_highTemps = null;
			_lowTemps = null;

			_hourlyTemps = null;


			if (Forecasts.Count == 0 || Hourly.Count == 0) return;


			graphRect = new RectF (padding, padding, canvas.Width - padding, canvas.Height - padding);// CGRect (rect.X + padding, rect.Y + padding, rect.Width - (padding * 2), rect.Height - (padding * 2));

			var days = Hourly.GroupBy (h => h.FCTTIME.mday).Select (g => g.First ().FCTTIME.weekday_name_abbrev).ToList ();

			var dayCount = hourly ? days.Count : Forecasts.Count;


			var xAxisScale = (graphRect.Width () + padding / 2) / dayCount;

			inset = xAxisScale / 2;


			var highest = (float)(hourly ? HourlyTemps.Max () : HighTemps.Max ());
			var lowest = (float)(hourly ? HourlyTemps.Min () : LowTemps.Min ());


			scaleHigh = (float)Math.Round (highest, MidpointRounding.AwayFromZero);
			scaleLow = lowest < 0 ? (float)Math.Round (lowest, MidpointRounding.AwayFromZero) : (float)Math.Round (lowest);

			var rangePadding = Settings.UomTemperature.IsImperial () ? scalePadding : (scalePadding / 2);


			scaleHigh += rangePadding;
			scaleLow -= rangePadding;

			scaleRange = scaleHigh - scaleLow;


			var scaleIncrement = scaleRange / dividerCount;

			scaleX = (graphRect.Width () - inset) / (hourly ? HourlyTemps.Count : Forecasts.Count);
			scaleY = graphRect.Height () / dividerCount;


			float x, y;

			var white = Color.White;


			// Draw x and y axis
			var paint = new Paint ();
			paint.Color = white;
			paint.StrokeWidth = 1;

			using (Path path = new Path ()) {

				path.MoveTo (graphRect.Left, graphRect.Top);

				path.LineTo (graphRect.Left, graphRect.Bottom);
				path.LineTo (graphRect.Right, graphRect.Bottom);

				paint.SetStyle (Paint.Style.Stroke);

				canvas.DrawPath (path, paint);
			}


			Color translucentBlack = Color.Argb (20, 0, 0, 0);

			paint.Color = translucentBlack;

			paint.SetStyle (Paint.Style.Fill);

			// Draw horizontal gridlines
			for (int i = 1; i < dividerCount; i += 2) {

				y = graphRect.Bottom - ((i + 1) * scaleY);

				using (RectF grid = new RectF (padding, y, graphRect.Right, y + scaleY))
					canvas.DrawRect (grid, paint);
			}


			drawLines (canvas, paint);

			float labelPadding = 6 * density;

			float yStep = scaleLow;

			// Draw y-axis labels
			for (int i = dividerCount; i >= 0; i--) {

				y = padding + (i * scaleY);

				var step = Math.Round (yStep).ToString ();

				drawLabel (canvas, graphRect.Left - labelPadding, y, Paint.Align.Right, step, true);

				yStep += scaleIncrement;
			}


			// Draw x-axis labels
			for (int i = 0; i < dayCount; i++) {

				x = padding + (i * xAxisScale);

				drawLabel (canvas, x, graphRect.Bottom + labelPadding, Paint.Align.Left, hourly ? days [i] : Forecasts [i]?.date?.weekday_short);
			}
		}


		void drawLines (Canvas canvas, Paint paint)
		{
			float x, y;

			// Draw curved graph line
			var whiteAlpha25 = Color.Argb (64, 255, 255, 255);
			//var whiteAlpha70 = Color.Argb (64, 255, 255, 255);

			var start = new PointF ();
			var end = new PointF ();

			using (Path path = new Path ()) {


				var count = hourly ? HourlyTemps.Count : (Forecasts.Count * 2);

				for (int i = 0; i < count; i++) {

					// adjusted index
					var ai = i;

					double temp;

					if (hourly) {

						temp = HourlyTemps [ai];

					} else {

						// reset start when switching from highs to lows
						if (i == Forecasts.Count) start = new PointF ();

						var highs = i < Forecasts.Count;

						ai = highs ? i : i - Forecasts.Count;

						temp = highs ? HighTemps [ai] : LowTemps [ai];
					}

					var percent = ((float)temp - scaleLow) / scaleRange;


					x = padding + inset + (ai * scaleX);

					y = graphRect.Bottom - (graphRect.Height () * percent);


					end = new PointF (x, y);


					if (!hourly) {

						path.AddCircle (end.X - lineWidth, end.Y - lineWidth, lineWidth / 2, Path.Direction.Cw);
					}


					if (start.IsEmpty ()) {

						path.MoveTo (end);

					} else {

						path.MoveTo (start);

						if (hourly) {

							path.LineTo (end);

						} else {

							var diff = (end.X - start.X) / 2;

							path.CubicTo (end.X - diff, start.Y, start.X + diff, end.Y, end.X, end.Y);
						}
					}

					start = end;
				}

				paint.Color = whiteAlpha25;
				paint.StrokeWidth = lineWidth;

				paint.SetStyle (Paint.Style.Stroke);

				canvas.DrawPath (path, paint);
			}
		}


		void drawLabel (Canvas canvas, float xCoord, float yCoord, Paint.Align alignment, string label, bool centerVertical = false)
		{
			var fontColor = Color.White;

			var textPaint = new TextPaint ();

			textPaint.Color = fontColor;

			textPaint.TextSize = fontSize;

			textPaint.TextAlign = alignment;

			textPaint.SetStyle (Paint.Style.Stroke);


			var frame = new Rect ();

			textPaint.GetTextBounds (label, 0, label.Length, frame);

			if (centerVertical) {

				yCoord -= frame.CenterY ();

			} else {

				yCoord += frame.Height ();
			}

			canvas.DrawText (label, xCoord, yCoord, textPaint);
		}
	}
}