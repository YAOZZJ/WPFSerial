using System;
using System.ComponentModel;

namespace WpfSerial
{
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
