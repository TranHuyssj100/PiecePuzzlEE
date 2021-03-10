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
        //base.OnInspectorGUI();
        serializedObject.Update();
        ImageCutter image = (ImageCutter)target;

        GUILayout.Label("SPRITE CUSTOMIZE", EditorStyles.boldLabel);
        GUILayout.BeginVertical();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("sprite"));
        if (GUILayout.Button("CLEAR"))
        {
            image.ClearSprite();
        }
        GUILayout.EndVertical();
        GUILayout.Space(10f);

        GUILayout.Label("PREFAP CREATER", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Path", GUILayout.MaxWidth(40));
        image.path = EditorGUILayout.TextField(image.path);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Theme", GUILayout.MaxWidth(40));
        image.theme = EditorGUILayout.TextField(image.theme);
        //if (GUILayout.Button("Create Theme"))
        //{
        //    image.CreateFolder();
        //}
        GUILayout.EndHorizontal();
        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("SPAWN"))
        {
            image.SpawnGObj();
        }
        if (GUILayout.Button("DELETE"))
        {
            image.RemoveAllGObj();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);

        GUILayout.Label("SAMPLE CREATER", EditorStyles.boldLabel);
        GUILayout.BeginVertical();
        if (GUILayout.Button("SAVE"))
        {
            image.SavePresetToJson();
        }
        if (GUILayout.Button("CUT BY SAMPLE"))
        {
            image.CutImageWithPreset();
        }
        GUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }


}
#endif