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
        var roomDto = Models.gameModel.MatchRoomDto;
        if (roomDto != null && roomDto.rightPlayerId != -1)
        {
            //有角色
            this.userDto = Models.gameModel.MatchRoomDto.uIdUdtoDic[Models.gameModel.MatchRoomDto.rightPlayerId];
        }
        else
        {
            SetPanelActive(false);
        }
    }

    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
        switch (eventCode)
        {
            case UIEvent.SET_LEFTPLAYER_DATA:
                this.userDto = message as UserDto;
                idTxt.text = userDto.name;

                SetPanelActive(true);
                break;
            default:
                break;
        }
    }
}
