using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MessageInfoPanel : UIBase
{
    /// <summary>
    /// 提示面板的内容文本
    /// </summary>
    private Text infoText;
    /// <summary>
    /// 控制信息透明度的组件
    /// </summary>
    private CanvasGroup cg;
    

    [SerializeField]
    [Range(0,3)]
    private float animationTime = 1;
    //动画计时
    private float timer;

    private void Awake()
    {
        Bind(UIEvent.MessageInfoPanel);
    }

    private void Start()
    {
        infoText = transform.GetChild(0).GetComponent<Text>();
        cg = transform.GetChild(0).GetComponent<CanvasGroup>();

        cg.alpha = 0;
    }

    public override void Execute(int eventCode, object message)
    {
        
        switch (eventCode)
        {
            case UIEvent.MessageInfoPanel:
                UIMsg uiMsg = message as UIMsg;
                ShowUIPanel(uiMsg.message,uiMsg.color);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 提示信息面板
    /// </summary>
    /// <param name="message">提示的消息</param>
    /// <param name="color">消息的颜色</param>
    private void ShowUIPanel(string message, Color color)
    {
        this.infoText.text = message;
        this.infoText.color = color;
        cg.alpha = 0;
        timer = 0;
        //开始动画
        StartCoroutine(UIPanelAnimation());
    }

    IEnumerator UIPanelAnimation() {
        while (cg.alpha < 1f)
        {
            cg.alpha += Time.deltaTime*2;
            yield return new WaitForEndOfFrame();
        }

        while (timer < animationTime)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        while (cg.alpha > 0f)
        {
            cg.alpha -= Time.deltaTime*3;
            yield return new WaitForEndOfFrame();
        }
        
    }
}

