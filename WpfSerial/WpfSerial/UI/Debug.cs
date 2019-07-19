using MahApps.Metro.Controls;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
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
        private void DebugStartup()
        {
        }
        private void BtnDebug1Click(object sender, RoutedEventArgs e)
        {
            txtDebug.Text = (MainDatacontent.Items[0].Value1++).ToString();
        }
        private void BtnDebug2Click(object sender, RoutedEventArgs e)
        {
        }
        private void BtnDebug3Click(object sender, RoutedEventArgs e)
        {
        }
        private void BtnDebug4Click(object sender, RoutedEventArgs e)
        {
        }
        private void BtnDebug5Click(object sender, RoutedEventArgs e)
        {
            txtDebug.Clear();
        }
    }
}