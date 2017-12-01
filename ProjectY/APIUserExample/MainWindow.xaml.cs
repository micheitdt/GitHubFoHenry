using APIUserExample.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace APIUserExample
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = MainViewModel.Instance;
        }

        private void BtnIPConnect_Click(object sender, RoutedEventArgs e)
        {
            this.BtnIPConnect.IsEnabled = false;
            this.MarketControlAry.IsEnabled = true;
            this.ContentControlAry.IsEnabled = true;
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
