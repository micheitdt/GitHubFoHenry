﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NetMQ;
using NetMQ.Sockets;
using System.Threading;

namespace Adapter.ESGlobalMD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DefaultSettings.Instance.Initialize();
            tbMessage.Text = string.Format("MD_IP={0}", DefaultSettings.Instance.MD_IP) + Environment.NewLine;
            tbMessage.Text += string.Format("MD_PORT={0}", DefaultSettings.Instance.MD_PORT) + Environment.NewLine;
            tbMessage.Text += string.Format("MD_ID={0}", DefaultSettings.Instance.MD_ID) + Environment.NewLine;
            tbMessage.Text += string.Format("PUB_ADDRESS={0}", DefaultSettings.Instance.PUB_ADDRESS);

            //RunUDP(DefaultSettings.Instance.LOCAL_UDP_IP, DefaultSettings.Instance.UDP_IP, DefaultSettings.Instance.UDP_PORT, DefaultSettings.Instance.PUB_ADDRESS);
        }

        private void RunUDP(string LOCAL_UDP_IP, string UDP_IP, int UDP_PORT, string PUB_ADDRESS)
        {
            //var socket = new PublisherSocket(PUB_ADDRESS);
            //var thread = new Thread(() =>
            //{
            //    try
            //    {
            //        using (var client = new UdpClient())
            //        {
            //            client.ExclusiveAddressUse = false;
            //            var localEp = new IPEndPoint(IPAddress.Any, UDP_PORT);
            //            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            //            client.ExclusiveAddressUse = false;
            //            client.Client.Bind(localEp);
            //            client.JoinMulticastGroup(IPAddress.Parse(UDP_IP), IPAddress.Parse(LOCAL_UDP_IP));
            //            byte[] bytes;
            //            string formatCode;
            //            string prefix;
            //            string productID;
            //            while (true)
            //            {
            //                bytes = client.Receive(ref localEp);
            //                if (bytes[0] == 27)
            //                {
            //                    formatCode = Encoding.ASCII.GetString(bytes, 1, 1) + Encoding.ASCII.GetString(bytes, 2, 1);
            //                    switch (formatCode)
            //                    {
            //                        case "21":
            //                        case "22":
            //                        case "27":
            //                        case "28":
            //                            productID = Encoding.ASCII.GetString(bytes, 14, 20).Trim();
            //                            prefix = string.Format("2#{0}#{1}", formatCode, productID);
            //                            socket.SendMoreFrame(prefix).SendFrame(bytes);
            //                            break;
            //                        default:
            //                            prefix = string.Format("2#{0}", formatCode);
            //                            socket.SendMoreFrame(prefix).SendFrame(bytes);
            //                            break;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //});
            //thread.Start();
        }
    }
}
