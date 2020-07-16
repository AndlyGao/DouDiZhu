/***
 *
 *  Title: 
 *        
 *          开始游戏的主逻辑
 *
 *  Description:
 *        功能：[本脚本的主要功能描述] 
 *
 *     
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HotUpdateModel
{
    public class StartGame : MonoBehaviour, IStartGame
    {
        public void ReceiveInfoStartRuning()
        {
            print("游戏热更新模块执行完毕，开始游戏主逻辑启动....");
            //调用与加载lua脚本
            LuaHelper.GetInstance().DoString("require 'LauchABFW'");
            //启动lua脚本，加载ab包中的指定资源
            LuaHelper.GetInstance().CallLuaFunction("LauchABFW", "StartABFW");
        }
    }//Class_end
}


