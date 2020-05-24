using System.Collections;
using System.Collections.Generic;
using Protocol.Code;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : UIBase
{
    private MessageData serverMsg = new MessageData();

    private Button dialogBtn;
    private GameObject dialogImg;
    private Button[] dialogBtns;

    private void Start()
    {
        dialogBtn = transform.Find("dialogBtn").GetComponent<Button>();
        dialogImg = transform.Find("dialogImg").gameObject;
        dialogBtns = dialogImg.GetComponentsInChildren<Button>();

        dialogBtn.onClick.AddListener(DialogBtnClick);
        for (int i = 0; i < dialogBtns.Length; i++)
        {
            int tempIndex = i+1;
            dialogBtns[i].onClick.AddListener(()=> {
                SendDialogMsg(tempIndex);
            });
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        dialogBtn.onClick.RemoveListener(DialogBtnClick);
        foreach (var btn in dialogBtns)
        {
            btn.onClick.RemoveAllListeners();
        }
    }

    private void DialogBtnClick()
    {
        dialogImg.SetActive(!dialogImg.activeInHierarchy);
    }

    private void SendDialogMsg(int msgType)
    {
        serverMsg.Set(OpCode.CHAT,ChatCode.CHAT_CREQ,msgType);
        Dispatch(AreaCode.NET, 0, serverMsg);
    }
}
