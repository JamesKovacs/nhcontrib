using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FelicePollano.Hbm2NetAddIn.Helpers;

namespace FelicePollano.Hbm2NetAddIn
{
    public class Hbm2NetRunner
    {
        private string workingDir;
        IRdtHelper rdtHelper;
        public Hbm2NetRunner(string workingDir,IRdtHelper rdtHelper)
        {
            this.rdtHelper = rdtHelper;
            this.workingDir = workingDir;
        }

        public void Run()
        {
            throw new Exception("Hallo Hallo");
        }
    }
}
