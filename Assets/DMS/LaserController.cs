using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserController : MonoBehaviour
{
    public LaserClass laserLight;
    public Light laserLightLamp;
    private float strobeTimer = 0f;
    private float localStrobe = 0f;
    private Coroutine goboShakeCoroutine;

    #region Main Features
    void Start()
    {
        laserLight.myLight = laserLightLamp;
        for (int i = 0; i < 100; i++)
        {
            HandleChannelUpdate(i, 0);
        }

    }

    #region Light Colors
    public void SetColorR(float value)
    {
        laserLight.myLight.color = new Color(value, laserLight.myLight.color.g, laserLight.myLight.color.b, laserLight.myLight.color.a);
    }
    public void SetColorG(float value)
    {
        laserLight.myLight.color = new Color(laserLight.myLight.color.r, value, laserLight.myLight.color.b, laserLight.myLight.color.a);
    }
    public void SetColorB(float value)
    {
        laserLight.myLight.color = new Color(laserLight.myLight.color.r, laserLight.myLight.color.g, value, laserLight.myLight.color.a);
    }
    public void SetColorA(float value)
    {
        laserLight.myLight.color = new Color(laserLight.myLight.color.r, laserLight.myLight.color.g, laserLight.myLight.color.b, value);
    }
    #endregion 

    // Method to adjust the light dimmer
    public void SetDimmer(float intensity)
    {
        laserLight.myLight.intensity = laserLight.defaultMaxIntensity * intensity;
    }

    // Method to change the gobo wheel
    public void SetGoboWheel(float goboIndex)
    {
        if (laserLight.gobos[(int)goboIndex] != null)
        {
            laserLight.myLight.cookie = laserLight.gobos[(int)goboIndex];
        }
    }

    // Method to toggle the prism effect
    public void TogglePrism(float state)
    {
        if (laserLight.gobos[1] != null)
        {
            laserLight.myLight.cookie = laserLight.gobos[1];
        }
    }

    // Method to rotate the prism
    public void RotatePrism(float angle)
    {
        laserLight.mainShape.transform.localEulerAngles = new Vector3(laserLight.mainShape.transform.localRotation.x, angle, laserLight.mainShape.transform.localRotation.z);
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
            laserLight.mainShape.transform.localEulerAngles = new Vector3(0, laserLight.mainShape.transform.localEulerAngles.y, 0);
        }
    }

    private IEnumerator GoboShakeEffect()
    {
        while (true)
        {
            float shakeAmount = 0.5f; // Shake amount, adjust as necessary
            laserLight.mainShape.transform.localEulerAngles += new Vector3(0, Random.Range(-shakeAmount, shakeAmount), 0);
            yield return new WaitForSeconds(0.05f);
        }
    }

    // Method to toggle the frost effect
    public void ToggleFrost(float state)
    {
        if (state > 128)
        {
            laserLight.myLight.spotAngle += 10; // Increase the spot angle to create a "frosted" effect
        }
        else
        {
            laserLight.myLight.spotAngle -= 10; // Reset the spot angle
        }
    }

    // Method to adjust focus
    public void SetFocus(float focusLevel)
    {
        // Assuming there's a property or effect in Unity to represent focus, like field of view or similar
        laserLight.myLight.range = focusLevel * 100; // Adjust this calculation as needed
    }

    // Method to adjust pan
    public void SetPan(float pan)
    {
        // Assuming we are rotating around the y-axis for pan
        laserLight.mainShape.transform.localEulerAngles = new Vector3(laserLight.mainShape.transform.localEulerAngles.x, laserLight.mainShape.transform.localEulerAngles.y, pan);
    }

    public void SetPanFine(float panFine)
    {
        laserLight.mainShape.transform.localEulerAngles += new Vector3(0, 0, panFine);
    }

    public void SetTilt(float tilt)
    {
        // Assuming we are rotating around the x-axis for tilt
        laserLight.mainShape.transform.localEulerAngles = new Vector3(tilt, laserLight.mainShape.transform.localEulerAngles.y, laserLight.mainShape.transform.localEulerAngles.z);
    }

    public void SetTiltFine(float tiltFine)
    {
        laserLight.mainShape.transform.localEulerAngles += new Vector3(tiltFine, 0, 0);
    }


    // Method to reset all settings to default
    public void ResetSettings(float value)
    {
        if (value > 128)
        {
            laserLight.Reset();
        }
    }

    // Method to toggle the lamp on or off
    public void ToggleLamp(float state)
    {

        if (state > 128)
        {
            laserLight.myLight.enabled = true;
        }
        else
        {
            laserLight.myLight.enabled = false;
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
                laserLight.myLight.enabled = !laserLight.myLight.enabled; // Toggle the light on and off
                strobeTimer = 0f; // Reset the timer after each toggle
            }
        }
        else if (!laserLight.myLight.enabled)
        {
            laserLight.myLight.enabled = true; // Ensure the light is on if strobe is not active
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
        var channelInfo = laserLight.localChannel.Find(c => c.Channel == channel);
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
            case "colorR":
                SetColorR(value / 255);
                break;

            case "colorG":
                SetColorG(value / 255);
                break;

            case "colorB":
                SetColorB(value / 255);
                break;

            case "colorA":
                SetColorA(value / 255);
                break;

            case "strobe":
                localStrobe = value / 10;
                break;

            case "dimmer":
                SetDimmer(value);
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

            case "focus":
                SetFocus(value);
                break;

            case "pan":
                SetPan(value);
                break;

            case "panFine":
                SetPanFine(value);
                break;

            case "tilt":
                SetTilt(value);
                break;

            case "tiltFine":
                SetTiltFine(value);
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
