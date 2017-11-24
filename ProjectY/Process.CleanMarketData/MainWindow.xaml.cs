using ServiceStack.Redis;
using System;
using System.Windows;

namespace Process.CleanMarketData
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string TSE_FORMAT1_HASH_KEY = "0#1";
        private const string TPEX_FORMAT1_HASH_KEY = "1#1";
        private const string FUTURES_I010_HASH_KEY = "2#11";
        private const string I010_HASH_KEY = "11";
        private const string TSE_FORMAT6_HASH_KEY = "0#6";
        private const string TPEX_FORMAT6_HASH_KEY = "1#6";
        private const string TSE_FORMAT17_HASH_KEY = "0#17";
        private const string TPEX_FORMAT17_HASH_KEY = "1#17";
        private const string I022_HASH_KEY = "27";
        private const string I082_HASH_KEY = "28";
        private const string I020_HASH_KEY = "21";
        private const string I080_HASH_KEY = "22";

        private static readonly string[] ALL_HASH_KEY = { TSE_FORMAT1_HASH_KEY, TPEX_FORMAT1_HASH_KEY, FUTURES_I010_HASH_KEY, I010_HASH_KEY,
        TSE_FORMAT6_HASH_KEY , TPEX_FORMAT6_HASH_KEY, TSE_FORMAT17_HASH_KEY, TPEX_FORMAT17_HASH_KEY, I022_HASH_KEY, I082_HASH_KEY, I020_HASH_KEY, I080_HASH_KEY};

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DefaultSettings.Instance.Initialize();
            tbMessage.Text += string.Format("REDIS_DB_IP={0}", DefaultSettings.Instance.REDIS_DB_IP) + Environment.NewLine;
            tbMessage.Text += string.Format("REDIS_DB_PORT={0}", DefaultSettings.Instance.REDIS_DB_PORT);

            RedisClient client = new RedisClient(DefaultSettings.Instance.REDIS_DB_IP, DefaultSettings.Instance.REDIS_DB_PORT);//預設第1個db
            client.Del(ALL_HASH_KEY);
            MessageBox.Show("已清空Redis資料", "注意", MessageBoxButton.OK);
            Environment.Exit(0);
        }
    }
}
