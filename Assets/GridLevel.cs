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
        numLevelofTheme = DataController.Instance.themeData.groupLevel.Length;
        //SpawnGridChild(ThemeName.Dog, 5);
    }

    void Update()
    {
        
    }


    public void SpawnGridChild(ThemeName _type, int _sizeLevel)
    {
        title.text = _type.ToString();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        Debug.Log(_type);
        DataController.Instance.themeData = DataController.LoadThemeData((int)_type);
        numLevelofTheme = DataController.Instance.themeData.groupLevel.Length; //GetThemeLevelCount(_type, _sizeLevel);
        for (int i=0; i < numLevelofTheme; i++)
        {
            Sprite _imgSprite = LevelController.LoadSpriteReview(i, _type, _sizeLevel);
            if (_imgSprite != null)
            {
                GameObject _gridChildClone = GameObject.Instantiate(gridChild, gameObject.transform);
                _gridChildClone.GetComponent<GridChild>().indexLevel = i;
                _gridChildClone.transform.GetChild(0).GetComponent<Image>().sprite = _imgSprite;
                _gridChildClone.transform.localScale = Vector3.zero;
                _gridChildClone.transform.DOScale(Vector3.one, 0.2f);
                if (i <= GameData.GetCurrentLevelByTheme(GameData.Theme))
                    _gridChildClone.GetComponent<GridChild>().isUnlock = true;
                else
                    _gridChildClone.GetComponent<GridChild>().isUnlock = false;

            }
            else
            {
                Debug.LogError("Sprite Null");
            }
        }
    }




}
