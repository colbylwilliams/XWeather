#if __MOBILE__

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.Sync;
#else
using Microsoft.WindowsAzure.MobileServices;
#endif


namespace NomadCode.Azure
{
	public partial class AzureClient // Get & Lookup
	{


#if OFFLINE_SYNC_ENABLED
		async Task<List<T>> getAsync<T> (IMobileServiceSyncTable<T> table, Expression<Func<T, bool>> where = null)
#else
		async Task<List<T>> getAsync<T> (IMobileServiceTable<T> table, Expression<Func<T, bool>> where = null)
#endif
			where T : AzureEntity, new()
		{
#if DEBUG
			var sw = new System.Diagnostics.Stopwatch ();
			sw.Start ();
#endif
			try
			{
				if (where == null)
					return await table?.ToListAsync ();

				return await table?.Where (where)?.ToListAsync ();
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


#if OFFLINE_SYNC_ENABLED
		async Task<T> getFirstOrDefault<T> (IMobileServiceSyncTable<T> table, Expression<Func<T, bool>> where)
#else
		async Task<T> getFirstOrDefault<T> (IMobileServiceTable<T> table, Expression<Func<T, bool>> where)
#endif
			where T : AzureEntity, new()
		{
			try
			{
				var allItems = await getAsync (table, where);

				return allItems.FirstOrDefault ();
			}
			catch (Exception)
			{
				throw;
			}
		}


#if OFFLINE_SYNC_ENABLED
		async Task<T> lookupItemAsync<T> (IMobileServiceSyncTable<T> table, string itemId)
#else
		async Task<T> lookupItemAsync<T> (IMobileServiceTable<T> table, string itemId)
#endif
		where T : AzureEntity, new()
		{
#if DEBUG
			var sw = new System.Diagnostics.Stopwatch ();
			sw.Start ();
#endif
			try
			{
				return await table?.LookupAsync (itemId);
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