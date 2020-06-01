using System.Collections;
using System.Collections.Generic;
using Protocol.Dto;
using UnityEngine;

/// <summary>
/// 游戏数据的存储类
/// </summary>
public class GameModel
{
    /// <summary>
    /// 登陆用户的数据
    /// </summary>
    public UserDto UserDto { get; set; }

    /// <summary>
    /// 匹配房间的数据
    /// </summary>
    public MatchRoomDto MatchRoomDto { get; set; }

    //public 

}
