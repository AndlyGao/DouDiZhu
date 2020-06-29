using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 声音模块管理器
/// </summary>
public class AudioManager : ManagerBase
{
    public const int PLAY_AUDIO = 0;

    public static AudioManager Instance = null;
    private AudioSource bgmAS;
    private bool isMute = true;
    private int currentType = -100;
    void Awake()
    {
        Instance = this;
        Add(new int[] { AudioEvent.BGMAUDIO, AudioEvent.CHANG_BGM_VOLUME, AudioEvent.ENABLE_OR_DISABLE_BGM },this);
    }

    public override void Execute(int eventCode, object message)
    {
        if (eventCode == AudioEvent.EFFECTAUDIO)
        {
            base.Execute(eventCode, message);
            return;
        }

        switch (eventCode)
        {
            case AudioEvent.BGMAUDIO:
                BGMHandle((int)message);  //-1：停止  0:普通 1:战斗 2:决战阶段
                break;
            case AudioEvent.CHANG_BGM_VOLUME:
                {
                    bgmAS.volume = (float)message;
                }
                break;
            case AudioEvent.ENABLE_OR_DISABLE_BGM:
                bool value = (bool)message;
                isMute = !value;
                if (!value)
                    bgmAS.Stop();
                else
                    bgmAS.Play();
                break;
            default:
                break;
        }
    }
    private void Start()
    {
        bgmAS = gameObject.AddComponent<AudioSource>();
        bgmAS.playOnAwake = false;
        bgmAS.loop = true;

        //BGMHandle(0);
    }

    public float GetVolume()
    {
        return bgmAS.volume;
    }

    /// <summary>
    /// 返回当前是否设置静音了
    /// </summary>
    /// <returns></returns>
    public bool GetIsMute()
    {
        return isMute;
    }

    private void BGMHandle(int type)
    {
        if (type == currentType)
        {
            return;
        }
        currentType = type;
        bgmAS.Stop();
        if (type == -1)
        {
            return;
        }

        switch (type)
        {
            
            case 0:
                bgmAS.clip = Resources.Load<AudioClip>("Sound/MusicEx/MusicEx_Welcome");
                break;
            case 1:
                bgmAS.clip = Resources.Load<AudioClip>("Sound/MusicEx/MusicEx_Normal");
                break;
            case 2:
                bgmAS.clip = Resources.Load<AudioClip>("Sound/MusicEx/MusicEx_Normal2");
                break;
        }

        if (!isMute)
        {
            bgmAS.Play();
        }
    }

}
