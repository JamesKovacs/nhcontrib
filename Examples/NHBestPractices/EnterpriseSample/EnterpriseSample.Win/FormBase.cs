using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using NHibernate.Burrow;
using System.Threading;

namespace EnterpriseSample.Win
{
    public class FormBase : Form
    {
        protected BurrowFramework burrow;

        protected void InitWorkSpace()
        {
            burrow = new BurrowFramework();
            burrow.InitWorkSpace();
        }

        protected void CloseWorkSpace()
        {
            burrow.CloseWorkSpace();
        }
    }
}
