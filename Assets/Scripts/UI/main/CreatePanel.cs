﻿using System;
using System.Collections;
using System.Collections.Generic;
using Protocol.Code;
using UnityEngine;
using UnityEngine.UI;

public class CreatePanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.CREATE_PANEL_ACTIVE);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.CREATE_PANEL_ACTIVE:
                SetPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    private InputField inputName;
    private Button btnCreate;

    private UIMsg uiMsg = new UIMsg();
    private MessageData serverMsg = new MessageData();

    // Start is called before the first frame update
    void Start()
    {

        inputName = transform.Find("inputName").GetComponent<InputField>();
        btnCreate = transform.Find("btnCreate").GetComponent<Button>();

        btnCreate.onClick.AddListener(CreateClick);

        SetPanelActive(false);
    }


    

    /// <summary>
    /// 创建角色名称
    /// </summary>
    private void CreateClick()
    {
        //判断是否合法
        if (string.IsNullOrEmpty(inputName.text))
        {
            uiMsg.Set("角色名称创建不合法！",Color.red);
            Dispatch(AreaCode.UI,UIEvent.MessageInfoPanel,uiMsg);
            return;
        }

        //给服务器发消息 创建角色
        serverMsg.Set(OpCode.USER,UserCode.CREAT_CREQ,inputName.text);
        Dispatch(AreaCode.NET,0,serverMsg);
    }
    
}
