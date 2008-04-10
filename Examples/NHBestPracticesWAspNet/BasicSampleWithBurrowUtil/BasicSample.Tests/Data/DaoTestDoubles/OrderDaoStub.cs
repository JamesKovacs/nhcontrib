using System;
using System.Collections.Generic;
using BasicSample.Core.DataInterfaces;
using BasicSample.Core.Domain;
using BasicSample.Tests.TestFactories;

namespace BasicSample.Tests.Data.DaoTestDoubles
{
    /// <summary>
    /// Stub DAO that can be used in place of OrderDao to simulate communications with 
    /// the DB without actually talking to the DB.
    /// </summary>
    public class OrderDaoStub : IOrderDao
    {
        public List<Order> GetByExample(Order exampleInstance, params string[] propertiesToExclude) {
            return new TestOrdersFactory().CreateOrders();
        }

        #region Not-Implemented DAO methods
 

        public IList<Order> GetOrdersPlacedBetween(DateTime startDate, DateTime endDate) {
            throw new NotImplementedException();
        }

        public Order GetById(long id, bool shouldLock) {
            throw new NotImplementedException();
        }

        public IList<Order> FindAll() {
            throw new NotImplementedException();
        }

        public IList<Order> FindByExample(Order exampleInstance, params string[] propertiesToExclude) {
            throw new NotImplementedException();
        }

        public long Save(Order entity) {
            throw new NotImplementedException();
        }

        public void SaveOrUpdate(Order entity) {
            throw new NotImplementedException();
        }

        public void Delete(Order entity) {
            throw new NotImplementedException();
        }
        #endregion

    }
}
