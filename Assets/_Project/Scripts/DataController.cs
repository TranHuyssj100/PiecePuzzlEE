using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataController : SingletonDontDestroyMonoBehavior<DataController>
{

//    private static string SavePath;
//    private static string BasePath;
//    private static string BASE_LEVELDATA = "Base_Level_Data";
//    private static string SAMPLE_ANSWER = "Sample_Answer";
//    private static string JsonSuffix = ".json";

//    private void Awake()
//    {
//        base.Awake();
//#if UNITY_EDITOR
//        SavePath = "Assets/Json";
//        BasePath = Application.streamingAssetsPath;
//#elif UNITY_ANDROID
//        SavePath = Application.persistentDataPath;
//        BasePath = "jar:file://" + Application.dataPath + "!assets/";
//#elif UNITY_IOS
//        SavePath = Application.persistentDataPath;
//        BasePath = Application.dataPath + "/Raw";
//#endif
 
//    }

//    private void Update()
//    {

//    }


//    public void LoadAllBaseCharInfo()
//    {
//        //load base hero data
//        string loadString = "";
//#if UNITY_EDITOR
//        loadString = File.ReadAllText(Path.Combine(BasePath, "Json" + "/" + BASE_LEVELDATA + JsonSuffix));
//#elif UNITY_ANDROID
//        WWW reader = new WWW(Path.Combine(BasePath, "Json" + "/" + BASE_CHARACTER_DATA_FILE_NAME + JsonSuffix));
//        while (!reader.isDone) { }
//        loadString = reader.text;
//#elif UNITY_IOS
//        loadString = File.ReadAllText(Path.Combine(BasePath, "Json" + "/" + BASE_CHARACTER_DATA_FILE_NAME + JsonSuffix));
//#endif
//        //baseCharacterData = JsonHelper.FromJson<BaseCharacterData>(loadString);
//    }
//    public void LoadAllUserCharInfo()
//    {
//        //load user hero data
//        if (!File.Exists(Path.Combine(SavePath, SAMPLE_ANSWER + JsonSuffix)))
//        {
//            CreateDefaultUserData();
//        }
//        string loadString = File.ReadAllText(Path.Combine(SavePath, SAMPLE_ANSWER + JsonSuffix));
//        //userCharacterData = JsonHelper.FromJson<UserCharacterData>(loadString);
//    }

//    //-------------------------------Character Data------------------------
//    private void CreateDefaultUserData()
//    {
//        //string _strData;
//        //_strData = "[";
//        //for (int i = 0; i < baseCharacterData.Length; i++)
//        //{
//        //    UserCharacterData characterData = new UserCharacterData
//        //    {
//        //        id = baseCharacterData[i].id,
//        //        name = baseCharacterData[i].name,
//        //        is_unlocked = i == 0 ? true : false,
//        //        is_equipped = i == 0 ? true : false
//        //    };
//        //    _strData += "\n" + JsonUtility.ToJson(characterData, true);
//        //    if (i == baseCharacterData.Length - 1)
//        //        break;
//        //    _strData += ",";
//        //}
//        //_strData += "]";
//        //File.WriteAllText(Path.Combine(SavePath, USER_CHARACTER_DATA_FILE_NAME + JsonSuffix), _strData);
//    }

//    #region JSONHELPER
//    //----------------------------JsonHelp---------DONT TOUCH------------------
//    public static class JsonHelper
//    {
//        public static T[] FromJson<T>(string jsonArray)
//        {
//            jsonArray = WrapArray(jsonArray);
//            return FromJsonWrapped<T>(jsonArray);
//        }

//        public static T[] FromJsonWrapped<T>(string jsonObject)
//        {
//            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(jsonObject);
//            return wrapper.items;
//        }

//        private static string WrapArray(string jsonArray)
//        {
//            return "{ \"items\": " + jsonArray + "}";
//        }

//        [System.Serializable]
//        private class Wrapper<T>
//        {
//            public T[] items;
//        }
//    }
//    #endregion
}


[System.Serializable]
public class LevelData
{
    public int index;
    public ThemeType theme;
    public int sample;
    public int numMove;
}

[System.Serializable]
public class SampleAnswer
{
    public int indexSample;
    public Queue<int> answers;
}
