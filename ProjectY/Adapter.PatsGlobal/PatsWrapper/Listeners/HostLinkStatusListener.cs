using System.Collections.Generic;

namespace Adapter.PatsGlobal.PatsWrapper
{
    /** when there is a host link status change all listeners will be notified of
    * the old and new status **/
    public class HostLinkStatusListener
    {
        private LinkedList<HostLinkStatusListenerInterface> listenerList;
        
        public HostLinkStatusListener()
        {
            listenerList = new LinkedList<HostLinkStatusListenerInterface>();
        }

        /** Add an object which implements the HostLinkStatusListenerInterface interface
         * to the list of listeners **/
        public void addListener(HostLinkStatusListenerInterface obj)
        {
            lock (listenerList)
            {
                listenerList.AddLast(obj);
            }
        }

        /** removes the listener object from the listener list **/
        public void RemoveListener(HostLinkStatusListenerInterface obj)
        {
            lock (listenerList)
            {
                listenerList.Remove(obj);
            }
        }

        /** when a host link status change occurs the Trigger method is called. This creates a thread
         * to execute the HostLinkStatusExecutionThread method which actions the listening objects**/
        public void Trigger(LinkStateStruct data)
        {            
            lock (listenerList)
            {
                LinkedList<HostLinkStatusListenerInterface>.Enumerator en = listenerList.GetEnumerator();
                for (int x = 0; x < listenerList.Count; x++)
                {
                    en.MoveNext();
                    HostLinkStatusListenerInterface obj = en.Current;
                    if (obj != null)
                    {
                        obj.HostLinkStatusAction(data.oldState, data.newState);
                    }
                }
            }

        }
    }
}
