using System;
using System.Threading.Tasks;
using Xunit;

namespace XWeather.Tests
{
	public class Tests
	{

		[Fact]
		public void ThisShouldPass ()
		{
			Assert.True (true);
		}

		[Fact]
		public async Task ThisShouldFail ()
		{
			await Task.Run (() => { throw new Exception ("boom"); });
		}
	}
}

