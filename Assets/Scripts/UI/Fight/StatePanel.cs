using System.Collections;
using System.Collections.Generic;
using Protocol.Dto;
using Protocol.Dto.Constant;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// player面板信息 基类
/// </summary>
public class StatePanel : UIBase
{
   
    /// <summary>
    /// 角色的数据
    /// </summary>
    protected UserDto userDto;

    protected Text readyTxt;
    protected Text clockTxt;
    protected Text operateTxt;
    protected Image identityImg;
    protected Text dialogTxt;

    protected Text idTxt;

    protected MessageData serverMsg;

    protected int showTime = 3;
    protected float timer = 0;
    private bool isShow = false;

    protected virtual void Awake()
    {
        Bind(UIEvent.PLAYER_READY,
            UIEvent.PLAYER_LEAVE,
            UIEvent.PLAYER_ENTER,
            UIEvent.PLAYER_HIDE_STATE,
            UIEvent.PLAYER_CHAT,
            UIEvent.HIDE_PLAYER_READYBTN,
            UIEvent.SHOW_PLAYER_JIAO_BTN_ACTIVE,
            UIEvent.BUQIANG_LANDLORD_OPERATE);
    }

    public override void Execute(int eventCode, object message)
    {
        if (userDto == null) return;
        switch (eventCode)
        {
            case UIEvent.BUQIANG_LANDLORD_OPERATE:
                {
                    BuQiangOperate((int)message);
                }

                break;
            case UIEvent.SHOW_PLAYER_JIAO_BTN_ACTIVE:
                {
                    SetTurn((int)message);
                }

                break;
            case UIEvent.PLAYER_READY:
                {
                    int userId = (int)message;
                    if (userDto.id == userId)//如果是自己
                    {
                        SetReadyState();
                    }
                }
                break;
            case UIEvent.PLAYER_LEAVE:
                {
                    
                    int userId = (int)message;
                    if ( userId == userDto.id)
                    {
                        
                        SetPanelActive(false);
                    }
                }
                break;
            case UIEvent.PLAYER_ENTER:
                {
                   
                    int userId = (int)message;
                   
                    if (userId == userDto.id)
                    {
                       // SetPanelActive(true);
                    }
                }
                break;
            case UIEvent.PLAYER_HIDE_STATE:
                {
                    //SetPanelActive(false);
                    //游戏开始了，准备txt隐藏
                    readyTxt.gameObject.SetActive(false);
                }
                break;
            case UIEvent.PLAYER_CHAT:
                ChatResponse(message as ChatMsg);
                break;

            default:
                break;
        }
    }

    private void BuQiangOperate(int userId)
    {
        if (userDto.id == userId)//如果是自己 
        {
            if (!operateTxt.gameObject.activeInHierarchy)
            {
                operateTxt.gameObject.SetActive(true);
            }
            operateTxt.text = "不抢";
        }
    }

    /// <summary>
    /// turn 响应
    /// </summary>
    /// <param name="userId"></param>
    protected virtual bool SetTurn(int userId)
    {
        //判断是不是自己叫
        //如果是自己 叫地主和不叫地主的按钮显示出来 并把时钟显示出来
        //不是自己 叫地主和不叫地主的按钮隐藏

        var flag = userId == this.userDto.id;
        clockTxt.gameObject.SetActive(flag);
        if (flag && operateTxt.gameObject.activeInHierarchy)
        {
            operateTxt.gameObject.SetActive(false);
        }
        return flag;
    }

    /// <summary>
    /// 准备响应
    /// </summary>
    protected virtual void SetReadyState()
    {
        readyTxt.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        readyTxt = transform.Find("readyTxt").GetComponent<Text>();
        clockTxt = transform.Find("clockTxt").GetComponent<Text>();
        operateTxt = transform.Find("operateTxt").GetComponent<Text>();
        identityImg = transform.Find("identityImg").GetComponent<Image>();
        dialogTxt = transform.Find("dialogTxt").GetComponent<Text>();
        idTxt = transform.Find("idTxt").GetComponent<Text>();

        serverMsg = new MessageData();
        //默认状态
        if(readyTxt != null)
            readyTxt.gameObject.SetActive(false);
        dialogTxt.gameObject.SetActive(false);
        clockTxt.gameObject.SetActive(false);
        operateTxt.gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置身份 
    /// </summary>
    /// <param name="identity">0就是地主  1 就是农民</param>
    protected void SetIdentity(int identity)
    {
        string identityStr = identity == 0 ? "Landlord" : "Framer";
        identityImg.sprite = Resources.Load<Sprite>("Identity/" + identityStr);
    }

    /// <summary>
    /// 外界调用显示对话
    /// </summary>
    /// <param name="content"></param>
    protected void ShowDialog(string content)
    {
        dialogTxt.text = content;
        SetDialogActive(true);
        //显示动画
        isShow = true;
        //当前还对话框还未消失  又重新发了一个消息
        timer = 0;
    }

    protected virtual void Update()
    {
        if (isShow)
        {
            timer += Time.deltaTime;
            if (timer >= showTime)
            {
                SetDialogActive(false);
                timer = 0;
                isShow = false;
            }
        }
    }

    /// <summary>
    /// 显示操作结果
    /// </summary>
    /// <param name="content"></param>
    protected virtual void SetOperateResult(string content)
    {

    }

    /// <summary>
    /// 设置对面面板显示与隐藏
    /// </summary>
    /// <param name="active"></param>
    protected void SetDialogActive(bool active)
    {
        dialogTxt.gameObject.SetActive(active);
    }

    protected virtual void ChatResponse(ChatMsg msg)
    {
        if (this.userDto == null) return;
        //如果是自己
        if (msg.userId == this.userDto.id)
        {
            //显示聊天框
            ShowDialog(ChatConstant.GetContent(msg.chatMsgType));
            //播放声音
            Dispatch(AreaCode.AUDIO,AudioEvent.EFFECTAUDIO,msg.chatMsgType);
        }
    }
}
