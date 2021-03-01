using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GridLevel : MonoBehaviour
{
    public GameObject gridChild;
    public TextMeshProUGUI title;
    public static GridLevel instance;
    int numLevelofTheme;
    private void Awake()
    {
        instance = this;

    }

    void Start()
    {
        //numLevelofTheme = DataController.Instance.themeData.groupLevel.Length;
        numLevelofTheme = DataController.themeData[GameData.Theme].levelCount;
        //SpawnGridChild(ThemeName.Dog, 5);
    }

    void Update()
    {
        
    }


    public void SpawnGridChild(int _idTheme, int _sizeLevel)
    {
        title.text = DataController.themeData[_idTheme].name;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        //DataController.Instance.themeData = DataController.LoadThemeData((int)_idTheme);
        //numLevelofTheme = DataController.Instance.themeData.groupLevel.Length; //GetThemeLevelCount(_type, _sizeLevel);
        numLevelofTheme = DataController.themeData[GameData.Theme].levelCount;
        Debug.Log(numLevelofTheme);
        for (int i=0; i < numLevelofTheme; i++)
        {
            Sprite _imgSprite = LevelController.LoadSpritePreview(i, DataController.themeData[GameData.Theme].name, _sizeLevel);
            if (_imgSprite != null)
            {
                GameObject _gridChildClone = GameObject.Instantiate(gridChild, gameObject.transform);
                _gridChildClone.GetComponent<GridChild>().indexLevel = i;
                _gridChildClone.transform.GetChild(0).GetComponent<Image>().sprite = _imgSprite;
                _gridChildClone.transform.localScale = Vector3.zero;
                _gridChildClone.transform.DOScale(Vector3.one, 0.2f);
                if (i <= GameData.GetCurrentLevelByTheme(GameData.Theme))
                    _gridChildClone.GetComponent<GridChild>().UnlockLevel();
                //else
                //    _gridChildClone.GetComponent<GridChild>().isUnlock = false;

            }
            else
            {
                Debug.LogError("Sprite Null");
            }
        }
    }




}
