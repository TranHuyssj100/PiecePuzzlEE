﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class LabelFieldEditor : EditorWindow
{

    Rect windowRect = new Rect(20, 20, 120, 50);

    public class MenuItems
    {
        [MenuItem("Tools/Player Prefs/Show PlayerPrefs")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(LabelFieldEditor), false, "Game Data");
        }

        [MenuItem("Tools/Player Prefs/ Delete PlayerPrefs")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }

   
    void OnGUI()
    {
        GUILayout.Label("PlayerPrefs", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("theme", ((ThemeType) PlayerPrefs.GetInt("theme")).ToString());
        EditorGUILayout.LabelField("level", PlayerPrefs.GetInt("level").ToString());
        EditorGUILayout.LabelField("No Ads", PlayerPrefs.GetInt("no_ads").ToString());
        EditorGUILayout.LabelField("Gold", PlayerPrefs.GetInt("gold").ToString());

        EditorGUILayout.Space(10f);
        GUILayout.Label("Level Completed", EditorStyles.boldLabel);
        for (int i=0; i <(int) ThemeType.NUM_OF_THEME; i++)
        {
            EditorGUILayout.LabelField(((ThemeType)i).ToString()+ "Level", PlayerPrefs.GetInt(((ThemeType)i).ToString() + "Level").ToString());

        }  
        
        EditorGUILayout.Space(10f);
        GUILayout.Label("Level Unlocked", EditorStyles.boldLabel);
        for(int i=0; i <(int) ThemeType.NUM_OF_THEME; i++)
        {
            EditorGUILayout.LabelField(((ThemeType)i).ToString()+ "Unlock", PlayerPrefs.GetInt(((ThemeType)i).ToString() + "Unlock").ToString());

        }
        this.Repaint();
    }

}
#endif