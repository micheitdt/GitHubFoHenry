using System.Collections;

namespace Adapter.PatsGlobal.PatsWrapper
{
    /** Exchange object contains all the exchange information including a list of all of its commodities**/
    public class   Exchange : BaseTradingObject
    {
        private readonly string name;
        private bool queryEnabled;
        private bool amendEnabled;
        private int strategy;
        private bool customDecimals;
        //private int decimalPlaces;
        private char ticketType;
        private bool RFQAccept;
        private bool RFQTickDown;
        private bool blockTrades;
        private bool basisTrades;
        private bool againstActuals;
        private bool crossTrades;

        private readonly Hashtable commodities;
        private readonly Hashtable orderTypes;

        public Exchange(ExchangeStruct exchange)
        {
            name = exchange.ExchangeName;
            queryEnabled = exchange.QueryEnabled == 'Y';
            amendEnabled = exchange.AmendEnabled == 'Y';
            strategy = exchange.Strategy;
            customDecimals = exchange.Decimals == 'Y';
            ticketType = exchange.TicketType;
            RFQAccept = exchange.RFQA == 'Y';
            RFQTickDown = exchange.RFQT == 'Y';
            blockTrades = exchange.EnableBlock == 'Y';
            basisTrades = exchange.EnableBasis == 'Y';
            againstActuals = exchange.EnableAA == 'Y';
            crossTrades = exchange.EnableCross == 'Y';
            commodities = new Hashtable();
            orderTypes = new Hashtable();
        }

        /** returns the unique key of the exchange **/
        public override string GetKey()
        {
            return name;
        }

        public override string  ToString()
        {
 	         return name;
        }

        /** adds a commodity to the exchange's table of commodities**/
        public void PutCommodity(Commodity commodity)
        {
            commodities.Add(commodity.GetKey(), commodity);
        }

        /** returns the commodity specified by the key**/
        public Commodity GetCommodity(string commodityKey)
        {
            return (Commodity)commodities[commodityKey];        
        }

        /** adds the ordertype to the table of ordertypes **/
        public void PutOrderType(OrderType orderType)
        {
            orderTypes.Add(orderType.GetKey(), orderType);
        }

        /** returns the order type specified by the key **/
        public OrderType GetorderType(string orderTypeKey)
        {
            return (OrderType)orderTypes[orderTypeKey];
        }

        /** returns the enumerator for the hash table of commodities **/
        public IDictionaryEnumerator GetCommoditiesEnumerator()
        {
            return commodities.GetEnumerator();
        }

        /** returns the enumerator for the hash table of commodities **/
        public IDictionaryEnumerator GetOrderTypesEnumerator()
        {
            return orderTypes.GetEnumerator();
        }

    }
}
