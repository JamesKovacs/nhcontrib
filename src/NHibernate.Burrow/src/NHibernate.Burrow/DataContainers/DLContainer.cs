namespace NHibernate.Burrow.DataContainers {
    /// <summary>
    /// Loader for getting the DLContainer
    /// </summary>
    public static class DLContainer {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static IDLContainer Impl {
            get { return HttpAppContainer.Current; }
        }

        public static IDomainSession DomainSession {
            get { return Impl.DomainSession;  }
            set { Impl.DomainSession = value; }
        }

    }
}