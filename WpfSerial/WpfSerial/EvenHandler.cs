using System.Windows;
using System.Windows.Controls;
using UsrCommunication;

namespace WpfSerial
{
    public partial class MainWindow : Window
    {
        SerialClass serialPort1 = new SerialClass();
        DataBinding SendDataCounterBing = new DataBinding();
        DataBinding RecvDataCounterBing = new DataBinding();

        private void Startup()
        {
            foreach (string str1 in SerialClass.GetSerialPropertyValue(SerialClass.SerialProperty.PortName))
            {
                cmbSerialPortName.Items.Add(str1);
            }
            foreach (string str1 in SerialClass.GetSerialPropertyValue(SerialClass.SerialProperty.BaudRate))
            {
                cmbBaudRate.Items.Add(str1);
            }
            foreach (string str1 in SerialClass.GetSerialPropertyValue(SerialClass.SerialProperty.DataBits))
            {
                cmbDataBits.Items.Add(str1);
            }
            foreach (string str1 in SerialClass.GetSerialPropertyValue(SerialClass.SerialProperty.StopBits))
            {
                cmbStopbits.Items.Add(str1);
            }
            foreach (string str1 in SerialClass.GetSerialPropertyValue(SerialClass.SerialProperty.Parity))
            {
                cmbParity.Items.Add(str1);
            }
            //***************************************************************
            cmbSerialPortName.SelectedIndex = 0;
            cmbBaudRate.SelectedIndex = 1;
            cmbDataBits.SelectedIndex = 0;
            cmbStopbits.SelectedIndex = 1;
            cmbParity.SelectedIndex = 0;
            //***************************************************************
            //btnSendData.IsEnabled = false;
            //***************************************************************
            this.statItmSendCounter.DataContext = SendDataCounterBing;
            this.statItmRecvCounter.DataContext = RecvDataCounterBing;

        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuItmExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// 刷新端口号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuItmFreshClick(object sender, RoutedEventArgs e)
        {
            MnuItmSerial.Items.Clear();
            //int length = UsrCommunication.SerialClass.GetPortName().Length;
            //string[] portName = UsrCommunication.SerialClass.GetPortName();

            //MenuItem[] mnuItm = new MenuItem[length];
            //for(int i = 0;i<length;i++)
            //{
            //    mnuItm[i].Header = portName[i];
            //    mnuItm[i].IsCheckable = true;
            //    mnuItm[i].IsChecked = false;
            //    MnuItmSerial.Items.Add(mnuItm[i]);
            //}
            foreach (string portName in UsrCommunication.SerialClass.GetPortName())
            {
                MenuItem mnuItm1 = new MenuItem
                {
                    Header = portName,
                    IsCheckable = true,
                    IsChecked = false
                };
                mnuItm1.Click += MnuItmCommMethodClick;
                MnuItmSerial.Items.Add(mnuItm1);
            }
        }
        /// <summary>
        /// 通讯方式/端口选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuItmCommMethodClick(object sender, RoutedEventArgs e)
        {
            MenuItem mnuItm1 = (MenuItem)sender;
            string header = (string)mnuItm1.Header;
            MnuItmFreshClick(sender, e);//清除串口isChecked状态
            //StatItmMessage.Content = mnuItm1.Header;
            switch (header)
            {
                case "_TCPClient":
                    if (MnuItmTCPClient.IsChecked)
                    {
                        MnuItmTCPServer.IsChecked = false;
                        MnuItmUDP.IsChecked = false;
                    }
                    break;
                case "_TCPServer":
                    if (MnuItmTCPServer.IsChecked)
                    {
                        MnuItmTCPClient.IsChecked = false;
                        MnuItmUDP.IsChecked = false;
                    }
                    break;
                case "_UDP":
                    if (MnuItmUDP.IsChecked)
                    {
                        MnuItmTCPClient.IsChecked = false;
                        MnuItmTCPServer.IsChecked = false;
                    }
                    break;
                default:
                    //串口选择,重新刷新端口,有没有其他不需要刷新的方法?
                    if (header.Contains("COM"))
                    {
                        MnuItmTCPServer.IsChecked = false;
                        MnuItmUDP.IsChecked = false;
                        MnuItmTCPClient.IsChecked = false;
                        MnuItmSerial.Items.Clear();
                        foreach (string portName in UsrCommunication.SerialClass.GetPortName())
                        {
                            MenuItem mnuItm2 = new MenuItem();
                            mnuItm2.Header = portName;
                            mnuItm2.IsCheckable = true;
                            if (portName == header)
                            {
                                mnuItm2.IsChecked = true;
                            }
                            else
                            {
                                mnuItm2.IsChecked = false;
                            }
                            mnuItm2.Click += MnuItmCommMethodClick;
                            MnuItmSerial.Items.Add(mnuItm2);
                        }
                        //MnuItmFleshClick(sender, e);
                        //StatItmMessage.Content = MnuItmSerial.Items.CurrentItem.ToString();
                    }
                    break;
            }
        }
        private void BtnOpenSerialPortClick(object sender, RoutedEventArgs e)
        {
        }
        private void BtnRefreshSerialPortClick(object sender, RoutedEventArgs e)
        {
            MnuItmFreshClick(sender, e);
        }
        private void BtnClearRecvDataClick(object sender, RoutedEventArgs e)
        {
            txtTerminal.Clear();
        }
        private void BtnClearSendDataClick(object sender, RoutedEventArgs e)
        {
            txtSendData.Clear();
        }
    }
}
