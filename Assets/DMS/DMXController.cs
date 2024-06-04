using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DMXChannel
{
    public int channelNumber;
    [Range(0, 255)]
    public float value;
}

public class DMXController : MonoBehaviour
{
    public static DMXController Instance;
    public int channelNumb;
    public int channelValue;
    [SerializeField]
    public  List<DMXChannel> channels = new List<DMXChannel>();
    public Dictionary<int, float> channelValues = new Dictionary<int, float>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        for (int i = 0; i < channels.Count; i++)
        {
            channelValues[i] = 0;
        }
    }
    public void SetChannelForAnimationSupport()
    {
        SetChannelValue(channelNumb, channelValue);
    }

    public void SetChannelValue(int channel, float value)
    {
        var dmxChannel = channels.Find(c => c.channelNumber == channel);
        if (dmxChannel != null)
        {
            dmxChannel.value = value;
        }
        else
        {
            channels.Add(new DMXChannel { channelNumber = channel, value = value });
        }

        ChannelSubscriber.NotifySubscribers(channel, value);
        if (channelValues.ContainsKey(channel))
        {
            channelValues[channel] = value;
        }
        else
        {
            channelValues.Add(channel, value);
        }
    }
    public int ChannelCount => channelValues.Count;
    public float GetChannelValue(int channel)
    {
        var dmxChannel = channels.Find(c => c.channelNumber == channel);
        return dmxChannel != null ? dmxChannel.value : 0;
    }
}

public static class ChannelSubscriber
{
    public static event Action<int, float> OnChannelUpdate;

    public static void NotifySubscribers(int channel, float value)
    {
        OnChannelUpdate?.Invoke(channel, value);
    }
}
