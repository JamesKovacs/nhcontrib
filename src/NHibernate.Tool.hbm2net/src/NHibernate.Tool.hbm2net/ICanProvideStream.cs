using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NHibernate.Tool.hbm2net
{
    ///<author>
    /// Felice Pollano (felice@felicepollano.com)
    ///</author>
    public interface ICanProvideStream
    {
        Stream GetStream(ClassMapping clazz,string outputDirectory,out string fileName);
        bool CheckIfSourceIsNewer(DateTime source, string directory, ClassMapping clazz, out FileInfo target);
    }
}
