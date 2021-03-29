using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoroutineQueue : MonoBehaviour
{
//protected Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();

    protected IEnumerator ShowObjZoomOut(Transform obj, float timeWait)
    {
        TweenCustom.ZoomOut(obj,1.2f, 0.5f);
        yield return new WaitForSeconds(timeWait);

    }
    protected IEnumerator ShowObjRightToLeft(Transform obj, float timeWait)
    {
        yield return new WaitForSeconds(timeWait);
        TweenCustom.RightToLeft(obj, 1f, 1f);
        //yield return new WaitForSeconds(timeWait);

    }

    protected IEnumerator CoroutineCoordinator(Queue<IEnumerator> _coroutineQueue)
    {
        while (true)
        {
            while (_coroutineQueue.Count > 0)
            {
                yield return StartCoroutine(_coroutineQueue.Dequeue());
            }
            yield return null;
        }
    }
}
