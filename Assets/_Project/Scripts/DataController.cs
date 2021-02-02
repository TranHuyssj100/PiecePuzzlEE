using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataController : SingletonDontDestroyMonoBehavior<DataController>
{

    private static string SAVEPATH;
    private static string BASEPATH;
    private static string LEVEL = "Level";
    private static string SAMPLE = "Sample";
    private static string JsonSuffix = ".json";

    public LevelData[] levelData;
    public SampleAnswer[] SampleAnswers;

    private void Awake()
    {
        base.Awake();
#if UNITY_EDITOR
        SAVEPATH = "Assets/_Project/Json";
        BASEPATH = Application.streamingAssetsPath;
#elif UNITY_ANDROID
        SavePath = Application.persistentDataPath;
        BasePath = "jar:file://" + Application.dataPath + "!assets/";
#elif UNITY_IOS
        SavePath = Application.persistentDataPath;
        BasePath = Application.dataPath + "/Raw";
#endif
        
        LoadLevelData();
        LoadSampleAnswer();

    }

    private void Update()
    {

    }



    public void LoadLevelData()
    {
        if (!File.Exists(Path.Combine(SAVEPATH, LEVEL + JsonSuffix)))
        {
            CreateDefaultLevelData();
        }
        string loadString = File.ReadAllText(Path.Combine(SAVEPATH, LEVEL + JsonSuffix));
        levelData = JsonHelper.FromJson<LevelData>(loadString);
    }
    public void LoadSampleAnswer()
    {
        //if (!File.Exists(Path.Combine(SAVEPATH, SAMPLE + JsonSuffix)))
        //{
        //    CreateSampleAnswer(0, 7, new int[] { 1, -3, 1, 2, -1, 1, 3, 0, 1, 4, -3, -2, 5, -2, -1, 6, -2, -2, 7, 1, -2 });
        //}
        string loadString = File.ReadAllText(Path.Combine(SAVEPATH, SAMPLE + JsonSuffix));
        SampleAnswers = JsonHelper.FromJson<SampleAnswer>(loadString);
    }
    
    void CreateDefaultLevelData()
    {
        string _strData;
        _strData = "[";
        LevelData _levelData = new LevelData();
        _levelData.index = 0;
        _levelData.sampleIndex = 0;
        _levelData.theme = ThemeType.Animal;
        _strData += "\n" + JsonUtility.ToJson(_levelData, true);
        _strData += "]";
        Directory.CreateDirectory(SAVEPATH);
        File.WriteAllText(Path.Combine(SAVEPATH, LEVEL + JsonSuffix), _strData);
    }

    void CreateSampleAnswer(int _index, int _numPiece, int [] _answer)
    {
        string _strData;
        _strData = "[";
        SampleAnswer _sample = new SampleAnswer();
        _sample.index = _index;
        _sample.numPiece = _numPiece;
        _sample.answers = _answer;
        _strData += "\n" + JsonUtility.ToJson(_sample, true);
        _strData += "]";
        Directory.CreateDirectory(SAVEPATH);
        File.WriteAllText(Path.Combine(SAVEPATH, SAMPLE + JsonSuffix), _strData);

    }



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
public class LevelData
{
    public ThemeType theme;
    public int index;
    public int sampleIndex;
}

[System.Serializable]
public class SampleAnswer
{
    public int index;
    public int numPiece;
    public int [] answers;
}
