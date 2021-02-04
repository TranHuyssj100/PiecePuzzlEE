using UnityEditor;
using UnityEngine;

public class LabelFieldEditor : EditorWindow
{
#if UNITY_EDITOR

    Rect windowRect = new Rect(20, 20, 120, 50);

    public class MenuItems
    {
        [MenuItem("Tools/Player Prefs/Show PlayerPrefs")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(LabelFieldEditor), false, "Game Data");
        }
    }
    void OnGUI()
    {
        GUILayout.Label("PlayerPrefs", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Level", PlayerPrefs.GetInt("Level").ToString());
        this.Repaint();
    }
#endif

}
