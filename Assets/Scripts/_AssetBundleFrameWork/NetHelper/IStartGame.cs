/***
 *
 *  Title: 
 *        
 *          接口： 游戏开始接口
 *          
 *
 *  Description:
 *        功能：
 *             与"从服务器下载更新最新的资源文件"（UpdateResourcesFileFromServer）脚本配合，
 *             进行自动化数值传递，特指定本接口。
 *     
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HotUpdateModel
{
    public interface IStartGame 
    {
        void ReceiveInfoStartRuning();
    }
}


