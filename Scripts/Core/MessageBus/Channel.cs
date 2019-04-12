using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Core.MessageBus;

public class Channel : MonoBehaviour
{
    public static string GetFullSubscribeType(SubscribeType type, int id, string curType)
    {
        return type.ToString("d") + id + "_" + curType;
    }

    private static int _curChannelId = 0;
    public static int GetChannelId()
    {
        return ++_curChannelId;
    }

    public static Dictionary<int, int> ChannelIdByNetId = new Dictionary<int, int>();

    public Dictionary<SubscribeType, int> ChannelIds = new Dictionary<SubscribeType, int>();
}
