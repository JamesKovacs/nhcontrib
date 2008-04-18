using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Tests.TestFactories;
using Rhino.Mocks;

namespace EnterpriseSample.Tests.Data.DaoTestDoubles
{
    public class MockCustomerDaoFactory
    {
        public ICustomerDao CreateMockCustomerDao() {
            MockRepository mocks = new MockRepository();

            ICustomerDao mockedCutomerDao = mocks.CreateMock<ICustomerDao>();
            Expect.Call(mockedCutomerDao.GetAll())
                .Return(new TestCustomersFactory().CreateCustomers());
            Expect.Call(mockedCutomerDao.GetById(null, false)).IgnoreArguments()
                .Return(new TestCustomersFactory().CreateCustomer());

            mocks.Replay(mockedCutomerDao);

            return mockedCutomerDao;
        }
    }
}
