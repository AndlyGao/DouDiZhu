using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPanel : UIBase {

    private void Awake()
    {
        
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            default:
                break;
        }
    }

    private Button setBtn;
    private Button closeBtn;
    private Toggle audioTog;
    private Slider volSld;
    private Button quitBtn;
    private Image bgImg;

    private void Start()
    {
        setBtn = transform.Find("setBtn").GetComponent<Button>();
        closeBtn = transform.Find("closeBtn").GetComponent<Button>();
        quitBtn = transform.Find("quitBtn").GetComponent<Button>();
        audioTog = transform.Find("audioTog").GetComponent<Toggle>();
        volSld = transform.Find("volSld").GetComponent<Slider>();
        bgImg = transform.Find("bgImg").GetComponent<Image>();

        //默认状态
        SetPanelActive(false);

        setBtn.onClick.AddListener(SetBtnClick);
        closeBtn.onClick.AddListener(CloseBtnClick);
        quitBtn.onClick.AddListener(QuitBtnClick);
        audioTog.onValueChanged.AddListener(AudioTogClick);
        volSld.onValueChanged.AddListener(VolSldValueChanged);
    }

    private void SetBtnClick()
    {
        SetPanelActive(true);
    }
    /// <summary>
    /// 关闭按钮事件
    /// </summary>
    private void CloseBtnClick()
    {
        SetPanelActive(false);
    }
    /// <summary>
    /// 退出按钮事件
    /// </summary>
    private void QuitBtnClick()
    {
        Application.Quit();
    }
    /// <summary>
    /// 关闭声音Toggle
    /// </summary>
    /// <param name="flag"></param>
    private void AudioTogClick(bool flag)
    {
        //声音操作
        //TODO
    }
    /// <summary>
    /// 滑动进度条时间
    /// </summary>
    /// <param name="value"></param>
    private void VolSldValueChanged(float value)
    {
        //声音操作
        //TODO
    }

}
