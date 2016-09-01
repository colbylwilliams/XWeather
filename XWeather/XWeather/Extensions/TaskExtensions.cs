using System.Threading.Tasks;

namespace XWeather
{
	public static class TaskExtensions
	{
		/* TaskStatus enum
         * Created, = 0
         * WaitingForActivation, = 1
         * WaitingToRun, = 2
         * Running, = 3
         * WaitingForChildrenToComplete, = 4
         * RanToCompletion, = 5
         * Canceled, = 6
         * Faulted = 7  */
		public static bool IsNullFinishCanceledOrFaulted<T> (this TaskCompletionSource<T> tcs)
		{
			return tcs == null || (int)tcs.Task.Status >= 5;
		}
	}
}