using System.Collections;
using System.Collections.Generic;
using Protocol.Code;
using Protocol.Dto;
using Protocol.Dto.Fight;
using UnityEngine;
using UnityEngine.UI;


public class MyPlayerStatePanel : StatePanel
{
    
    public Text testChupaiTxt;
    protected override void Awake()
    {
        base.Awake();
        Bind(UIEvent.SET_MYPLAYER_DATA,UIEvent.CHANGE_MUTIPLIER,UIEvent.testChupai,UIEvent.GameOver);
    }

    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
        switch (eventCode)
        {
            case UIEvent.testChupai:
                {
                    var msg = message as List<int>;
                    var s = string.Empty;
                    foreach (var int32 in msg)
                    {
                        s += int32+"/";
                    }
                    testChupaiTxt.text = s;
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
                {
                    UpdateMutiplier((int)message);
                    fightState = FightState.fight;
                }
                break;
            case UIEvent.GameOver:
                {
                    var overDto = message as OverDto;
                    bool isWin = overDto.winUserIdsList.Contains(this.userDto.id);
                    int beens = 0;
                    if (overDto.winIdentity == Identity.Landlord)
                    {
                        beens = isWin ? userDto.beens + overDto.beens : userDto.beens - overDto.beens / 2;
                    }
                    else
                    {
                        beens = isWin ? userDto.beens + overDto.beens / 2 : userDto.beens - overDto.beens;
                    }                   
                    //把豆子加一加
                    UpdateBeens(" x " + beens);
                    //暂停播放背景音乐
                    Dispatch(AreaCode.AUDIO, AudioEvent.BGMAUDIO, -1);
                    //播放结束音乐
                    audioMsg.Set("Sound/Fight/",isWin ? "MusicEx_Win" : "MusicEx_Lose");
                    Dispatch(AreaCode.AUDIO,AudioEvent.EFFECTAUDIO, audioMsg);
                    //6s后播放背景音乐
                }
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
    private Button exitBtn;


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
        exitBtn = transform.Find("exitBtn").GetComponent<Button>();

        readyBtn.onClick.AddListener(ReadyClick);
        chupaiBtn.onClick.AddListener(ChuPaiClick);
        buchuBtn.onClick.AddListener(BuChuClick);
        jiaoBtn.onClick.AddListener(JiaoClick);
        bujiaoBtn.onClick.AddListener(BuJiaoClick);
        exitBtn.onClick.AddListener(Exit);

        chupaiBtn.gameObject.SetActive(false);
        buchuBtn.gameObject.SetActive(false);
        jiaoBtn.gameObject.SetActive(false);
        bujiaoBtn.gameObject.SetActive(false);
        mutiplierTxt.gameObject.SetActive(false);

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

    protected void Exit() {

        //像服务器发送消息。我退出了
        switch (fightState)
        {
            case FightState.match:
                serverMsg.Set(OpCode.MATCHROOM,MatchRoomCode.BACK_CREQ,null);
                break;

            case FightState.fight:
                serverMsg.Set(OpCode.FIGHT, FightCode.BACKTOFIGHT_CREQ, null);
                break;

        }
        Dispatch(AreaCode.NET,0,serverMsg);
    }

    

    protected override bool SetQiangTurn(int userId)
    {
       var flag = base.SetQiangTurn(userId);

        jiaoBtn.gameObject.SetActive(flag);
        bujiaoBtn.gameObject.SetActive(flag);

        return flag;

    }

    protected override bool SetChuTurn(int userId)
    {
        var flag = base.SetChuTurn(userId);
        chupaiBtn.gameObject.SetActive(flag);
        buchuBtn.gameObject.SetActive(flag);
        return flag;
    }
    protected override string SetOperateResult(int userId, string content)
    {
        var str = base.SetOperateResult(userId, content);
        if (str == "抢地主")
        {
            jiaoBtn.gameObject.SetActive(false);
            bujiaoBtn.gameObject.SetActive(false);
        }
        return null;
    }


    public void UpdateBeens(string content)
    {
        beensTxt.text = content;
    }

    public void UpdateMutiplier(int mutiplier)
    {
        if (!mutiplierTxt.gameObject.activeInHierarchy)
        {
            mutiplierTxt.gameObject.SetActive(true);
        }
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
        //出牌
        Dispatch(AreaCode.CHARACTER,CharactorEvent.CHUPAI_CREQ,null);
    }

    private void BuChuClick()
    {
        serverMsg.Set(OpCode.FIGHT,FightCode.BUCHU_CREQ,null);
        Dispatch(AreaCode.NET,0,serverMsg);
    }

    private void JiaoClick()
    {
        serverMsg.Set(OpCode.FIGHT, FightCode.QIANG_LANDLORD_CREQ, true);
        Dispatch(AreaCode.NET, 0, serverMsg);
    }

    private void BuJiaoClick()
    {
        serverMsg.Set(OpCode.FIGHT, FightCode.QIANG_LANDLORD_CREQ, false);
        Dispatch(AreaCode.NET, 0, serverMsg);
    }
}
