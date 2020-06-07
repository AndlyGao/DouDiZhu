﻿using System.Collections;
using System.Collections.Generic;
using Protocol.Code;
using Protocol.Dto;
using Protocol.Dto.Card;
using Protocol.Dto.Constant;
using Protocol.Dto.Fight;
using UnityEngine;

public class MyPlayerCtrl : CharacterBase
{
    private void Awake()
    {
        Bind(CharactorEvent.SET_MYPLAYER_CARDS, CharactorEvent.SET_LANDLORD_TABLECARDS,CharactorEvent.BUCHU,CharactorEvent.BUCHU);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case CharactorEvent.SET_MYPLAYER_CARDS:
                StartCoroutine(InitCards(message as List<CardDto>));
                break;
            case CharactorEvent.SET_LANDLORD_TABLECARDS:
                CreatTableCards(message as LandlordDto);
                break;
            case CharactorEvent.BUCHU:
                ChuPai();
                break;

            case CharactorEvent.CHUPAI://这是服务器发来的
                ChuPaiResonse(message as ChuPaiDto);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 存储所有的牌
    /// </summary>
    private List<CardItem> myCardsList = new List<CardItem>();
    /// <summary>
    /// 1.存储所有牌的信息。用来得到地主牌的时候，给重新排序 =》服务器只发了3张底牌过来
    /// 2.抢了地主以后也可以让服务器把底牌(3张) 和 所有的牌(17+3)发过来 就不需要这个List 
    /// </summary>
    private List<CardDto> myCard = new List<CardDto>();

    private Transform cardParent;

    private MessageData serverMsg = new MessageData();
    private UIMsg uiMsg = new UIMsg();
    private ChuPaiDto chuDto = new ChuPaiDto();

    private void Start()
    {
        cardParent = transform.GetChild(0);
    }

    private IEnumerator InitCards(List<CardDto> cards)
    {
        //如果已经有牌了全部清空
        if (myCardsList.Count > 0)
        {
            foreach (var item in myCardsList)
            {
                Destroy(item.gameObject);
            }
            myCardsList.Clear();
            myCard.Clear();
        }
        cards.SortEx();
        yield return new WaitForSeconds(0.2f);

        var cardPrefab = Resources.Load(GlobalData.MyCardPath);
        for (int i = 0; i < cards.Count; i++)
        {
            CreatCard(cards[i],i,cardPrefab);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void CreatTableCards(LandlordDto dto)
    {
        if (dto.landLordId == Models.gameModel.UserDto.id)//是自己
        {
            var cardPrefab = Resources.Load(GlobalData.MyCardPath);
            for (int i = 0; i < dto.tableCardsList.Count; i++)
            {
                CreatCard(dto.tableCardsList[i], myCardsList.Count, cardPrefab);
            }
            //排序
            myCard.SortEx();
            for (int i = 0; i < myCardsList.Count; i++)
            {
                myCardsList[i].Init(myCard[i],i,true,dto.tableCardsList.Contains(myCard[i]));
            }
        }
        
    }

    private void CreatCard(CardDto dto,int index,Object cardPrefab)
    {
        var cardGO = Instantiate(cardPrefab,cardParent) as GameObject;
        cardGO.name = dto.name;
        var cardItem = cardGO.GetComponent<CardItem>();
        cardItem.Init(dto,index,true);
        myCardsList.Add(cardItem);
        myCard.Add(dto);
        //位置 根据牌数量设置位置
       // cardGO.transform.localPosition += new Vector3(GlobalData.MyCardXOffset * index, 0,0);

    }

    /// <summary>
    /// 出牌方法
    /// </summary>
    private void ChuPai()
    {
        //获取所有的牌
        var tempList = new List<CardDto>();
        foreach (var card in myCardsList)
        {
            if (card.IsSelect)
            {
                tempList.Add(card.CardInfo);
            }
        }
        //排序
        tempList.SortEx();
        //判断是否合法 return
        chuDto.Set(Models.gameModel.UserDto.id,tempList);
        if (!chuDto.isLeagl)
        {
            uiMsg.Set("不合法，不能出牌",Color.red);
            Dispatch(AreaCode.UI,UIEvent.MessageInfoPanel,uiMsg);
            return;
        }
        //合法：向服务器发送出牌请求 服务器判断是不是比上家的大
        serverMsg.Set(OpCode.FIGHT,FightCode.CHUPAI_CREQ,chuDto);
        Dispatch(AreaCode.NET,0,serverMsg);
    }

    /// <summary>
    /// 出的牌服务器来消息了 说明出牌成功了
    /// </summary>
    /// <param name="cards">玩家出的牌 由于是客户端发过去的。是已经排过顺序的</param>
    private void ChuPaiResonse(ChuPaiDto dto)
    {
        //将这些牌销毁(隐藏)

        foreach (var card in dto.cardsList)
        {
            for (int i = 0; i < myCardsList.Count; i++)
            {
                if (myCardsList[i].CardInfo == card)
                {
                    myCardsList[i].gameObject.SetActive(false);
                    break;
                }
            }
        }
        
    }

}
