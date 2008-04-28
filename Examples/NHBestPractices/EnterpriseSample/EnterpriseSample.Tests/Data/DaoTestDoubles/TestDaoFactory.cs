using System;
using System.Collections.Generic;
using System.Text;
using EnterpriseSample.Core.DataInterfaces;

namespace EnterpriseSample.Tests.Data.DaoTestDoubles
{
    public class TestDaoFactory : IDaoFactory
    {
        public ICustomerDao GetCustomerDao()
        {
            return new MockCustomerDaoFactory().CreateMockCustomerDao();
        }

        public IHistoricalOrderSummaryDao GetHistoricalOrderSummaryDao()
        {
            return new MockHistoricalOrderSummaryDaoFactory().CreateMockHistoricalOrderSummariesDao();
        }

        public IOrderDao GetOrderDao()
        {
            return new MockOrderDaoFactory().CreateMockOrderDao();
        }

        public ISupplierDao GetSupplierDao()
        {
            throw new NotImplementedException();
        }
    }
}