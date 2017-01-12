using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nethereum.TestRPCRunner;

namespace ConsoleSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var launcher = new TestRPCEmbeddedRunner();
            launcher.RedirectOuputToDebugWindow = true;
            launcher.Arguments = "--port 8546";
            launcher.StartTestRPC();

            Console.ReadLine();
            launcher.StopTestRPC();
        }
    }
}
