﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TweenCustom
{

    public static void ReWard(GameObject coinImg, GameObject text, int value)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(DOVirtual.Float(0, value, 0.8f, (x) => { text.GetComponent<TextMeshProUGUI>().text = "+ " + Mathf.RoundToInt(x).ToString(); }))
             .Append(coinImg.transform.DOScale(Vector3.one * 1.8f, 0.3f))
             .Append(coinImg.transform.DOScale(Vector3.one, 0.1f));
    }

    public static void ZoomOut(Transform gameObject, float strength , float duration)
    {
        gameObject.localScale = Vector3.zero;
        Sequence seq = DOTween.Sequence();
        seq.Append(gameObject.DOScale(Vector3.one * strength, duration))
            .Append(gameObject.DOScale(Vector3.one, duration/4));

    }  
    public static void ZoomIn(Transform gameObject, float strength , float duration)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(gameObject.DOScale(Vector3.one * strength, duration))
            .Append(gameObject.DOScale(Vector3.zero, duration/4));

    }  
    public static void ZoomOutandIn(Transform gameObject,float strength , float duration)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(gameObject.DOScale(Vector3.one* strength, duration))
            .Append(gameObject.DOScale(Vector3.one, duration/4));

    }    
    public static void ZoomInandOut(Transform gameObject,float strength ,float duration)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(gameObject.DOScale(Vector3.one* strength, duration))
            .Append(gameObject.DOScale(Vector3.one, duration/4));
    }   


    public static void ToBottom (Transform transform, float strength, float duration)
    {
        Sequence seq = DOTween.Sequence();
        Vector3 oldPos = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y+strength, transform.position.z);
        seq.Append(transform.DOMove(oldPos - Vector3.up * strength/2, duration/2))
           .Append(transform.DOMove(oldPos, duration));
    }  
    public static void ToUpper (Transform transform, float strength, float duration)
    {
        Sequence seq = DOTween.Sequence();
        Vector3 oldPos = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y-strength, transform.position.z);
        seq.Append(transform.DOMove(oldPos + Vector3.up * strength/2, duration/2))
           .Append(transform.DOMove(oldPos, duration));
    }
}