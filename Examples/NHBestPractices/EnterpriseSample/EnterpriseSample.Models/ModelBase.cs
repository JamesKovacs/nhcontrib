using System;
using System.Collections.Generic;
using System.Text;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using EnterpriseSample.Core.DataInterfaces;

namespace EnterpriseSample.Models
{
    public class ModelBase
    {
        public IDaoFactory DaoFactory
        {
            get
            {
                IWindsorContainer container = new WindsorContainer(new XmlInterpreter());
                return (IDaoFactory)container.Resolve("DaoFactory");
            }
        }
    }
}
