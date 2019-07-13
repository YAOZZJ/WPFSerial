using OxyPlot;
using System;
using System.ComponentModel;

namespace WpfSerial
{
    class DataBinding : INotifyPropertyChanged//要实现绑定到变量，必须实现INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;//必须实现
        
        //可以通过该方法触发事件,不知道跟PropertyChanged?.Invoke哪个好
        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
        private int intvalue;//私有
        private String strvalue;//私有
        private PlotModel plotModel;
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
        public PlotModel PlotModel
        {
            get
            {
                return plotModel;
            }
            set
            {
                plotModel = value;
                RaisePropertyChanged("PlotModel");
            }
        }
    }
}
