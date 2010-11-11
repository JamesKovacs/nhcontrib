using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;

namespace FelicePollano.Hbm2NetAddIn.Helpers
{
    public class SavedProjectItemEventArgs:EventArgs
    {
        public ProjectItem ProjectItem { get; set; }
        public string SavedPath { get; set; }
    }
}
