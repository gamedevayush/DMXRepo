using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LocalChannelInfo
{
    public string ChannelName;
    public int Channel;
    [Range(0,255)]
    public float LocalValue;
}
[System.Serializable]
public class SharpyClass
{
    public List<LocalChannelInfo> localChannel;

    [Header("Engine Settings")]
    public Light myLight;
    public float defaultMaxIntensity;
    public Texture2D[] gobos;
    public GameObject mainShape;
    public int ChannelSupport;
    public void Reset()
    {
        // Implement logic to reset the channels to default values
        foreach (var channelInfo in localChannel)
        {
            channelInfo.LocalValue = 0; // or any default value
        }
    }
    
}
