using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace NHibernate.Tool.hbm2net
{
    class SourceFileInfoMap
    {
        Dictionary<XmlDocument, FileInfo> map = new Dictionary<XmlDocument, FileInfo>();
        private SourceFileInfoMap()
        {

        }
        static SourceFileInfoMap instance;
        public static SourceFileInfoMap Instance
        {
            get 
            {
                if (null == instance)
                    instance = new SourceFileInfoMap();
                return instance;
            }

        }
        public void Add(XmlDocument doc, FileInfo fs)
        {
            map[doc] = fs;
        }
        public FileInfo LookupByMapping(ClassMapping clazz)
        {
            //throw if not found
           return map[clazz.XMLElement.OwnerDocument];
        }
    }
}
