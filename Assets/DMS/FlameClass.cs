using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FlameClass
{
    public List<LocalChannelInfo> localChannel;

    [Header("Engine Settings")]
    public ParticleSystem myLight;
    public void Reset()
    {
        // Implement logic to reset the channels to default values
        foreach (var channelInfo in localChannel)
        {
            channelInfo.LocalValue = 0; // or any default value
        }
    }

}
