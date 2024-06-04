using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpyController : MonoBehaviour
{
    public SharpyClass sharpyLight;
    public Light sharpyLightLamp;
    private float strobeTimer = 0f;
    private float localStrobe = 0f;
    private Coroutine goboShakeCoroutine;

    #region Main Features
    void Start()
    {
        sharpyLight.myLight = sharpyLightLamp;
        for (int i = 0; i < 100; i++)
        {
            HandleChannelUpdate(i, 0);
        }

    }

    #region Light Colors
    public void SetColorR(float value)
    {
        sharpyLight.myLight.color = new Color(value, sharpyLight.myLight.color.g, sharpyLight.myLight.color.b, sharpyLight.myLight.color.a);
    }
    public void SetColorG(float value)
    {
        sharpyLight.myLight.color = new Color(sharpyLight.myLight.color.r, value, sharpyLight.myLight.color.b, sharpyLight.myLight.color.a);
    }
    public void SetColorB(float value)
    {
        sharpyLight.myLight.color = new Color(sharpyLight.myLight.color.r, sharpyLight.myLight.color.g, value, sharpyLight.myLight.color.a);
    }

    #endregion

    // Method to adjust the light dimmer
    public void SetDimmer(float intensity)
    {
        sharpyLight.myLight.intensity = sharpyLight.defaultMaxIntensity * intensity;
    }

    // Method to change the gobo wheel
    public void SetGoboWheel(float goboIndex)
    {
        if (sharpyLight.gobos[(int)goboIndex] != null)
        {
            sharpyLight.myLight.cookie = sharpyLight.gobos[(int)goboIndex];
        }
    }

    // Method to toggle the prism effect
    public void TogglePrism(float state)
    {
        if (sharpyLight.gobos[1] != null)
        {
            sharpyLight.myLight.cookie = sharpyLight.gobos[1];
        }
    }



    // Method to toggle the gobo shake effect
    public void ToggleGoboShake(float state)
    {
        if (state > 128)
        {
            if (goboShakeCoroutine != null)
                StopCoroutine(goboShakeCoroutine);
            goboShakeCoroutine = StartCoroutine(GoboShakeEffect());
        }
        else
        {
            if (goboShakeCoroutine != null)
            {
                StopCoroutine(goboShakeCoroutine);
                goboShakeCoroutine = null;
            }
            sharpyLight.mainShape.transform.localEulerAngles = new Vector3(0, sharpyLight.mainShape.transform.localEulerAngles.y, 0);
        }
    }

    private IEnumerator GoboShakeEffect()
    {
        while (true)
        {
            float shakeAmount = 0.5f; // Shake amount, adjust as necessary
            sharpyLight.mainShape.transform.localEulerAngles += new Vector3(0, Random.Range(-shakeAmount, shakeAmount), 0);
            yield return new WaitForSeconds(0.05f);
        }
    }

    // Method to toggle the frost effect
    public void ToggleFrost(float state)
    {
        if (state > 128)
        {
            sharpyLight.myLight.spotAngle = 50; // Increase the spot angle to create a "frosted" effect
        }
        else
        {
            sharpyLight.myLight.spotAngle = 10; // Reset the spot angle
        }
    }

    // Method to adjust focus
    public void SetFocus(float focusLevel)
    {
        // Assuming there's a property or effect in Unity to represent focus, like field of view or similar
        sharpyLight.myLight.spotAngle = 10 + (focusLevel / 6); // Adjust this calculation as needed
    }

    // Method to adjust pan
    // Method to rotate the prism

    // Method to adjust pan (rotating around the y-axis)
    public void SetPan(float pan)
    {
        sharpyLight.mainShape.gameObject.transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, pan);
    }

    public void RotatePrism(float angle)
    {
        sharpyLight.myLight.transform.localEulerAngles = new Vector3(-90,0,angle);
    }

    public void SetTilt(float tilt)
    {
        sharpyLight.mainShape.transform.localEulerAngles = new Vector3(0, tilt,0);
    }

    // Method to reset all settings to default
    public void ResetSettings(float value)
    {
        if (value > 128)
        {
            sharpyLight.Reset();
        }
    }

    // Method to toggle the lamp on or off
    public void ToggleLamp(float state)
    {

        if (state > 128)
        {
            sharpyLight.myLight.enabled = true;
        }
        else
        {
            sharpyLight.myLight.enabled = false;
        }
    }

    #endregion
    #region Update
    private void Update()
    {
        if (localStrobe > 0)
        {
            strobeTimer += Time.deltaTime;
            float strobeInterval = 1.0f / localStrobe; // Calculate the interval from the strobe frequency

            if (strobeTimer >= strobeInterval)
            {
                sharpyLight.myLight.enabled = !sharpyLight.myLight.enabled; // Toggle the light on and off
                strobeTimer = 0f; // Reset the timer after each toggle
            }
        }
        else if (!sharpyLight.myLight.enabled)
        {
            sharpyLight.myLight.enabled = true; // Ensure the light is on if strobe is not active
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
        var channelInfo = sharpyLight.localChannel.Find(c => c.Channel == channel);
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
        switch (channelName)
        {

            case "dimmer":
                SetDimmer(value);
                break;

            case "colorR":
                SetColorR(value / 255);
                break;

            case "colorG":
                SetColorG(value / 255);
                break;

            case "colorB":
                SetColorB(value / 255);
                break;

            case "strobe":
                localStrobe = value / 10;
                break;

            case "pan":
                SetPan(value);
                break;

            case "tilt":
                SetTilt(value);
                break;

            case "focus":
                SetFocus(value);
                break;

            case "goboWheel":
                //SetGoboWheel(value);
                break;

            case "prismBool":
                //TogglePrism(value);
                break;

            case "prismRotation":
                RotatePrism(value);
                break;

            case "goboShakeBool":
                //SetGoboWheel(value);
                break;

            case "frostBool":
                ToggleFrost(value);
                break;

            case "resetBool":
                ResetSettings(value);
                break;

            case "lampOnBool":
                RotatePrism(value);
                break;

        }
    }

}
