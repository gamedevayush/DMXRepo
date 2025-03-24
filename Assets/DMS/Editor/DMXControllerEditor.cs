using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(DMXController))]
public class DMXControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();  // Draw the default inspector

        DMXController script = (DMXController)target; // Cast the target of this inspector to DMXController

        // Create a slider for each channel
        var keys = new List<int>(script.channelValues.Keys);
        keys.Sort();  // Optional: sort keys to display them in order

        foreach (int channel in keys)
        {
            // Use EditorGUILayout.Slider to create a slider for each channel
            float value = EditorGUILayout.Slider("Channel " + channel, script.channelValues[channel], 0, 360);
            if (value != script.channelValues[channel])  // Only call SetChannelValue if the value has changed
            {
                script.SetChannelValue(channel, value);
            }
        }

        if (GUILayout.Button("Reset Channels"))
        {
            foreach (var channel in keys)
            {
                script.SetChannelValue(channel, 0);  // Reset each channel to 0 or any default value
            }
        }
    }
}
