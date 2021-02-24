#if UNITY_EDITOR
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
        
        for(int i=0; i <(int) ThemeType.NUM_OF_THEME; i++)
        {
            EditorGUILayout.LabelField(((ThemeType)i).ToString()+ "Level", PlayerPrefs.GetInt(((ThemeType)i).ToString() + "Level").ToString());

        }
        this.Repaint();
    }

}
#endif