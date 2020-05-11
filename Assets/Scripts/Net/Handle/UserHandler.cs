using System.Collections;
using System.Collections.Generic;
using Protocol.Code;
using Protocol.Dto;
using UnityEngine;

public class UserHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case UserCode.GET_INFO_SRES:
                GetInfoResponse(value as UserDto);
                break;

            case UserCode.CREAT_SRES:
                CreatUserResponse(value as UserDto);
                break;
            case UserCode.ONLINE_SRES:
                OnLineResponse(value as UserDto);
                break;
            default:
                break;
        }
    }

    UIMsg uiMsg = new UIMsg();
    MessageData serverMsg = new MessageData();

    private void GetInfoResponse(UserDto userDto)
    {
        Debug.Log("GetInfoResponse ==> " + userDto.info);
        if (userDto.info.Equals("非法登陆") )
        {
            uiMsg.Set("非法登陆", Color.red);
            Dispatch(AreaCode.UI,UIEvent.MessageInfoPanel,uiMsg);
            //隐藏创建面板
            Dispatch(AreaCode.UI, UIEvent.CREATE_PANEL_ACTIVE, false);
        }
        else if (userDto.info.Equals("没有角色"))
        {
            uiMsg.Set("没有角色", Color.red);
            Dispatch(AreaCode.UI, UIEvent.MessageInfoPanel, uiMsg);
            //显示创建面板
            Dispatch(AreaCode.UI, UIEvent.CREATE_PANEL_ACTIVE, true);

        }
        else if (userDto.info.Equals("获取角色成功"))
        {
            uiMsg.Set("获取角色成功", Color.green);
            Dispatch(AreaCode.UI, UIEvent.MessageInfoPanel, uiMsg);
            //隐藏创建面板
            Dispatch(AreaCode.UI, UIEvent.CREATE_PANEL_ACTIVE, false);
            //显示角色信息面板
            Dispatch(AreaCode.UI,UIEvent.INIT_USERPANEL_INFO,userDto);
            //角色向服务器发送上线请求  == 让服务起来做，获取到信息后自动登陆
            //serverMsg.Set(OpCode.USER, UserCode.ONLINE_CREQ, null);
            //Dispatch(AreaCode.NET, 0, serverMsg);
            //保存服务器发送来的角色数据
            Models.gameModel.UserDto = userDto;

        }
    }

    private void CreatUserResponse(UserDto userDto)
    {
        Debug.Log("CreatUserResponse ==> " + userDto.info);
        if (userDto.info.Equals("非法登陆"))
        {
            uiMsg.Set("非法登陆", Color.red);
            Dispatch(AreaCode.UI, UIEvent.MessageInfoPanel, uiMsg);
        }
        else if (userDto.info.Equals("重复创建"))
        {
            uiMsg.Set("重复创建", Color.red);
            Dispatch(AreaCode.UI, UIEvent.MessageInfoPanel, uiMsg);
        }
        else if (userDto.info.Equals("创建角色成功"))
        {
            uiMsg.Set("创建角色成功", Color.green);
            Dispatch(AreaCode.UI, UIEvent.MessageInfoPanel, uiMsg);
            //隐藏角色创建面板
            Dispatch(AreaCode.UI,UIEvent.CREATE_PANEL_ACTIVE,false);
            //服务器创建角色成功，向服务器获取角色信息
            serverMsg.Set(OpCode.USER, UserCode.GET_INFO_CREQ, null);
            Dispatch(AreaCode.NET, 0, serverMsg);
        }
    }

    private void OnLineResponse(UserDto userDto)
    {
        Debug.Log("OnLineResponse ==> " + userDto.info);

        if (userDto.info.Equals("非法登陆"))
        {
            uiMsg.Set("非法登陆", Color.red);
            Dispatch(AreaCode.UI, UIEvent.MessageInfoPanel, uiMsg);
        }
        else if (userDto.info.Equals("没有角色"))
        {
            uiMsg.Set("没有角色", Color.red);
            Dispatch(AreaCode.UI, UIEvent.MessageInfoPanel, uiMsg);
        }
        else if (userDto.info.Equals("角色上线成功"))
        {
            //TODO 角色上线之后干什么
        }
    }

}
