#if __MOBILE__

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NomadCode.Azure
{
	public partial class AzureClient
	{

#if OFFLINE_SYNC_ENABLED

		/// <summary>
		/// Pushes all local changes, then pulls all remote changes
		/// </summary>
		/// <typeparam name="T">The Table to sync.</typeparam>
		public async Task SyncAsync<T> ()
			where T : AzureEntity, new()
		{
			await syncAsync (getTable<T> ());
		}

#endif

		/// <summary>
		/// Gets object of type T with an Id matching the itemId param
		/// </summary>
		/// <param name="itemId">The items Id.</param>
		/// <typeparam name="T">The Table to query.</typeparam>
		public async Task<T> GetAsync<T> (string itemId)
			where T : AzureEntity, new()
		{
			return await lookupItemAsync (getTable<T> (), itemId);
		}


		/// <summary>
		/// Gets the items.
		/// </summary>
		/// <param name="where">Where.</param>
		/// <typeparam name="T">The Table to query.</typeparam>
		public async Task<List<T>> GetAsync<T> (Expression<Func<T, bool>> where = null)
			where T : AzureEntity, new()
		{
			return await getAsync (getTable<T> (), where);
		}


		/// <summary>
		/// Finds the first item returned by the where expression.
		/// </summary>
		/// <param name="where">Where.</param>
		/// <typeparam name="T">The Table to query.</typeparam>
		public async Task<T> FirstOrDefault<T> (Expression<Func<T, bool>> where)
			where T : AzureEntity, new()
		{
			return await getFirstOrDefault (getTable<T> (), where);
		}


		/// <summary>
		/// Inserts or updates the items.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <typeparam name="T">The Table to save to.</typeparam>
		public async Task SaveAsync<T> (T item)
			where T : AzureEntity, new()
		{
			await insertOrUpdateAsync (getTable<T> (), item);
		}


		/// <summary>
		/// Inserts or updates the items in the list.
		/// </summary>
		/// <param name="items">Items.</param>
		/// <typeparam name="T">The Table to save to.</typeparam>
		public async Task SaveAsync<T> (List<T> items)
			where T : AzureEntity, new()
		{
			await insertOrUpdateAsync (getTable<T> (), items);
		}


		/// <summary>
		/// Deletes the item with the Id.
		/// </summary>
		/// <param name="itemId">Item identifier.</param>
		/// <typeparam name="T">The Table to query.</typeparam>
		public async Task DeleteAsync<T> (string itemId)
			where T : AzureEntity, new()
		{
			var table = getTable<T> ();

			var item = await lookupItemAsync (table, itemId);

			await deleteAsync (table, item);
		}


		/// <summary>
		/// Deletes the items.
		/// </summary>
		/// <param name="item">Item to delete.</param>
		/// <typeparam name="T">The Table to query.</typeparam>
		public async Task DeleteAsync<T> (T item)
			where T : AzureEntity, new()
		{
			await deleteAsync (getTable<T> (), item);
		}


		/// <summary>
		/// Deletes the items.
		/// </summary>
		/// <param name="items">Items to delete</param>
		/// <typeparam name="T">The Table to query.</typeparam>
		public async Task DeleteAsync<T> (List<T> items)
			where T : AzureEntity, new()
		{
			await deleteAsync (getTable<T> (), items);
		}


		/// <summary>
		/// Deletes the items returned by the where expression.
		/// </summary>
		/// <param name="where">Where expression</param>
		/// <typeparam name="T">The Table to query.</typeparam>
		public async Task DeleteAsync<T> (Expression<Func<T, bool>> where = null)
			where T : AzureEntity, new()
		{
			var table = getTable<T> ();

			var items = await getAsync (table, where);

			await deleteAsync (getTable<T> (), items);
		}
	}
}
#endif