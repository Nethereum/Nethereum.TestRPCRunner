using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nethereum.TestRPCRunner
{
    public class TestRPCEmbeddedRunner: IDisposable
    {
        public Process Process { get; private set; }
        public string TempTestRPCFile { get; private set; }
        public string Arguments { get; set; }
        public bool RedirectOuputToDebugWindow { get; set; } = true;

        public void StartTestRPC()
        {
            var processStartInfo = new ProcessStartInfo()
            {
                CreateNoWindow = false,
                UseShellExecute = false,
            };

            ExtractTestRpcToTempFile();
            processStartInfo.FileName = "node";
            processStartInfo.Arguments = TempTestRPCFile + " " + Arguments;

            if (RedirectOuputToDebugWindow)
            {
                processStartInfo.RedirectStandardOutput = true;
               
            }

            Process = Process.Start(processStartInfo);

            if (RedirectOuputToDebugWindow)
            {
                Task.Run(() =>
                {
                    while (!Process.StandardOutput.EndOfStream)
                    {
                        Debug.WriteLine(Process.StandardOutput.ReadLine());
                    }
                });
            }

        }

        private void ExtractTestRpcToTempFile()
        {
            var assembly = typeof(TestRPCEmbeddedRunner).GetTypeInfo().Assembly;
            string script;
            using (var stream = assembly.GetManifestResourceStream(assembly.GetManifestResourceNames()[0]))
            using (var reader = new StreamReader(stream))
            {
                script = reader.ReadToEnd();
            }
            TempTestRPCFile = Path.GetTempFileName();
            using (var file = File.CreateText(TempTestRPCFile))
            {
                file.Write(script);
                file.Flush();
            }
        }

        public void StopTestRPC()
        {
            if (Process != null && !Process.HasExited)
            {
                Process.Kill();
                Process = null;

                if (File.Exists(TempTestRPCFile))
                {
                    File.Delete(TempTestRPCFile);
                }
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopTestRPC();

                if (Process != null)
                {
                    Process.Dispose();
                    Process = null;
                }
            }
        }

    }
}
