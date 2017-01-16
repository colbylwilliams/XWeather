#if __MOBILE__

using System;
using System.Collections.Generic;

using Microsoft.WindowsAzure.MobileServices;

#if OFFLINE_SYNC_ENABLED
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
#endif


namespace NomadCode.Azure
{
	public partial class AzureClient
	{
		const string dbPath = @"nomad.db";


		static Uri appUri { get; set; }


		static AzureClient _shared;
		public static AzureClient Shared => _shared ?? (_shared = new AzureClient ());


		static MobileServiceClient _client;
		static MobileServiceClient Client => _client ?? (_client = new MobileServiceClient (appUri));


		#region tables

#if OFFLINE_SYNC_ENABLED

		static Dictionary<Type, IMobileServiceSyncTable> tables = new Dictionary<Type, IMobileServiceSyncTable> ();

		IMobileServiceSyncTable<T> getTable<T> ()
			where T : AzureEntity, new()
		{
			if (!Client.SyncContext.IsInitialized)
			{
				throw new Exception ("Client isn't Initialized.  Call RegisterTable on each table then call Initialize before performing CRUD operations.");
			}

			var type = typeof (T);

			if (!tables.ContainsKey (type))
			{
				var table = Client.GetSyncTable<T> ();

				tables.Add (type, table);

				return table;
			}

			return tables [type] as IMobileServiceSyncTable<T>;
		}

#else

		static Dictionary<Type, IMobileServiceTable> tables = new Dictionary<Type, IMobileServiceTable> ();

		IMobileServiceTable<T> getTable<T> ()
			where T : AzureEntity, new()
		{
			var type = typeof (T);

			if (!tables.ContainsKey (type))
			{
				var table = Client.GetTable<T> ();

				tables.Add (type, table);

				return table;
			}

			return tables [type] as IMobileServiceTable<T>;
		}

#endif

		#endregion

		AzureClient ()
		{
			CurrentPlatform.Init ();
		}


#if OFFLINE_SYNC_ENABLED

		MobileServiceSQLiteStore store;

		public void RegisterTable<T> ()
			where T : AzureEntity
		{
			if (store == null)
			{
				store = new MobileServiceSQLiteStore (dbPath);
			}

			store.DefineTable<T> ();
		}


		public async Task InitializeAzync (string mobileAppUri)
		{
			if (string.IsNullOrWhiteSpace (mobileAppUri))
			{
				throw new ArgumentException ("Cannot be null, empty, or whitespace.", nameof (mobileAppUri));
			}

			Uri uri;

			if (!Uri.TryCreate (mobileAppUri, UriKind.Absolute, out uri))
			{
				throw new ArgumentException ("Invalid Url", nameof (mobileAppUri));
			}

			appUri = uri;

			if (!Client.SyncContext.IsInitialized)
			{
				// Uses the default conflict handler, which fails on conflict
				// To use a different conflict handler, pass a parameter to InitializeAsync.
				// For more details, see http://go.microsoft.com/fwlink/?LinkId=521416.
				await Client.SyncContext.InitializeAsync (store);
			}
			else
			{
				throw new Exception ("Client already Initialized.  Call RegisterTable on each table then call Initialize ONCE.");
			}
		}

#else

		public void Initialize (string mobileAppUri)
		{
			if (string.IsNullOrWhiteSpace (mobileAppUri))
			{
				throw new ArgumentException ("Cannot be null, empty, or whitespace.", nameof (mobileAppUri));
			}

			Uri uri;

			if (!Uri.TryCreate (mobileAppUri, UriKind.Absolute, out uri))
			{
				throw new ArgumentException ("Invalid Url", nameof (mobileAppUri));
			}

			appUri = uri;
		}

#endif
	}
}
#endif