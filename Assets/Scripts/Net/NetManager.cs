using Protocol.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 网络模块
/// </summary>
public class NetManager : ManagerBase
{
    public static NetManager Instance = null;

    //private ClientSocket client = new ClientSocket("192.168.31.143", 6666);
    private ClientSocket client = new ClientSocket("35.236.135.83", 6666);
    private void Awake()
    {
        Debug.Log("当前服务器地址：35.236.135.83");
        Instance = this;

        Add(0,this);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case 0:
                {
                    if (client.IsConneted)
                        client.StartSend(message as MessageData);
                    else
                        MsgCenter.Instance.Dispatch(AreaCode.UI, UIEvent.MessageInfoPanel, new UIMsg("网络未连接！",Color.red));
                }
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        Connected();
    }


    public void Connected()
    {
        client.Connect();
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        client.DisConnect();
#endif
    }

    private void Update()
    {
        if (client == null)
        {
            return;
        }

        while (client.messageDataQueue.Count > 0)
        {
            MessageData msg = client.messageDataQueue.Dequeue();

            //处理消息
            ProcessServerMsg(msg);
            
        }
    }

    //处理注册登录
    HandlerBase accountHandler = new AccountHandler();
    HandlerBase userHandler = new UserHandler();
    HandlerBase matchHandler = new MatchHandler();
    HandlerBase chatHandler = new ChatHandler();
    HandlerBase fightHandler = new FightHandler();
    /// <summary>
    /// 处理服务器发来的消息
    /// </summary>
    /// <param name="msg"></param>
    private void ProcessServerMsg(MessageData msg)
    {
        switch (msg.OpCode)
        {
            case OpCode.ACCOUNT:
                accountHandler.OnReceive(msg.SubCode,msg.Value);
                break;
            case OpCode.USER:
                userHandler.OnReceive(msg.SubCode,msg.Value);
                break;
            case OpCode.MATCHROOM:
                matchHandler.OnReceive(msg.SubCode, msg.Value);
                break;
            case OpCode.CHAT:
                chatHandler.OnReceive(msg.SubCode, msg.Value);
                break;
            case OpCode.FIGHT:
                fightHandler.OnReceive(msg.SubCode,msg.Value);
                break;
            default:
                break;
        }
    }
}

