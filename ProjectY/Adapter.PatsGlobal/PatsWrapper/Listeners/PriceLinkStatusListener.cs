using System.Collections.Generic;

namespace Adapter.PatsGlobal.PatsWrapper
{
    /** when there is a price link status change all listeners will be notified of
     * the old and new status **/

    public class PriceLinkStatusListener
    {
        private readonly LinkedList<PriceLinkStatusListenerInterface> listenerList;
        
        public PriceLinkStatusListener()
        {
            listenerList = new LinkedList<PriceLinkStatusListenerInterface>();
        }

        /** Add an object which implements the PriceLinkStatusListenerInterface interface
         * to the list of listeners **/
        public void addListener(PriceLinkStatusListenerInterface obj)
        {
            lock (listenerList)
            {
                listenerList.AddLast(obj);
            }
        }

        /** removes an existing listener from the list **/
        public void removeListener(PriceLinkStatusListenerInterface obj)
        {
            lock (listenerList)
            {
                if (listenerList.Find(obj) != null)
                    listenerList.Remove(obj);
            }
        }

        /** when a price link status change occurs the Trigger method is called. This creates a thread
         * to execute the PriceLinkStatusExecutionThread method which actions the listening objects**/
        public void Trigger(LinkStateStruct data)
        {            
            lock (listenerList)
            {
                LinkedList<PriceLinkStatusListenerInterface>.Enumerator en = listenerList.GetEnumerator();
                for (int x = 0; x < listenerList.Count; x++)
                {
                    en.MoveNext();
                    PriceLinkStatusListenerInterface obj = en.Current;
                    if (obj != null)
                    {
                        obj.PriceLinkStatusAction(data.oldState, data.newState);
                    }
                }
            }

        }
    }
}
