

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ABFW;


namespace HotUpdateModel
{
    public class TestStartGame : MonoBehaviour, IStartGame
    {
        //场景名称
        private string _ScenesName = "mainscenes";
        //AB包名称
        private string _AssetBundleName = "mainscenes/ui.ab";
        //资源名称
        private string _AssetName = "UIContDown.prefab";


        public void ReceiveInfoStartRuning()
        {
            //加载倒计时UI界面
            LoadUICountDown();
        }

        //加载倒计时UI界面
        public void LoadUICountDown()
        {
            //加载指定AssetBundle 包
            AssetBundleMgr.GetInstance().LoadAssetBundlePackage(_ScenesName, _AssetBundleName, LoadAllABComplete);
        }

        /// <summary>
        /// 回调函数： 所有的AB包都已经加载完毕了。
        /// </summary>
        /// <param name="abName"></param>
        private void LoadAllABComplete(string abName)
        {
            UnityEngine.Object tmpObj = null;

            //提取资源
            tmpObj = AssetBundleMgr.GetInstance().LoadAsset(_ScenesName, _AssetBundleName, _AssetName, false);
            if (tmpObj!=null)
            {
                Instantiate(tmpObj);
            }
        }

    }//Class_end
}


