using System.Collections;
using System.Collections.Generic;
using HotUpdateModel;
using UnityEngine;

public class LuaStart : MonoBehaviour,IStartGame
{
    public void ReceiveInfoStartRuning()
    {
        Debug.Log(GetType()+ "/ReceiveInfoStartRuning()/ 开始lua补丁调用");
        //调用lua脚本
        LuaHelper.GetInstance().DoString("require 'RegisterChange'");                       //引入lua脚本
        LuaHelper.GetInstance().CallLuaFunction("RegisterChange", "HotFixRegister");   //调用lua函数

    }
}