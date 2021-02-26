using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThemeChild : MonoBehaviour
{
    public int index;
    public bool isUnlock;
    public TextMeshProUGUI titleTxt;
    public TextMeshProUGUI priceTxt;
    public UnityEngine.UI.Image image;
    public UnityEngine.UI.Button button;


    [System.Serializable]
    public struct ThemeInfo
    {
        public int index;
        public string name;
        public Sprite image;
        public int price;
        [Range(5,7)] public int size;
    }
}
