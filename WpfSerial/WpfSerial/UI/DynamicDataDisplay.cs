using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using UsrCommunication;
using UsrMethod;
using MahApps.Metro.Controls;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using System.Diagnostics;
using System.Collections;

namespace WpfSerial
{
    public partial class MainWindow : MetroWindow
    {
        //=======================================================================================
        int xaxis = 0;
        int yaxis = 0;
        int group = 100;//组距
        Queue q = new Queue();
        Random random = new Random();

        private ObservableDataSource<Point> dataSource = new ObservableDataSource<Point>();
        private PerformanceCounter performanceCounter = new PerformanceCounter();
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        /// <summary>
        /// 从x轴那个地方开始画
        /// </summary>
        private int currentSecond = 60;

        bool wendu = true;
        //=======================================================================================
        public void DynamicDataDisplayInit()
        {

            plotter.AddLineGraph(dataSource, Colors.Red, 2, "百分比");
            plotter.NewLegendVisible = true;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);
            dispatcherTimer.Tick += timer_Tick;
            dispatcherTimer.IsEnabled = true;
            plotter.Viewport.FitToView();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            performanceCounter.CategoryName = "Processor";
            performanceCounter.CounterName = "% Processor Time";
            performanceCounter.InstanceName = "_Total";

            double x = currentSecond;
            double y = performanceCounter.NextValue();
            //double y = random.Next(40, 50);

            Point point = new Point(x, y);


            dataSource.AppendAsync(base.Dispatcher, point);

            if (wendu)
            {
                if (q.Count < group)
                {
                    q.Enqueue((int)y);//入队
                    yaxis = 0;
                    foreach (int c in q)
                        if (c > yaxis)
                            yaxis = c;
                }
                else
                {
                    q.Dequeue();//出队
                    q.Enqueue((int)y);//入队
                    yaxis = 0;
                    foreach (int c in q)
                        if (c > yaxis)
                            yaxis = c;
                }

                if (currentSecond - group > 0)
                    xaxis = currentSecond - group;
                else
                    xaxis = 0;

                Debug.Write("{0}\n", yaxis.ToString());
                plotter.Viewport.Visible = new System.Windows.Rect(xaxis, 0, group, yaxis);
            }
            currentSecond++;
        }

    }
}