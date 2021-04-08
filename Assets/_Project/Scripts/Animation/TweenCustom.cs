using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Security.AccessControl;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using Boo.Lang;

public class TweenCustom
{

    public static void ReWard(GameObject coinImg, GameObject text, int value)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(DOVirtual.Float(0, value, 0.8f, (x) => { text.GetComponent<TextMeshProUGUI>().text = "+ " + Mathf.RoundToInt(x).ToString(); }))
             .Append(coinImg.transform.DOScale(Vector3.one * 1.8f, 0.3f))
             .Append(coinImg.transform.DOScale(Vector3.one, 0.1f));
    }

    public static void ZoomOut(Transform transform, float strength , float duration)
    {
        transform.localScale = Vector3.zero;
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(Vector3.one * strength, duration))
            .Append(transform.DOScale(Vector3.one, duration/4));

    }  
    public static void ZoomIn(Transform gameObject, float strength , float duration)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(gameObject.DOScale(Vector3.one * strength, duration))
            .Append(gameObject.DOScale(Vector3.zero, duration/4));

    }  
    public static void ZoomOutandIn(Transform transform,float strength , float duration)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(Vector3.one + Vector3.one* strength, duration))
            .Append(transform.DOScale(Vector3.one, duration/4));

    }    
    public static void ZoomInandOut(Transform transform,float strength ,float duration)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(Vector3.one - Vector3.one * strength, duration))
            .Append(transform.DOScale(Vector3.one, duration/4));
    }   
    public static void ToBottom (Transform transform, float strength, float duration)
    {
        Sequence seq = DOTween.Sequence();
        Vector3 oldPos = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y+strength, transform.position.z);
        seq.Append(transform.DOMove(oldPos -Vector3.up * strength/8, duration))
           .Append(transform.DOMove(oldPos, duration/4));
    }  
    public static void ToUpper (Transform transform, float strength, float duration)
    {
        Sequence seq = DOTween.Sequence();
        Vector3 oldPos = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y-strength, transform.position.z);
        seq.Append(transform.DOMove(oldPos + Vector3.up * strength/8, duration))
           .Append(transform.DOMove(oldPos, duration/4));
    }


    public static void TextAutoComplete(TextMeshProUGUI textMesh, string txtBegin, string txtEnd, float duration, bool isLoop )
    {
        Debug.Log("Animate");
        if (isLoop)
        {
            DOTween.To(
                () => txtBegin,
                x => txtBegin = x,
                txtEnd,
                duration).OnUpdate(() => textMesh.text = txtBegin).SetLoops(-1);
        }
        else
        {
            DOTween.To(
                () => txtBegin,
                x => txtBegin = x,
                txtEnd,
                duration).OnUpdate(() => textMesh.text = txtBegin);
        }
    }

    public static void RangeTextRunner(int begin, int end, float duration, TextMeshProUGUI text )
    {
        DOVirtual.Float(begin, end, duration, ((x) => { text.text = ((int)x).ToString(); }));
    }

    public static void ProgressBar(Image fill, float duration, TweenCallback f)
    {
        DOVirtual.Float(0, 1, duration, (x) => { fill.fillAmount = x; }).OnComplete(f);

    }

    public static void RightToLeft(Transform transform, float strength, float duration)
    {
       
    }

    public static void MoveObject(Transform transform, Vector3 end, float duration)
    {
        transform.DOMove(end, duration);
    }
}