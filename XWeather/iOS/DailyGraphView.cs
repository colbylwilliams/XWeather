using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using XWeather.Domain;
using XWeather.Clients;
using System.Linq;
using SettingsStudio;
using CoreText;
using Foundation;

namespace XWeather.iOS
{
	[Register ("DailyGraphView")]
	public class DailyGraphView : UIView
	{
		/* Logic
		 * - Get the highest high and the lowest low and add ~ 10 degrees (maybe 5 for celcius) on
		 *   both ends.
		 * - Get the width and height of the graph and devide accordingly for each coord
		 * - Draw both high and low graph lines along the x axis, then animate them up into place
		 */

		List<ForecastDay> Forecasts => WuClient.Shared?.Selected?.Forecasts;


		List<double> _highTemps;
		List<double> HighTemps => _highTemps ?? (_highTemps = Forecasts.Select (f => f.HighTemp (Settings.UomTemperature)).ToList ());


		List<double> _lowTemps;
		List<double> LowTemps => _lowTemps ?? (_lowTemps = Forecasts.Select (f => f.LowTemp (Settings.UomTemperature)).ToList ());


		nfloat inset = 12;

		nfloat padding = 28;

		nfloat lineWidth = 4;

		int dividerCount = 8;

		int offset = 2;


		public DailyGraphView () { }

		public DailyGraphView (IntPtr handle) : base (handle) { }


		public override void Draw (CGRect rect)
		{
			base.Draw (rect);

			var graphRect = new CGRect (rect.X + padding, rect.Y + padding, rect.Width - (padding * 2), rect.Height - (padding * 2));

			var highest = (nfloat)HighTemps.Max ();
			var lowest = (nfloat)LowTemps.Min ();

			var scaleHigh = NMath.Round (highest, MidpointRounding.AwayFromZero);
			var scaleLow = lowest < 0 ? NMath.Round (lowest, MidpointRounding.AwayFromZero) : NMath.Round (lowest);

			offset = Settings.UomTemperature.IsImperial () ? offset : (offset / 2);

			scaleHigh += offset;
			scaleLow -= offset;

			var scaleRange = scaleHigh - scaleLow;

			var scaleIncrement = scaleRange / dividerCount;

			var scaleX = graphRect.Width / Forecasts.Count;
			var scaleY = graphRect.Height / dividerCount;


			var dot = new CGRect (0, 0, lineWidth, lineWidth);

			nfloat x, y;

			CGPoint start = CGPoint.Empty;
			CGPoint end = CGPoint.Empty;

			var dividerHeight = graphRect.Height / dividerCount;

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

						y = (i * scaleY) + dividerHeight;

						color.SetFill ();
						ctx.FillRect (new CGRect (graphRect.GetMinX (), graphRect.GetMaxY () - y, graphRect.Width, dividerHeight));
						ctx.StrokePath ();
					}
				}


				// Draw curved graph line
				using (UIColor color = UIColor.White.ColorWithAlpha (0.25f), dotColor = UIColor.White.ColorWithAlpha (0.70f)) {

					for (int i = 0; i < (Forecasts.Count * 2); i++) {

						if (i == Forecasts.Count) {

							// last dot on high temps
							dot.X = start.X - (lineWidth / 2);
							dot.Y = start.Y - (lineWidth / 2);

							using (CGPath p = new CGPath ()) {

								dotColor.SetFill ();

								ctx.AddEllipseInRect (dot);
								ctx.DrawPath (CGPathDrawingMode.Fill);
							}

							start = CGPoint.Empty;
						}

						var highs = i < Forecasts.Count;

						var ai = highs ? i : i - Forecasts.Count;

						var temp = highs ? HighTemps [ai] : LowTemps [ai];

						x = padding + inset + (ai * scaleX);

						nfloat percent = ((nfloat)temp - scaleLow) / scaleRange;

						y = graphRect.Bottom - (graphRect.Height * percent);

						end = new CGPoint (x, y);

						dot.X = start.X - (lineWidth / 2);
						dot.Y = start.Y - (lineWidth / 2);

						using (CGPath p = new CGPath ()) {

							if (start == CGPoint.Empty) {

								p.MoveToPoint (end);

							} else {

								color.SetStroke ();
								ctx.SetLineWidth (lineWidth);

								p.MoveToPoint (start);

								nfloat diff = (end.X - start.X) / 2;

								p.AddCurveToPoint (end.X - diff, start.Y, start.X + diff, end.Y, end.X, end.Y);

								ctx.AddPath (p);
								ctx.DrawPath (CGPathDrawingMode.Stroke);

								dotColor.SetFill ();

								ctx.AddEllipseInRect (dot);
								ctx.DrawPath (CGPathDrawingMode.Fill);
							}
						}

						start = end;
					}

					// last dot on low temps
					dot.X = start.X - (lineWidth / 2);
					dot.Y = start.Y - (lineWidth / 2);


					using (CGPath p = new CGPath ()) {

						dotColor.SetFill ();

						ctx.AddEllipseInRect (dot);
						ctx.DrawPath (CGPathDrawingMode.Fill);
					}


					for (int i = 0; i < Forecasts.Count; i++) {

					}
				}

				// Draw y-axis labels

				nfloat yStep = scaleLow;

				ctx.SaveState ();
				ctx.TranslateCTM (0, rect.Height);
				ctx.ScaleCTM (1, -1);


				for (int i = 0; i <= dividerCount; i++) {

					y = padding + (i * scaleY);

					var step = NMath.Round (yStep).ToString ();

					DrawCgLabel (ctx, rect, y, padding - 6, UITextAlignment.Right, step, false, true);

					yStep += scaleIncrement;
				}


				// Draw x-axis labels
				for (int i = 0; i < Forecasts.Count; i++) {

					x = padding + inset + (i * scaleX);

					DrawCgLabel (ctx, rect, padding - 6, x, UITextAlignment.Center, Forecasts [i]?.date?.weekday_short, false);
				}

				ctx.RestoreState ();
			}
		}


		void DrawCgLabel (CGContext ctx, CGRect rect, nfloat yCoord, nfloat xCoord, UITextAlignment alignment, string label, bool flipContext = true, bool centerVertical = false)
		{
			// Draw light the sunrise and Sunset labels at the ends of the light box
			using (UIColor fontColor = UIColor.White, shadowColor = UIColor.Black.ColorWithAlpha (0.1f)) {

				var fontAttributes = new UIFontAttributes (new UIFontFeature (CTFontFeatureNumberSpacing.Selector.ProportionalNumbers));

				using (var desc = UIFont.SystemFontOfSize (11f).FontDescriptor.CreateWithAttributes (fontAttributes)) {

					using (UIFont font = UIFont.FromDescriptor (desc, 11f)) {

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
								//measurementAttrString = new NSMutableAttributedString (new string ('5', label.Length), stringAttributes)) {

								//creating a container for out attributed string 
								using (CTFramesetter framesetter = new CTFramesetter (attrString)) {
									//measurementFramesetter = new CTFramesetter (measurementAttrString)) {

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