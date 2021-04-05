using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionGrid : GridChild
{
    public override void OnClick()
    {
        if (isUnlock)
        {
            GameMaster.instance.OpenPanel(GameMaster.instance.preview);
            GameMaster.instance.preview.transform.Find("Bg").Find("Image").GetComponent<Image>().sprite = transform.GetChild(0).GetComponent<Image>().sprite;
        }
    }
}
