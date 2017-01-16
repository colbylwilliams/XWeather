#if __MOBILE__
using System;

using Newtonsoft.Json;

using Microsoft.WindowsAzure.MobileServices;
#else
using Microsoft.Azure.Mobile.Server;
#endif

namespace NomadCode.Azure
{
	public class AzureEntity
#if !__MOBILE__
	 : EntityData
#endif
	{
#if __MOBILE__

		public string Id { get; set; }

		[Deleted]
		[JsonProperty (PropertyName = "Deleted")]
		public bool Deleted { get; set; }

		[CreatedAt]
		[JsonProperty (PropertyName = "CreatedAt")]
		public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

		[UpdatedAt]
		[JsonProperty (PropertyName = "UpdatedAt")]
		public DateTimeOffset? UpdatedAt { get; set; }

		[Version]
		[JsonProperty (PropertyName = "Version")]
		public byte [] Version { get; set; }

		[JsonIgnore]
		public bool HasId => !string.IsNullOrEmpty (Id);

#endif
	}
}