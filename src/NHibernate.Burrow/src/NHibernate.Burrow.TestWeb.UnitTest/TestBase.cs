using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using NUnit.Framework;
using WatiN.Core; 

namespace NHibernate.Burrow.TestWeb.UnitTest
{
    /// <summary>
    /// based on code writen by Mikhail Dikov http://www.mikhaildikov.com/2008/01/using-asp.html
    /// </summary>
    public class TestBase
    {

        private const string devServerPort = "12345";
        private IE ie;
        private string rootUrl;
        private Process cmdProcess;
        private const string rootpath = "NHibernate.Burrow.TestWeb/";
        protected  IE IE
        {
            get{ return ie;}
        }
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            bool IsWebStarted;
            rootUrl = string.Format("http://localhost:{0}/", devServerPort);
          
                // Check if Dev WebServer runs
                ie = new IE(rootUrl);
                IsWebStarted = ie.ContainsText("Directory Listing -- /");
            

            if (!IsWebStarted)
            {
                // If not start it
                string command = Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "WebDev.WebServer.EXE");

                string rootPhyPath = Environment.CurrentDirectory.Remove( Environment.CurrentDirectory.IndexOf(@".UnitTest")) ;
                    
                    //Environment.CurrentDirectory.Substring(0,Environment.CurrentDirectory.LastIndexOf('\\'));
                string commandArgs = string.Format(" /path:\"{0}\" /port:{1} /vapth:\"/{2}\"", rootPhyPath, devServerPort, rootpath);

                 cmdProcess = new Process();
                cmdProcess.StartInfo.Arguments = commandArgs;
                cmdProcess.StartInfo.CreateNoWindow = true;
                cmdProcess.StartInfo.FileName = command;
                cmdProcess.StartInfo.UseShellExecute = false;
                cmdProcess.StartInfo.WorkingDirectory = command.Substring(0, command.LastIndexOf('\\'));
                cmdProcess.Start();

                // .. and try one more time to see if the server is up
                ie.GoTo(rootUrl);
            }
            Assert.IsTrue(ie.ContainsText("Directory Listing -- /"));

            // Give some time to crank up
            Thread.Sleep(1000);
        }

        [TearDown]
        public void TearDown()
        {
            ie.Close();
        }
 

        protected  void GoTo(string path)
        {
            if (path.IndexOf(".aspx") < 0)
                path = path + "/Default.aspx";
            ie.GoTo(rootUrl + path);
        }

        
        #endregion


        protected void AssertText(string s)
        {
            Assert.IsTrue(IE.ContainsText(s));
        }

        protected  void AssertTestSuccessMessageShown()
        {
            AssertText("Congratulations! Test passed.");
        }

    }
}