using System.Collections;
using System.Collections.Generic;
using Protocol.Code;
using Protocol.Dto;
using UnityEngine;

public class ChatHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case ChatCode.CHAT_SRES:
                ChatResponse(value as ChatDto);
                break;
            default:
                break;
        }
    }

    private ChatMsg chatMsg = new ChatMsg();

    private void ChatResponse(ChatDto dto)
    {
        chatMsg.Set(dto.userId,dto.chatType);
        Dispatch(AreaCode.UI,UIEvent.PLAYER_CHAT,chatMsg);
    }
}
