using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLevelCtr : MonoBehaviour
{
    public GameObject tilePrefap;
    public int sizeGrid;

    public Transform origin;


    private void Start()
    {
        CreateGridBroad(5, origin);
    }

    public void CreateGridBroad(int _size, Transform _origin)
    {
        GameObject _broad = new GameObject("GridBroad");
        for( int i=0; i<_size; i++)
        {
            for(int j=0; j<_size; j++)
            {
                GameObject _tileClone= Instantiate(tilePrefap, origin.position + new Vector3(i, j, 0), Quaternion.identity);
                _tileClone.transform.parent = _origin;
                
            }
        }
    }

}
