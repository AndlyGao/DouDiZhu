﻿using System.Collections;
using System.Collections.Generic;
using Protocol.Dto;
using Protocol.Dto.Card;
using Protocol.Dto.Constant;
using Protocol.Dto.Fight;
using UnityEngine;
using UnityEngine.UI;
public enum FightState
{
    match,
    fight,
}
/// <summary>
/// player面板信息 基类
/// </summary>
public class StatePanel : UIBase
{
    private Coroutine timeCoroutine;

    public FightState fightState = FightState.match;
    /// <summary>
    /// 角色的数据
    /// </summary>
    protected UserDto userDto;

    protected Text readyTxt;
    /// <summary>
    /// 时钟txt  后期替换计时器
    /// </summary>
    protected Text clockTxt;
    /// <summary>
    /// 抢不抢 处不处的提示Txt 到了自己出牌或者抢了，就该隐藏了
    /// </summary>
    protected Text operateTxt;
    protected Image identityImg;
    protected Text dialogTxt;

    protected Text idTxt;
    //protected Text chupaiResTxt;
    protected Transform chupaiParent;
    protected List<CardItem> chupaiResultList = new List<CardItem>();

    protected MessageData serverMsg;
    protected AudioMsg audioMsg;

    protected int showTime = 3;
    protected float timer = 0;
    private bool isShow = false;

    protected virtual void Awake()
    {
        Bind(UIEvent.PLAYER_READY,
            UIEvent.PLAYER_LEAVE,
            UIEvent.PLAYER_ENTER,
            UIEvent.PLAYER_HIDE_STATE,
            UIEvent.PLAYER_CHAT,
            UIEvent.HIDE_PLAYER_READYBTN,
            UIEvent.SHOW_PLAYER_JIAO_BTN_ACTIVE,
            UIEvent.SHOW_PLAYER_CHUPAI_BTN_ACTIVE,
            UIEvent.BUQIANG_LANDLORD_OPERATE,
            UIEvent.QIANG_LANDLORD_OPERATE,
            UIEvent.CHUPAI_OPERATE,
            UIEvent.BUCHU_OPERATE,
            UIEvent.PLAYER_CHANGE_IDENTITY,
            UIEvent.GAME_RESTAET);
    }

