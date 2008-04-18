using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Tests.TestFactories;
using Rhino.Mocks;

namespace EnterpriseSample.Tests.Data.DaoTestDoubles
{
    public class MockHistoricalOrderSummaryDaoFactory
    {
        public IHistoricalOrderSummaryDao CreateMockHistoricalOrderSummariesDao() {
            MockRepository mocks = new MockRepository();

            IHistoricalOrderSummaryDao mockedDao = mocks.CreateMock<IHistoricalOrderSummaryDao>();
            Expect.Call(mockedDao.GetCustomerOrderHistoryFor(null)).IgnoreArguments()
                .Return(new TestHistoricalOrderSummariesFactory().CreateHistoricalOrderSummaries());

            mocks.Replay(mockedDao);

            return mockedDao;
        }
    }
}
