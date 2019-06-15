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
            foreach (string portName in UsrCommunication.SerialClass.GetPortName())
            {
                MenuItem mnuItm1 = new MenuItem();
                mnuItm1.Header = portName;
                mnuItm1.IsCheckable = true;
                mnuItm1.IsChecked = false;
                MnuItmSerial.Items.Add(mnuItm1);
            }
        }
        private void MnuItmCommMethodClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
