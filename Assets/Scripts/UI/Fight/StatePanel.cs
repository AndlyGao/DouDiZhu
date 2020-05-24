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
    protected Image identityImg;
    protected Text dialogTxt;

    protected Text idTxt;

    protected MessageData serverMsg;

    protected int showTime = 3;
    protected float timer = 0;
    private bool isShow = false;

    protected virtual void Awake()
    {
        Bind(UIEvent.PLAYER_READY,UIEvent.PLAYER_LEAVE,UIEvent.PLAYER_ENTER,UIEvent.PLAYER_HIDE_STATE,UIEvent.PLAYER_CHAT);
    }

    public override void Execute(int eventCode, object message)
    {
        if (userDto == null) return;
        switch (eventCode)
        {
            
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
                    SetPanelActive(false);
                }
                break;
            case UIEvent.PLAYER_CHAT:
                ChatResponse(message as ChatMsg);
                break;
            default:
                break;
        }
    }

    protected virtual void SetReadyState()
    {
        readyTxt.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        readyTxt = transform.Find("readyTxt").GetComponent<Text>();
        identityImg = transform.Find("identityImg").GetComponent<Image>();
        dialogTxt = transform.Find("dialogTxt").GetComponent<Text>();
        idTxt = transform.Find("idTxt").GetComponent<Text>();

        serverMsg = new MessageData();
        //默认状态
        if(readyTxt != null)
            readyTxt.gameObject.SetActive(false);
        dialogTxt.gameObject.SetActive(false);

        
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
