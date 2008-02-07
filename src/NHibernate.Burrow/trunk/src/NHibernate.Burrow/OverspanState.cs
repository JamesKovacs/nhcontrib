using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Burrow
{
    public enum OverspanMode {
        /// <summary>
        /// conversation status tracked by post or get parameter
        /// </summary>
        Post,
        /// <summary>
        /// conversation status tracked only by get parameter
        /// </summary>
        GetOnly,
        /// <summary>
        /// conversation status tracked by cookie (very careful with this mode as it's session wide. 
        /// </summary>
        Cookie, 
        /// <summary>
        /// Converstaion does not span
        /// </summary>
        None
    }
    
    public class OverspanState
    {
        private readonly string name;
        private readonly string value;
        private readonly OverspanMode mode;

        public OverspanMode Mode {
            get { return mode; }
        }
        public string Name {
            get { return name; }
        }
        public string Value {
            get { return value; }
        }

        public OverspanState(string name, string value, OverspanMode mode) {
            this.name = name;
            this.mode = mode;
            this.value = value;
        }
    }
}
