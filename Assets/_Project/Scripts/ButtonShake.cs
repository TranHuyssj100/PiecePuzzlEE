using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonShake :MonoBehaviour, IPointerDownHandler
{
    float duration = 0.2f;
    public void  OnPointerDown(PointerEventData eventData )
    {
        //DOTween.CompleteAll();
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOShakeScale(duration, 0.2f, 5, 90, false))
           .Append(transform.DOScale(1, duration));
    }
}
