﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EventCodes {

}

/// <summary>
/// 存储所有的UI事件码
/// </summary>
public class UIEvent
{
    public const int START_PANEL_ACTIVE = 0;//设置开始面板的显示
    public const int REGIST_PANEL_ACTIVE = 1;//设置注册面板的显示
    public const int INIT_USERPANEL_INFO = 2;//当前账号的角色信息（登录成功后从服务器获取）
    public const int SHOW_ENTERROOMBTN_ACTIVE = 3;//匹配成功  进入房间按钮
    public const int CREATE_PANEL_ACTIVE = 4;//创建角色面板的显示

    public const int SET_TABLE_CARDS = 5;//设置底牌
    public const int SET_LEFTPLAYER_DATA = 6;//设置左边玩家的信息
    public const int SET_RIGHTPLAYER_DATA = 13;//设置右边玩家的信息
    public const int SET_MYPLAYER_DATA = 16;//设置自己玩家的信息

    public const int PLAYER_READY = 7;//玩家准备
    public const int PLAYER_ENTER = 8;//玩家进入
    public const int PLAYER_LEAVE = 9;//玩家离开
    public const int PLAYER_CHANGE_IDENTITY = 11;//改变身份
    public const int PLAYER_HIDE_STATE = 12;//开始游戏，信息隐藏  该发牌了

    public const int HIDE_PLAYER_READYBTN = 17;//隐藏player准备按钮

    public const int SHOW_PLAYER_JIAO_BTN_ACTIVE = 14;//玩家叫 按钮
    public const int SHOW_PLAYER_CHUPAI_BTN_ACTIVE = 15;//玩家出牌按钮


    public const int PLAYER_CHAT = 18;//玩家聊天

    public const int MessageInfoPanel = int.MaxValue;// 
    
}

public class AudioEvent
{
    public const int EFFECTAUDIO = 0;
    public const int BGMAUDIO = 0;
}

/// <summary>
/// 关于ui消息的事件码
/// </summary>
public class UIMsg {
    public string message;
    public UnityEngine.Color color;

    public void Set(string msg,UnityEngine.Color col) {
        this.message = msg;
        this.color = col;
    }

    public void SetColor(UnityEngine.Color col) {
        this.color = col;
    }

    public void SetMsg(string msg) {
        this.message = msg;
    }
}

/// <summary>
/// 聊天信息的事件码
/// </summary>
public class ChatMsg
{
    public int userId;
    public int chatMsgType;

    public ChatMsg()
    {

    }

    public void Set(int userId,int chatMsgType)
    {
        this.userId = userId;
        this.chatMsgType = chatMsgType;
    }
}

/// <summary>
/// 关于场景的事件码
/// </summary>
public class SceneEvent {
    public const int LOAD_SCENE = 0;
}


public class LoadSceneMsg {
    public int sceneBuildIndex;
    public string sceneBuildName;
    public Action onSceneLoaded;

    public LoadSceneMsg() {
        this.sceneBuildIndex = -1;
        this.sceneBuildName = null;
        this.onSceneLoaded = null;
    }

    public LoadSceneMsg(int index,Action loadedAction) {
        this.sceneBuildIndex = index;
        this.onSceneLoaded = loadedAction;
    }

    public LoadSceneMsg(string name, Action loadedAction)
    {
        this.sceneBuildName = name;
        this.onSceneLoaded = loadedAction;
    }
}
