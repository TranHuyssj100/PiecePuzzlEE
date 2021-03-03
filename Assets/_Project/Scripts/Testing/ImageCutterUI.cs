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

        if (GUILayout.Button("SpawnGO"))
        {
            image.SpawnGObj();
        }
        if (GUILayout.Button("Delete"))
        {
            image.RemoveAllGObj();
        }
        if (GUILayout.Button("Save Prefab"))
        {
            image.SaveSelectedAsPrefab();
        }
    }


}
#endif