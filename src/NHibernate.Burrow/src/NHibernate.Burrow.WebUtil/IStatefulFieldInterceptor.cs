namespace NHibernate.Burrow.WebUtil
{
    public interface IStatefulFieldInterceptor
    {
        /// <summary>
        /// called after loaded from States
        /// </summary>
        /// <param name="objectOriginallyLoaded">objectStoredInViewState</param>
        /// <returns>the object set to the Overspan field</returns>
        /// <remarks>
        ///  return objectOriginallyLoaded for simple application
        /// </remarks>
        object OnLoad(object objectOriginallyLoaded);

        /// <summary>
        /// called before saving into ViewState
        /// </summary>
        /// <param name="toSave"></param>
        /// <returns>the object to save to the viewstate</returns>
        /// <remarks>
        /// return toSave for simple application
        /// </remarks>
        /// <param name="objectInStateContainer">object currently in the state container (could be ViewState or Session)</param>
        object OnSave(object toSave, object objectInStateContainer);
    }
}