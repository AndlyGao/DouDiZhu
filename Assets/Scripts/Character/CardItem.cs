using Protocol.Dto.Card;
using UnityEngine;
/// <summary>
/// 保存牌的信息
/// </summary>
public class CardItem : MonoBehaviour
{
    private bool isSelect;
    private Color selectingColor;

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
            //spriteRender.color = value ? Color.green : Color.white;
            transform.localPosition += new Vector3(0, GlobalData.CardSelectedYOffset, 0) * (this.isSelect ? 1 : -1);

        }
    }
    /// <summary>
    /// 是不是自己的牌  不是自己的不能点
    /// </summary>
    private bool isMine;
    [HideInInspector]
    public bool isTableCard;

	//render
	private SpriteRenderer spriteRender;

    private void Awake()
    {
        selectingColor = new Color(128 / 255, 255 / 255, 255 / 255, 1);
        spriteRender = GetComponent<SpriteRenderer>();
        this.IsSelect = false;
    }
   

    public void Init(CardDto cardInfo,int index,bool isMine,bool isTableCard = false)
    {
        this.gameObject.SetActive(true);
        this.name = cardInfo.name;
        this.CardInfo = cardInfo;
        this.isMine = isMine;

        //还原默认属性
        string path = "Poker/" + (isMine ? cardInfo.name : "CardBack");
        var sp = Resources.Load<Sprite>(path);
        if (spriteRender == null)
        {
            spriteRender = GetComponent<SpriteRenderer>();
        }
        this.spriteRender.sprite = sp;
        this.spriteRender.sortingOrder = index;
        this.isTableCard = isTableCard;
    }


    private void OnMouseDown()
    {
        if (isMine)
        {
            SelectCardCtrl.Instance.PointerDownCard(this);
        }
    }

    public void BeingSelect(bool flag)
    {
        spriteRender.color = flag ? selectingColor : Color.white;
    }

    public void OnMouseEnter()
    {
        SelectCardCtrl.Instance.PointerEnterCard(this);

    }


    private void OnMouseUp()
    {
        SelectCardCtrl.Instance.PointerUpCard();
    }

    private void OnMouseExit()
    {
        SelectCardCtrl.Instance.PointerExitCard(this);
    }
}
