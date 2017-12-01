using System.Collections.Generic;
using System.Threading;

namespace Adapter.PatsGlobal.PatsWrapper
{
    /**
     * The Logon Status Listener class notifies all objects of a Logon Status change
     * The list containing  all the listeners is thread safe and when a logon status
     * change occurs will cycle through the listeners calling the LogonStatusAction
     * method within that class
     * **/
    public class LogonStatusListener
    {
        private LogonStatusStruct logonStatus;
        private readonly LinkedList<LogonStatusListenerInterface> listenerList;

        public LogonStatusListener()
        {
            listenerList = new LinkedList<LogonStatusListenerInterface>();
        }

        /** Add an object which implements the LogonStatusListenerInterface interface
         * to the list of listeners **/
        public void AddListener(LogonStatusListenerInterface obj)
        {
            lock (listenerList)
            {
                listenerList.AddLast(obj);
            }
        }

        /** removes an existing listener from the list **/
        public void RemoveListener(LogonStatusListenerInterface obj)
        {
            lock (listenerList)
            {
                listenerList.Remove(obj);
            }
        }

        /** when a status change occurs the Trigger method is called. This creates a thread
         * to execute the LogonStatusExecutionThread method which actions the listening objects**/
        public void Trigger(LogonStatusStruct logonStatus)
        {
            this.logonStatus = logonStatus;
            var thread = new Thread(LogonStatusExecutionThread);
            thread.Start();
        }

        /** The method executed on another thread to action the listenining class **/
        private void LogonStatusExecutionThread()
        {
            string statusStr = "";
            
            switch (logonStatus.Status)
            {
                case Constants.LogonStatusLogonFailed: statusStr = "Failed"; break;
                case Constants.LogonStatusLogonSucceeded: statusStr = "Succeeded"; break;
                case Constants.LogonStatusForcedOut: statusStr = "Forced Logout"; break;
                case Constants.LogonStatusObsoleteVers: statusStr = "API Version no longer supported"; break;
                case Constants.LogonStatusWrongEnv: statusStr = "Wrong Environment"; break;
                case Constants.LogonStatusDatabaseErr: statusStr = "Core could not attach to Database"; break;
                case Constants.LogonStatusInvalidUser: statusStr = "Invalid username"; break;
                case Constants.LogonStatusLogonRejected: statusStr = "Rejected"; break;
                case Constants.LogonStatusInvalidAppl: statusStr = "Invalid Application or License"; break;
                case Constants.LogonStatusLoggedOn: statusStr = "Already Logged on"; break;
                case Constants.LogonStatusInvalidLogonState: statusStr = "Invalid logon state"; break;
            }

            string reason = logonStatus.Reason;
            string defaultTrader = logonStatus.DefaultTraderAccount;

            lock (listenerList)
            {
                foreach (var listener in listenerList)
                {
                    listener.LogonStatusAction(logonStatus.Status, statusStr, reason, defaultTrader);
                }
            }
        }
    }
}
