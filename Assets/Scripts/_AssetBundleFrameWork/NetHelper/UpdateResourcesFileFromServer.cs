/***
 *
 *  Title: 
 *        
 *          从服务器下载更新最新的资源文件，获取资源（ab包、Lua、配置文件Json/xml）
 *          
 *  Description:
 *          本脚本是核心实现脚本，主要开发思路：
 *          
 *          1： 下载“校验文件”到客户端
 *          2： 根据“校验文件”，客户端逐条读取资源文件，然后与本客户端相同的资源文件，进行MD5编码对比。
 *          3： 如果客户端没有服务器（增加的）文件，直接下载服务器端文件即可。
 *          4： 客户端存在与服务端相同的文件名，但是MD5编码对比不一致，说明服务器端对应的资源文件发生的更新，则客户端
 *              下载最新的资源文件。
 *     
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ABFW;


namespace HotUpdateModel
{
    public class UpdateResourcesFileFromServer:MonoBehaviour
    {
        //是否启用本脚本（是否联网下载服务器更新资源）
        public bool EnableSelf = true;                      //默认启用
        //PC平台的资源（包含AsserBundl）下载路径
        private string _DownloadPath = string.Empty;
        //HTTP 服务器地址
        private string _ServerURL = PathTools.SERVER_URL;



        void Awake()
        {
            //默认启用热更新
            if (EnableSelf)
            {
                //PC平台的资源（包含AsserBundl）下载路径
                _DownloadPath = PathTools.GetABOutPath();
                //检测资源进行对比更新
                StartCoroutine(CheckUpdate(_ServerURL));
            }
            //不启用热更新
            else {
                Debug.Log("### WARING:  "+GetType()+"/Awake()/本脚本已经勾选禁用！， 停止从服务器下载更新服务，特此通知！");
                //通知其他游戏主逻辑，开始运行
                //BroadcastMessage("ReceiveInfoStartRuning", SendMessageOptions.DontRequireReceiver);
                BroadcastMessage(ABDefine.ReceiveInfoStartRuning, SendMessageOptions.DontRequireReceiver);
            }
        }

        IEnumerator CheckUpdate(string serverUrl)
        {
            //服务端校验文件
            string fileURL = serverUrl +ABDefine.ProjectVerifyFile;

            //WWW请求，下载服务端的校验文件
            WWW www = new WWW(fileURL);
            //等待www 加载完成，再往下运行。
            yield return www;

            //网络错误检查
            if ((www.error!=null) && (!string.IsNullOrEmpty(www.error)))
            {
                Debug.LogError("### ERROR ！  发生网络错误，请检查服务器是否开启、URL是否正确、网络是否正常链接。 详细错误： www.error: "+www.error);
                yield break;
            }
            //判断客户端本地是否有此目录（下载目录）
            if (!Directory.Exists(_DownloadPath))
            {
                Directory.CreateDirectory(_DownloadPath);
            }
//            //开始下载校验文件，且写入本地
//            File.WriteAllBytes(_DownloadPath + ABDefine.ProjectVerifyFile,www.bytes);

            /*  步骤2： 根据“校验文件”，客户端逐条读取资源文件，然后与本客户端相同的资源文件，进行MD5编码对比。  */
            //读取资源文件里面的内容
            string strServerFileText = www.text;
            string[] lines = strServerFileText.Split('\n');//按照换行进行截取

            //遍历每一行
            for (int i = 0; i < lines.Length; i++)
            {
                //如果校验文件出现“空行”，则不予处理
                if (string.IsNullOrEmpty(lines[i]))
                {
                    continue;
                }
                //得到校验文件每行的“文件名”与“MD5”编码
                string[] fileAndMD5=lines[i].Split('|');
                string strServerFileName = fileAndMD5[0].Trim();  //（服务端）文件名称
                string strServerMD5 = fileAndMD5[1].Trim(); //(服务端) 文件所对应的MD5编码
                //得到本地这个文件
                string strLocalFile = _DownloadPath + "/" + strServerFileName;

                /*  步骤3： 如果客户端没有服务器（增加的）文件，直接下载服务器端文件即可。 */
                if (!File.Exists(strLocalFile))
                {
                    //Debug.Log(GetType() + "/DownloadResourceAndCheckUpdate()/strLocalFile: " + strLocalFile + " 不存在，需要从服务器下载。");
                    Debug.Log("有更新--文件不存在");
                    MsgCenter.Instance.Dispatch(AreaCode.UI,UIEvent.SHOW_UPDATE_CHOICE,true);
                    yield break;
//                    //对于文件本地不存在的文件夹，需要进行创建
//                    string dir = Path.GetDirectoryName(strLocalFile);
//                    if (!string.IsNullOrEmpty(dir))
//                    {
//                        Directory.CreateDirectory(dir);
//                    }
//                    //通过WWW，开始正式下载服务端的文件，且写入本地指定路径
//                    yield return StartCoroutine(DownLoadFileByWWW(serverUrl + "/" + strServerFileName, strLocalFile));
                }
                /*  步骤4： 客户端存在与服务端相同的文件名，但是MD5编码对比不一致，说明服务器端对应的资源文件发生的更新，则客户端
              下载最新的资源文件。  */
                else
                {
                    //根据客户端本地文件名称，得到本地的MD5编码
                    string strLocalMD5 = Helps.GetMD5Vlues(strLocalFile);

                    //服务器端MD5编码，与本地生成的MD5编码，做比较
                    if (!strLocalMD5.Equals(strServerMD5))
                    {
                        Debug.Log("有更新--文件不一样");
                        MsgCenter.Instance.Dispatch(AreaCode.UI,UIEvent.SHOW_UPDATE_CHOICE,true);
                        yield break;
//                        //如果比较不一致，删除本地对应文件。
//                        File.Delete(strLocalFile);
//                        //从服务器下载新的对应文件
//                        yield return StartCoroutine(DownLoadFileByWWW(serverUrl+"/"+strServerFileName,strLocalFile)); 
//                        //提示
//                        Debug.Log(GetType() + "/DownloadResourceAndCheckUpdate()/MD5 服务端与客户端比较不一致，从服务器更新这个资源文件。 (服务器)资源文件名称：" + strServerFileName);                            
                    }
                }
            }//for_end
            BroadcastMessage(ABDefine.ReceiveInfoStartRuning, SendMessageOptions.DontRequireReceiver);
        }

        public void StartDownload()
        {
            //检测资源进行对比更新
            StartCoroutine(DownloadResourceAndCheckUpdate(_ServerURL));
        }
        
        /// <summary>
        /// 检测资源进行对比更新
        /// </summary>
        /// <param name="serverUrl">服务器下载URL</param>
        /// <returns></returns>
        IEnumerator DownloadResourceAndCheckUpdate(string serverUrl)
        {
            /*  步骤1： 下载“校验文件”到客户端  */
            //参数检查
            if (string.IsNullOrEmpty(serverUrl))
            {
                Debug.LogError(GetType()+ "/DownloadResourceAndCheckUpdate()/协程的方法参数不能为null, 请检查重试！ " );
                yield break; //相当于C#方法 return null;
            }
            //服务端校验文件
            string fileURL = serverUrl +ABDefine.ProjectVerifyFile;

            //WWW请求，下载服务端的校验文件
            WWW www = new WWW(fileURL);
            //等待www 加载完成，再往下运行。
            yield return www;

            //网络错误检查
            if ((www.error!=null) && (!string.IsNullOrEmpty(www.error)))
            {
                Debug.LogError("### ERROR ！  发生网络错误，请检查服务器是否开启、URL是否正确、网络是否正常链接。 详细错误： www.error: "+www.error);
                yield break;
            }
            //判断客户端本地是否有此目录（下载目录）
            if (!Directory.Exists(_DownloadPath))
            {
                Directory.CreateDirectory(_DownloadPath);
            }
            //开始下载校验文件，且写入本地
            File.WriteAllBytes(_DownloadPath + ABDefine.ProjectVerifyFile,www.bytes);

            /*  步骤2： 根据“校验文件”，客户端逐条读取资源文件，然后与本客户端相同的资源文件，进行MD5编码对比。  */
            //读取资源文件里面的内容
            string strServerFileText = www.text;
            string[] lines = strServerFileText.Split('\n');//按照换行进行截取

            //遍历每一行
            for (int i = 0; i < lines.Length; i++)
            {
                //如果校验文件出现“空行”，则不予处理
                if (string.IsNullOrEmpty(lines[i]))
                {
                    continue;
                }
                //得到校验文件每行的“文件名”与“MD5”编码
                string[] fileAndMD5=lines[i].Split('|');
                string strServerFileName = fileAndMD5[0].Trim();  //（服务端）文件名称
                string strServerMD5 = fileAndMD5[1].Trim(); //(服务端) 文件所对应的MD5编码
                //得到本地这个文件
                string strLocalFile = _DownloadPath + "/" + strServerFileName;

                /*  步骤3： 如果客户端没有服务器（增加的）文件，直接下载服务器端文件即可。 */
                if (!File.Exists(strLocalFile))
                {
                    Debug.Log(GetType() + "/DownloadResourceAndCheckUpdate()/strLocalFile: " + strLocalFile + " 不存在，需要从服务器下载。");
                    //对于文件本地不存在的文件夹，需要进行创建
                    string dir = Path.GetDirectoryName(strLocalFile);
                    if (!string.IsNullOrEmpty(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    //通过WWW，开始正式下载服务端的文件，且写入本地指定路径
                    yield return StartCoroutine(DownLoadFileByWWW(serverUrl + "/" + strServerFileName, strLocalFile));
                }
                /*  步骤4： 客户端存在与服务端相同的文件名，但是MD5编码对比不一致，说明服务器端对应的资源文件发生的更新，则客户端
              下载最新的资源文件。  */
                else
                {
                    //根据客户端本地文件名称，得到本地的MD5编码
                    string strLocalMD5 = Helps.GetMD5Vlues(strLocalFile);

                    //服务器端MD5编码，与本地生成的MD5编码，做比较
                    if (!strLocalMD5.Equals(strServerMD5))
                    {
                        //如果比较不一致，删除本地对应文件。
                        File.Delete(strLocalFile);
                        //从服务器下载新的对应文件
                        yield return StartCoroutine(DownLoadFileByWWW(serverUrl+"/"+strServerFileName,strLocalFile)); 
                        //提示
                        Debug.Log(GetType() + "/DownloadResourceAndCheckUpdate()/MD5 服务端与客户端比较不一致，从服务器更新这个资源文件。 (服务器)资源文件名称：" + strServerFileName);                            
                    }
                }
            }//for_end

            yield return new WaitForEndOfFrame();
            //提示完成
            Debug.Log(GetType() + "/DownloadResourceAndCheckUpdate()/更新完成！ 可以开始游戏主逻辑！");
            //向下广播，通知游戏启动主逻辑开始
            BroadcastMessage(ABDefine.ReceiveInfoStartRuning, SendMessageOptions.DontRequireReceiver);
            

        }//DownloadResourceAndCheckUpdate_end


        /// <summary>
        /// 通过WWWW下载文件，且写入本地指定路径
        /// </summary>
        /// <param name="wwwURL">WWW 的URL地址</param>
        /// <param name="localFileAddress">本地文件地址</param>
        /// <returns></returns>
        IEnumerator DownLoadFileByWWW(string wwwURL,string localFileAddress)
        {
            //开始网络下载
            WWW www = new WWW(wwwURL);
            //等待www加载完成，往下运行
            yield return www;
            //检查网络状态
            if (www.error!=null && (!string.IsNullOrEmpty(www.error)))
            {
                Debug.LogError(GetType()+ "/DownLoadFileByWWW()/WWW 下载出现问题，请检查！ Error info: "+www.error);
            }
            //下载完成，写入本地文件
            File.WriteAllBytes(localFileAddress,www.bytes);
        }//DownLoadFileByWWW_end

    }//Class_end
}


