using System.Collections;
using System.Collections.Generic;
using Protocol.Dto.Card;
using UnityEngine;

public class MyPlayerCtrl : CharacterBase
{
    private void Awake()
    {
        Bind(CharactorEvent.SET_MYPLAYER_CARDS);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case CharactorEvent.SET_MYPLAYER_CARDS:
                StartCoroutine(InitCards(message as List<CardDto>));
                break;
            default:
                break;
        }
    }

    private List<CardItem> myCardsList = new List<CardItem>();

    private Transform cardParent;

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
        }
        yield return new WaitForSeconds(0.2f);

        var cardPrefab = Resources.Load(GlobalData.MyCardPath);
        for (int i = 0; i < cards.Count; i++)
        {
            CreatCard(cards[i],i,cardPrefab);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void CreatCard(CardDto dto,int index,Object cardPrefab)
    {
        var cardGO = Instantiate(cardPrefab,cardParent) as GameObject;
        cardGO.name = dto.name;
        var cardItem = cardGO.GetComponent<CardItem>();
        cardItem.Init(dto,index,true);
        myCardsList.Add(cardItem);

        //位置 根据牌数量设置位置
        cardGO.transform.localPosition += new Vector3(GlobalData.MyCardXOffset * index, 0,0);

    }


}
