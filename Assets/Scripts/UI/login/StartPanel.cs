using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : UIBase
{
    private UIMsg uiMsg;

    private MessageData serverMsg;

    private void Awake()
    {
        Bind(UIEvent.START_PANEL_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.START_PANEL_ACTIVE:
                SetPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    private Button btnLogin;
    private Button btnClose;
    private InputField inputAccount;
    private InputField inputPassword;

    // Use this for initialization
    void Start()
    {
        uiMsg = new UIMsg();
        serverMsg = new MessageData();

        btnLogin = transform.Find("btnLogin").GetComponent<Button>();
        btnClose = transform.Find("btnClose").GetComponent<Button>();
        inputAccount = transform.Find("inputAccount").GetComponent<InputField>();
        inputPassword = transform.Find("inputPassword").GetComponent<InputField>();

        btnLogin.onClick.AddListener(LoginClick);
        btnClose.onClick.AddListener(CloseClick);

        //面板需要默认隐藏
        SetPanelActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        btnLogin.onClick.RemoveListener(LoginClick);
        btnClose.onClick.RemoveListener(CloseClick);
    }

    /// <summary>
    /// 登录按钮的点击事件处理
    /// </summary>
    private void LoginClick()
    {
        if (string.IsNullOrEmpty(inputAccount.text)) {
            uiMsg.Set("空",Color.red);
            Dispatch(AreaCode.UI,UIEvent.MessageInfoPanel,uiMsg);
            return;
        }
            
            
        if (string.IsNullOrEmpty(inputPassword.text)
            || inputPassword.text.Length < 4
            || inputPassword.text.Length > 16)
            return;

        //需要和服务器交互了
        AccountDto dto = new AccountDto(inputAccount.text,inputPassword.text);
        serverMsg.Set(OpCode.ACCOUNT,AccountCode.LOGIN,dto);
        Dispatch(AreaCode.NET,0,serverMsg);


    }

    private void CloseClick()
    {
        SetPanelActive(false);
    }

}
