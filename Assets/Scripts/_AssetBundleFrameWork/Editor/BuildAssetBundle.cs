/***
 *
 *   Title: "AssetBundle简单框架"项目
 *
 *   Description:
 *          功能： 对标记的资源进行打包输出
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;   //引入Unity编辑器，命名空间
using System.IO;     //引入的C#IO,命名空间
using HotUpdateModel;//热更新模块

namespace ABFW
{
    public class BuildAssetBundle {

        #region  暂时不用
        ///// <summary>
        ///// 打包生成所有的AssetBundles(包)
        ///// </summary>
        //[MenuItem("AssetBundelTools/BuildAllAssetBundles")]
        //public static void BuildAllAB()
        //{
        //    //打包AB输出路径
        //    string strABOutPathDIR = string.Empty;

        //    //获取"StreamingAssets"数值
        //    strABOutPathDIR = PathTools.GetABOutPath();

        ////判断生成输出目录文件夹
        //    if (!Directory.Exists(strABOutPathDIR))
        //    {
        //        Directory.CreateDirectory(strABOutPathDIR);
        //    }
        //    //打包生成
        //    BuildPipeline.BuildAssetBundles(strABOutPathDIR, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        //}
        #endregion

        /// <summary>
        /// 打包生成所有的AssetBundles(包),且拷贝资源文件，最后生成校验文件
        /// </summary>
        [MenuItem("AssetBundelTools/BuildAllAssetBundles")]
        public static void BuildAllAB()
        {
            //打包AB输出路径
            string strABOutPathDIR = string.Empty;

            //获取"StreamingAssets"数值
            strABOutPathDIR = PathTools.GetABOutPath();

            //判断生成输出目录文件夹
            if (!Directory.Exists(strABOutPathDIR))
            {
                Directory.CreateDirectory(strABOutPathDIR);
            }
            //打包生成
            BuildPipeline.BuildAssetBundles(strABOutPathDIR, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);

            /*  拷贝所有资源文件，到发布区  */
            CopyLuaFileToSA.CopyLuaFileTo();

            /*  生成校验文件 */
            CreateVerifyFiles.CreateFileMethod();
        }


    }//Class_end
}
