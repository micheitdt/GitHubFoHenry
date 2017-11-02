using System.Collections.Generic;


namespace Adapter.TaifexGlobalPATS.ApiPATS
{
    public partial class ClientAPI
    {
        private partial class Callbacks
        {
            private class callbackItem
            {
                public readonly int callbackID;
                public readonly object data;

                public callbackItem(int callbackID, object data)
                {
                    this.callbackID = callbackID;
                    this.data = data;
                }
            }

            private class CallbackList
            {
                private readonly LinkedList<callbackItem> list;

                public CallbackList()
                {
                    list = new LinkedList<callbackItem>();
                }

                public void put(callbackItem cbItem)
                {
                    lock (list)
                    {
                        list.AddLast(cbItem);
                    }
                }

                public callbackItem getNextCallback()
                {
                    lock (list)
                    {
                        if (list.Count > 0)
                        {
                            callbackItem cbI = list.First.Value;
                            list.RemoveFirst();
                            return cbI;
                        }
                        return null;
                    }
                }
            }
        }
    }
}