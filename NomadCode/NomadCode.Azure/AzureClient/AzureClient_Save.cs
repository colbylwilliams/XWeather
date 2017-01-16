#if __MOBILE__

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;

#if OFFLINE_SYNC_ENABLED
using System.Linq.Expressions;
using Microsoft.WindowsAzure.MobileServices.Sync;
#endif


namespace NomadCode.Azure
{
	public partial class AzureClient // Insert & Update
	{


#if OFFLINE_SYNC_ENABLED
		async Task insertAsync<T> (IMobileServiceSyncTable<T> table, T item, Expression<Func<T, bool>> where = null, bool pull = true)
#else
		async Task insertAsync<T> (IMobileServiceTable<T> table, T item)
#endif
			where T : AzureEntity, new()
		{
#if DEBUG
			var sw = new System.Diagnostics.Stopwatch ();
			sw.Start ();
#endif
			try
			{
				await table?.InsertAsync (item);

#if OFFLINE_SYNC_ENABLED

				if (pull)
				{
					sync (table, where);
				}
#endif

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
		async Task updateAsync<T> (IMobileServiceSyncTable<T> table, T item, Expression<Func<T, bool>> where = null, bool pull = true)
#else
		async Task updateAsync<T> (IMobileServiceTable<T> table, T item)
#endif
			where T : AzureEntity, new()
		{
#if DEBUG
			var sw = new System.Diagnostics.Stopwatch ();
			sw.Start ();
#endif
			try
			{
				await table?.UpdateAsync (item);

#if OFFLINE_SYNC_ENABLED

				if (pull)
				{
					sync (table, where);
				}
#endif
			}
			catch (MobileServicePreconditionFailedException<T> preconditionFailed)
			{
				log ($"UpdateItemAsync for {typeof (T).Name} failed due to a conflict detected between the local and server version : {(preconditionFailed.InnerException ?? preconditionFailed).Message}");

				// To resolve the conflict, update the version of the item being committed. Otherwise, 
				// it will continue to throw MobileServicePreConditionFailedExceptions

				item.Version = preconditionFailed.Item.Version;

				// Updating recursively here just in case another change happened while the user was making a decision
				await updateAsync (table, item);
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
		Task insertOrUpdateAsync<T> (IMobileServiceSyncTable<T> table, T item, Expression<Func<T, bool>> where = null, bool pull = true)
#else
		Task insertOrUpdateAsync<T> (IMobileServiceTable<T> table, T item)
#endif
			where T : AzureEntity, new()
		{
			if (item.HasId)
			{
#if OFFLINE_SYNC_ENABLED
				return updateAsync (table, item, where, pull);
#else
				return updateAsync (table, item);
#endif
			}
#if OFFLINE_SYNC_ENABLED
			return insertAsync (table, item, where, pull);
#else
			return insertAsync (table, item);
#endif
		}



#if OFFLINE_SYNC_ENABLED
		async Task insertOrUpdateAsync<T> (IMobileServiceSyncTable<T> table, List<T> items, Expression<Func<T, bool>> where = null, bool pull = true)
#else
		async Task insertOrUpdateAsync<T> (IMobileServiceTable<T> table, List<T> items)
#endif
			where T : AzureEntity, new()
		{
#if DEBUG
			var sw = new System.Diagnostics.Stopwatch ();
			sw.Start ();
#endif
			try
			{
				foreach (var item in items)
				{
					if (item.HasId)
					{
#if OFFLINE_SYNC_ENABLED
						await updateAsync (table, item, where, false);
#else
						await updateAsync (table, item);
#endif
					}
					else
					{
#if OFFLINE_SYNC_ENABLED
						await insertAsync (table, item, where, false);
#else
						await insertAsync (table, item);
#endif
					}
				}
#if OFFLINE_SYNC_ENABLED

				if (pull)
				{
					sync (table, where);
				}
#endif

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