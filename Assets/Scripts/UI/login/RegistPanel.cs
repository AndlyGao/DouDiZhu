using Protocol.Code;
using Protocol.Dto;
using UnityEngine;
using UnityEngine.UI;

public class RegistPanel : UIBase
{

    private UIMsg uiMsg;

    private MessageData serverMsg;
    private void Awake()
    {
        Bind(UIEvent.REGIST_PANEL_ACTIVE);

    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.REGIST_PANEL_ACTIVE:
                SetPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    private Button btnReigist;
    private Button btnClose;
    private InputField inputAccount;
    private InputField inputPassword;
    private InputField inputRepeat;

    // Use this for initialization
    void Start()
    {
        uiMsg = new UIMsg();
        serverMsg = new MessageData();

        btnReigist = transform.Find("btnReigist").GetComponent<Button>();
        btnClose = transform.Find("btnClose").GetComponent<Button>();
        inputAccount = transform.Find("inputAccount").GetComponent<InputField>();
        inputPassword = transform.Find("inputPassword").GetComponent<InputField>();
        inputRepeat = transform.Find("inputRepeat").GetComponent<InputField>();

        btnClose.onClick.AddListener(CloseClick);
        btnReigist.onClick.AddListener(RegistClick);

        SetPanelActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        btnClose.onClick.RemoveListener(CloseClick);
        btnReigist.onClick.RemoveListener(RegistClick);
    }

    /// <summary>
    /// 注册按钮的点击事件处理
    /// </summary>
    private void RegistClick()
    {
        if (string.IsNullOrEmpty(inputAccount.text)) {
            uiMsg.Set("账号不能为空...",Color.red);
            Dispatch(AreaCode.UI,UIEvent.MessageInfoPanel,uiMsg);
            return;
        }

        if (string.IsNullOrEmpty(inputPassword.text)
            || inputPassword.text.Length < 4
            || inputPassword.text.Length > 16) {
            uiMsg.Set("密码长度不正确（4-16位）...", Color.red);
            Dispatch(AreaCode.UI, UIEvent.MessageInfoPanel, uiMsg);
            return;
        }

        if (string.IsNullOrEmpty(inputRepeat.text)
            || inputRepeat.text != inputPassword.text) {
            uiMsg.Set("密码不一致...", Color.red);
            Dispatch(AreaCode.UI, UIEvent.MessageInfoPanel, uiMsg);
            return;
        }
            
        Debug.Log("开始注册...");
        //需要和服务器交互了
        AccountDto dto = new AccountDto(inputAccount.text, inputPassword.text);
        serverMsg.Set(OpCode.ACCOUNT, AccountCode.REGISTER_CREQ, dto);
        Dispatch(AreaCode.NET, 0, serverMsg);
    }

    private void CloseClick()
    {
        SetPanelActive(false);
    }
}
