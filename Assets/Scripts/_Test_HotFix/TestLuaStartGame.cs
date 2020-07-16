

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HotUpdateModel
{
    public class TestLuaStartGame : MonoBehaviour, IStartGame
    {
        public void ReceiveInfoStartRuning()
        {
            Debug.Log(GetType()+ "/ReceiveInfoStartRuning()/ 开始lua补丁调用");
            //调用lua脚本
            LuaHelper.GetInstance().DoString("require 'Test_ProjectHotFixListInfo'");                       //引入lua脚本
            LuaHelper.GetInstance().CallLuaFunction("Test_ProjectHotFixListInfo", "TestM");   //调用lua函数

        }
    }//Class_end
}


