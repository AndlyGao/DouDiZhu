﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchPanel : UIBase {

    private Button matchBtn;
    private Image bgImg;
    private Text promptInfoTxt;
    private Button cancelBtn;
    private Button enterBtn;


    //匹配动画变量

    private float timer;//动画频率计时器
    private const float intervalTime = 1;//动画间隔时间

    private string defaultTxt = "正在匹配中";
    private int dotCount;//动画中的点的数量

    private void Awake()
    {
        Bind(UIEvent.SHOW_ENTERROOMBTN_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {

        switch (eventCode)
        {
            case UIEvent.SHOW_ENTERROOMBTN_ACTIVE:
                enterBtn.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }


    void Start () {
        matchBtn = transform.Find("matchBtn").GetComponent<Button>();
        bgImg = transform.Find("bgImg").GetComponent<Image>();
        promptInfoTxt = transform.Find("promptInfoTxt").GetComponent<Text>();
        cancelBtn = transform.Find("cancelBtn").GetComponent<Button>();
        enterBtn = transform.Find("enterBtn").GetComponent<Button>();

        matchBtn.onClick.AddListener(StartMatch);
        cancelBtn.onClick.AddListener(CancelMatch);
        enterBtn.onClick.AddListener(EnterGame);

        ObjectsActive(false);
        enterBtn.gameObject.SetActive(false);

        //SetPanelActive(false);
    }

    void Update()
    {
        if (!promptInfoTxt.IsActive())
            return;

        timer += Time.deltaTime;
        if (timer >= intervalTime)
        {
            DoAnimation();
            timer = 0;
        }
    }



    public override void OnDestroy()
    {
        base.OnDestroy();
        matchBtn.onClick.RemoveListener(StartMatch);
        cancelBtn.onClick.RemoveListener(CancelMatch);
        enterBtn.onClick.RemoveListener(EnterGame);

    }

    /// <summary>
    /// 匹配按钮
    /// </summary>
    private void StartMatch()
    {
        //TODO 向服务器发起匹配请求

        //匹配动画
        ObjectsActive(true);
        DoAnimation();

    }

    /// <summary>
    /// 进入游戏按钮
    /// </summary>
    private void EnterGame()
    {
        
    }

    /// <summary>
    /// 取消匹配按钮
    /// </summary>
    private void CancelMatch()
    {
        ObjectsActive(false);
    }

    /// <summary>
    /// 控制面板物体显示、影藏
    /// </summary>
    /// <param name="b"></param>
    private void ObjectsActive(bool b) {
        promptInfoTxt.gameObject.SetActive(b);
        cancelBtn.gameObject.SetActive(b);

        //初始化动画  起始点数
        dotCount = 0;
    }


    private void DoAnimation() {
        promptInfoTxt.text = defaultTxt;

        //点增加
        dotCount++;
        if (dotCount > 5)
        {
            dotCount = 1;
        }

        for (int i = 0; i < dotCount; i++)
        {
            promptInfoTxt.text += ".";
        }
    
    }
}
