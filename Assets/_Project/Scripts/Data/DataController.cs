using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;


public class DataController : SingletonDontDestroyMonoBehavior<DataController>
{

    private static string SAVESAMPLE;
    private static string SAVETHEME;
    private static string LEVEL = "Level";
    private static string SAMPLE = "Sample";
    private static string JsonSuffix = ".json";

    //public TextMeshProUGUI txtDebug;
    public ThemeData themeData;

    private void Awake()
    {
        base.Awake();
#if UNITY_EDITOR
        SAVESAMPLE = Application.streamingAssetsPath +"/Json/Answers";
        SAVETHEME = Application.streamingAssetsPath + "/Json/Themes";
#elif UNITY_ANDROID
        SAVESAMPLE = "jar:file://" + Application.dataPath + "!assets/";
        SAVETHEME =  "jar:file://" + Application.dataPath + "!assets/";
#endif
        themeData = LoadThemeData(GameData.Theme);        
    }



    private void Update()
    {

    }


    public static ThemeData LoadThemeData(int _type)
    {
        string loadString ;
#if UNITY_EDITOR
        loadString = File.ReadAllText(Path.Combine(SAVETHEME, ((ThemeName)_type).ToString() + JsonSuffix));
        //Debug.LogError(Path.Combine(SAVETHEME, ((ThemeType)_type).ToString() + JsonSuffix));
#elif UNITY_ANDROID
        WWW reader = new WWW(Path.Combine(SAVETHEME, "Json/Themes/" + ((ThemeType)_type).ToString() + JsonSuffix));
        while (!reader.isDone) { }
        loadString = reader.text;
#endif
        return JsonHelper.FromJson<ThemeData>(loadString)[0];
    }


    public static SampleAnswer LoadSampleAnswer(int _indexSample)
    {
        string loadString;
#if UNITY_EDITOR
        loadString = File.ReadAllText(Path.Combine(SAVESAMPLE, _indexSample.ToString() + JsonSuffix));
        //Debug.LogError(Path.Combine(SAVESAMPLE, _indexSample.ToString() + JsonSuffix));
#elif UNITY_ANDROID
        WWW reader = new WWW(Path.Combine(SAVESAMPLE, "Json/Answers/" +_indexSample.ToString() + JsonSuffix));
        while (!reader.isDone) { }
        loadString = reader.text;
#endif
        return JsonHelper.FromJson<SampleAnswer>(loadString)[0];
    }

    //public static int GetThemeLevelCount(ThemeName themeName,int size)
    //{
    //    //DirectoryInfo dir = new DirectoryInfo("Assets/_Project/Resources/Themes/" + themeName.ToString() + "/" + +size + "x" + size);
    //    Resources.Load("Themes/ " + themeName.ToString() + " / " + +size + "x" + size).
    //    return Mathf.RoundToInt(dir.GetFiles().Length);
    //}

    public static List<string> GetAllTheme()
    {
        //Debug.LogError(SAVETHEME);
        DirectoryInfo dir = new DirectoryInfo(SAVETHEME);
        FileInfo[] allFile = dir.GetFiles("*.json");
        List<string> allFileName = new List<string>();
        for (int i = 0; i < allFile.Length; i++)
            allFileName.Add(Path.GetFileName(allFile[i].Name.Remove(allFile[i].Name.Length - 5, 5)));
        return allFileName;
        //return Mathf.RoundToInt(dir.GetFiles().Length/2);
    }
    //void CreateDefaultLevelData()
    //{
    //    string _strData;
    //    _strData = "[";
    //    LevelData _levelData = new LevelData();
    //    _levelData.index = 0;
    //    _levelData.sampleIndex = 0;
    //    _levelData.theme = ThemeType.Animal;
    //    _strData += "\n" + JsonUtility.ToJson(_levelData, true);
    //    _strData += "]";
    //    Directory.CreateDirectory(SAVELEVEL);
    //    File.WriteAllText(Path.Combine(SAVELEVEL, "0" + JsonSuffix), _strData);
    //}

    //void CreateSampleAnswer(int _index, int _numPiece, int [] _answer)
    //{
    //    string _strData;
    //    _strData = "[";
    //    SampleAnswer _sample = new SampleAnswer();
    //    _sample.index = _index;
    //    //_sample.numPiece = _numPiece;
    //    _sample.answers = _answer;
    //    _strData += "\n" + JsonUtility.ToJson(_sample, true);
    //    _strData += "]";
    //    Directory.CreateDirectory(SAVESAMPLE);
    //    File.WriteAllText(Path.Combine(SAVESAMPLE, "0" + JsonSuffix), _strData);

    //}



#region JSONHELPER
    //----------------------------JsonHelp---------DONT TOUCH------------------
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string jsonArray)
        {
            jsonArray = WrapArray(jsonArray);
            return FromJsonWrapped<T>(jsonArray);
        }

        public static T[] FromJsonWrapped<T>(string jsonObject)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(jsonObject);
            return wrapper.items;
        }

        private static string WrapArray(string jsonArray)
        {
            return "{ \"items\": " + jsonArray + "}";
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] items;
        }
    }
#endregion
}

[System.Serializable]
public class ThemeData
{
    public ThemeName theme;
    public int size;
    public LevelData[] groupLevel;

}

[System.Serializable]
public class LevelData
{
    public int index;
    public int sampleIndex;
}

[System.Serializable]
public class SampleAnswer
{
    public int index;
    public int [] pieceNames;
    public int [] answers;
}


