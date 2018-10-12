
using PortForward.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace PortForward
{
    class Program
    {

        static void Main(string[] args)
        {


            #region print Help
            Console.WriteLine("author:  lith");
            Console.WriteLine("email:   litsoft@126.com");
            Console.WriteLine("----Lith端口转发----");
            Console.WriteLine("从文件名获取配置信息,分为“本地端口转发工具”和“端口桥接工具”。");
            Console.WriteLine("----本地端口转发工具----");
            Console.WriteLine("     配置信息格式为：");
            Console.WriteLine("         PortForwardLocal--inputConnPort--outputConnHost--outputConnPort--NoPrint.exe");
            Console.WriteLine("     Demo:");
            Console.WriteLine("         PortForwardLocal--8000--192.168.1.5--3384--NoPrint.exe");
            Console.WriteLine("         把本地的8000端口转发至 主机192.168.1.5的3384端口");
            Console.WriteLine("     NoPrint:定义是否回显，若指定为NoPrint则不实时回显连接信息");
            Console.WriteLine("");
            Console.WriteLine("----端口桥接工具----");          
            Console.WriteLine("     客户端格式为：");
            Console.WriteLine("         PortForwardClient--authToken--serverHost--outputConnPort-localConnHost--localConnPort--ConnectCount--NoPrint.exe");
            Console.WriteLine("     服务端格式为：");
            Console.WriteLine("         PortForwardServer--authToken--inputConnPort--outputConnPort--NoPrint.exe");
            Console.WriteLine("     Demo:");
            Console.WriteLine("         PortForwardClient--authToken--192.168.1.100--6203--abc.com--3389--5.exe");
            Console.WriteLine("         PortForwardServer--authToken--6202--6203.exe");
            Console.WriteLine("");
            Console.WriteLine("     说明:");
            Console.WriteLine("         把服务端（serverHost）的 inputConnPort端口转发至 客户端连接的 主机localConnHost的端口localConnPort");
            Console.WriteLine("         服务端和客户端通过端口outputConnPort连接");
            Console.WriteLine("     autoToken   :权限校验字段，服务端和客户端必须一致");
            Console.WriteLine("     ConnectCount:客户端保持的空闲连接个数，推荐5");
            Console.WriteLine("     NoPrint     :定义是否回显，若指定为NoPrint则不实时回显连接信息，可不指定");
            Console.WriteLine("----------------");
            Console.WriteLine("");
            #endregion


            //通过文件名获取配置
            string configString = Path.GetFileNameWithoutExtension(Application.ExecutablePath);

            //configString = "PortForwardClient--authToken--192.168.3.162--6203--192.168.3.162--3389--2";

            try
            {

           
                var config = configString.Split(new string[] { "--" }, StringSplitOptions.RemoveEmptyEntries);
                switch (config[0])
                {
                    case "PortForwardClient": RunClient(config);break;
                    case "PortForwardServer": RunServer(config); break;
                    case "PortForwardLocal": RunLocal(config); break;
                    default: Console.WriteLine("错误：配置（文件名）不合法！！请修改文件名。"); break;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"出错："+ex.GetBaseException().Message);
            }
            Console.ReadLine();
        }

        #region RunClient
        static void RunClient(string[]config)
        {
            var mng = new ClientManager()
            {
                authToken = config[1],
                server_Host = config[2],
                outputConn_Port = int.Parse(config[3]),
                localConn_Host = config[4],
                localConn_Port = int.Parse(config[5])
            };

            int connectCount = int.Parse(config[6]);

            Console.WriteLine($"当前为 端口桥接工具-客户端：");
            Console.WriteLine($"authToken:{mng.authToken}");
            Console.WriteLine($"server_Host:{mng.server_Host}");
            Console.WriteLine($"outputConn_Port:{mng.outputConn_Port}");
            Console.WriteLine($"localConn_Host:{mng.localConn_Host}");
            Console.WriteLine($"localConn_Port:{mng.localConn_Port}");
            Console.WriteLine($"connectCount:{connectCount}");

            //定义显示输出
            if (config.Length <= 7 || "NoPrint" != config[7])
                mng.ConsoleWriteLine = Console.WriteLine;

            mng.ConsoleWriteLine("开始...");
            mng.StartConnectThread(connectCount);

        }

        #endregion

        #region RunServer
        static void RunServer(string[] config)
        {
            var mng = new ServerManager()
            {
                authToken = config[1],
                inputConn_Port = int.Parse(config[2]),
                outputConn_Port = int.Parse(config[3])
            };

            Console.WriteLine($"当前为 端口桥接工具-服务端：");
            Console.WriteLine($"authToken:{mng.authToken}");
            Console.WriteLine($"inputConn_Port:{mng.inputConn_Port}");
            Console.WriteLine($"outputConn_Port:{mng.outputConn_Port}");


            //定义显示输出
            if (config.Length <= 4 || "NoPrint" != config[4])
                mng.ConsoleWriteLine = Console.WriteLine;
            mng.ConsoleWriteLine("开始...");
            mng.StartLinstening();
        }
        #endregion

        #region RunLocal
        static void RunLocal(string[] config)
        {
            var mng = new LocalManager()
            {
                inputConn_Port = int.Parse(config[1]),
                outputConn_Host = config[2],
                outputConn_Port = int.Parse(config[3])
            };


            Console.WriteLine($"当前为 本地端口转发工具：");
            Console.WriteLine($"inputConn_Port:{mng.inputConn_Port}");
            Console.WriteLine($"outputConn_Host:{mng.outputConn_Host}");
            Console.WriteLine($"outputConn_Port:{mng.outputConn_Port}");


            //定义显示输出
            if (config.Length <= 4 || "NoPrint" != config[4])
                mng.ConsoleWriteLine = Console.WriteLine;

            mng.ConsoleWriteLine("开始...");
            mng.StartLinstening();
        }
        #endregion


    }
}
