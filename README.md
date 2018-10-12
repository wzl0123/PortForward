# PortForward
## ----Lith端口转发----
>从文件名获取配置信息,分为“本地端口转发工具”和“端口桥接工具”。

     author:  lith
     email:   litsoft@126.com

## ----本地端口转发工具----
     配置信息格式为：
         PortForwardLocal--inputConnPort--outputConnHost--outputConnPort--NoPrint.exe
     Demo:
         PortForwardLocal--8000--192.168.1.5--3384--NoPrint.exe
         把本地的8000端口转发至 主机192.168.1.5的3384端口
     NoPrint:
         定义是否回显，若指定为NoPrint则不实时回显连接信息

## ----端口桥接工具----
     客户端格式为：
         PortForwardClient--authToken--serverHost--outputConnPort-localConnHost--localConnPort--ConnectCount--NoPrint.exe
     服务端格式为：
         PortForwardServer--authToken--inputConnPort--outputConnPort--NoPrint.exe
     Demo:
         PortForwardClient--authToken--192.168.1.100--6203--abc.com--3389--5.exe
         PortForwardServer--authToken--6202--6203.exe

     说明:
         把服务端（serverHost）的 inputConnPort端口转发至 客户端连接的 主机localConnHost的端口localConnPort
         服务端和客户端通过端口outputConnPort连接
     autoToken:
         权限校验字段，服务端和客户端必须一致
     ConnectCount:
         客户端保持的空闲连接个数，推荐5
     NoPrint:
        定义是否回显，若指定为NoPrint则不实时回显连接信息，可不指定
----------------
