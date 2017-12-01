namespace Adapter.PatsGlobal.PatsWrapper
{
    public class OrderType : BaseTradingObject
    {
        private readonly string name;
        private string exchangeName;
        private int orderTypeID;
        private byte pricesRequired;
        private byte volumesRequired;
        private byte datesRequired;
        private bool autoCreated;
        private bool timeTriggered;
        private bool exchangeSynthetic;
        private bool GTC;
        private string ticketType;

        public OrderType(OrderTypeStruct ordertype)
        {
            //StructUnPacker SUP = new StructUnPacker(buffer);
            //name = SUP.GetString(Constants.SIZE_OF_ORDERTYPE);
            //exchangeName = SUP.GetString(Constants.SIZE_OF_EXCHANGENAME);
            //orderTypeID = SUP.GetInt();
            //pricesRequired = SUP.GetByte();
            //volumesRequired = SUP.GetByte();
            //datesRequired = SUP.GetByte();
            //autoCreated = YNToBool(SUP.GetChar());
            //timeTriggered = YNToBool(SUP.GetChar());
            //exchangeSynthetic = YNToBool(SUP.GetChar());
            //GTC = YNToBool(SUP.GetChar());
            //ticketType = SUP.GetString(Constants.SIZE_OF_TICKETTYPE);

            name = ordertype.OrderType;
            exchangeName = ordertype.ExchangeName;
            orderTypeID = ordertype.OrderTypeID;
            pricesRequired = ordertype.NumPricesReqd;
            volumesRequired = ordertype.NumVolumesReqd;
            datesRequired = ordertype.NumDatesReqd;
            autoCreated = YNToBool(ordertype.AutoCreated);
            timeTriggered = YNToBool(ordertype.TimeTriggered);
            exchangeSynthetic = YNToBool(ordertype.RealSynthetic);
            GTC = YNToBool(ordertype.GTCFlag);
            ticketType = ordertype.TicketType;
        }

        public override string GetKey()
        {
            return name;    
        }

        public override string ToString()
        {
            return name;
        }
    }
}
