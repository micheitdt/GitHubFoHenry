using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Adapter.TaifexGlobalPATS.ApiPATS
{
    public class PriceDetailHashTable : Dictionary<int, PriceStruct>
    {

    }
    public interface LogonStatusListenerInterface
    {
        void LogonStatusAction(int StatusInt, string statusStr, string reason, string defaultTrader);
    }

    public interface HostLinkStatusListenerInterface
    {
        void HostLinkStatusAction(int oldStatus, int newStatus);
    }

    public interface PriceLinkStatusListenerInterface
    {
        void PriceLinkStatusAction(int oldStatus, int newStatus);
    }

    public interface DownloadCompleteListenerInterface
    {
        void DownloadCompleteAction();
    }
    public interface ContractAddedListenerInterface
    {
        void ContractAddedAction();
    }

    public class Listeners
    {
        public readonly LogonStatusListener logonStatusListener;
        public readonly HostLinkStatusListener hostLinkStatusListener;
        public readonly PriceLinkStatusListener priceLinkStatusListener;
        public readonly DownloadCompleteListener downloadCompleteListener;
        public readonly ContractAddedListener contractAddedListener;

        public Listeners()
        {
            logonStatusListener = new LogonStatusListener();
            hostLinkStatusListener = new HostLinkStatusListener();
            priceLinkStatusListener = new PriceLinkStatusListener();
            downloadCompleteListener = new DownloadCompleteListener();
            contractAddedListener = new ContractAddedListener();

        }
    }    
}
