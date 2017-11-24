﻿using ServiceStack.Redis;
using System;
using System.Windows;

namespace Service.Redis.CleanMD
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
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
            Environment.Exit(0);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DefaultSettings.Instance.Initialize();
            tbMessage.Text += string.Format("REDIS_DB_IP={0}", DefaultSettings.Instance.REDIS_DB_IP) + Environment.NewLine;
            tbMessage.Text += string.Format("REDIS_DB_PORT={0}", DefaultSettings.Instance.REDIS_DB_PORT);

            RedisClient client = new RedisClient(DefaultSettings.Instance.REDIS_DB_IP, DefaultSettings.Instance.REDIS_DB_PORT);//預設第1個db
            client.FlushDb();
            MessageBox.Show("已清空Redis資料", "注意", MessageBoxButton.OK);
            Environment.Exit(0);
        }
    }
}
