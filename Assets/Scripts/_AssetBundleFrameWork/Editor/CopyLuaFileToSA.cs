/***
 *
 *  Title: "热更新流程设计"课程项目
 *        
 *         拷贝所有lua编辑器区的文件，到发布区。
 *
 *  Description:
 *        功能：
 *     
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.IO;
using ABFW;


namespace HotUpdateModel
{
    public class CopyLuaFileToSA 
    {
        //定义拷贝lua文件的源目录（lua编辑区）
        //private static string _LuaDIRPath = Application.dataPath + "/LuaScripts/src";
        private static string _LuaDIRPath = Application.dataPath + PathTools.LUA_RESOURCE_PATH;

        //定义目标目录（lua发布区）
        //private static string _CopyTargetDIR = PathTools.GetABOutPath() + "/LUA";
        private static string _CopyTargetDIR = PathTools.GetABOutPath() + PathTools.LUA_DEPLOY_PATH;

        [MenuItem("AssetBundelTools/CopyLuaFileToStreamingAssets")]
        public static void CopyLuaFileTo()
        {
            //参数检查
            //Debug.Log("_LuaDIRPath="+ _LuaDIRPath);
            //Debug.Log("_CopyTargetDIR=" + _CopyTargetDIR);

            //定义目录与文件结构
            DirectoryInfo dirInfo = new DirectoryInfo(_LuaDIRPath);
            FileInfo[] files=dirInfo.GetFiles();

            //如果拷贝的目标路径不存在，则创建
            if (!Directory.Exists(_CopyTargetDIR))
            {
                Directory.CreateDirectory(_CopyTargetDIR);
            }

            //开始循环拷贝文件
            foreach (FileInfo infoObj in files)
            {
                File.Copy(infoObj.FullName, _CopyTargetDIR+"/"+ infoObj.Name,true);
            }

            //Unity编辑器窗体刷新
            Debug.Log("CopyLuaFileToSA.cs/CopyLuaFileTo()/ lua文件已经拷贝的指定区域！");
            AssetDatabase.Refresh();
        }

		

    }//Class_end
}


