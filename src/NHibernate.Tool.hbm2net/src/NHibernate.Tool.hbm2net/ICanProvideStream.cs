using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NHibernate.Tool.hbm2net
{
    public interface ICanProvideStream
    {
        Stream GetStream(ClassMapping clazz,string outputDirectory);
    }
}
