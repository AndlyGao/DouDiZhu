using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;


/// <summary>
/// 关于编码的工具类
/// </summary>
public static class EncodTool
{
    #region 粘包拆包问题

    /// <summary>
    /// 粘包   包头 + 包尾
    /// </summary>
    /// <returns></returns>
    public static byte[] EncodeMessage(byte[] data)
    {
        //Console.WriteLine(Encoding.Default.GetString(data));
        //内存流对象
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                //先写入长度
                bw.Write(data.Length);
                //再写入数据
                bw.Write(data);

                //将新的消息从流转成数组
                byte[] byteArray = new byte[(int)ms.Length];
                Buffer.BlockCopy(ms.GetBuffer(), 0, byteArray, 0, (int)ms.Length);
                return byteArray;
            }

        }
    }

    /// <summary>
    /// 拆包
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static byte[] DecodeMessage(ref List<byte> data)
    {

        if (data.Count < 4)
        {
            //throw new Exception("数据长度不足4，不能构成一条消息...");
            return null;
        }

        using (MemoryStream ms = new MemoryStream(data.ToArray()))
        {
            using (BinaryReader br = new BinaryReader(ms))
            {
                //得到本条消息的包头信息   length为这条消息的长度
                int length = br.ReadInt32();
                int dataRemainLength = (int)(ms.Length - ms.Position);
                //消息不够长
                if (dataRemainLength < length)
                {
                    //throw new Exception("数据长度不够包头信息约定的长度，无法构成一条完整的消息...");
                    return null;
                }

                byte[] dateArray = br.ReadBytes(length);
                //将剩余的消息存到数组里面
                data.Clear();
                if (dataRemainLength - length > 0)
                    data.AddRange(br.ReadBytes(dataRemainLength));
                return dateArray;
            }
        }

    }

    #endregion

    #region 构造发送消息类
    /// <summary>
    /// 把MessageData类转换成字节数组  发送出去
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static byte[] EncodeMsg(MessageData message)
    {
        MemoryStream ms = new MemoryStream();
        BinaryWriter bw = new BinaryWriter(ms);
        bw.Write(message.OpCode);
        bw.Write(message.SubCode);
        //如果不等于null 才需要把object转换成字节数组
        if (message.Value != null)
        {
            byte[] valueBytes = EncodeObj(message.Value);
            bw.Write(valueBytes);
        }
        byte[] bytesMsg = new byte[(int)ms.Length];
        Buffer.BlockCopy(ms.GetBuffer(), 0, bytesMsg, 0, (int)ms.Length);

        bw.Close();
        ms.Close();
        return bytesMsg;

    }

    /// <summary>
    /// 将收到的字节数组 转换成MessageData对象 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static MessageData DecodeMsg(byte[] data)
    {
        MessageData msg = new MessageData();
        using (MemoryStream ms = new MemoryStream(data))
        {
            using (BinaryReader br = new BinaryReader(ms))
            {
                msg.OpCode = br.ReadInt32();
                msg.SubCode = br.ReadInt32();
                //还有剩余的字节数没有读取
                if (ms.Length > ms.Position)
                {
                    byte[] valueBytes = br.ReadBytes((int)(ms.Length - ms.Position));
                    object value = DecodeObj(valueBytes);
                    msg.Value = value;
                }
            }
        }
        return msg;
    }

    #endregion

    #region 把一个object转换成byte[]

    /// <summary>
    /// 序列化对象
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static byte[] EncodeObj(object value)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, value);
            byte[] valueBytes = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, valueBytes, 0, (int)ms.Length);
            return valueBytes;
        }
    }

    /// <summary>
    /// 反序列化对象
    /// </summary>
    /// <param name="valueBytes"></param>
    /// <returns></returns>
    public static object DecodeObj(byte[] valueBytes)
    {
        using (MemoryStream ms = new MemoryStream(valueBytes))
        {
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(ms);
            return obj;
        }
    }

    #endregion
}



