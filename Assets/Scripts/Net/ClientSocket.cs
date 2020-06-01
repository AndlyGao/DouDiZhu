using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System;
using UnityEngine;

/// <summary>
/// 客户端Socket的封装
/// </summary>
public class ClientSocket  {

    private Socket clientSocket;

    private string ip;
    private int port;

    /// <summary>
    /// 构造连接对象
    /// </summary>
    public ClientSocket(string ip,int port)
    {

        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        this.ip = ip;
        this.port = port;
    }

    public void DisConnect()
    {
        //清空数据
        rcvBytes = null;
        data.Clear();
        isProcessRcvMsg = false;
        messageDataQueue.Clear();

        if (!clientSocket.Connected) return;
        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
        clientSocket = null;
    }

    public void Connect()
    {
        try
        {
            clientSocket.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), ConnectCallBack => {
                clientSocket.EndConnect(ConnectCallBack);
                Debug.Log("连接服务器成功...");
                //开始接收消息
                StartReceive();
            }, clientSocket);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    #region 接收消息

    /// <summary>
    /// 接受消息缓冲区
    /// </summary>
    private byte[] rcvBytes = new byte[1024];

    /// <summary>
    /// 一旦接受到数据，存到缓存区里面
    /// </summary>
    private List<byte> data = new List<byte>(); 

    /// <summary>
    /// 是否正在处理消息
    /// </summary>
    private bool isProcessRcvMsg = false;

    /// <summary>
    /// 接受到的消息
    /// </summary>
    public Queue<MessageData> messageDataQueue = new Queue<MessageData>();


    /// <summary>
    /// 开始异步接受消息
    /// </summary>
    private void StartReceive() {
        if (clientSocket == null && !clientSocket.Connected)
        {
            Debug.LogError("连接服务器失败...无法接受消息"); 
            return;
        }

        clientSocket.BeginReceive(rcvBytes,0,rcvBytes.Length,SocketFlags.None,ReceiveCallBack =>{
            
            try
            {
                
                int length = clientSocket.EndReceive(ReceiveCallBack);
                byte[] rcvBuffer = new byte[length];
                Buffer.BlockCopy(rcvBytes,0,rcvBuffer,0,length);
                //处理接收到的数据
                data.AddRange(rcvBuffer);
                if (isProcessRcvMsg == false)
                {
                    ProcessReceiveMsg();
                }
                //继续接受消息
                StartReceive();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

        },clientSocket);
    }

    /// <summary>
    /// 处理接受到的消息
    /// </summary>
    private void ProcessReceiveMsg() {
        isProcessRcvMsg = true;

        //解析数据包
        byte[] temData = EncodTool.DecodeMessage(ref data);

        if (temData == null)
        {
            isProcessRcvMsg = false;
            return;
        }

        MessageData msg = EncodTool.DecodeMsg(temData);
        messageDataQueue.Enqueue(msg);
        //Debug.Log("收到服务器消息： " + msg.Value.ToString());

        //递归 一直接受消息
        ProcessReceiveMsg();
    }

    #endregion

    #region 发送消息

    public void StartSend(int opCode,int subCode,object value) {
        MessageData msg = new MessageData(opCode, subCode, value);
        StartSend(msg);
    }

    public void StartSend(MessageData msg) {

        byte[] data = EncodTool.EncodeMsg(msg);
        byte[] packet = EncodTool.EncodeMessage(data);

        try
        {
            clientSocket.Send(packet);
        }
        catch (Exception e)
        {

            Debug.LogError(e.Message);
        }
    }


    #endregion
}
