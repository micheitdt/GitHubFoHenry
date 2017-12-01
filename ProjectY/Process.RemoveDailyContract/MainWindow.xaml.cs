using ServiceStack.Redis;
using System;
using System.Threading;
using System.Windows;


namespace Process.RemoveDailyContract
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DefaultSettings.Instance.Initialize();
            tbMessage.Text = string.Format("REDIS_IP={0}", DefaultSettings.Instance.REDIS_IP) + Environment.NewLine;
            tbMessage.Text += string.Format("REDIS_PORT={0}", DefaultSettings.Instance.REDIS_PORT) + Environment.NewLine;
            tbMessage.Text += string.Format("REDIS_DB_NUMBER={0}", DefaultSettings.Instance.REDIS_DB_NUMBER);

            Process(DefaultSettings.Instance.REDIS_IP, DefaultSettings.Instance.REDIS_PORT, DefaultSettings.Instance.REDIS_DB_NUMBER, DefaultSettings.Instance.KEYS_TO_REMOVE);
        }

        private void Process(string redisIP, int redisPort, long redisDbNumber, string[] keys)
        {
            ThreadPool.QueueUserWorkItem(x =>
            {
                using (var db = new RedisClient(redisIP, redisPort, null, redisDbNumber))
                {
                    db.Del(keys);
                }
                Environment.Exit(0);
            });
        }
    }
}
