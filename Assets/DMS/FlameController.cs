using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour
{
    public FlameClass flame;
    public ParticleSystem flameThrower;

    #region Main Features
    void Start()
    {
        flame.myLight = flameThrower;
        flame.myLight.Play();
        var emission = flame.myLight.emission;
        emission.enabled =false;
        for (int i = 0; i < 100; i++)
        {
            HandleChannelUpdate(i, 0);
        }

    }



    // Method to adjust the light dimmer
    public void SetDimmer(float intensity)
    {
        var emission = flame.myLight.emission;
        if (intensity < 128)
        {
            emission.enabled = false;
        }
        else
        {
            emission.enabled = true;
        }
    }

    #endregion

    #region Related To Outside Calls
    void OnEnable()
    {
        // Subscribe to the channel updates
        ChannelSubscriber.OnChannelUpdate += HandleChannelUpdate;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks or unwanted behavior when the object is disabled
        ChannelSubscriber.OnChannelUpdate -= HandleChannelUpdate;
    }

    private void HandleChannelUpdate(int channel, float value)
    {
        var channelInfo = flame.localChannel.Find(c => c.Channel == channel);
        if (channelInfo != null)
        {
            channelInfo.LocalValue = value;
            UpdatePropertyBasedOnChannel(channelInfo.ChannelName, value);
        }
    }
    #endregion


    private void UpdatePropertyBasedOnChannel(string channelName, float value)
    {
        // Example: Matching channel names to properties
        if (channelName == "status")
        {
            SetDimmer(value);
        }
    }

}
