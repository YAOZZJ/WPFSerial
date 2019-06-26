using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using UsrCommunication;

namespace WpfSerial
{
    public partial class MainWindow : Window
    {
        #region Global data
        SerialClass SerialPort1 = new SerialClass();
        DataBinding SendDataCounterBing = new DataBinding();
        DataBinding RecvDataCounterBing = new DataBinding();
        #endregion
        #region 串口通讯事件
        private void SerialPort1_ComReceiveDataEvent(object sender, SerialPortEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if ((bool)chkHexDisplay.IsChecked)
                {
                    txtRecvData.Text += UsrMethod.UsrConversion.Byte2HexString(e.receiveBytes);
                }
                else
                {
                    txtRecvData.Text += UsrMethod.UsrConversion.Byte2String(e.receiveBytes);
                }
                RecvDataCounterBing.IntValue = SerialPort1.receiveBytesCount;
                txtRecvData.ScrollToEnd();
            }));
        }
        private void SerialPort1_ComOpenEvent(object sender, SerialPortEventArgs e)
        {
            if (SerialPort1.IsOpen)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    Message(SerialPort1.GetSerialPortStatus());
                    btnOpenSerialPort.Content = "Close";
                    btnOpenSerialPort.Background = Brushes.Red;
                    statbrMain.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xCA, 0x51, 0x00));
                    cmbBaudRate.IsEnabled = false;
                    cmbCommMethod.IsEnabled = false;
                    cmbDataBits.IsEnabled = false;
                    cmbParity.IsEnabled = false;
                    cmbSerialPortName.IsEnabled = false;
                    cmbStopbits.IsEnabled = false;
                    btnRefreshSerialPort.IsEnabled = false;
                    btnSendData.IsEnabled = true;
                    MnuItmCommPort.IsEnabled = false;
                }));
                SerialPort1.ComReceiveDataEvent += SerialPort1_ComReceiveDataEvent;
            }
            else
            {
                Message(e.message);
            }
        }
        private void SerialPort1_ComCloseEvent(object sender, SerialPortEventArgs e)
        {
            if (!SerialPort1.IsOpen)
            {
                    Dispatcher.Invoke(new Action(() =>
                {
                    Message("Closed");
                    btnOpenSerialPort.Content = "Open";
                    btnOpenSerialPort.Background = default;
                    statbrMain.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x7A, 0xCC));
                    cmbBaudRate.IsEnabled = true;
                    cmbCommMethod.IsEnabled = true;
                    cmbDataBits.IsEnabled = true;
                    cmbParity.IsEnabled = true;
                    cmbSerialPortName.IsEnabled = true;
                    cmbStopbits.IsEnabled = true;
                    btnRefreshSerialPort.IsEnabled = true;
                    btnSendData.IsEnabled = false;
                    MnuItmCommPort.IsEnabled = true;
                }));
                    SerialPort1.ComReceiveDataEvent -= SerialPort1_ComReceiveDataEvent;
            }
        }
        #endregion
            #region 普通方法
            /// <summary>
            /// 程序加载时执行,combobox列表初始化
            /// </summary>
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
            //默认选项
            cmbSerialPortName.SelectedIndex = 0;
            cmbBaudRate.SelectedIndex = 1;
            cmbDataBits.SelectedIndex = 0;
            cmbStopbits.SelectedIndex = 1;
            cmbParity.SelectedIndex = 0;
            cmbCommMethod.SelectedIndex = 0;
            btnSendData.IsEnabled = false;
            //***************************************************************
            //控件使能初始化
            //btnSendData.IsEnabled = false;
            //***************************************************************
            //数据绑定
            this.statItmSendCounter.DataContext = SendDataCounterBing;
            this.statItmRecvCounter.DataContext = RecvDataCounterBing;
            //***************************************************************
            //注册事件
            SerialPort1.ComOpenEvent += SerialPort1_ComOpenEvent;
            SerialPort1.ComCloseEvent += SerialPort1_ComCloseEvent;
            //***************************************************************
            //时间
            DispatcherTimer Timer_1S = new DispatcherTimer();
            Timer_1S.Tick += new EventHandler(TimeCycle_1S);
            Timer_1S.Interval = new TimeSpan(0, 0, 0, 1);
            Timer_1S.Start();
        }
        /// <summary>
        /// 显示用户消息
        /// </summary>
        /// <param name="message"></param>
        private void Message(string message)
        {
            StatItmMessage.Content = message;
        }
        private void TimeCycle_1S(object sender, EventArgs e)
        {
            txtblkCurrentTime.Text = DateTime.Now.ToLocalTime().ToString();
        }
        #endregion
        #region UI事件
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
        /// <summary>
        /// 打开/关闭串口端口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenSerialPortClick(object sender, RoutedEventArgs e)
        {
            if(!SerialPort1.IsOpen)
            {
                SerialPort1.Open(cmbSerialPortName.SelectionBoxItem.ToString(),
                             cmbBaudRate.SelectionBoxItem.ToString(),
                             cmbDataBits.SelectionBoxItem.ToString(),
                             cmbStopbits.SelectionBoxItem.ToString(),
                             cmbParity.SelectionBoxItem.ToString(),
                             "None"
                );
            }
            else
            {
                SerialPort1.Close();
            }
        }
        /// <summary>
        /// 刷新串口号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRefreshSerialPortClick(object sender, RoutedEventArgs e)
        {
            MnuItmFreshClick(sender, e);
        }
        /// <summary>
        /// 清除接收数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClearRecvDataClick(object sender, RoutedEventArgs e)
        {
            txtRecvData.Clear();
        }
        /// <summary>
        /// 清除发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClearSendDataClick(object sender, RoutedEventArgs e)
        {
            txtSendData.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSendDataClick(object sender, RoutedEventArgs e)
        {
            if ((bool)chkHexSend.IsChecked)
            {
                SerialPort1.SerialPortSend(UsrMethod.UsrConversion.HexString2Byte(txtSendData.Text));
            }
            else
            {
                SerialPort1.SerialPortSend(txtSendData.Text);
            }
            SendDataCounterBing.IntValue = SerialPort1.sendBytesCount;
        }
        private void CmbCommMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb1 = sender as ComboBox;
            tabCtrlCommMethod.SelectedIndex = cmb1.SelectedIndex;
        }
        /// <summary>
        /// 正则表达式文本输入判断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtSendData_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if ((bool)chkHexSend.IsChecked)
            {
                Regex re = new Regex(@"[0-9A-Fa-f]");
                e.Handled = !re.IsMatch(e.Text);
            }
        }
        private void ChkHexSendChecked(object sender, RoutedEventArgs e)
        {
            if ((bool)chkHexSend.IsChecked)
            {
                txtSendData.Text = UsrMethod.UsrConversion.String2Hex(txtSendData.Text);
            }
            else
            {
                txtSendData.Text = UsrMethod.UsrConversion.Hex2String(txtSendData.Text);
            }
        }
        private void ChkHexDisplayChecked(object sender, RoutedEventArgs e)
        {
            if ((bool)chkHexDisplay.IsChecked)
            {
                txtRecvData.Text = UsrMethod.UsrConversion.String2Hex(txtRecvData.Text);
            }
            else
            {
                txtRecvData.Text = UsrMethod.UsrConversion.Hex2String(txtRecvData.Text);
            }
        }
        #endregion
    }
}
