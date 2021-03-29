using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HaloRotation : MonoBehaviour
{
    public Vector3 euler ;
    void Update()
    {
        transform.Rotate(euler *Time.deltaTime);
    }
}
