using Adapter.TaifexGlobalPATS.TradingObjects;
using CommonLibrary;
using System;

namespace Adapter.TaifexGlobalPATS.ApiPATS
{
    public class EventDelegateTickerArgs : EventArgs
    {
        public EventDelegateTickerArgs(TickerUpdStruct tickerData)
        {
            this.tickerData = tickerData;
        }

        public TickerUpdStruct tickerData { get; set; }
    }

    public class SslEventArgs : EventArgs
    {
        public SslEventArgs(bool messageData)
        {
            sslVisible = messageData;
        }

        public bool sslVisible { get; set; }
    }


    public class MyEventArgs : EventArgs
    {
        public MyEventArgs(string messageData)
        {
            Message = messageData;
        }

        public string Message { get; set; }
    }


    public class EventConnectionDelegateArgs : EventArgs
    {
        public EventConnectionDelegateArgs(int status)
        {
            ConnectionStatus = status;
        }

        public int ConnectionStatus { get; set; }
    }

    public class EventDelegateArgs : EventArgs
    {
        public EventDelegateArgs(string message, LoaderEnum status)
        {
            MessageDescription = message;
            Status = status;
        }

        public string MessageDescription { get; set; }

        public LoaderEnum Status { get; set; }

        //public event EventHandler<EventDelegateArgs> CommsState;
    }

    public class EventDelegateSelectorArgs : EventArgs
    {
        public EventDelegateSelectorArgs(string message)
        {
            MessageDescription = message;
        }

        public string MessageDescription { get; set; }

        //public event EventHandler<EventDelegateSelectorArgs> SelectorMessage;
    }

    public class EventDelegatePriceUpdateArgs : EventArgs
    {
        public EventDelegatePriceUpdateArgs(PriceStruct pricedata, PriceUpdateStruct exchange)
        {
            PriceUpdateObject = pricedata;
            ExchangeLookup = exchange;
        }

        public PriceStruct PriceUpdateObject { get; set; }

        public PriceUpdateStruct ExchangeLookup { get; set; }
    }

    public class EventDelegateMessageArgs : EventArgs
    {
        public EventDelegateMessageArgs(MessageStruct message)
        {
            Message = message;
        }

        public MessageStruct Message { get; set; }
    }

    public class EventDelegateFillDetailArgs : EventArgs
    {
        public EventDelegateFillDetailArgs(object fillmessage)
        {
            filldetail = fillmessage;
        }

        public object filldetail { get; set; }
    }

    public class EventDelegateOrderDetailArgs : EventArgs
    {
        public EventDelegateOrderDetailArgs(object message)
        {
            OrderDetailHistoryObject = message;
        }

        public object OrderDetailHistoryObject { get; set; }

        //public event EventHandler<EventDelegateOrderDetailArgs> OrderDetailMessage;
    }
}