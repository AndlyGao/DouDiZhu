using System.Collections;
using System.Collections.Generic;
using Protocol.Dto.Card;
using UnityEngine;
using UnityEngine.UI;

public class UpPanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.SET_TABLE_CARDS,UIEvent.GAME_RESTAET);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.SET_TABLE_CARDS:
                SetTableCards(message as List<CardDto>);
                break;
            case UIEvent.GAME_RESTAET:
                {
                    //给牌隐藏了
                    SetTabCardsBack();
                }
                break;
            default:
                break;
        }
    }

    

    private Image[] cardImg;
    private Sprite defaultSprite;
    private void Start()
    {
        cardImg = new Image[3];
        cardImg[0] = transform.Find("cardImg1").GetComponent<Image>();
        cardImg[1] = transform.Find("cardImg2").GetComponent<Image>();
        cardImg[2] = transform.Find("cardImg3").GetComponent<Image>();

        defaultSprite = cardImg[0].sprite;
    }

    private void SetTableCards(List<CardDto> dto)
    {
        cardImg[0].sprite = Resources.Load<Sprite>("Poker/" + dto[0].name);
        cardImg[1].sprite = Resources.Load<Sprite>("Poker/" + dto[1].name);
        cardImg[2].sprite = Resources.Load<Sprite>("Poker/" + dto[2].name);
    }

    private void SetTabCardsBack()
    {
        foreach (var img in cardImg)
        {
            img.sprite = defaultSprite;
        }
    }
}
