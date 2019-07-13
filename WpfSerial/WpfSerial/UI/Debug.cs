using MahApps.Metro.Controls;
using OxyPlot;
using OxyPlot.Series;
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
        DataBinding dataBinding = new DataBinding();
        private void DebugStartup()
        {
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            oxyLineGraph.DataContext = dataBinding;


        }
        private void BtnDebug1Click(object sender, RoutedEventArgs e)
        {
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
        private double lastUpdateTime;
        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            double seconds = ((RenderingEventArgs)e).RenderingTime.TotalSeconds;
            if (seconds > this.lastUpdateTime + 0.02)
            {
                dataBinding.PlotModel = CreatePlot();
                lastUpdateTime = seconds;
                //txtDebug.Text = seconds.ToString();
            }
        }
        private double x;
        private PlotModel CreatePlot()
        {
            var pm = new PlotModel { Title = "Plot updated: " + DateTime.Now };
            pm.Series.Add(new FunctionSeries(Math.Sin, x, x + 4, 0.01));
            x += 0.1;
            return pm;
        }
    }
}