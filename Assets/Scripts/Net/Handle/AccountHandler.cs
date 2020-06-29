using Protocol.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AccountHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case AccountCode.LOGIN:
                LoginResponse(value.ToString());
                break;
            case AccountCode.REGISTER_SRES:
                RegisterResponse(value.ToString());
                break;
            default:
                break;
        }
    }

    

    private UIMsg uiMsg = new UIMsg();
    private MessageData serverMsg = new MessageData();
    /// <summary>
    /// 登录响应
    /// </summary>
    public void LoginResponse(string value) {
        uiMsg.Set(value,UnityEngine.Color.red);
        if (value == "登录成功")
        {
            //跳转场景
            LoadSceneMsg msg = new LoadSceneMsg(1,()=> {
                Debug.Log("登陆成功 ：进入游戏界面");
                //向服务器发送请求获取角色信息
                serverMsg.Set(OpCode.USER,UserCode.GET_INFO_CREQ,null);
                Dispatch(AreaCode.NET,0,serverMsg);

                

            });
            Dispatch(AreaCode.SCENE,SceneEvent.LOAD_SCENE,msg);

            //uiMsg.SetColor(UnityEngine.Color.green);
            //Dispatch(AreaCode.UI,UIEvent.MessageInfoPanel,uiMsg);
            return;
        }

        Dispatch(AreaCode.UI, UIEvent.MessageInfoPanel, uiMsg);
    }

    /// <summary>
    /// 注册响应
    /// </summary>
    /// <param name="v"></param>
    private void RegisterResponse(string value)
    {
        uiMsg.Set(value, UnityEngine.Color.red);
        if (value == "注册成功")
        {
            //跳转到登录界面
            Dispatch(AreaCode.UI,UIEvent.REGIST_PANEL_ACTIVE,false);
            Dispatch(AreaCode.UI, UIEvent.START_PANEL_ACTIVE, true);

            uiMsg.SetColor(UnityEngine.Color.green);
            Dispatch(AreaCode.UI,UIEvent.MessageInfoPanel,uiMsg);
            return;
        }

        Dispatch(AreaCode.UI,UIEvent.MessageInfoPanel,uiMsg);
    }
}

