using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 消息类
/// </summary>
public class MessageData
{
    /// <summary>
    /// 操作码
    /// </summary>
    public int OpCode { set; get; }
    /// <summary>
    /// 子操作
    /// </summary>
    public int SubCode { set; get; }
    /// <summary>
    /// 参数
    /// </summary>
    public object Value { set; get; }

    public MessageData()
    {

    }

    public MessageData(int opCode, int subCode, object value)
    {
        this.OpCode = opCode;
        this.SubCode = subCode;
        this.Value = value;
    }

    public void Set(int opCode, int subCode, object value) {
        this.OpCode = opCode;
        this.SubCode = subCode;
        this.Value = value;
    }
}

