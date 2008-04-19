namespace NHibernate.Burrow.WebUtil
{
    internal class ConversationalDataVSFInterceptor : IStatefulFieldInterceptor
    {
        public ConversationalDataVSFInterceptor() {}

        #region IStatefulFieldInterceptor Members

        public object OnSave(object toSave, object objectInStateContainer)
        {
            if (toSave == null)
            {
                return null;
            }
            ConversationalData<object> retVal = objectInStateContainer as ConversationalData<object>;
            if (retVal == null)
            {
                retVal = new ConversationalData<object>();
            }
            retVal.Value = toSave;
            return retVal;
        }

        public object OnLoad(object objectOriginallyLoaded)
        {
            if (objectOriginallyLoaded == null)
            {
                return null;
            }
            else
            {
                return ((ConversationalData<object>) objectOriginallyLoaded).Value;
            }
        }

        #endregion
    }
}