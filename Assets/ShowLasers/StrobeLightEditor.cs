using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StrobeLightManager))]
public class StrobeLightEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StrobeLightManager manager = (StrobeLightManager)target;

        int selectedEffect = EditorGUILayout.IntField("Enter Effect Index", 0);

        if (GUILayout.Button("Apply Effect"))
        {
            manager.SetEffect(selectedEffect);
        }
    }
}