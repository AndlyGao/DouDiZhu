using System.Collections;
using System.Collections.Generic;
using Protocol.Code;
using Protocol.Dto;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayerStatePanel : StatePanel
{
    
    protected override void Awake()
    {
        base.Awake();
        Bind(UIEvent.SHOW_PLAYER_CHUPAI_BTN_ACTIVE, UIEvent.SHOW_PLAYER_JIAO_BTN_ACTIVE, UIEvent.SET_MYPLAYER_DATA,UIEvent.CHANGE_MUTIPLIER);
    }

    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
        switch (eventCode)
        {
            case UIEvent.SHOW_PLAYER_JIAO_BTN_ACTIVE:
                {
                    bool b = (bool)message;
                    jiaoBtn.gameObject.SetActive(b);
                    bujiaoBtn.gameObject.SetActive(b);
                }
               
                break;

            case UIEvent.SHOW_PLAYER_CHUPAI_BTN_ACTIVE:
                {
                    bool b = (bool)message;
                    chupaiBtn.gameObject.SetActive(b);
                    buchuBtn.gameObject.SetActive(b);
                }
              
                break;

            case UIEvent.SET_MYPLAYER_DATA:
                {
                    this.userDto = message as UserDto;
                    identityImg.gameObject.SetActive(true);
                    readyBtn.gameObject.SetActive(true);
                    idTxt.text = userDto.name;
                    UpdateBeens(" x " + userDto.beens.ToString());
                }
                break;
            case UIEvent.CHANGE_MUTIPLIER:
                UpdateMutiplier((int)message);
                break;
            
            default:
                break;
        }
    }


    private Button readyBtn;
    private Text beensTxt;
    private Text mutiplierTxt;
    private Button chupaiBtn;
    private Button buchuBtn;
    private Button jiaoBtn;
    private Button bujiaoBtn;


    protected override void Start()
    {
        base.Start();

        beensTxt = transform.Find("beensTxt").GetComponent<Text>();
        mutiplierTxt = transform.Find("mutiplierTxt").GetComponent<Text>();
        readyBtn = transform.Find("readyBtn").GetComponent<Button>();
        chupaiBtn = transform.Find("chupaiBtn").GetComponent<Button>();
        buchuBtn = transform.Find("buchuBtn").GetComponent<Button>();
        jiaoBtn = transform.Find("jiaoBtn").GetComponent<Button>();
        bujiaoBtn = transform.Find("bujiaoBtn").GetComponent<Button>();

        readyBtn.onClick.AddListener(ReadyClick);
        chupaiBtn.onClick.AddListener(ChuPaiClick);
        buchuBtn.onClick.AddListener(BuChuClick);
        jiaoBtn.onClick.AddListener(JiaoClick);
        bujiaoBtn.onClick.AddListener(BuJiaoClick);

        chupaiBtn.gameObject.SetActive(false);
        buchuBtn.gameObject.SetActive(false);
        jiaoBtn.gameObject.SetActive(false);
        bujiaoBtn.gameObject.SetActive(false);

    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        readyBtn.onClick.RemoveListener(ReadyClick);
        chupaiBtn.onClick.RemoveListener(ChuPaiClick);
        buchuBtn.onClick.RemoveListener(BuChuClick);
        jiaoBtn.onClick.RemoveListener(JiaoClick);
        bujiaoBtn.onClick.RemoveListener(BuJiaoClick);
    }

    public void UpdateBeens(string content)
    {
        beensTxt.text = content;
    }

    public void UpdateMutiplier(int mutiplier)
    {
        mutiplierTxt.text = " X " + mutiplier;
    }

    protected override void SetReadyState()
    {
        base.SetReadyState();
        readyBtn.gameObject.SetActive(false);
    }

    private void ReadyClick()
    {
        serverMsg.Set(OpCode.MATCHROOM,MatchRoomCode.READY_CREQ,null);
        Dispatch(AreaCode.NET, 0, serverMsg);
    }


    private void ChuPaiClick()
    {

    }

    private void BuChuClick()
    {

    }

    private void JiaoClick()
    {

    }

    private void BuJiaoClick()
    {

    }
}
