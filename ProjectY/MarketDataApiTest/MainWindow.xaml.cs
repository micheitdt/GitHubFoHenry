using MarketDataApi;
using System;
using System.Windows;

namespace MarketDataApiTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MarketDataApi.MarketDataApi api;
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //api = new MarketDataApi.MarketDataApi("203.66.93.83", 6688);
            api = new MarketDataApi.MarketDataApi("127.0.0.1", 6688);
            api.TaifexI020Received += api_TaifexI020Received;
            api.TaifexI080Received += api_TaifexI080Received;
            api.TseFormat6Received += api_TseFormat6Received;
            api.TpexFormat6Received += api_TpexFormat6Received;
        }

        void api_TpexFormat6Received(object sender, MarketDataApi.MarketDataApi.TpexFormat6ReceivedEventArgs e)
        {           
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                this.Title = string.Format("TPEX,{0},{1},{2},{3}", e.PacketData.StockID, e.PacketData.LastPrice, e.PacketData.TotalVolume, e.PacketData.BidData[0].Price);
            }));            
        }

        void api_TseFormat6Received(object sender, MarketDataApi.MarketDataApi.TseFormat6ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                this.Title = string.Format("TSE,{0},{1},{2},{3}", e.PacketData.StockID, e.PacketData.LastPrice, e.PacketData.TotalVolume, e.PacketData.BidData[0].Price);
            }));            
        }

        void api_TaifexI080Received(object sender, MarketDataApi.MarketDataApi.TaifexI080ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                this.Title = string.Format("I080,{0},{1},{2},{3},{4}", e.PacketData.B_ProdId, e.PacketData.B_BuyOrderBook[0].MatchPrice, e.PacketData.B_BuyOrderBook[0].MatchQuantity, e.PacketData.B_SellOrderBook[0].MatchPrice, e.PacketData.B_SellOrderBook[0].MatchQuantity);
            }));            
        }

        void api_TaifexI020Received(object sender, MarketDataApi.MarketDataApi.TaifexI020ReceivedEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                this.Title = string.Format("I020,{0},{1}", e.PacketData.B_ProdId, e.PacketData.B_FirstMatchPrice);
            }));            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AdapterCode adapterCode;
            switch (tbAdapter.Text)
            {
                case "0":
                    adapterCode = AdapterCode.TSE; break;
                case "1":
                    adapterCode = AdapterCode.TPEX; break;
                case "2":
                    adapterCode = AdapterCode.TAIFEX_FUTURES_DAY; break;
                case "3":
                    adapterCode = AdapterCode.TAIFEX_OPTIONS_DAY; break;
                case "4":
                    adapterCode = AdapterCode.TAIFEX_FUTURES_NIGHT; break;
                case "5":
                    adapterCode = AdapterCode.TAIFEX_OPTIONS_NIGHT; break;
                default:
                    //adapterCode = AdapterCode.TSE;
                    return;
            }
            api.Sub(adapterCode, tbType.Text, tbSymbol.Text);
        }
    }
}
