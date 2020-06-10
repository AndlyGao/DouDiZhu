using System.Collections;
using System.Collections.Generic;
using Protocol.Dto;
using Protocol.Dto.Fight;
using UnityEngine;

public class RightPlayerCtrl : CharacterBase
{
    private void Awake()
    {
        Bind(CharactorEvent.SET_RIGHTLAYER_CARDS, CharactorEvent.SET_LANDLORD_TABLECARDS,CharactorEvent.GMAE_RESTART);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case CharactorEvent.SET_RIGHTLAYER_CARDS:
                StartCoroutine(InitCards());
                break;
            
            case CharactorEvent.SET_LANDLORD_TABLECARDS:
                CreatTableCards(message as LandlordDto);
                break;
            case CharactorEvent.GMAE_RESTART://重新开始
                Restart();
                break;
            default:
                break;
        }
    }

    private Transform cardParent;
    private List<GameObject> myCardsList = new List<GameObject>();

    private void Start()
    {
        cardParent = transform.GetChild(0);
    }
    private void Restart()
    {
        //如果已经有牌了全部清空
        if (myCardsList.Count > 0)
        {
            foreach (var item in myCardsList)
            {
                Destroy(item);
            }
            myCardsList.Clear();
        }
    }
    private IEnumerator InitCards()
    {
        
        yield return new WaitForSeconds(0.2f);

        var cardPrefab = Resources.Load(GlobalData.OtherCardPath);
        for (int i = 0; i < 17; i++)
        {
            CreatCard(i, cardPrefab);
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void CreatTableCards(LandlordDto dto)
    {
        if (dto.landLordId == Models.gameModel.MatchRoomDto.rightPlayerId)//是自己
        {
            var cardPrefab = Resources.Load(GlobalData.OtherCardPath);
            for (int i = 0; i < dto.tableCardsList.Count; i++)
            {
                CreatCard(myCardsList.Count, cardPrefab);
            }
        }

    }
    private void CreatCard(int index, Object cardPrefab)
    {
        var cardGO = Instantiate(cardPrefab, cardParent) as GameObject;
        cardGO.transform.localPosition += new Vector3(GlobalData.OtherCardXOffset * index, 0);
        cardGO.GetComponent<SpriteRenderer>().sortingOrder = index;
        myCardsList.Add(cardGO);
    }
}
