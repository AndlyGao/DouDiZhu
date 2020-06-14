using System.Collections;
using System.Collections.Generic;
using Protocol.Code;
using Protocol.Dto.Fight;
using UnityEngine;
using UnityEngine.UI;

public class OverPanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.GameOver,UIEvent.BACKTOFIGHT);
    }

    protected override void SetPanelActive(bool active)
    {
        bgImg.SetActive(active);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        backBtn.onClick.RemoveListener(BackToFight_CREQ);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.GameOver:
                GameOver(message as OverDto);
                break;

            case UIEvent.BACKTOFIGHT:
                BackToFight_SRES();
                break;
            default:
                break;
        }


    }


    private GameObject bgImg;
    private Text resultTxt;
    private Text beensTxt;
    private Button backBtn;


    private MessageData serverMsg = new MessageData();
    private void Start()
    {
        bgImg = transform.Find("bgImg").gameObject;
        resultTxt = bgImg.transform.Find("resultTxt").GetComponent < Text>() ;
        beensTxt = bgImg.transform.Find("beensTxt").GetComponent < Text>() ;
        backBtn = bgImg.transform.Find("backBtn").GetComponent < Button>();
        backBtn.onClick.AddListener(BackToFight_CREQ);
        SetPanelActive(false);
    }

    private void BackToFight_SRES()
    {
        SetPanelActive(false);
    }

    /// <summary>
    /// 重新开始了  //默认准备
    /// </summary>
    private void BackToFight_CREQ()
    {
        //给服务器发消息我要再来
        serverMsg.Set(OpCode.FIGHT,FightCode.BACKTOFIGHT_CREQ,null);
        Dispatch(AreaCode.NET,0,serverMsg);
    }
    
    private void GameOver(OverDto dto)
    {
        var identity = dto.winIdentity;
        //var winUser = dto.winUserIdsList;
        var beens = dto.beens;
        beensTxt.text = "欢乐豆 ：+ " + beens;
        resultTxt.text = identity.ToString() + "胜利";
        SetPanelActive(true);
    }
}
