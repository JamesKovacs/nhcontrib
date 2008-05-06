using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Burrow
{
    public interface ITransactionManager
    {

         /// <summary>
         /// begin transactions
         /// </summary>
        void Begin();

        /// <summary>
        /// Commit transactions
        /// </summary>
        void Commit();


        /// <summary>
        /// Rollback transactions
        /// </summary>
        /// <remarks>
        /// afterwords the work Space will become inavailable
        /// </remarks>
        void Rollback();

        event System.EventHandler RolledBack;
    }
}
