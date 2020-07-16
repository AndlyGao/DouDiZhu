/***
 *
 *   Title: "AssetBundle简单框架"项目
 *          框架内部验证测试
 *
 *   Description:
 *          功能： 
 *              框架整体验证测试脚本
 *              
 *
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFW
{
	public class TestClass_ABToolFramework:MonoBehaviour
	{
        //场景名称
        private string _ScenesName = "prefabs";
        //AB包名称
        private string _AssetBundelName = "prefabs/prefabs.ab";
        //资源名称
        private string _AssetName = "_Eviromens.prefab";


        private void Start()
        {
            Debug.Log(GetType() + "开始'ABFW'框架测试 ");
            //调用AB包（连锁智能调用AB包【集合】）
            StartCoroutine(AssetBundleMgr.GetInstance().LoadAssetBundlePack(_ScenesName, _AssetBundelName, LoadAllABComplete));
        }

        //回调函数： 所有的AB包都已经加载完毕了
        private void LoadAllABComplete(string abName)
        {
            Debug.Log(GetType() + "所有的AB包都已经加载完毕了");
            UnityEngine.Object tmpObj = null;

            //提取资源
            tmpObj=(UnityEngine.Object)AssetBundleMgr.GetInstance().LoadAsset(_ScenesName, _AssetBundelName, _AssetName,false);
            if (tmpObj!=null)
            {
                Instantiate(tmpObj);
            }
        }

        //测试销毁资源
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log(GetType()+" 测试销毁资源");
                AssetBundleMgr.GetInstance().DisposeAllAssets(_ScenesName);
            }
        }



    }
}


