using System.Collections.Generic;

namespace EnterpriseSample.Presenters.ViewInterfaces
{
    /// <summary>
    /// A generic view for listing any type of objects.  This doesn't have an AttachPresenter method
    /// since there's no communication back to the presenter from the view.
    /// </summary>
    public interface IListObjectsView<T>
    {
        IList<T> ObjectsToList { set; }
    }
}
