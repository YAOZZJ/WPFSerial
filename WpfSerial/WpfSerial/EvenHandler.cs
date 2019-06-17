using System.Windows;
using System.Windows.Controls;
using UsrCommunication;

namespace WpfSerial
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MnuItmExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void MnuItmFleshClick(object sender, RoutedEventArgs e)
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
        private void MnuItmCommMethodClick(object sender, RoutedEventArgs e)
        {
            MenuItem mnuItm1 = (MenuItem)sender;
            //StatItmMessage.Content = mnuItm1.Header;
            switch (mnuItm1.Header)
            {
                case "_TCPClient":
                    if(MnuItmTCPClient.IsChecked)
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
                case "COM1":
                case "COM2":
                case "COM3":
                case "COM4":
                    //StatItmMessage.Content = MnuItmSerial.Items.CurrentItem.ToString();
                    break;
                default:
                    break;
            }
        }
    }
}
