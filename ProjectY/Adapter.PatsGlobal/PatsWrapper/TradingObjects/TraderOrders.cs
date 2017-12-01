namespace Adapter.PatsGlobal.PatsWrapper
{

    public class TraderOrderID : BaseTradingObject
    {
        private string _orderid = null;
        private string _contractname = null;
        private string _exchange = null;
        private string _ordertype = null;
      

        /*private void TraderOrderIDInfo(string OrderID, string Exchange, string ContractName, string ContractDate, string OrderType, ClientAPI clientAPI)
        {
            _orderid = OrderID;
            _contractname = ContractName;
            _exchange = Exchange;
            _contractdate = ContractDate;
            _ordertype = OrderType;
            clientAPI.PutTraderOrders(this);
        }*/

        public override string GetKey()
        {
            return _orderid;
        }

        public override string ToString()
        {
            return _orderid;
        }
        public string GetContractName()
        {
            return _contractname;
        }
        public string GetExchange()
        {
            return _exchange;
        }
        public string GetOrderID()
        {
            return _orderid;
        }
        public string GetOrderType()
        {
            return _ordertype;
        }
    }
}

 
