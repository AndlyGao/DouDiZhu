using System;
using System.Collections;
using System.Collections.Generic;
using HotUpdateModel;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.SHOW_UPDATE_CHOICE,UIEvent.SHOW_CLOSEBTN);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.SHOW_UPDATE_CHOICE:
                SetPanelActive(true);
                break;
            case UIEvent.SHOW_CLOSEBTN:
            {
                closeBtn.gameObject.SetActive(true);
                tipTxt.text = "更新已完成";
            }
                break;
            default:
                break;
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        
        confirmBtn.onClick.RemoveListener(Confirm);
        cancelBtn.onClick.RemoveListener(Cancel);
        closeBtn.onClick.RemoveListener(Close);
    }

    private Button confirmBtn, cancelBtn,closeBtn;
    private Text tipTxt;
    private UpdateResourcesFileFromServer urffs;
    
    private void Start()
    {
        urffs = FindObjectOfType<UpdateResourcesFileFromServer>();
        confirmBtn = transform.Find("confirmBtn").GetComponent<Button>();
        cancelBtn = transform.Find("cancelBtn").GetComponent<Button>();
        closeBtn = transform.Find("closeBtn").GetComponent<Button>();
        tipTxt = transform.Find("tipTxt").GetComponent<Text>();
        
        confirmBtn.onClick.AddListener(Confirm);
        cancelBtn.onClick.AddListener(Cancel);
        closeBtn.onClick.AddListener(Close);
        closeBtn.gameObject.SetActive(false);
        SetPanelActive(false);
    }

    private void Confirm()
    {
        //进行更新
        urffs.StartDownload();
        tipTxt.text = "正在更新中...";

        confirmBtn.gameObject.SetActive(false);
        cancelBtn.gameObject.SetActive(false);
    }

    private void Cancel()
    {
        //直接开始游戏
        SetPanelActive(false);
    }
    
    private void Close()
    {
        SetPanelActive(false);
    }
}
