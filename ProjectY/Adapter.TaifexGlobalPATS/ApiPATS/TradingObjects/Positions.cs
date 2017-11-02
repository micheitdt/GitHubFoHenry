using System;

using C_Sharp_Demo_App.Classes;

namespace C_Sharp_Demo_App.PatsystemsAPI.TradingObjects
{
    public static class Positions
    {
        public static object DoGetPositionTag(ptPositionType positionType, PriceUpdateStruct exchangeinfo, string traderaccount, PriceStruct priceDetail)
        {
            var pos = new PositionManager(exchangeinfo, traderaccount, priceDetail);
            
            switch (positionType)
            {
                case ptPositionType.po_NetPos:
                    {
                        return pos.NetPos();
                    }
                case ptPositionType.po_Average:
                    {
                        return pos.AvPrice().ToString();
                    }
                case ptPositionType.po_Last:
                    {
                        var prices = new PriceStruct();
                        if (ClientAPIMethods.DoGetPriceForContract(exchangeinfo, traderaccount, ref prices))
                            return prices.Last0;
                        return "";
                    }

                case ptPositionType.po_CashBuyingPower:
                    {
                        return Convert.ToString(pos.CashBuyingPowerRemaining());
                    }
                case ptPositionType.po_BuyingPowerRemaining:
                    {
                        return pos.BPremaining();
                    }

                case ptPositionType.po_Contract:
                    break;
                case ptPositionType.po_With_Margin_Buys_Avg_Price:
                    break;
                case ptPositionType.po_With_Margin_Sells_avg_price:
                    break;

                case ptPositionType.po_Buys:
                    {
                        return pos.Buys();
                    }

                case ptPositionType.po_Sells:
                    {
                        return pos.Sells();
                    }

                case ptPositionType.po_Open_Pl:
                    {
                        return pos.OpenPL();
                    }
                case ptPositionType.po_Cum_Pl:
                    break;
                case ptPositionType.po_Total_Pl:
                    {
                        return Math.Round(pos.TotalPL());
                    }

                case ptPositionType.po_Commission:
                    break;
                case ptPositionType.po_MarginPerLot:
                    {
                        return pos.MarginPerLot();
                    }


                case ptPositionType.po_Position_End:
                    break;

                case ptPositionType.po_ContractType:
                    {
                        var commodity = new CommodityStruct();
                        if (ClientAPIMethods.DoGetCommodityByName(ref exchangeinfo, ref commodity))
                            return commodity.ContractName;
                        return "";
                    }

                case ptPositionType.po_PLBurnRate:
                    {
                        return pos.PLburnRate();
                    }

                case ptPositionType.po_OpenPositionExposure:
                    {
                        return pos.OpenPositionExposure();
                    }

                case ptPositionType.po_MarginPaid:
                    {
                        return pos.TotalMarginPaid();
                    }
                default:
                    {
                        return null;
                    }

            }
            return null;
        }
    }
}
