using System;
using System.Configuration;
using System.Web;
using ProjectBase.Utils;

namespace ProjectBase.Data
{
    /// <summary>
    /// Implements the Open-Session-In-View pattern using <see cref="NHibernateSessionManager" />.
    /// Inspiration for this class came from Ed Courtenay at 
    /// http://sourceforge.net/forum/message.php?msg_id=2847509.
    /// </summary>
    public class NHibernateSessionModule : IHttpModule
    {
        public void Init(HttpApplication context) {
            context.BeginRequest += new EventHandler(BeginTransaction);
            context.EndRequest += new EventHandler(CommitAndCloseSession);
        }

        /// <summary>
        /// Opens a session within a transaction at the beginning of the HTTP request.  Note that 
        /// it ONLY begins transactions for those designated as being transactional.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginTransaction(object sender, EventArgs e) {
            NHibernateSessionManager.Init("D:/nhcontrib/trunk/Examples/EnterpriseSample/EnterpriseSample.Web/Config/NorthwindNHibernate.config");
        }

        /// <summary>
        /// Commits and closes the NHibernate session provided by the supplied <see cref="NHibernateSessionManager"/>.
        /// Assumes a transaction was begun at the beginning of the request; but a transaction or session does
        /// not *have* to be opened for this to operate successfully.
        /// </summary>
        private void CommitAndCloseSession(object sender, EventArgs e) {
            try {
                // Commit every session factory that's holding a transactional session
                NHibernateSessionManager.Instance.CommitTransaction();
            }
            finally {
                // No matter what happens, make sure all the sessions get closed
                NHibernateSessionManager.Instance.CloseSession();
            }
        }

        public void Dispose() { }
    }
}
