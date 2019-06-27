using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
/// <summary>
/// 配置文件操作类 - 建议有空仔细研究（感谢Roc提供）
/// 简单调用方法请查看后面
/// </summary>
/// 
namespace UsrMethod
{
    public class UsrConfigIni
    {
        // 读写INI文件相关。
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString", CharSet = CharSet.Ansi)]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSectionNames", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer, int nSize, string filePath);
        [DllImport("KERNEL32.DLL ", EntryPoint = "GetPrivateProfileSection", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileSection(string lpAppName, byte[] lpReturnedString, int nSize, string filePath);
        private string configPath = null;//配置文件路径;
        /// <summary>
        /// 默认构造函数
        /// 用默认路径存放配置文件; 软件当前目录的\resource\config\config.ini
        /// </summary>
        public UsrConfigIni()
        {
            this.configPath = System.AppDomain.CurrentDomain.BaseDirectory + @"resource\config\config.ini";
            string msg = getFileFolderByFilePath(configPath);
            {
                if (msg == null)
                {
                    MessageBox.Show("配置文件路径有误");
                    return;
                }
            }
            if (!Directory.Exists(msg))//判断文件夹是否存在,如果不存在,就进入if创建
            {
                Directory.CreateDirectory(msg);
            }
            //如果config.txt,文件不存在,则创建
            if (!File.Exists(this.configPath))
            {
                StreamWriter sw = new StreamWriter(this.configPath, true, Encoding.Default);
                if (sw != null)
                {
                    sw.Close();//File.Create创建文件不能立即释放资源
                }
            }
        }
        public UsrConfigIni(string configPath)
        {
            this.configPath = configPath;
            string msg = getFileFolderByFilePath(configPath);
            {
                if (msg == null)
                {
                    MessageBox.Show("配置文件路径有误");
                    return;
                }
            }
            if (!Directory.Exists(msg))//判断文件夹是否存在,如果不存在,就进入if创建
            {
                Directory.CreateDirectory(msg);
            }
            //如果config.txt,文件不存在,则创建
            if (!File.Exists(this.configPath))
            {
                StreamWriter sw = new StreamWriter(this.configPath, true, Encoding.Default);
                if (sw != null)
                {
                    sw.Close();//File.Create创建文件不能立即释放资源
                }
            }
        }
        /// <summary>
        /// 向INI写入数据。
        /// </summary>
        /// <PARAM name="Section">节点名。</PARAM>
        /// <PARAM name="Key">键名。</PARAM>
        /// <PARAM name="Value">值名。</PARAM>
        public void Write(string Section, string Key, string Value, string path = null)
        {
            if(path == null)
            {
                path = configPath;
            }
            WritePrivateProfileString(Section, Key, Value, path);
        }

        /// <summary>
        /// 读取INI数据。
        /// </summary>
        /// <PARAM name="Section">节点名。</PARAM>
        /// <PARAM name="Key">键名。</PARAM>
        /// <PARAM name="Path">值名。</PARAM>
        /// <returns>相应的值。</returns>
        public string Read(string Section, string Key, string path = null)
        {
            if (path == null)
            {
                path = configPath;
            }
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, path);
            return temp.ToString();
        }
        /// <summary>
        /// 读取一个ini里面所有的节
        /// </summary>
        /// <param name="sections"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public int GetAllSectionNames(out string[] sections, string path = null)
        {
            if (path == null)
            {
                path = configPath;
            }
            int MAX_BUFFER = 32767;
            IntPtr pReturnedString = Marshal.AllocCoTaskMem(MAX_BUFFER);
            int bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, path);
            if (bytesReturned == 0)
            {
                sections = null;
                return -1;
            }
            string local = Marshal.PtrToStringAnsi(pReturnedString, (int)bytesReturned).ToString();
            Marshal.FreeCoTaskMem(pReturnedString);
            //use of Substring below removes terminating null for split
            sections = local.Substring(0, local.Length - 1).Split('\0');
            return 0;
        }
        /// <summary>
        /// 得到某个节点下面所有的key和value组合
        /// </summary>
        /// <param name="section"></param>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public int GetAllKeyValues(string section, out string[] keys, out string[] values, string path = null)
        {
            if (path == null)
            {
                path = configPath;
            }
            byte[] b = new byte[65535];
            GetPrivateProfileSection(section, b, b.Length, path);
            string s = System.Text.Encoding.Default.GetString(b);
            string[] tmp = s.Split((char)0);
            ArrayList result = new ArrayList();
            foreach (string r in tmp)
            {
                if (r != string.Empty)
                    result.Add(r);
            }
            keys = new string[result.Count];
            values = new string[result.Count];
            for (int i = 0; i < result.Count; i++)
            {
                string[] item = result[i].ToString().Split(new char[] { '=' });
                if (item.Length == 2)
                {
                    keys[i] = item[0].Trim();
                    values[i] = item[1].Trim();
                }
                else if (item.Length == 1)
                {
                    keys[i] = item[0].Trim();
                    values[i] = "";
                }
                else if (item.Length == 0)
                {
                    keys[i] = "";
                    values[i] = "";
                }
            }
            return 0;
        }
        private string getFileFolderByFilePath(string filePath)//根据文件完整路径得到该文件所在的文件夹路径;
        {
            try
            {
                if (filePath.Contains("/"))
                {
                    return filePath.Substring(0, filePath.LastIndexOf(@"/"));
                }
                else if (filePath.Contains(@"\"))
                {
                    return filePath.Substring(0, filePath.LastIndexOf(@"\"));
                }
                else
                {
                    return null;//请检查参数:filePath是否正确;
                }
            }
            catch
            {
                return null;//请检查参数:filePath是否正确;
            }
        }
    }
}