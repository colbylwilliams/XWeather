using System;
using System.Collections.Generic;
using System.Linq;

using CoreAnimation;
using CoreGraphics;
using CoreText;
using Foundation;
using UIKit;

using SettingsStudio;

using XWeather.Clients;
using XWeather.Domain;

namespace XWeather.iOS
{
	[Register ("DailyGraphView")]
	public class DailyGraphView : UIView
	{
		List<ForecastDay> Forecasts => WuClient.Shared?.Selected?.Forecasts ?? new List<ForecastDay> ();

		List<HourlyForecast> Hourly => WuClient.Shared?.Selected?.HourlyForecasts ?? new List<HourlyForecast> ();


		List<double> _highTemps;
		List<double> HighTemps => _highTemps ?? (_highTemps = Forecasts.Select (f => f.HighTemp (Settings.UomTemperature)).ToList ());


		List<double> _lowTemps;
		List<double> LowTemps => _lowTemps ?? (_lowTemps = Forecasts.Select (f => f.LowTemp (Settings.UomTemperature)).ToList ());


		List<double> _hourlyTemps;
		List<double> HourlyTemps => _hourlyTemps ?? (_hourlyTemps = Hourly.Select (h => h.Temp (Settings.UomTemperature)).ToList ());


		nfloat padding = 28;

		nfloat lineWidth = 2;

		nfloat fontSize = 11;


		int dividerCount = 8;

		int scalePadding = 4;


		bool hourly => !Settings.HighLowGraph;


		CAShapeLayer _layer;
		CAShapeLayer layer => _layer ?? (_layer = new CAShapeLayer { Frame = Bounds });


		CGRect graphRect;

		nfloat scaleHigh, scaleLow, scaleRange, scaleX, scaleY, inset;


		public DailyGraphView () { }

		public DailyGraphView (IntPtr handle) : base (handle) { }


		UITapGestureRecognizer tapGesture = new UITapGestureRecognizer (t => { Settings.HighLowGraph = !Settings.HighLowGraph; t.View.SetNeedsDisplay (); });


		public override void WillMoveToSuperview (UIView newsuper)
		{
			base.WillMoveToSuperview (newsuper);

			if (newsuper != null) {
				AddGestureRecognizer (tapGesture);
			} else {
				RemoveGestureRecognizer (tapGesture);
			}
		}


		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			if (Layer?.Sublayers == null || !Layer.Sublayers.Contains (layer)) {

				Layer.AddSublayer (layer);
			}
		}


		public override void Draw (CGRect rect)
		{
			base.Draw (rect);


			_highTemps = null;
			_lowTemps = null;

			_hourlyTemps = null;


			if (Forecasts.Count == 0 || Hourly.Count == 0) return;


			graphRect = new CGRect (rect.X + padding, rect.Y + padding, rect.Width - (padding * 2), rect.Height - (padding * 2));


			var days = Hourly.GroupBy (h => h.FCTTIME.mday).Select (g => g.First ().FCTTIME.weekday_name_abbrev).ToList ();

			var dayCount = hourly ? days.Count : Forecasts.Count;


			var xAxisScale = (graphRect.Width + padding / 2) / dayCount;

			inset = xAxisScale / 2;


			var highest = (nfloat)(hourly ? HourlyTemps.Max () : HighTemps.Max ());
			var lowest = (nfloat)(hourly ? HourlyTemps.Min () : LowTemps.Min ());


			scaleHigh = NMath.Round (highest, MidpointRounding.AwayFromZero);
			scaleLow = lowest < 0 ? NMath.Round (lowest, MidpointRounding.AwayFromZero) : NMath.Round (lowest);

			var rangePadding = Settings.UomTemperature.IsImperial () ? scalePadding : (scalePadding / 2);


			scaleHigh += rangePadding;
			scaleLow -= rangePadding;

			scaleRange = scaleHigh - scaleLow;


			var scaleIncrement = scaleRange / dividerCount;

			scaleX = (graphRect.Width - inset) / (hourly ? HourlyTemps.Count : Forecasts.Count);
			scaleY = graphRect.Height / dividerCount;


			nfloat x, y;


			using (CGContext ctx = UIGraphics.GetCurrentContext ()) {

				// Draw x and y axis
				using (UIColor color = UIColor.White) {

					color.SetStroke ();
					ctx.SetLineWidth (1);

					using (CGPath p = new CGPath ()) {

						p.MoveToPoint (graphRect.GetMinX (), graphRect.GetMaxY ());

						p.AddLines (new [] {
							new CGPoint (graphRect.GetMinX (), graphRect.GetMinY ()),
							new CGPoint (graphRect.GetMinX (), graphRect.GetMaxY ()),
							new CGPoint (graphRect.GetMaxX (), graphRect.GetMaxY ())
						});

						ctx.AddPath (p);
						ctx.DrawPath (CGPathDrawingMode.Stroke);
					}
				}


				// Draw horizontal gridlines
				using (UIColor color = UIColor.Black.ColorWithAlpha (0.08f)) {

					for (int i = 1; i < dividerCount; i += 2) {

						y = (i + 1) * scaleY;

						color.SetFill ();
						ctx.FillRect (new CGRect (graphRect.GetMinX (), graphRect.GetMaxY () - y, graphRect.Width, scaleY));
						ctx.StrokePath ();
					}
				}



				drawLines ();


				// Draw y-axis labels

				nfloat yStep = scaleLow;

				ctx.SaveState ();
				ctx.TranslateCTM (0, rect.Height);
				ctx.ScaleCTM (1, -1);


				for (int i = 0; i <= dividerCount; i++) {

					y = padding + (i * scaleY);

					var step = NMath.Round (yStep).ToString ();

					drawLabel (ctx, rect, y, padding - 6, UITextAlignment.Right, step, false, true);

					yStep += scaleIncrement;
				}


				// Draw x-axis labels
				for (int i = 0; i < dayCount; i++) {

					x = padding + (i * xAxisScale);

					drawLabel (ctx, rect, padding - 6, x, UITextAlignment.Left, hourly ? days [i] : Forecasts [i]?.date?.weekday_short, false);
				}

				ctx.RestoreState ();
			}

		}


