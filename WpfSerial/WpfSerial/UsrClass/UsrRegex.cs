using System.Text.RegularExpressions;

namespace WpfSerial.UsrClass
{
    class UsrMethod
    {
        public static bool IsHex(string strData)
        {
            Regex regex = new Regex(@"[a-zA-Z0-9]+@[a-zA-z0-9]+\.com");
            return false;
        }
    }
}
