using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPanel : UIBase {

    private Button setBtn;
    private Button closeBtn;
    private Toggle audioTog;
    private Slider volSld;
    private Button quitBtn;
    private Image bgImg;
    private Text audioTxt;
    private Text volTxt;

    private void Start()
    {
        setBtn = transform.Find("setBtn").GetComponent<Button>();
        closeBtn = transform.Find("closeBtn").GetComponent<Button>();
        quitBtn = transform.Find("quitBtn").GetComponent<Button>();
        audioTog = transform.Find("audioTog").GetComponent<Toggle>();
        audioTog.isOn = !AudioManager.Instance.GetIsMute();
        volSld = transform.Find("volSld").GetComponent<Slider>();
        volSld.value = AudioManager.Instance.GetVolume();
        bgImg = transform.Find("bgImg").GetComponent<Image>();
        audioTxt = transform.Find("audioTxt").GetComponent<Text>();
        volTxt = transform.Find("volTxt").GetComponent<Text>();

        //默认状态
        //SetPanelActive(false);
        Active(false);

        setBtn.onClick.AddListener(SetBtnClick);
        closeBtn.onClick.AddListener(CloseBtnClick);
        quitBtn.onClick.AddListener(QuitBtnClick);
        audioTog.onValueChanged.AddListener(AudioTogClick);
        volSld.onValueChanged.AddListener(VolSldValueChanged);
    }

    private void Active(bool b)
    {
        closeBtn.gameObject.SetActive(b);
        audioTog.gameObject.SetActive(b);
        volSld.gameObject.SetActive(b);
        quitBtn.gameObject.SetActive(b);
        bgImg.gameObject.SetActive(b);
        audioTxt.gameObject.SetActive(b);
        volTxt.gameObject.SetActive(b);
    }

    private void SetBtnClick()
    {
        Active(!bgImg.gameObject.activeInHierarchy);
    }
    /// <summary>
    /// 关闭按钮事件
    /// </summary>
    private void CloseBtnClick()
    {
        Active(false);
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
        Dispatch(AreaCode.AUDIO,AudioEvent.ENABLE_OR_DISABLE_BGM,flag);
    }
    /// <summary>
    /// 滑动进度条时间
    /// </summary>
    /// <param name="value"></param>
    private void VolSldValueChanged(float value)
    {
        //声音操作
        Dispatch(AreaCode.AUDIO, AudioEvent.CHANG_BGM_VOLUME, value);
    }

}
