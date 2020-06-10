using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpManager : MonoBehaviour
{
    /// <summary>
    /// 储存所需要的分辨率 我这里储存16：9 要别的分辨率可以打印所有去看
    /// </summary>
    public enum EResolution
    {
        _1280x720 = 8,
        _1600x900 = 13,
        _1920x1080 = 15,
    }
    Resolution[] resolutions;

    private void Start()
    {
        //所有可以设置的分辨率 unity自带
        resolutions = Screen.resolutions;

        ////可以打印出来去选择自己需要的分辨率
        //foreach (Resolution item in resolutions)
        //{
        //    Debug.Log(item.width + "x" + item.height);
        //}
        SetResolution(EResolution._1920x1080);
    }

    /// <summary>
    /// 设置分辨率
    /// </summary>
    public void SetResolution(EResolution type)
    {
        //前两个是分辨率 后两个是是否全屏 最后一个是以多少Hz来显示默认为0
        Screen.SetResolution(resolutions[(int)type].width, resolutions[(int)type].height, false);
    }

    /// <summary>
    /// 是否全屏
    /// </summary>
    /// <param name="isFull"></param>
    public void IsFullScreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }
}
