using BasicSample.Core.DataInterfaces;
using BasicSample.Tests.TestFactories;
using Rhino.Mocks;

namespace BasicSample.Tests.Data.DaoTestDoubles
{
    public class MockOrderDaoFactory
    {
        public IOrderDao CreateMockOrderDao() {
            MockRepository mocks = new MockRepository();

            IOrderDao mockedOrderDao = mocks.CreateMock<IOrderDao>();
            Expect.Call(mockedOrderDao.GetByExample(null)).IgnoreArguments()
                .Return(new TestOrdersFactory().CreateOrders());
            
            mocks.Replay(mockedOrderDao);

            return mockedOrderDao;
        }
    }
}
