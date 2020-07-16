/***
 *
 *  Title: "热更新流程设计"课程项目
 *        
 *        创建项目的校验文件。
 *
 *  Description:
 *        功能：
 *          1：针对生成的AB包，与各种资源文件（lua/Menifest/Json。。。），生成MD5校验文件。
 *          2：本步骤需要在Unity的编辑器下运行，需要先执行AssetBundle 文件生成。
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
    public class CreateVerifyFiles 
    {
        [MenuItem("AssetBundelTools/Create Verify Files")]
        public static void CreateFileMethod()
        {
            /* 定义局部变量 */
            string abOutPath = string.Empty;                            //AssetBundle 的输出路径
            string verifyFileOutPath = string.Empty;                    //校验文件的路径
            List<string> fileList= new List<string>();                  //存储所有合法文件的相对路径信息集合


            /* 定义校验文件的输出路径*/
            abOutPath = PathTools.GetABOutPath();
            //verifyFileOutPath = abOutPath + "/ProjectVerifyFile.txt";//Original 
            verifyFileOutPath = abOutPath + ABDefine.ProjectVerifyFile;

            //Debug.Log("verifyFileOutPath=" + verifyFileOutPath);

            /* 如果本项目已经有校验文件，则进行覆盖。*/
            if (File.Exists(verifyFileOutPath))
            {
                File.Delete(verifyFileOutPath);
            }

            /* 遍历当前文件夹（校验文件的输出路径），所有的文件，生成MD5编码。*/
            ListFile(new DirectoryInfo(abOutPath),ref fileList);

            /* 把文件的名称与对应的MD5编码，写入校验文件。*/
            WriteVerifyFile(verifyFileOutPath, abOutPath,fileList);
        }//CreateFileMethod_end

        /// <summary>
        /// 遍历当前文件夹（校验文件的输出路径），得到所有合法的文件
        /// </summary>
        /// <param name="fileSysInfo">文件(夹)路径信息</param>
        /// <param name="fileList">输入输出参数：把所有合法的文件（相对路径）写入集合</param>
        private static void ListFile(FileSystemInfo fileSysInfo,ref List<string> fileList)
        {
            //文件系统转为目录系统
            DirectoryInfo dirInfo=fileSysInfo as DirectoryInfo;
            //获取文件夹下所有的文件信息（文件系统，包括文件，与文件夹）
            FileSystemInfo[] fileSysInfos=dirInfo.GetFileSystemInfos();  //这里我们把文件与目录，都看成文件系统信息

            foreach (FileSystemInfo item in fileSysInfos)
            {
                //假定文件系统，先是一个文件类型
                FileInfo fileInfo=item as FileInfo;
                //如果是文件，就写入集合中
                if (fileInfo != null)   //就是文件类型
                {
                    //把Win系统中路径分割符改为Unity的类型
                    string strFileFullName = fileInfo.FullName.Replace("\\","/");
                    //过滤无效文件
                    string fileExt = Path.GetExtension(strFileFullName);
                    if (fileExt.EndsWith(".meta"))
                    {
                        continue;
                    } else if (fileExt.EndsWith(".bak"))
                    {
                        continue;
                    }
                    //合法类型文件
                    fileList.Add(strFileFullName);//??
                }
                else {
                    //就是文件夹（目录）,递归调用下一层文件夹
                    ListFile(item,ref fileList);
                     
                }

            }


        }


        /// <summary>
        /// 把文件的名称与对应的MD5编码，写入校验文件
        /// </summary>
        /// <param name="verifyFileOutPath">写入校验文件的路径</param>
        /// <param name="abOutPath">AssetBundle 的输出路径</param>
        /// <param name="fileLists">存储所有合法文件的相对路径信息集合</param>
        private static void WriteVerifyFile(string verifyFileOutPath,string abOutPath, List<string> fileLists)
        {
            using (FileStream fs=new FileStream(verifyFileOutPath,FileMode.CreateNew))
            {
                using (StreamWriter sw=new StreamWriter(fs))
                {
                    for (int i = 0; i < fileLists.Count; i++)
                    {
                        //获取文件的名称
                        string strFile = fileLists[i];
                        //测试
                        //Debug.Log(strFile);
                        //生成此文件的对应MD5编码数值
                        //string strFileMD5 = "MD5 编码xxxxxxyyyyyzzzzz  临时测试使用"; //？？？ 本表达式没有写完，需要定义一个生成MD5 的方法。
                        string strFileMD5 = Helps.GetMD5Vlues(strFile);

                        //把文件中的全路径信息去除，保留相对路径。
                        string strTrueFileName = strFile.Replace(abOutPath + "/", string.Empty);
                        //做参数检查，写入文件前。
                        //。。。。
                        //写入文件
                        sw.WriteLine(strTrueFileName + "|" + strFileMD5);
                    }//for_end
                }
            }

            //提示用户生成操作完毕
            Debug.Log("CreateVerifyFiles.cs/WriteVerifyFile()/创建校验文件成功！");
            //刷新Unity 编辑器
            AssetDatabase.Refresh();
        }//WriteVerifyFile_end



    }//Class_end
}


