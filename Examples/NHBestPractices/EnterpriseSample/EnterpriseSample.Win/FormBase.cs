using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using NHibernate.Burrow;

namespace EnterpriseSample.Win
{
    public class FormBase : Form
    {
        private readonly BurrowFramework burrow;
        
        public FormBase()
        {
            burrow=new BurrowFramework();
            burrow.InitWorkSpace();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            burrow.CloseWorkSpace();
        }
    }
}
