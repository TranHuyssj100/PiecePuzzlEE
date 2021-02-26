#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AnswerBuilder))]

public class AnswerBuilderEditor : Editor
{
    private void Awake()
    {
        
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        AnswerBuilder answerBuilder = target as AnswerBuilder;
        GUILayout.Label("LEVEL INFOR", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("theme"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("level"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("size"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("listTexture"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("listSample"));
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Spawn Piece"))
        {
            answerBuilder.Intial();
        }
        if (GUILayout.Button("Check"))
        {
            answerBuilder.CheckAnswer();
        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Clear"))
        {
            answerBuilder.Clear();
        }
        GUILayout.EndVertical();
        GUILayout.Space(20);

        GUILayout.Label("JSON", EditorStyles.boldLabel);
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("jsonFileName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("listAnswerForSample"));
        GUILayout.BeginVertical();
        //GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create"))
        {
            answerBuilder.CreateJson(false);
        }
        if(GUILayout.Button("Update"))
        {
            answerBuilder.CreateJson(true);
        }
        //GUILayout.EndHorizontal();
        GUILayout.EndVertical();


        serializedObject.ApplyModifiedProperties();

    }
}
#endif