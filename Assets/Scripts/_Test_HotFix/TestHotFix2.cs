

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace HotUpdateModel
{
    [Hotfix]
    public class TestHotFix2 : MonoBehaviour
    {
        LuaEnv luaenv = new LuaEnv();

        private void Start()
        {
            Debug.Log("测试HotFix技术，示例2");
        }

        //调用函数
        public void InvokeInCsharp()
        {
            Debug.Log("这是在C#中执行的方法");
        }

        //lua语言执行的内容
        public void InvokeInLua()
        {
            luaenv.DoString(@"xlua.hotfix(
                                           CS.HotUpdateModel.TestHotFix2,'InvokeInCsharp',function()
                                             print('这是在lua中执行的方法')    
                                           end
                                         )");
        }
    }//Class_end
}