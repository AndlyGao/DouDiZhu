using System.Collections;
using System.Collections.Generic;
using Protocol.Dto;
using UnityEngine;
using UnityEngine.UI;

public class RightPlayerStatePanel : StatePanel
{
    
    protected override void Awake()
    {
        base.Awake();
        Bind(UIEvent.SET_RIGHTPLAYER_DATA);
    }
    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
        switch (eventCode)
        {
            case UIEvent.SET_RIGHTPLAYER_DATA:
                this.userDto = message as UserDto;
                break;
            default:
                break;
        }
    }

    protected override void Start()
    {
        base.Start();

        SetPanelActive(false);
    }


}
