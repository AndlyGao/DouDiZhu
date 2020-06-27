using System.Collections;
using System.Collections.Generic;
using Protocol.Code;
using Protocol.Dto;
using UnityEngine;

public class MatchHandler : HandlerBase
{
    private UIMsg uiMsg = new UIMsg();

    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case MatchRoomCode.STARTMATCH_SRES:
                StartMatchResponse(value as MatchRoomDto) ;
                break;
            case MatchRoomCode.STARTMATCH_BRO:
                StartMatchBro(value as UserDto);
                break;
            case MatchRoomCode.CANCELMATCH_BRO:
                CancelMatchBro((int)value);
                break;
            case MatchRoomCode.ENTERROOM_SRES:
                EnterRoom(value as MatchRoomDto) ;
                break;
            case MatchRoomCode.READY_BRO:
                ReadyBro((int)value);
                break;
            case MatchRoomCode.START_BRO:
                StartBro();
                break;
            case MatchRoomCode.BACK_SRES:
                {
                    LoadSceneMsg msg = new LoadSceneMsg(1, () => {
                        Debug.Log("返回匹配界面。。。");
                        //当前还没匹配。所以匹配数据要为空
                        Models.gameModel.MatchRoomDto = null;
                        //向服务器发送请求获取角色信息
                        var serverMsg = new MessageData();
                        serverMsg.Set(OpCode.USER, UserCode.GET_INFO_CREQ, null);
                        Dispatch(AreaCode.NET, 0, serverMsg);
                    });
                    Dispatch(AreaCode.SCENE, SceneEvent.LOAD_SCENE, msg);
                }
                break;
            default:
                break;
        }
    }

    private void StartMatchResponse(MatchRoomDto dto)
    {
        
        //显示进入房间按钮
        Dispatch(AreaCode.UI,UIEvent.SHOW_ENTERROOMBTN_ACTIVE,null);

        Debug.Log(string.Format("匹配到房间{0} 里面有 {1} 个人", dto.id,dto.uIdUdtoDic.Count));
        
    }

    private void EnterRoom(MatchRoomDto dto)
    {

        //存储本地
        Models.gameModel.MatchRoomDto = dto;
        Debug.Log("更新房间信息成功");
        //更新现在在房间内的玩家
        UpdatePlayersInfo();

        //更新自己的信息
        UserDto myUserDto = Models.gameModel.UserDto;
        Dispatch(AreaCode.UI, UIEvent.SET_MYPLAYER_DATA, myUserDto);
    }

    /// <summary>
    /// 他人进入的广播消息
    /// </summary>
    /// <param name="dto"></param>
    private void StartMatchBro(UserDto dto)
    {
        
        //更新房间其他角色信息
        Models.gameModel.MatchRoomDto.Add(dto);
       
        //更新现在在房间内的玩家
        UpdatePlayersInfo();

        //战斗场景玩家状态信息更新
       // Dispatch(AreaCode.UI, UIEvent.PLAYER_ENTER, dto.id);
        //给用户一个提示
        uiMsg.Set(string.Format("有新玩家 ：{0}  加入",dto.name),Color.green);
        Dispatch(AreaCode.UI,UIEvent.MessageInfoPanel,uiMsg);
    }


    private void CancelMatchBro(int userId)
    {
        //移除之前先把信息保存下来
        var leaveUserDto = Models.gameModel.MatchRoomDto.uIdUdtoDic[userId];
        //移除数据
        Models.gameModel.MatchRoomDto.Delete(userId);
        //更新当前玩家信息
        UpdatePlayersInfo();
        //玩家离开了房间  把准备文字和对话面板  地主或农民的ui隐藏
        Dispatch(AreaCode.UI, UIEvent.PLAYER_LEAVE, userId);
        //提示消息  xxx 离开了房间
        uiMsg.Set(string.Format("玩家 ：{0}  离开", leaveUserDto.name), Color.green);
        Dispatch(AreaCode.UI, UIEvent.MessageInfoPanel, uiMsg);
        
    }


    private void ReadyBro(int userId)
    {
        //提示消息  xxx 准备了
        uiMsg.Set(string.Format("玩家 ：{0}  准备", Models.gameModel.MatchRoomDto.GetNameById(userId)), Color.green);
        Dispatch(AreaCode.UI, UIEvent.MessageInfoPanel, uiMsg);
        //保存数据
        Models.gameModel.MatchRoomDto.Ready(userId);
        // 战斗场景ui信息更新
        //显示玩家状态信息面板的准备文字
        Dispatch(AreaCode.UI,UIEvent.PLAYER_READY,userId);
    }


    private void StartBro()
    {
        //开始游戏
        //TODO
        uiMsg.Set(string.Format("开始游戏了"), Color.green);
        Dispatch(AreaCode.UI, UIEvent.MessageInfoPanel, uiMsg);
    }

    /// <summary>
    /// 更新房间内其他玩家的信息
    /// </summary>
    private void UpdatePlayersInfo()
    {
        
        //重置玩家在房间的位置信息
        Models.gameModel.MatchRoomDto.ResetPosition(Models.gameModel.UserDto.id);
        //更新现在在房间内的玩家
        if (Models.gameModel.MatchRoomDto.leftPlayerId != -1)
        {
            UserDto leftUserDto = Models.gameModel.MatchRoomDto.uIdUdtoDic[Models.gameModel.MatchRoomDto.leftPlayerId];
            Dispatch(AreaCode.UI, UIEvent.SET_LEFTPLAYER_DATA, leftUserDto);
        }
        if (Models.gameModel.MatchRoomDto.rightPlayerId != -1)
        {
            UserDto rightUserDto = Models.gameModel.MatchRoomDto.uIdUdtoDic[Models.gameModel.MatchRoomDto.rightPlayerId];
            Dispatch(AreaCode.UI, UIEvent.SET_RIGHTPLAYER_DATA, rightUserDto);
        }
    }
}
