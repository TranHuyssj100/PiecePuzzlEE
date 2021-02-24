using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GridLevel : MonoBehaviour
{
    public GameObject gridChild;
    public TextMeshProUGUI title; 

    int numLevelofTheme;
    
    void Start()
    {
        numLevelofTheme = DataController.Instance.themeData.groupLevel.Length;
        
        SpawnGridChild(ThemeType.Dog, 5);
    }

    void Update()
    {
        
    }


    public void SpawnGridChild(ThemeType _type, int _sizeLevel)
    {
        title.text = _type.ToString();
        for(int i=0; i< numLevelofTheme; i++)
        {
            Sprite _imgSprite = LevelController.LoadSpriteReview(i, _type, _sizeLevel);
            if (_imgSprite != null)
            {
                GameObject _gridChildClone = GameObject.Instantiate(gridChild, gameObject.transform);
                _gridChildClone.GetComponent<GridChild>().indexLevel = i;
                _gridChildClone.transform.GetChild(0).GetComponent<Image>().sprite = _imgSprite;
                _gridChildClone.transform.localScale = Vector3.zero;
                _gridChildClone.transform.DOScale(Vector3.one, 0.2f);
            }
            else
            {
                Debug.LogError("Sprite Null");
            }
        }
    }




}
