#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ImageCutter)), CanEditMultipleObjects]
public class ImageCutterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ImageCutter image = (ImageCutter)target;
        if (GUILayout.Button("Clear Sprite"))
        {
            image.ClearSprite();
        }
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Path", GUILayout.MaxWidth(40));
        image.path = EditorGUILayout.TextField(image.path);
        if (GUILayout.Button("Create Directory"))
        {
            image.CreateFolder();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("SpawnGO"))
        {
            image.SpawnGObj();
        }
        if (GUILayout.Button("Delete"))
        {
            image.RemoveAllGObj();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Sample"))
        {
            image.SaveSampleToJson();
        }
        if (GUILayout.Button("Cut With Preset"))
        {
            image.CutImageWithPreset();
        }
        GUILayout.EndHorizontal();
    }


}
#endif