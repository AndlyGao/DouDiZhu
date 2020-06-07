using Protocol.Dto.Card;
using UnityEngine;
/// <summary>
/// 保存牌的信息
/// </summary>
public class CardItem : MonoBehaviour
{
    private bool isSelect;
	/// <summary>
    /// 数据
    /// </summary>
	public CardDto CardInfo { get; set; }
	/// <summary>
    /// 是否被选中
    /// </summary>
	public bool IsSelect { get { return this.isSelect; } set
        {
            this.isSelect = value;
            transform.localPosition += new Vector3(0, GlobalData.CardSelectedYOffset, 0) * (this.isSelect ? 1 : -1);
        }
    }
    /// <summary>
    /// 是不是自己的牌  不是自己的不能点
    /// </summary>
    public bool isMine;
	//render
	private SpriteRenderer spriteRender;

    private void Start()
    {
        //spriteRender = GetComponent<SpriteRenderer>();
        //startPos = transform.localPosition;
    }

    public void Init(CardDto cardInfo,int index,bool isMine,bool isTableCard = false)
    {
        this.CardInfo = cardInfo;
        this.isMine = isMine;

        //还原默认属性
        if (IsSelect)
        {
            IsSelect = false;
        }
        string path = "Poker/" + (isMine ? cardInfo.name : "CardBack");
        var sp = Resources.Load<Sprite>(path);
        if (spriteRender == null)
        {
            spriteRender = GetComponent<SpriteRenderer>();
        }
        this.spriteRender.sprite = sp;
        this.spriteRender.sortingOrder = index;

        if (isTableCard)
        {
            IsSelect = true;
        }
    }


    private void OnMouseDown()
    {
        if (isMine)
        {
            this.IsSelect = !this.IsSelect;
        }
    }
}
