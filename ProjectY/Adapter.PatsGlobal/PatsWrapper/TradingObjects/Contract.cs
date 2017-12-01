using System;
using System.Windows.Controls;

namespace Adapter.PatsGlobal.PatsWrapper
{
    public class Contract : BaseTradingObject
    {
        private string commodityName;
        private string contractDate;
        private string exchangeName;
        private string expiry;
        private string lastTradeDate;
        private int numberOfLegs;
        private int ticksPerPoint;
        private string tickSize;
        //private bool tradable;
        private int GTStatus;
        private string margin;
        private char ESATemplate;
        private string marketRef;
        private string lnExchangeName;
        private string lnCommodityName;
        private string lnContractDate;
        //private LegStruct[] legStruct;
        private Control[] priceListeners;

        //private Commodity commodity;
        //private readonly Prices prices;

        public delegate void UpdatePrices(Contract contract);
        public event UpdatePrices SubscribeTo = delegate { };

        public Contract(ExtendedContractStruct extContract)
        {
            priceListeners = new Control[0];
            //prices = new Prices();

            commodityName = extContract.ContractName;
            contractDate = extContract.ContractDate;
            exchangeName = extContract.ExchangeName;
            expiry = extContract.ExpiryDate;
            lastTradeDate = extContract.LastTradeDate;
            numberOfLegs = extContract.NumberOfLegs;
            ticksPerPoint = extContract.TicksPerPoint;
            tickSize = extContract.TickSize;
            // tradable = YNToBool(extContract.Tradable);
            GTStatus = extContract.GTStatus;
            margin = extContract.Margin;
            ESATemplate = extContract.ESATemplate;
            marketRef = extContract.MarketRef;
            lnExchangeName = extContract.lnExhcangeName;
            lnCommodityName = extContract.lnContractName;
            lnContractDate = extContract.lnContractDate;


            //legStruct = new LegStruct[16];
            //for (int x = 0; x < numberOfLegs; x++)

            //    //legStruct[x] = new LegStruct();
            //    //legStruct[x].contractType = SUP.GetString(10);
            //    //legStruct[x].contractName = SUP.GetString(10);
            //    //legStruct[x].contractDate = SUP.GetString(10);
            //    //legStruct[x].strikePrice = SUP.GetString(10);
            //    //legStruct[x].ratio = SUP.GetString(10);
            //    Application.DoEvents();

            //}


        }

        public override string GetKey()
        {
            return contractDate;
        }

        public override string ToString()
        {
            return contractDate;
        }

        /** each contract has an array of legSruct which contains info on each leg's makeup **/
        public class LegStruct
        {
            public string contractType;
            public string contractDate;
            public string strikePrice;
            public string ratio;
            public string contractName;
            public LegStruct() { }
        }

        public void Subscribe(Control control)
        {
            Array.Resize(ref priceListeners, priceListeners.Length + 1);
            priceListeners.SetValue(control, priceListeners.Length - 1);
            // clientAPI.clientAPIMethods.doSubscribeToContract(this);

            foreach (Control con in priceListeners)
                con.UpdateLayout();

        }

        public void Subscribe(UpdatePrices control)
        {
            //Array.Resize(ref priceListeners, priceListeners.Length + 1);
            //priceListeners.SetValue(control, priceListeners.Length - 1);
            //SubscribeTo += control;
            //  clientAPI.clientAPIMethods.doSubscribeToContract(this);

            foreach (Control con in priceListeners)
                con.UpdateLayout();

        }

        /*private void PriceUpdate(byte[] buffer)
        {            
            prices.PriceUpdate(buffer);
            foreach (Control control in priceListeners)
                clientAPI.guiToolbar.UpdateControl(control);  
            SubscribeTo(this);
        }*/

        //         public PriceStruct Bid
        //         {
        //             get
        //             {
        //                 return prices.Bid;
        //             }
        //         }
    }
}
