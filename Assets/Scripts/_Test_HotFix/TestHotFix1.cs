
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;


namespace HotUpdateModel
{
    [Hotfix]
    public class TestHotFix1 :MonoBehaviour
    {
        LuaEnv luaenv = new LuaEnv();

        private void Start()
        {
            Debug.Log("测试lua中的‘热补丁’ 技术");
            //执行一个调用函数
            InvokeRepeating("InvokeInCsharp",1F,2F);
        }

        //调用函数
        void InvokeInCsharp()
        {
            Debug.Log("在C#中执行的方法");
        }

        //lua语言执行的内容
        public void InvokeInLua()
        {
            Debug.Log("准备lua调用");
            luaenv.DoString(@"xlua.hotfix(
                                           CS.HotUpdateModel.TestHotFix1,'InvokeInCsharp',function()
                                             print('在lua中执行的方法')    
                                           end
                                         )");
        }

    }//Class_end
}


