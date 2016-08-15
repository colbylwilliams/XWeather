using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;

using Android.Support.Design.Widget;
using Android.Support.V4.View;


namespace XWeather.Droid
{
	[Register ("xweather.droid.ScrollAwareFABBehavior")]
	public class ScrollAwareFABBehavior : CoordinatorLayout.Behavior
	{

		public ScrollAwareFABBehavior () { }


		public ScrollAwareFABBehavior (Context context, IAttributeSet attrs) : base () { }


		public override bool OnStartNestedScroll (CoordinatorLayout coordinatorLayout, Java.Lang.Object child, View directTargetChild, View target, int nestedScrollAxes)
		{
			return nestedScrollAxes == ViewCompat.ScrollAxisVertical || base.OnStartNestedScroll (coordinatorLayout, child, directTargetChild, target, nestedScrollAxes);
		}


		public override void OnNestedScroll (CoordinatorLayout coordinatorLayout, Java.Lang.Object child, View target, int dxConsumed, int dyConsumed, int dxUnconsumed, int dyUnconsumed)
		{
			base.OnNestedScroll (coordinatorLayout, child, target, dxConsumed, dyConsumed, dxUnconsumed, dyUnconsumed);

			var button = child as FloatingActionButton;

			if (dyConsumed > 0 && button.Visibility == ViewStates.Visible) {

				button.Hide ();

			} else if (dyConsumed < 0 && button.Visibility != ViewStates.Visible) {

				button.Show ();
			}
		}
	}
}