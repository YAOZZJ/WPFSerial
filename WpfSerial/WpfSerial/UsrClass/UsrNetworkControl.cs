﻿using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;

namespace UsrMethod
{
    class UsrNetworkControl
    { 
        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <returns>本机IP地址</returns>
        public static string[] GetLocalIP()
    {
        try
        {
            string HostName = Dns.GetHostName(); //得到主机名
            IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                ArrayList ipList = new ArrayList();
            for (int i = 0; i < IpEntry.AddressList.Length; i++)
            {
                //从IP地址列表中筛选出IPv4类型的IP地址
                //AddressFamily.InterNetwork表示此IP为IPv4,
                //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                        //return IpEntry.AddressList[i].ToString();
                        ipList.Add(IpEntry.AddressList[i].ToString());
                }
            }
            return (string[])ipList.ToArray(typeof(string));
            }
        catch (Exception)
            {
                //return ex.Message;
                return null;
        }
    }
}
}
