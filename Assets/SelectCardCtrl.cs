using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCardCtrl : MonoBehaviour
{
    private static SelectCardCtrl instance;
    public static SelectCardCtrl Instance { get {
            if (instance == null)
            {
                instance = new GameObject("SelectCardCtrl").AddComponent<SelectCardCtrl>();
            }
            return instance;
     } }

    private void Awake()
    {
        instance = this;
    }

    private bool canOperate;
    private CardItem firstAddCard;//鼠标离开时只有 第一张牌才会被添加上
    private CardItem lastAddCard;//用来将上一个移除的也添加上
    private CardItem lastRemovedCard;//用来将上一个添加的也移除掉

    private bool startAdd;
    private bool startRem;
    private List<CardItem> seletedCards = new List<CardItem>();

    public void PointerEnterCard(CardItem card)
    {
        if (canOperate)
        {
            if (seletedCards.Contains(card))
            {
                startAdd = false;
                startRem = true;
                lastRemovedCard = card;
                seletedCards.Remove(card);
                card.BeingSelect(false);

                if (seletedCards.Contains(lastAddCard) && lastAddCard != null && startRem)
                {
                    seletedCards.Remove(lastAddCard);
                    lastAddCard.BeingSelect(false);
                }
            }
            else
            {
                startAdd = true;
                startRem = false;
                lastAddCard = card;
                seletedCards.Add(card);
                card.BeingSelect(true);

                if (!seletedCards.Contains(lastRemovedCard) && lastRemovedCard != null && startAdd)
                {
                    seletedCards.Add(lastRemovedCard);
                    lastAddCard.BeingSelect(true);
                    lastRemovedCard = null;
                }
            }
        }
    }

    public void PointerExitCard(CardItem card)
    {
        //什么情况下，离开了也Add呢
        if (canOperate)
        {
            if (!seletedCards.Contains(card))
            {
                if (card == firstAddCard)
                {
                    seletedCards.Add(card);
                    card.BeingSelect(true);
                }
                
            }
           
        }
    }

    public void PointerUpCard()
    {
        if (canOperate)
        {
            foreach (var card in seletedCards)
            {
                card.IsSelect = !card.IsSelect;
                card.BeingSelect(false);
            }
            seletedCards.Clear();
            firstAddCard = null;
            lastAddCard = null;
            lastRemovedCard = null;
            canOperate = false;
        }
        
    }

    /// <summary>
    /// 调用了此方法才可以调用其他功能
    /// </summary>
    public void PointerDownCard(CardItem card)
    {
        canOperate = true;
        firstAddCard = card;
        PointerEnterCard(card);
    }
}
