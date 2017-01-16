#if __MOBILE__

using System;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;


namespace NomadCode.Azure
{
	public partial class AzureClient // Authenticate
	{
		public Uri AlternateLoginHost { get; set; }

		public MobileServiceAuthenticationProvider AuthProvider { get; set; } = MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory;


#if DEBUG
		public bool Authenticated => !string.IsNullOrEmpty (Client?.CurrentUser?.MobileServiceAuthenticationToken);
#else
		public bool Authenticated => true;
#endif


#if __IOS__
		public async Task<bool> AuthenticateAsync (UIKit.UIViewController view)
#else
		public async Task<bool> AuthenticateAsync (Android.App.Activity view)
#endif
		{
#if DEBUG
			var sw = new System.Diagnostics.Stopwatch ();
			sw.Start ();
#endif
			try
			{
#if DEBUG
				// local server can't process auth, so point to the real one
				if (AlternateLoginHost != null)
				{
					Client.AlternateLoginHost = AlternateLoginHost;
				}
#endif
				var creds = getItemFromKeychain (AuthProvider.ToString ());

				if (string.IsNullOrEmpty (creds?.Item1) || string.IsNullOrEmpty (creds?.Item2))
				{
					var user = await Client.LoginAsync (view, AuthProvider);

					if (!string.IsNullOrEmpty (user.UserId) && !string.IsNullOrEmpty (user.MobileServiceAuthenticationToken))
					{
						saveItemToKeychain (AuthProvider.ToString (), user.UserId, user.MobileServiceAuthenticationToken);
					}
				}
				else
				{
					Client.CurrentUser = new MobileServiceUser (creds.Item1)
					{
						MobileServiceAuthenticationToken = creds.Item2
					};
				}

				return Authenticated;
#if !DEBUG
			}
			catch (Exception)
			{
				return false;
#else
			}
			catch (Exception e)
			{
				logDebug ($"AuthenticateAsync failed : {(e.InnerException ?? e).Message}");
				return false;
			}
			finally
			{
				sw.Stop ();
				logDebug ($"AuthenticateAsync took {sw.ElapsedMilliseconds} milliseconds");
#endif
			}
		}


		public async Task LogoutAsync ()
		{
#if DEBUG
			var sw = new System.Diagnostics.Stopwatch ();
			sw.Start ();
#endif
			try
			{
				resetPreferenceStore ();

				removeItemFromKeychain (AuthProvider.ToString ());

				await Client.LogoutAsync ();

#if OFFLINE_SYNC_ENABLED

				foreach (var table in tables.Values)
				{
					await table.PurgeAsync ();
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
				logDebug ($"LogoutAsync failed : {(e.InnerException ?? e).Message}");
				throw;
			}
			finally
			{
				sw.Stop ();
				logDebug ($"LogoutAsync took {sw.ElapsedMilliseconds} milliseconds");
#endif
			}
		}
	}
}
#endif