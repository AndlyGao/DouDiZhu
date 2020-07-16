using System.Collections;
using System.Collections.Generic;
using HotUpdateModel;
using UnityEngine;

public class GameStart : MonoBehaviour,IStartGame
{

    public void ReceiveInfoStartRuning()
    {
        MsgCenter.Instance.Dispatch(AreaCode.UI,UIEvent.SHOW_CLOSEBTN,null);
    }
}
