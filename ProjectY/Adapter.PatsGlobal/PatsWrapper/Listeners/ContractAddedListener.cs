using System.Collections.Generic;

namespace Adapter.PatsGlobal.PatsWrapper
{
    /** when the download is complete all listeners will be notified **/
    public class ContractAddedListener
    {
        private readonly LinkedList<ContractAddedListenerInterface> listenerList;

        public ContractAddedListener()
        {
            listenerList = new LinkedList<ContractAddedListenerInterface>();
        }
        /** Add an object which implements the DownloadCompleteListenerInterface interface
         * to the list of listeners **/
        public void addListener(ContractAddedListenerInterface obj)
        {
            lock (listenerList)
            {
                listenerList.AddLast(obj);
            }
        }

        /** removes an existing listener from the list **/
        public void removeListener(ContractAddedListenerInterface obj)
        {
            lock (listenerList)
            {
                if (listenerList.Find(obj) != null)
                    listenerList.Remove(obj);
            }
        }

        /** when the download is complete all listeners are notified **/
        public void Trigger()
        {
            lock (listenerList)
            {
                LinkedList<ContractAddedListenerInterface>.Enumerator en = listenerList.GetEnumerator();
                for (int x = 0; x < listenerList.Count; x++)
                {
                    en.MoveNext();
                    ContractAddedListenerInterface obj = en.Current;
                    if (obj != null)
                    {
                        obj.ContractAddedAction();
                    }
                }
            }
        }
    }
}
