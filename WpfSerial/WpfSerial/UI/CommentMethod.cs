using MahApps.Metro.Controls;
using System;
using System.Windows.Threading;
using UsrCommunication;
using UsrMethod;

namespace WpfSerial
{
    public partial class MainWindow : MetroWindow
    {
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
            foreach (string str1 in UsrNetworkControl.GetLocalIP())
            {
                //cmbLocalNetAddr.Items.Clear();
                cmbLocalNetAddr.Items.Add(str1);
            }
            //***************************************************************
            //默认选项
            cmbSerialPortName.SelectedIndex = 0;
            cmbBaudRate.SelectedIndex = 1;
            cmbDataBits.SelectedIndex = 0;
            cmbStopbits.SelectedIndex = 1;
            cmbParity.SelectedIndex = 0;
            cmbCommMethod.SelectedIndex = 0;
            cmbLocalNetAddr.SelectedIndex = cmbLocalNetAddr.Items.Count - 1;
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
            //***************************************************************
            //gridSplitter1.Visibility = Visibility.Collapsed;
            //tabOthers.Visibility = Visibility.Collapsed;
            //DynamicDataDisplayInit();
            DebugStartUp();
        }
        /// <summary>
        /// 显示用户消息
        /// </summary>
        /// <param name="message"></param>
        private void Message(string message)
        {
            StatItmMessage.Content = message;
        }
        /// <summary>
        /// 时间刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeCycle_1S(object sender, EventArgs e)
        {
            txtblkCurrentTime.Text = DateTime.Now.ToLocalTime().ToString();
        }
        /// <summary>
        /// 定时发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerCycleSend(object sender, EventArgs e)
        {
            SendData();
        }
        #endregion
    }
}
