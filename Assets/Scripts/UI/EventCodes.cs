using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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

    public const int MessageInfoPanel = int.MaxValue;// 
    
}


/// <summary>
/// 关于ui消息的时间码
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
