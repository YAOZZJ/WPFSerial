using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace WpfSerial
{
    #region OxyPlot 绑定源
    /// <summary>
    /// MainWindow 绑定源
    /// </summary>
    public class MainDatacontent
    {
        public PlotModel ModelLineGraph { get; private set; }
        public PlotModel ModelColumnChartGraph { get; private set; }
        public MainDatacontent()
        {
            OxyPlotViewLineGraph();
            OxyPlotViewColumnChartGraph();
        }
        private void OxyPlotViewLineGraph()
        {
            // Create the plot model
            var tmp = new PlotModel { Title = "Simple example", Subtitle = "using OxyPlot" };
            //===================================================================================
            //sin(x)/x,使用描点法
            //Color = OxyColors.Goldenrod,有MarkerType就不能有颜色
            var lineSeriesSinX = new LineSeries { Title = "Sin(x)/x", MarkerType = MarkerType.Circle };
            int N = 100;
            double[] x = new double[N];
            double[] y = new double[N];
            x[0] = 0; y[0] = 0;
            x[1] = 100; y[1] = 50;
            for (int i = 1; i <= N - 1; i++)
            {
                x[i] = (double)i / 1;
                y[i] = Math.Sin(x[i]) / x[i];
                lineSeriesSinX.Points.Add(new DataPoint(x[i], y[i]));
            }
            //===================================================================================
            var lineSeriesRadom = new LineSeries { Title = "Radom" };

            //===================================================================================
            //添加到ModelLineGraph
            tmp.Series.Add(lineSeriesSinX);
            tmp.Series.Add(new FunctionSeries(Math.Sin, 0, 100, 0.1, "sin(x)"));//sin(x),使用函数
            tmp.Series.Add(lineSeriesRadom);
            //===================================================================================
            //应用
            this.ModelLineGraph = tmp;
            //===================================================================================
            var rd = new Random();
            Task.Factory.StartNew(() =>
            {
                while (lineSeriesRadom.Points.Count < 20)
                {
                    var x1 = rd.NextDouble() * 1000 % 10;
                    var y1 = rd.NextDouble() * 50 % 9;
                    lineSeriesRadom.Points.Add(new DataPoint(x1, y1));
                    this.ModelLineGraph.InvalidatePlot(true);
                    Thread.Sleep(500);

                }
            });
        }
        private void OxyPlotViewColumnChartGraph()
        {
            // Create some data
            this.Items = new Collection<Item>
                            {
                                new Item {Label = "Apples", Value1 = 37, Value2 = 12, Value3 = 19},
                                new Item {Label = "Pears", Value1 = 7, Value2 = 21, Value3 = 9},
                                new Item {Label = "Bananas", Value1 = 23, Value2 = 2, Value3 = 29}
                            };

            // Create the plot model
            var tmp = new PlotModel { Title = "Column series", LegendPlacement = LegendPlacement.Outside, LegendPosition = LegendPosition.RightTop, LegendOrientation = LegendOrientation.Vertical };

            // Add the axes, note that MinimumPadding and AbsoluteMinimum should be set on the value axis.
            tmp.Axes.Add(new CategoryAxis { ItemsSource = this.Items, LabelField = "Label" });
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Left, MinimumPadding = 0, AbsoluteMinimum = 0 });

            // Add the series, note that the BarSeries are using the same ItemsSource as the CategoryAxis.
            tmp.Series.Add(new ColumnSeries { Title = "2009", ItemsSource = this.Items, ValueField = "Value1" });
            tmp.Series.Add(new ColumnSeries { Title = "2010", ItemsSource = this.Items, ValueField = "Value2" });
            tmp.Series.Add(new ColumnSeries { Title = "2011", ItemsSource = this.Items, ValueField = "Value3" });

            this.ModelColumnChartGraph = tmp;
        }

        public Collection<Item> Items { get; set; }

        public PlotModel Model1 { get; set; }
    }
    public class Item
    {
        public string Label { get; set; }
        public double Value1 { get; set; }
        public double Value2 { get; set; }
        public double Value3 { get; set; }
    }
    #endregion
}