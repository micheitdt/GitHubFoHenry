using System.Collections.Generic;

namespace Adapter.TaifexGlobalPATS.ApiPATS
{
    /** when the download is complete all listeners will be notified **/
    public class DownloadCompleteListener
    {
        private readonly LinkedList<DownloadCompleteListenerInterface> listenerList;

        public DownloadCompleteListener()
        {
            listenerList = new LinkedList<DownloadCompleteListenerInterface>();
        }
        /** Add an object which implements the DownloadCompleteListenerInterface interface
         * to the list of listeners **/
        public void AddListener(DownloadCompleteListenerInterface obj)
        {
            lock (listenerList)
            {
                listenerList.AddLast(obj);
            }
        }

        /** removes an existing listener from the list **/
        public void RemoveListener(DownloadCompleteListenerInterface obj)
        {
            lock (listenerList)
            {
                listenerList.Remove(obj);
            }
        }

        /** when the download is complete all listeners are notified **/
        public void Trigger()
        {
            lock (listenerList)
            {
                foreach (var listener in listenerList)
                {
                    listener.DownloadCompleteAction();
                }
                
                /*LinkedList<DownloadCompleteListenerInterface>.Enumerator en = listenerList.GetEnumerator();
                for (int x = 0; x < listenerList.Count; x++)
                {
                    en.MoveNext();
                    DownloadCompleteListenerInterface obj = en.Current;
                    obj.DownloadCompleteAction();
                }*/
            }
        }        
    }    
}
