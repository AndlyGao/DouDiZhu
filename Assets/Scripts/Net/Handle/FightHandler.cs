using System.Collections.Generic;
using Protocol.Code;
using Protocol.Dto.Card;
using UnityEngine;

public class FightHandler : HandlerBase
{

    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case FightCode.GET_CARDS_SRES:
                GetCardsResponse(value as List<CardDto>);
                break;
            case FightCode.QIANG_LANDLORD_SRES:

                break;
            case FightCode.CHUPAI_SRES:

                break;
            case FightCode.TURN_SRES:

                break;
            case FightCode.TURN_BRO://该谁抢地主
                TurnToQiang((int)value);
                break;
            case FightCode.QIANG_LANDLORD_BRO:

                break;
            case FightCode.CHUPAI_BRO:

                break;
            case FightCode.BUCHU_BRO:

                break;
            case FightCode.BUQIANG_LANDLORD_BRO://谁不抢
                BuQiangLandlordResponse((int)value);
                break;
            case FightCode.LEAVE_BRO:

                break;
            case FightCode.OVER_BRO:

                break;
            default:
                break;
        }
    }

    private Queue<CardItem> cardPool = new Queue<CardItem>(54);

    /// <summary>
    /// 给每个玩家发牌
    /// </summary>
    /// <param name="cards"></param>
    private void GetCardsResponse(List<CardDto> cards)
    {
        //改变倍数为1
        Dispatch(AreaCode.UI,UIEvent.CHANGE_MUTIPLIER,1);
        //让玩家隐藏readyTxt
        Dispatch(AreaCode.UI,UIEvent.PLAYER_HIDE_STATE,null);
        //根据服务器发来的牌，生成预设物体
        Dispatch(AreaCode.CHARACTER,CharactorEvent.SET_MYPLAYER_CARDS,cards);
        Dispatch(AreaCode.CHARACTER,CharactorEvent.SET_LEFTPLAYER_CARDS,null);
        Dispatch(AreaCode.CHARACTER,CharactorEvent.SET_RIGHTLAYER_CARDS,null);
       
    }

    private void BuQiangLandlordResponse(int userId)
    {
        Dispatch(AreaCode.UI,UIEvent.BUQIANG_LANDLORD_OPERATE,userId);
    }

    private void TurnToQiang(int userId)
    {
        Dispatch(AreaCode.UI,UIEvent.SHOW_PLAYER_JIAO_BTN_ACTIVE,userId);
    }
}
