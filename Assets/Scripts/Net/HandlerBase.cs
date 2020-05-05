using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 客户端处理消息基类
/// </summary>
public abstract class HandlerBase
{
    public abstract void OnReceive(int subCode,object value);

    /// <summary>
    /// 方便发消息
    /// </summary>
    /// <param name="areaCode"></param>
    /// <param name="eventCode"></param>
    /// <param name="message"></param>
    protected void Dispatch(int areaCode, int eventCode, object message) {
        MsgCenter.Instance.Dispatch(areaCode,eventCode,message);
    }
}

