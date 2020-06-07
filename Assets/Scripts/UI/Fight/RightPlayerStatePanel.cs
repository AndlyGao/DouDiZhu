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
                idTxt.text = userDto.name;
                //有没有准备？
                if (Models.gameModel.MatchRoomDto.readyUidList.Contains(this.userDto.id))
                {
                    readyTxt.gameObject.SetActive(true);
                }
                SetPanelActive(true);
                break;
            default:
                break;
        }
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


}