		void drawLines ()
		{
			layer.RemoveAllAnimations ();

			var dot = new CGRect (0, 0, lineWidth, lineWidth);

			nfloat x, y;

			CGPoint start = CGPoint.Empty;
			CGPoint end = CGPoint.Empty;


			// Draw curved graph line
			using (UIColor color = UIColor.White.ColorWithAlpha (0.25f), dotColor = UIColor.White.ColorWithAlpha (0.70f)) {

				//color.SetStroke ();

				//dotColor.SetFill ();

				//ctx.SetLineWidth (lineWidth);

				using (CGPath path = new CGPath ()) {


					var count = hourly ? HourlyTemps.Count : (Forecasts.Count * 2);

					for (int i = 0; i < count; i++) {

						// adjusted index
						var ai = i;

						double temp;

						if (hourly) {

							temp = HourlyTemps [ai];

						} else {

							// reset start when switching from highs to lows
							if (i == Forecasts.Count) start = CGPoint.Empty;

							var highs = i < Forecasts.Count;

							ai = highs ? i : i - Forecasts.Count;

							temp = highs ? HighTemps [ai] : LowTemps [ai];
						}

						var percent = ((nfloat)temp - scaleLow) / scaleRange;


						x = padding + inset + (ai * scaleX);

						y = graphRect.GetMaxY () - (graphRect.Height * percent);

						end = new CGPoint (x, y);


						if (!hourly) {

							dot.X = end.X - (lineWidth / 2);
							dot.Y = end.Y - (lineWidth / 2);

							path.AddEllipseInRect (dot);

							//ctx.AddEllipseInRect (dot);
						}


						if (start == CGPoint.Empty) {

							path.MoveToPoint (end);

						} else {

							path.MoveToPoint (start);

							if (hourly) {

								path.AddLineToPoint (end);

							} else {

								var diff = (end.X - start.X) / 2;

								path.AddCurveToPoint (end.X - diff, start.Y, start.X + diff, end.Y, end.X, end.Y);
							}
						}

						start = end;
					}

					// draw all dots to context
					//if (!hourly) ctx.DrawPath (CGPathDrawingMode.Fill);

					// add line path to context
					layer.Path = path;
					//ctx.AddPath (path);

					// draw lines
					//ctx.DrawPath (CGPathDrawingMode.Stroke);

					layer.LineWidth = lineWidth;
					layer.StrokeColor = color.CGColor;
					layer.FillColor = dotColor.CGColor;

					CABasicAnimation pathAnimation = new CABasicAnimation { KeyPath = "strokeEnd" };
					pathAnimation.Duration = 1.0;
					pathAnimation.From = NSNumber.FromNFloat (0);
					pathAnimation.To = NSNumber.FromNFloat (1);
					layer.AddAnimation (pathAnimation, "strokeEndAnimation");

				}
			}
		}


		void drawLabel (CGContext ctx, CGRect rect, nfloat yCoord, nfloat xCoord, UITextAlignment alignment, string label, bool flipContext = true, bool centerVertical = false)
		{
			// Draw light the sunrise and Sunset labels at the ends of the light box
			using (UIColor fontColor = UIColor.White, shadowColor = UIColor.Black.ColorWithAlpha (0.1f)) {

				var fontAttributes = new UIFontAttributes (new UIFontFeature (CTFontFeatureNumberSpacing.Selector.ProportionalNumbers));

				using (var desc = UIFont.SystemFontOfSize (fontSize).FontDescriptor.CreateWithAttributes (fontAttributes)) {

					using (UIFont font = UIFont.FromDescriptor (desc, fontSize)) {

						// calculating the range of our attributed string
						var range = new NSRange (0, label.Length);

						// set justification for text block
						using (var alignStyle = new NSMutableParagraphStyle { Alignment = alignment }) {

							// add stylistic attributes to out attributed string
							var stringAttributes = new UIStringAttributes {
								ForegroundColor = fontColor,
								Font = font,
								ParagraphStyle = alignStyle
							};

							var target = new CGSize (float.MaxValue, float.MaxValue);

							NSRange fit;

							using (NSMutableAttributedString attrString = new NSMutableAttributedString (label, stringAttributes)) {

								//creating a container for out attributed string 
								using (CTFramesetter framesetter = new CTFramesetter (attrString)) {


									CGSize frameSize = framesetter.SuggestFrameSize (range, null, target, out fit);


									if (alignment == UITextAlignment.Center) xCoord -= (frameSize.Width / 2);

									if (alignment == UITextAlignment.Right) xCoord -= frameSize.Width;


									// subtract the frameSize so the flipped context behaves as expected
									yCoord -= frameSize.Height;

									if (centerVertical) yCoord += (frameSize.Height / 2);


									var textRect = new CGRect (xCoord, yCoord, frameSize.Width, frameSize.Height);

									using (CGPath path = new CGPath ()) {

										path.AddRect (textRect);

										ctx.SetShadow (new CGSize (0.0f, 1.0f), 0.0f, shadowColor.CGColor);

										using (CTFrame frame = framesetter.GetFrame (range, path, null)) {

											if (flipContext) {

												ctx.SaveState ();
												ctx.TranslateCTM (0, rect.Height);
												ctx.ScaleCTM (1, -1);

												frame.Draw (ctx);

												ctx.RestoreState ();

											} else {

												frame.Draw (ctx);
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
}