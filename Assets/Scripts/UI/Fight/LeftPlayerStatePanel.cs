using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol.Dto.Card;
using Protocol.Dto;

public class LeftPlayerStatePanel : StatePanel
{
   
    protected override void Awake()
    {
        base.Awake();
        Bind(UIEvent.SET_LEFTPLAYER_DATA);

    }

    protected override void Start()
    {
        base.Start();

        SetPanelActive(false);
    }

    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
        switch (eventCode)
        {
            case UIEvent.SET_LEFTPLAYER_DATA:
                this.userDto = message as UserDto;
                break;
            default:
                break;
        }
    }
}
