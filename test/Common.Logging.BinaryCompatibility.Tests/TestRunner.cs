using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Common.Logging;
using Common.Logging.Simple;
using NUnit.Framework;

namespace Common
{
    [TestFixture]
    public class TestRunner
    {
        private static readonly ILog Log;

        static TestRunner()
        {
            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter();
            Log = LogManager.GetCurrentClassLogger();
        }

        private readonly string commonLoggingVersion = string.Format("{0}", Log.GetType().Assembly.GetName().Version);
        private readonly string publicKeyToken = BitConverter.ToString(Log.GetType().Assembly.GetName().GetPublicKeyToken());
        private readonly string TestExecutableFileName = "CommonLoggingBinaryCompatibilityTest.exe";

        [Test]
        public void CanUpgradeFromVersion12()
        {
            CompileAppAgainstVersion12();
            AdjustAppConfigAssemblyRedirect();

            Process process = new Process();

            List<string> logLines = new List<string>();

            using (process)
            {
                process.StartInfo.FileName = TestExecutableFileName;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.ErrorDataReceived += (sender, evt) =>
                                                 {
                                                     if (!string.IsNullOrEmpty(evt.Data))
                                                     {
                                                         Log.Error(evt.Data);
                                                     }
                                                 };
                process.OutputDataReceived += (sender, evt) =>
                                                 {
                                                     if (!string.IsNullOrEmpty(evt.Data))
                                                     {
                                                         logLines.Add(evt.Data);
                                                         Log.Info(evt.Data);
                                                     }
                                                 };
                
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                Assert.AreEqual(0, process.ExitCode);
                // 6 levels, 3 entries per level
                Assert.AreEqual(18, logLines.Count);
            }
        }

        private void CompileAppAgainstVersion12()
        {
            var codeDomProvider = CodeDomProvider.CreateProvider("C#");
            var parameters = new CompilerParameters();
            parameters.GenerateExecutable = true;
            parameters.OutputAssembly = TestExecutableFileName;
            parameters.ReferencedAssemblies.Add("System.dll");
            if (string.IsNullOrEmpty(publicKeyToken))
            {
                parameters.ReferencedAssemblies.Add("./1.2/Common.Logging-unsigned.dll");
            }
            else
            {
                parameters.ReferencedAssemblies.Add("./1.2/Common.Logging-signed.dll");
            }

            string code = GetManifestResourceText(this.GetType(), "CommonLoggingBinaryCompatibilityTest.csharp");
            var results = codeDomProvider.CompileAssemblyFromSource(parameters, code);
            if (results.Errors.Count > 0)
            {
                foreach (CompilerError CompErr in results.Errors)
                {
                    Log.Error(  "Line number " + CompErr.Line +
                                ", Error Number: " + CompErr.ErrorNumber +
                                ", '" + CompErr.ErrorText);
                }
                Assert.Fail();
            }
        }

        private void AdjustAppConfigAssemblyRedirect()
        {
            string appConfigFileName = "CommonLoggingBinaryCompatibilityTest.exe.config";
            string appConfigXml = GetManifestResourceText(this.GetType(), appConfigFileName);
            appConfigXml = appConfigXml
                .Replace(
                "{version}",
                commonLoggingVersion
                )
                .Replace(
                "{publicKeyToken}",
                publicKeyToken
                )
                ;
            File.WriteAllText(TestExecutableFileName + ".config", appConfigXml);
        }

        private string GetManifestResourceText(Type type, string name)
        {
            string code = null;
            using (var stm = new StreamReader(this.GetType().Assembly.GetManifestResourceStream(type, name)))
            {
                code = stm.ReadToEnd();
            }
            return code;
        }
    }
}
