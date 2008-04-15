using System;
using System.Collections.Generic;
using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Tests.TestFactories;

namespace EnterpriseSample.Tests.Data.DaoTestDoubles
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

        public List<Order> GetOrdersPlacedBetween(DateTime startDate, DateTime endDate) {
            throw new Exception("The method or operation is not implemented.");
        }

        public Order GetById(long id, bool shouldLock) {
            throw new Exception("The method or operation is not implemented.");
        }

        public List<Order> GetAll() {
            throw new Exception("The method or operation is not implemented.");
        }

        public Order GetUniqueByExample(Order exampleInstance, params string[] propertiesToExclude) {
            throw new Exception("The method or operation is not implemented.");
        }

        public Order Save(Order entity) {
            throw new Exception("The method or operation is not implemented.");
        }

        public Order SaveOrUpdate(Order entity) {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Delete(Order entity) {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CommitChanges() {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
