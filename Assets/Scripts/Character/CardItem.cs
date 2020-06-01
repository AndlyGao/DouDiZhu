using Protocol.Dto.Card;
using UnityEngine;
/// <summary>
/// 保存牌的信息
/// </summary>
public class CardItem : MonoBehaviour
{
	/// <summary>
    /// 数据
    /// </summary>
	public CardDto CardInfo { get; set; }
	/// <summary>
    /// 是否被选中
    /// </summary>
	public bool IsSelect { get; set; }
    /// <summary>
    /// 是不是自己的牌  不是自己的不能点
    /// </summary>
    public bool isMine;
	//render
	private SpriteRenderer spriteRender;

    private Vector3 startPos;

    

    private void Start()
    {
        //spriteRender = GetComponent<SpriteRenderer>();
        startPos = transform.localPosition;
    }

    public void Init(CardDto cardInfo,int index,bool isMine)
    {
        this.CardInfo = cardInfo;
        this.isMine = isMine;

        //还原默认属性
        if (IsSelect)
        {
            IsSelect = false;
            transform.localPosition = startPos;
        }

        string path = "Poker/" + (isMine ? cardInfo.name : "CardBack");
        var sp = Resources.Load<Sprite>(path);
        if (spriteRender == null)
        {
            spriteRender = GetComponent<SpriteRenderer>();
        }
        this.spriteRender.sprite = sp;
        this.spriteRender.sortingOrder = index;
    }


    private void OnMouseDown()
    {
        if (isMine)
        {
            this.IsSelect = !this.IsSelect;
            transform.localPosition = this.IsSelect ? startPos + new Vector3(0, GlobalData.CardSelectedYOffset, 0) : startPos;
        }
    }
}
