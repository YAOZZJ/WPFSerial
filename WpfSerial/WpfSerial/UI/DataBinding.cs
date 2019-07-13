using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace WpfSerial
{
    /// <summary>
    /// MainWindow 绑定源
    /// </summary>
    public class MainDatacontent
    {
        public PlotModel LineGraphModel { get; private set; }
        public MainDatacontent()
        {
            this.Items = new Collection<Item>
            {
                new Item {Label = "lalala",Value = 123}
            };

            int N = 100;
            double[] x = new double[N];
            double[] y = new double[N];
            this.LineGraphModel = new PlotModel { Title = "Example 1" ,LegendPosition = LegendPosition.RightTop, LegendOrientation = LegendOrientation.Vertical };
            //LineGraphModel.LegendOrientation = LegendOrientation.Horizontal;
            var barSerial = new BarSeries() { Title = "bar" };
            var lineSerial = new LineSeries() { Title = "Sin(x)/x" };
            var ccsss = new ColumnSeries();
            ccsss.Items.Add(new ColumnItem(233));
            lineSerial.Points.Add(new DataPoint(0, 0));
            for (int i = 1; i <= N - 1; i++)
            {
                x[i] = (double)i / 10;
                y[i] = Math.Sin(x[i])/x[i];
                lineSerial.Points.Add(new DataPoint(x[i], y[i]));
            }
            barSerial.Items.Add(new BarItem(55));
            barSerial.Items.Add(new BarItem(66));
            this.LineGraphModel.Series.Add(lineSerial);
            this.LineGraphModel.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 0.1, "sin(x)"));
            //this.LineGraphModel.Series.Add(barSerial);
            //this.LineGraphModel.Series.Add(ccsss);
            //this.LineGraphModel.Series.Add(new ColumnSeries {Title = "666" ,ItemsSource = this.Items, ValueField = "Value"});
            var rd = new Random();
            var lineSerialEamdom = new LineSeries() { Title = "Random" };
            this.LineGraphModel.Series.Add(lineSerialEamdom);
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var x1 = rd.NextDouble() * 1000 % 10;
                    var y1 = rd.NextDouble() * 50 % 9;
                    lineSerialEamdom.Points.Add(new DataPoint(x1, y1));
                    LineGraphModel.InvalidatePlot(true);
                    Thread.Sleep(500);
                }
            });

        }
        public Collection<Item> Items { get; set; }
    }
    
    public class Item
    {
        public string Label { get; set; }
        public double Value { get; set; }
    }
        

        class DataBinding : INotifyPropertyChanged//要实现绑定到变量，必须实现INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;//必须实现
        private int intvalue;//私有
        private String strvalue;//私有
        public int IntValue
        {
            get { return intvalue; }//获取值时将私有字段传出；
            set
            {
                intvalue = value;//赋值时将值传给私有字段
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IntValue"));
                //一旦执行了赋值操作说明其值被修改了，则立马通过INotifyPropertyChanged接口告诉UI(IntValue)被修改了
            }
        }
        public String StrValue
        {
            get { return strvalue; }
            set
            {
                strvalue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StrValue"));
            }
        }
    }
}
