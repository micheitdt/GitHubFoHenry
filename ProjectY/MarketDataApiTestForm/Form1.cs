using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MarketDataApi;

namespace MarketDataApiTest
{
    public partial class Form1 : Form
    {
        MarketDataApi.MarketDataApi api;
        private delegate void UIInvoke( String MsgStr );
        private UIInvoke UI;
        //------------------------------------------------------------------------------------------------------------------------------------------
        public Form1()
        {
            InitializeComponent();
            UI = new UIInvoke( Update_UI );
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            api = new MarketDataApi.MarketDataApi( IPtxt.Text, 6688 );
            api.TaifexI020Received  += api_TaifexI020Received;  /// <- 期貨I020[成交]回呼事件
            api.TaifexI080Received  += api_TaifexI080Received;  /// <- 期貨I080[委買委賣]回呼事件
            api.TseFormat6Received  += api_TseFormat6Received;  /// <- 上市現貨格式6(Format6)回呼事件
            api.TpexFormat6Received += api_TpexFormat6Received; /// <- 上櫃現貨格式6(Format6)回呼事件

            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <- 註冊報價(區分I020,I080,現貨格式6)
        private void button2_Click(object sender, EventArgs e)
        {           
            api.Sub( Rtn_adapterCode( AdapterCodetxt.Text ), tbType.Text, tbSymbol.Text );
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <- 取消註冊報價
        private void button3_Click(object sender, EventArgs e)
        {
            api.UnSub( Rtn_adapterCode( AdapterCodetxt.Text ), tbType.Text, tbSymbol.Text );
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        void Update_UI( String MsgStr )
        {
            if ( tbMessage.Lines.Length == 50 )
                tbMessage.Text = "";
            tbMessage.AppendText( MsgStr + "\r\n" );
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <- 期貨I080[委買委賣]回呼事件
        void api_TaifexI080Received( object sender, MarketDataApi.MarketDataApi.TaifexI080ReceivedEventArgs e )
        {
            /// <- Bid Asl 0~1表示1檔~5檔
            String fData;
            fData = string.Format( "5檔I080,{0},Bid1 {1}({2}),Bid2 {3}({4}),Ask1 {5}({6}),Ask2 {7}({8})"
                , e.PacketData.B_ProdId
                , e.PacketData.B_BuyOrderBook[ 0 ].MatchPrice
                , e.PacketData.B_BuyOrderBook[ 0 ].MatchQuantity
                , e.PacketData.B_BuyOrderBook[1].MatchPrice
                , e.PacketData.B_BuyOrderBook[1].MatchQuantity
                , e.PacketData.B_SellOrderBook[ 0 ].MatchPrice
                , e.PacketData.B_SellOrderBook[ 0 ].MatchQuantity                
                , e.PacketData.B_SellOrderBook[ 1 ].MatchPrice
                , e.PacketData.B_SellOrderBook[ 1 ].MatchQuantity );
            this.BeginInvoke( UI, new object[] { fData } );
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <- 期貨I020[成交]回呼事件
        void api_TaifexI020Received( object sender, MarketDataApi.MarketDataApi.TaifexI020ReceivedEventArgs e )
        {
            String fData;
            fData = string.Format( "成交I020,{0},{1}({2})({3}),{4}", e.PacketData.B_ProdId
                , e.PacketData.B_FirstMatchPrice
                , e.PacketData.B_FirstMatchQnty
                , e.PacketData.B_MatchTotalQty
                , e.PacketData.H_InformationTime );
            this.BeginInvoke( UI, new object[] { fData } );
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <- 上櫃現貨格式6回呼事件
        void api_TpexFormat6Received( object sender, MarketDataApi.MarketDataApi.TpexFormat6ReceivedEventArgs e )
        {
            String fData;
            fData = string.Format( "Tpex(上櫃),{0},{1}({2}),{3}({4},{5}({6})"
                , e.PacketData.StockID
                , e.PacketData.LastPrice
                , e.PacketData.TotalVolume
                , e.PacketData.BidData[ 0 ].Price
                , e.PacketData.BidData[ 0 ].Volume
                , e.PacketData.AskData[ 0 ].Price
                , e.PacketData.AskData[ 0 ].Volume );
            this.BeginInvoke( UI, new object[] { fData } );
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <- 上市現貨格式6回呼事件
        void api_TseFormat6Received( object sender, MarketDataApi.MarketDataApi.TseFormat6ReceivedEventArgs e )
        {
            String fData;
            fData = string.Format( "TSE(上櫃),{0},{1}({2}),{3}({4},{5}({6})"
                , e.PacketData.StockID
                , e.PacketData.LastPrice
                , e.PacketData.TotalVolume
                , e.PacketData.BidData[ 0 ].Price
                , e.PacketData.BidData[ 0 ].Volume
                , e.PacketData.AskData[ 0 ].Price
                , e.PacketData.AskData[ 0 ].Volume );
            this.BeginInvoke(UI, new object[] { fData } );
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
        /// <- AdapterCode轉換
        AdapterCode Rtn_adapterCode( string Str )
        {
            AdapterCode adapterCode = AdapterCode.TSE; ;
            switch ( AdapterCodetxt.Text.Substring( 0, 1 ) )
            {
                case "0":
                    adapterCode = AdapterCode.TSE; break;                  /// <- 上市
                case "1":
                    adapterCode = AdapterCode.TPEX; break;                 /// <- 上櫃
                case "2":
                    adapterCode = AdapterCode.TAIFEX_FUTURES_DAY; break;   /// <- 期貨AM盤
                case "3":
                    adapterCode = AdapterCode.TAIFEX_OPTIONS_DAY; break;   /// <- 選擇權AM盤
                case "4":
                    adapterCode = AdapterCode.TAIFEX_FUTURES_NIGHT; break; /// <- 期貨PM盤
                case "5":
                    adapterCode = AdapterCode.TAIFEX_OPTIONS_NIGHT; break; /// <- 選擇權PM盤
                default:
                    adapterCode = AdapterCode.TSE;
                    break;
            }
            return adapterCode;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------
    }
}
