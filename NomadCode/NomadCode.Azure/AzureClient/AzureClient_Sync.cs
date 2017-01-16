#if __MOBILE__ && OFFLINE_SYNC_ENABLED

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace NomadCode.Azure
{
	public partial class AzureClient // Pull
	{

		bool performingFullRefresh;

		void sync<T> (IMobileServiceSyncTable<T> table, Expression<Func<T, bool>> where = null)
			where T : AzureEntity, new()
		{
			// we want to fire and forget
			Task.Run (async () =>
			{
				await syncAsync (table, where);

				try
				{
					// update the registrants and sponsor databases around every hour
					if (!performingFullRefresh && shouldRefresh)
					{
						performingFullRefresh = true;

						await getAsync (table, null);

						lastRefresh = DateTime.UtcNow;

						performingFullRefresh = false;
					}
				}
				catch (Exception ex)
				{
					logDebug<T> (ex);
					lastRefresh = default (DateTime);
				}
			});
		}


		async Task syncAsync<T> (IMobileServiceSyncTable<T> table, Expression<Func<T, bool>> where = null)
		where T : AzureEntity, new()
		{
#if DEBUG
			var sw = new System.Diagnostics.Stopwatch ();
			sw.Start ();
#endif
			try
			{
				var query = where == null ? table?.CreateQuery () : table?.Where (where);

				var queryId = where == null ? $"{table?.TableName ?? typeof (T).Name}" : $"{table?.TableName ?? typeof (T).Name}{syncQueryFormat}";

				// pull executed against a table that has pending local updates tracked by the context 
				// automatically triggers a context push first, doing so manually is redundant
				await table?.PullAsync (queryId, query);
			}
			catch (MobileServicePushFailedException mspfe)
			{
				Console.WriteLine ($"Push {typeof (T).Name} table Failed with errors:");

				foreach (var error in mspfe.PushResult?.Errors)
				{
					Console.WriteLine ($"    {error.RawResult}");
				}

				Console.WriteLine ($"    Status: {mspfe.PushResult.Status}");

				switch (mspfe.PushResult.Status)
				{
					case MobileServicePushStatus.CancelledByAuthenticationError:
						// Push was aborted due to authentication error.

						//await AuthenticateAsync ();

						break;
					case MobileServicePushStatus.CancelledByNetworkError:
						// Push was aborted due to network error.

						break;
					case MobileServicePushStatus.CancelledByOperation:
						// Push was aborted by IMobileServiceTableOperation.

						break;
					case MobileServicePushStatus.CancelledBySyncStoreError:
						// Push was aborted due to error from sync store.

						break;
					case MobileServicePushStatus.CancelledByToken:
						// Push was aborted due to cancellation.

						break;
					case MobileServicePushStatus.Complete:
						// All table operations in the push action were completed, possibly with errors.

						break;
					case MobileServicePushStatus.InternalError:
						// Push failed due to an internal error.

						break;
				}
#if !DEBUG
			}
			catch (Exception)
			{
				throw;
#else
			}
			catch (Exception e)
			{
				logDebug<T> (e);
				throw;
			}
			finally
			{
				sw.Stop ();
				logDebug<T> (sw.ElapsedMilliseconds);
#endif
			}
		}
	}
}
#endif