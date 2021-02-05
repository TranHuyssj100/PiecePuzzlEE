using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataController : SingletonDontDestroyMonoBehavior<DataController>
{

    private static string SAVELEVEL;
    private static string SAVESAMPLE;
    private static string SAVETHEME;
    private static string LEVEL = "Level";
    private static string SAMPLE = "Sample";
    private static string JsonSuffix = ".json";

    public ThemeData themeData;

    private void Awake()
    {
        base.Awake();
//#if UNITY_EDITOR
        //SAVELEVEL = "Assets/Json/Levels/";
        //SAVESAMPLE = "Assets/Json/Answers/";
        //SAVETHEME = "Assets/Json/themes/";
//#elif UNITY_ANDROID
        SAVELEVEL =  Application.dataPath + "/Json/Levels/";
        SAVESAMPLE = Application.dataPath +  "/Json/Answers/";
        SAVETHEME =  Application.dataPath +  "/Json/themes/";
//#endif
        //SAVELEVEL = "Assets/_Project/Resources/Json/Levels/";
        //SAVESAMPLE = "Assets/_Project/Resources/Json/Answers/";
        //SAVETHEME = "Assets/_Project/Resources/Json/themes/";
        themeData = LoadThemeData(0);
    }



    private void Update()
    {

    }


    public static ThemeData LoadThemeData(int _type)
    {
        string loadString ;
        //#if UNITY_EDITOR
        //loadString = File.ReadAllText(Path.Combine(SAVETHEME, ((ThemeType)_type).ToString() + JsonSuffix));
        //#elif UNITY_ANDROID
        WWW reader = new WWW(Path.Combine(SAVETHEME, ((ThemeType)_type).ToString() + JsonSuffix));
        while (!reader.isDone) { }
        loadString = reader.text;
        //#endif
        return JsonHelper.FromJson<ThemeData>(loadString)[0];
    }

    public static LevelData [] LoadLevelData(int _index)
    {
        //if (!File.Exists(Path.Combine(SAVEPATH, LEVEL + JsonSuffix)))
        //{
        //    CreateDefaultLevelData();
        //}
        string loadString = File.ReadAllText(Path.Combine(SAVELEVEL, _index.ToString() + JsonSuffix));
        return JsonHelper.FromJson<LevelData>(loadString);
    }
    public static SampleAnswer LoadSampleAnswer(int _indexSample)
    {
        //if (!File.Exists(Path.Combine(SAVEPATH, SAMPLE + JsonSuffix)))
        //{
        //    CreateSampleAnswer(0, 7, new int[] { 1, -3, 1, 2, -1, 1, 3, 0, 1, 4, -3, -2, 5, -2, -1, 6, -2, -2, 7, 1, -2 });
        //}
        string loadString = File.ReadAllText(Path.Combine(SAVESAMPLE, _indexSample.ToString() + JsonSuffix));
        //Debug.Log(loadString);
        return JsonHelper.FromJson<SampleAnswer>(loadString)[0];
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
    public ThemeType theme;
    public bool unlock;
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
