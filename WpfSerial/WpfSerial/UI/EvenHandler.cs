using MahApps.Metro.Controls;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using UsrCommunication;
using UsrMethod;

namespace WpfSerial
{
    public partial class MainWindow : MetroWindow
    {
        #region Global data
        SerialClass SerialPort1 = new SerialClass();
        DataBinding SendDataCounterBing = new DataBinding();
        DataBinding RecvDataCounterBing = new DataBinding();
        DispatcherTimer TimerSend = new DispatcherTimer();

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
        /// <summary>
        /// 串口打开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 串口关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            if (!SerialPort1.IsOpen)
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
            SendData();
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        private void SendData()
        {
            if (!SerialPort1.IsOpen) return;
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
        /// <summary>
        /// 通讯方式选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbCommMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb1 = sender as ComboBox;
            //tabCtrlCommMethod.SelectedIndex = cmb1.SelectedIndex;
            if (cmb1.SelectedIndex > 1) tabCtrlCommMethod.SelectedIndex = 1;
            else tabCtrlCommMethod.SelectedIndex = cmb1.SelectedIndex;
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
        /// <summary>
        /// 判断输入是否数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtSendCyclePreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {

            Regex re = new Regex(@"[0-9]");
            e.Handled = !re.IsMatch(e.Text);
        }
        /// <summary>
        /// Hex发送更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Hex显示更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 最前端显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuItmTopDisplayChecked(object sender, RoutedEventArgs e)
        {
            // MenuItem chk1 = sender as MenuItem;
            //this.Topmost = (bool)chk1.IsChecked;
            this.Topmost = (bool)mnuItmTopDisplay.IsChecked;
        }
        private void MnuItmHideParameterChecked(object sender, RoutedEventArgs e)
        {
            if ((bool)mnuItmHideParameter.IsChecked)
            {
                this.docPnlParameter.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.docPnlParameter.Visibility = Visibility.Visible;
            }
        }
        private void MnuItmOthersClick(object sender, RoutedEventArgs e)
        {
            if ((bool)mnuItmOthers.IsChecked)
            {
                tabOthers.Visibility = Visibility.Visible;
                gridSplitter1.Visibility = Visibility.Visible;
                RowDefOthers.Height = GridLength.Auto;
                RowDefRecv.Height = default;
            }
            else
            {
                tabOthers.Visibility = Visibility.Collapsed;
                gridSplitter1.Visibility = Visibility.Collapsed;
                RowDefRecv.Height = default;
                RowDefRecv.Height = GridLength.Auto;
            }
        }
        /// <summary>
        /// 定时发送选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChkSendTimerChecked(object sender, RoutedEventArgs e)
        {
            CheckBox chk1 = sender as CheckBox;
            int time = Convert.ToInt32(txtSendCycle.Text);//异常处理未添加
            if ((bool)chk1.IsChecked)
            {
                TimerSend.Tick += new EventHandler(TimerCycleSend);
                TimerSend.Interval = new TimeSpan(0, 0, 0, 0, time);
                TimerSend.Start();
            }
            else
            {
                TimerSend.Stop();
                TimerSend.Tick -= new EventHandler(TimerCycleSend);
            }
        }

        private void MnuItmOpen_Click(object sender, RoutedEventArgs e)
        {
            string path = UsrTextOperate.OpenDialog();
            UsrMethod.UsrTextOperate usrFile = new UsrTextOperate(path, "", Encoding.Default);

            usrFile.Clear();
            UsrMethod.UsrTextOperate usrFile2 = new UsrTextOperate(path, "", Encoding.Default);
            txtSendData.Text = usrFile2.ReadLine(seek: "all");
        }

        private void MnuItmSave_Click(object sender, RoutedEventArgs e)
        {
            string path = UsrTextOperate.OpenDialog();
            UsrMethod.UsrTextOperate usrFile = new UsrTextOperate(path, "", Encoding.Default);
            usrFile.Clear();
            usrFile = new UsrTextOperate(path, "", Encoding.Default);
            usrFile.Write(txtRecvData.Text);
        }

       
    }
    #endregion
}
