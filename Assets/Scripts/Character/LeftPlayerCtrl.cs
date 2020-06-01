using System.Collections;
using System.Collections.Generic;
using Protocol.Dto.Card;
using UnityEngine;

public class LeftPlayerCtrl : CharacterBase
{
    private void Awake()
    {
        Bind(CharactorEvent.SET_LEFTPLAYER_CARDS);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case CharactorEvent.SET_LEFTPLAYER_CARDS:
                StartCoroutine(InitCards());
                break;
            default:
                break;
        }
    }

    private Transform cardParent;

    private void Start()
    {
        cardParent = transform.GetChild(0);
    }

    private IEnumerator InitCards()
    {
        var cardPrefab = Resources.Load(GlobalData.OtherCardPath);
        for (int i = 0; i < 17; i++)
        {
            CreatCard(i, cardPrefab);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void CreatCard(int index, Object cardPrefab)
    {
        var cardGO = Instantiate(cardPrefab, cardParent) as GameObject;
        cardGO.transform.localPosition += new Vector3(GlobalData.OtherCardXOffset * index, 0);
        cardGO.GetComponent<SpriteRenderer>().sortingOrder = index;
    }
}
