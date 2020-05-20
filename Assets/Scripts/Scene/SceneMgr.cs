using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : ManagerBase
{
    public static SceneMgr Instance;

    //private SceneManager sceneManager;

    private LoadSceneMsg loadSceneMsg;

    private Action OnSceneLoaded = null;


    private void Awake()
    {
        Instance = this;
        //注册消息
        Add(SceneEvent.LOAD_SCENE,this);

        //场景加载完成  回调事件
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

    }

    /// <summary>
    /// 场景加载完成回调
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="loadSceneMode"></param>
    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (this.OnSceneLoaded != null)
            OnSceneLoaded();
        //调用完成重置
        this.OnSceneLoaded = null;
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case SceneEvent.LOAD_SCENE:
                loadSceneMsg = message as LoadSceneMsg;
                LoadScene(loadSceneMsg);
                break;
            default:
                break;
        }
    }

    private void LoadScene(LoadSceneMsg msg)
    {
        if (msg.sceneBuildIndex != -1)
            SceneManager.LoadScene(msg.sceneBuildIndex);
        if(!string.IsNullOrEmpty(msg.sceneBuildName))
            SceneManager.LoadScene(msg.sceneBuildName);
        if (msg.onSceneLoaded != null)
            OnSceneLoaded = msg.onSceneLoaded;
    }
}
