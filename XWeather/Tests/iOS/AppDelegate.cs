using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Foundation;
using UIKit;

using Xunit.Runner;
using Xunit.Sdk;


namespace XWeather.Tests.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : RunnerAppDelegate
    {

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
			//We need this to ensure the execution assembly is part of the app bundle
			AddExecutionAssembly (typeof(ExtensibilityPointFactory).Assembly);
            

            // tests can be inside the main assembly
            AddTestAssembly(Assembly.GetExecutingAssembly());
            // otherwise you need to ensure that the test assemblies will 
            // become part of the app bundle
            AddTestAssembly(typeof(XWeather.Tests.Tests).Assembly);

#if false
			// you can use the default or set your own custom writer (e.g. save to web site and tweet it ;-)
			Writer = new TcpTextWriter ("10.0.1.2", 16384);
			// start running the test suites as soon as the application is loaded
			AutoStart = true;
			// crash the application (to ensure it's ended) and return to springboard
			TerminateAfterExecution = true;
#endif
            return base.FinishedLaunching(app, options);
		}
    }
}