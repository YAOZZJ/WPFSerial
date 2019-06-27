using Microsoft.Win32;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace UsrMethod
{
    /// <summary>
    /// 同步文件IO
    /// </summary>
    class UsrTextOperate
    {
        #region 全局变量,类
        FileStream currentFileStream = null;
        StreamWriter streamWriter = null;
        StreamReader streamReader = null;
        private Encoding encoding = Encoding.Default;
        #endregion
        #region 构造函数
        /// <summary>
        /// 打开指定路径的文件
        /// </summary>
        /// <param name="file_path"></param>
        /// <param name="file_cmd"></param>
        public UsrTextOperate(string file_path, string file_cmd,Encoding lencoding)
        {
            if (file_path == null) return;
            switch(file_cmd)
            {
                case "read":
                    currentFileStream = new FileStream(file_path, FileMode.OpenOrCreate, FileAccess.Read);
                    break;

               case "write":
                    currentFileStream = new FileStream(file_path, FileMode.OpenOrCreate, FileAccess.Write);
                    break;
                default:
                    currentFileStream = new FileStream(file_path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    break;
            }
            encoding = lencoding;
        }
        /// <summary>
        /// 关闭文件流
        /// </summary>
        public void Close()
        {
            currentFileStream.Close();
        }
        #endregion
        /// <summary>
        /// 打开文件对话框,获取选择的文件路径
        /// </summary>
        /// <returns></returns>
        public static String  OpenDialog()
        {
            string file = "";
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "所有文件(*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                 file = dialog.FileName;
                
            }
            return file;
        }
        public void Clear()

        {currentFileStream.Seek(0, SeekOrigin.Begin);
            currentFileStream.SetLength(0);
            currentFileStream.Close();
        }
        #region 文本写入
        /// <summary>
        /// 在打开的文本上写入一行.begin,current,end
        /// </summary>
        /// <param name="text"></param>
        /// <param name="lineNumber"></param>
        public void WriteLine(string text,string seek = "end",long lineNumber = 0)
        {
            //可选参数形式:WriteLine(text),WriteLine(text,"end")
            //具名参数形式:WriteLine(text,long 10)
            streamWriter = new StreamWriter(currentFileStream, encoding);
            if (streamWriter != null)
            {
                switch(seek.ToLower().Trim())
                {
                    case "begin":
                        streamWriter.BaseStream.Seek(lineNumber, SeekOrigin.Begin);//相对于Begin偏移0
                        break;
                    case "end":
                        streamWriter.BaseStream.Seek(lineNumber, SeekOrigin.End);//相对于End偏移0
                        break;
                    case "current":
                        streamWriter.BaseStream.Seek(lineNumber, SeekOrigin.Current);
                        break;
                }
                streamWriter.WriteLine(text);
                streamWriter.Flush();//清理当前编写器的所有缓冲区，使所有缓冲数据写入基础设备
                streamWriter.Close();
            }
        }
        /// <summary>
        /// 在打开的文本上写入多行.begin,current,end
        /// </summary>
        /// <param name="text"></param>
        /// <param name="lineNumber"></param>
        public void WriteLine(ArrayList strAry, string seek = "end", long lineNumber = 0)
        {
            //可选参数形式:WriteLine(text),WriteLine(text,"end")
            //具名参数形式:WriteLine(text,lineNumber: 10)
            streamWriter = new StreamWriter(currentFileStream, encoding);
            if (streamWriter != null)
            {
                switch (seek.ToLower().Trim())
                {
                    case "begin":
                        streamWriter.BaseStream.Seek(lineNumber, SeekOrigin.Begin);//相对于Begin偏移0字节
                        break;
                    case "end":
                        streamWriter.BaseStream.Seek(lineNumber, SeekOrigin.End);//相对于End偏移0
                        break;
                    case "current":
                        streamWriter.BaseStream.Seek(lineNumber, SeekOrigin.Current);
                        break;
                }
                foreach (string text in strAry)
                {
                    streamWriter.WriteLine(text);
                    streamWriter.Flush();//清理当前编写器的所有缓冲区，使所有缓冲数据写入基础设备
                }
                streamWriter.Close();
            }
        }
        /// <summary>
        /// 全部写入
        /// </summary>
        /// <param name="text"></param>
        public void Write(string text)
        {
            streamWriter = new StreamWriter(currentFileStream, encoding);
            streamWriter.Write(text);
            streamWriter.Close();
        }
        #endregion
        /// <summary>
        /// 在打开的文本上读入一行.begin,current,end
        /// </summary>
        /// <param name="text"></param>
        /// <param name="lineNumber"></param>
        public string ReadLine(long lineNumber = 0, string seek = "end")
        {
            string text = "";
            streamReader = new StreamReader(currentFileStream, encoding);
            string str = streamReader.ReadToEnd();
            string[] arrayStr = Regex.Split(str, "\r\n");
            int length = arrayStr.Length - 1;
            //return length.ToString();
            if (streamReader != null)
            {
                switch (seek.ToLower().Trim())
                {
                    case "begin":
                        //streamReader.BaseStream.Seek(lineNumber, SeekOrigin.Begin);
                        if (lineNumber <= length)
                        {
                            text = arrayStr[lineNumber];
                        }
                        else
                        {
                            text = arrayStr[length];
                        }
                        break;
                    case "end":
                        if (lineNumber <= length)
                        {
                            text = arrayStr[length - lineNumber];
                        }
                        else
                        {
                            text = arrayStr[0];
                        }
                        //streamReader.BaseStream.Seek(lineNumber, SeekOrigin.End);
                        break;
                    case "all":
                        text = str;
                        break;
                    //case "current":
                    //    streamReader.BaseStream.Seek(lineNumber, SeekOrigin.Current);
                    //    break;
                }
                //text = streamReader.ReadLine();
                streamReader.Close();
            }
            return text;
        }
    }
}
