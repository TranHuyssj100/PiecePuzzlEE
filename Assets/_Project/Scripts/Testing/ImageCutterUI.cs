#if UNITY_EITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ImageCutter)), CanEditMultipleObjects]
public class ImageCutterUI : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ImageCutter image = (ImageCutter)target;
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Path", GUILayout.MaxWidth(40));
        image.path = EditorGUILayout.TextField(image.path);
        if (GUILayout.Button("Create Directory"))
        {
            image.CreateFolder();
        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button("SpawnGO"))
        {
            image.SpawnGObj();
        }
        if (GUILayout.Button("Delete"))
        {
            image.RemoveAllGObj();
        }
        
    }


}
#endif