    public override void Execute(int eventCode, object message)
    {
        if (userDto == null) return;
        switch (eventCode)
        {
            case UIEvent.BUQIANG_LANDLORD_OPERATE:
                {
                    SetOperateResult((int)message,"不抢");
                }

                break;
            case UIEvent.QIANG_LANDLORD_OPERATE:
                {
                    // 抢地主要不要提示  有待商榷
                    SetOperateResult((int)message,"抢地主");
                }

                break;
            case UIEvent.CHUPAI_OPERATE:
                {
                    SetOperateResult(message as ChuPaiDto);
                }

                break;
            case UIEvent.BUCHU_OPERATE:
                {
                    SetOperateResult((int)message, "过");
                }

                break;
            case UIEvent.PLAYER_CHANGE_IDENTITY:
                {
                    int userId = (int)message;
                   
                    SetIdentity(userId == userDto.id);
                }
                break;
            case UIEvent.SHOW_PLAYER_JIAO_BTN_ACTIVE:
                {
                    SetQiangTurn((int)message);
                }

                break;
            case UIEvent.SHOW_PLAYER_CHUPAI_BTN_ACTIVE:
                {
                    SetChuTurn((int)message);
                }

                break;
            case UIEvent.PLAYER_READY:
                {
                    int userId = (int)message;
                    if (userDto.id == userId)//如果是自己
                    {
                        SetReadyState();
                    }
                }
                break;
            case UIEvent.PLAYER_LEAVE:
                {
                    
                    int userId = (int)message;
                    if ( userId == userDto.id)
                    {
                        
                        SetPanelActive(false);
                    }
                }
                break;
            case UIEvent.PLAYER_ENTER:
                {
                   
                    int userId = (int)message;
                   
                    if (userId == userDto.id)
                    {
                       // SetPanelActive(true);
                    }
                }
                break;
            case UIEvent.PLAYER_HIDE_STATE:
                {
                    //SetPanelActive(false);
                    //游戏开始了，准备txt隐藏
                    HideReayState();
                }
                break;
            case UIEvent.PLAYER_CHAT:
                ChatResponse(message as ChatMsg);
                break;

            case UIEvent.GAME_RESTAET:
                {
                    //不抢的ui隐藏
                    operateTxt.gameObject.SetActive(false);
                    //出牌结果ui隐藏
                    //chupaiResTxt.gameObject.SetActive(false);
                    HideChupaiResult();
                }
                break;
            
            default:
                break;
        }
    }


    
    /// <summary>
    /// 隐藏出牌结果
    /// </summary>
    private void HideChupaiResult()
    {

        for (int i = 0; i < chupaiResultList.Count; i++)
        {
            chupaiResultList[i].gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 显示出牌结果
    /// </summary>
    /// <param name="cards"></param>
    private void ShowChupaiResult(List<CardDto> cards)
    {
        //chupaiResTxt.gameObject.SetActive(true);
        HideChupaiResult();
        
        for (int i = 0; i < cards.Count; i++)
        {
            chupaiResultList[i].Init(cards[i],i,true);
        }
        //chupaiResTxt.text = content;
    }

    /// <summary>
    /// 显示操作结果
    /// 如果是抢地主  ： 自己什么都不显示
    /// 如果是出牌  ： 显示自己出的什么牌
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="content"></param>
    protected virtual string SetOperateResult(int userId,string content)
    {
        var flag = userDto.id == userId;
        if (flag)//如果是自己 
        {
            //clockTxt.gameObject.SetActive(flag);

            if (!operateTxt.gameObject.activeInHierarchy)
            {
                operateTxt.gameObject.SetActive(true);
            }
            operateTxt.text = content;
            if (content == "过")
                //chupaiResTxt.gameObject.SetActive(false);
                HideChupaiResult();
        }
        if (content == "抢地主") {
            operateTxt.gameObject.SetActive(false);//操作结果ui隐藏
        }

        return content;
    }

    /// <summary>
    /// 出牌结果的单独方法
    /// </summary>
    /// <param name="cards"></param>
    protected virtual void SetOperateResult(ChuPaiDto dto)
    {
        var flag = dto.userId == this.userDto.id;
        

        //是自己
        if (flag)
        {
            ShowChupaiResult(dto.cardsList);
            //chupaiResTxt.gameObject.SetActive(true);
            //string content = string.Empty;
            //foreach (var card in dto.cardsList)
            //{
            //    content += card.name;
            //}
            //chupaiResTxt.text = content; 
        }
    }

    /// <summary>
    /// 抢地主 轮流转
    /// </summary>
    /// <param name="userId"></param>
    protected virtual bool SetQiangTurn(int userId)
    {
        //判断是不是自己叫
        //如果是自己 叫地主和不叫地主的按钮显示出来 并把时钟显示出来
        //不是自己 叫地主和不叫地主的按钮隐藏

        var flag = userId == this.userDto.id;

        //将上次的计时取消
        if (timeCoroutine != null)
        {
            StopCoroutine(timeCoroutine);
            timeCoroutine = null;
        }

        clockTxt.text = 30.ToString();
        clockTxt.gameObject.SetActive(flag);
        if (flag)
        {
            timeCoroutine = StartCoroutine(CalcuTime(clockTxt,30));
        }
        
        if (flag && operateTxt.gameObject.activeInHierarchy)
        {
            operateTxt.gameObject.SetActive(false);
        }
        return flag;
    }

    /// <summary>
    /// 出牌轮流转
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    protected virtual bool SetChuTurn(int userId)
    {
        //判断是不是自己出
        //如果是自己 出牌和不出的按钮显示出来 并把时钟显示出来
        //不是自己 叫地主和不叫地主的按钮隐藏

        var flag = userId == this.userDto.id;

        //将上次的计时取消
        if (timeCoroutine != null)
        {
            StopCoroutine(timeCoroutine);
            timeCoroutine = null;
        }

        clockTxt.text = 30.ToString();
        clockTxt.gameObject.SetActive(flag);
        if (flag)
        {
            timeCoroutine = StartCoroutine(CalcuTime(clockTxt,30));

            operateTxt.gameObject.SetActive(false);
            //chupaiResTxt.gameObject.SetActive(false);
            HideChupaiResult();
        }
        return flag;
    }

    /// <summary>
    /// 准备响应
    /// </summary>
    protected virtual void SetReadyState()
    {
        readyTxt.gameObject.SetActive(true);
    }

    /// <summary>
    /// 游戏开始了  需要隐藏一些东西
    /// </summary>
    protected virtual void HideReayState()
    {
        //准备按钮隐藏
        readyTxt.gameObject.SetActive(false);
        //计算time携程停止
        clockTxt.gameObject.SetActive(false);
        if (timeCoroutine != null)
        {
            StopCoroutine(timeCoroutine);
            timeCoroutine = null;
        }
        

        //抢不抢地主ui隐藏  需要override
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        readyTxt = transform.Find("readyTxt").GetComponent<Text>();
        clockTxt = transform.Find("clockTxt").GetComponent<Text>();
        operateTxt = transform.Find("operateTxt").GetComponent<Text>();
        identityImg = transform.Find("identityImg").GetComponent<Image>();
        dialogTxt = transform.Find("dialogTxt").GetComponent<Text>();
        idTxt = transform.Find("idTxt").GetComponent<Text>();
        //chupaiResTxt = transform.Find("chupaiResTxt").GetComponent<Text>();
        chupaiParent = transform.Find("chupaiParent");

        //创建20张牌
        for (int i = 0; i < 20; i++)
        {
            var cardPrefab = Resources.Load(GlobalData.MyCardPath);
            var cardGO = Instantiate(cardPrefab, chupaiParent) as GameObject;
            chupaiResultList.Add(cardGO.GetComponent<CardItem>());
            Destroy(cardGO.GetComponent<BoxCollider2D>());
            cardGO.SetActive(false);
        }


        serverMsg = new MessageData();
        audioMsg = new AudioMsg();
        //默认状态
        if(readyTxt != null)
            readyTxt.gameObject.SetActive(false);
        dialogTxt.gameObject.SetActive(false);
        clockTxt.gameObject.SetActive(false);
        operateTxt.gameObject.SetActive(false);
        //chupaiResTxt.gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置身份 
    /// </summary>
    /// <param name="identity"></param>
    protected void SetIdentity(bool identity)
    {
        string identityStr = identity ? "Landlord" : "Farmer";
        identityImg.sprite = Resources.Load<Sprite>("Identity/" + identityStr);
    }

    /// <summary>
    /// 外界调用显示对话
    /// </summary>
    /// <param name="content"></param>
    protected void ShowDialog(string content)
    {
        dialogTxt.text = content;
        SetDialogActive(true);
        //显示动画
        isShow = true;
        //当前还对话框还未消失  又重新发了一个消息
        timer = 0;
    }

    protected virtual void Update()
    {
        if (isShow)
        {
            timer += Time.deltaTime;
            if (timer >= showTime)
            {
                SetDialogActive(false);
                timer = 0;
                isShow = false;
            }
        }
    }

    

    /// <summary>
    /// 设置对面面板显示与隐藏
    /// </summary>
    /// <param name="active"></param>
    protected void SetDialogActive(bool active)
    {
        dialogTxt.gameObject.SetActive(active);
    }

    protected virtual void ChatResponse(ChatMsg msg)
    {
        if (this.userDto == null) return;
        //如果是自己
        if (msg.userId == this.userDto.id)
        {
            //显示聊天框
            ShowDialog(ChatConstant.GetContent(msg.chatMsgType));

            audioMsg.Set("Sound/Chat/Chat_",msg.chatMsgType.ToString());
            Dispatch(AreaCode.AUDIO,AudioEvent.EFFECTAUDIO,audioMsg);
        }
    }

    IEnumerator CalcuTime(Text txt,int num)
    {
        yield return new WaitForSeconds(1);

        for (int i = num; i >= 0; i--)
        {
            txt.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
    }
}
