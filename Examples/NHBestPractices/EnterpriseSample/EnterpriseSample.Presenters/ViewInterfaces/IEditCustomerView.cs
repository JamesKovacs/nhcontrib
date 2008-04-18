using EnterpriseSample.Core.Domain;

namespace EnterpriseSample.Presenters.ViewInterfaces
{
    public interface IEditCustomerView
    {
        void AttachPresenter(EditCustomerPresenter presenter);
        Customer Customer { set; }
        void UpdateValuesOn(Customer customer);
    }
}
