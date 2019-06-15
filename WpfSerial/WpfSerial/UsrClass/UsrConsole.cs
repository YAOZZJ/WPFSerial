using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace UsrConsole
{
    class UsrConsoleClass
    {
        [DllImport("Kernel32.dll")]
        private static extern bool AllocConsole(); //启动窗口   
        [DllImport("kernel32.dll", EntryPoint = "FreeConsole")]
        private static extern bool FreeConsole();      //释放窗口，即关闭   
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        extern static IntPtr FindWindow(string lpClassName, string lpWindowName);//找出运行的窗口   

        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        extern static IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert); //取出窗口运行的菜单   

        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        extern static IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags); //灰掉按钮

        /// <summary>
        /// 在UI用调用console
        /// </summary>
        public UsrConsoleClass()
        {
            AllocConsole();
            IntPtr windowHandle = FindWindow(null, Process.GetCurrentProcess().MainModule.FileName);
            IntPtr closeMenu = GetSystemMenu(windowHandle, IntPtr.Zero);
            uint SC_CLOSE = 0xF060;
            RemoveMenu(closeMenu, SC_CLOSE, 0x0);
            Console.BackgroundColor = ConsoleColor.Green;
            //Console.WindowLeft = 100;
            //Console.WindowTop = 1;
            //#if DEBUG
            //#else
            try
            {
                Console.Clear();//必须
                                //logger.Trace("输出一条记录信息成功！");//最常见的记录信息，一般用于普通输出
                                //logger.Debug("输出一条Debug信息成功！"); //同样是记录信息，不过出现的频率要比Trace少一些，一般用来调试程序
                                //logger.Info("输出一条消息类型信息成功！");//信息类型的消息
                                //logger.Warn("输出一条警告信息成功");//警告信息，一般用于比较重要的场合
                                //logger.Error("输出一条错误信息成功！");//错误信息
                                //logger.Fatal("输出一条致命信息成功！");//致命异常信息。一般来讲，发生致命异常之后程序将无法继续执行。
            }
            catch (System.Exception)
            { }
            //#endif
        }
    }
}
