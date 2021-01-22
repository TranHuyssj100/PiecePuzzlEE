using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.Playables;
using System.Collections.Generic;
using System.Linq;

public class Piece : MonoBehaviour
{

    public bool isOnGridMap=false;
    public bool isMouseDown = false;
    public bool canSetPosition=true;
    
    Vector3 oldPosition;

    private void Start()
    {
        canSetPosition = true;
        oldPosition = transform.position;
    }


    private void Update()
    {
    }

    private void OnMouseDown()
    {
        isMouseDown = true;
    }

    private void OnMouseDrag()
    {
        //Debug.Log("OnMouseDrag");
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z-1));
    }

    private void OnMouseUp()
    {
        SetPositionPiece();
        //isMouseDown = false;
        //if(isOnGridMap && canSetPosition)
        //{
        //    SetPositionPiece();
        //}
        //else
        //{
        //    transform.DOMove(oldPosition, 0.5f);
        //    canSetPosition = true;
        //}
    }
    

    void SetPositionPiece()
    {
        transform.DOMove(new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y-0.5f)+0.5f, transform.position.z),0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        Debug.Log(collision.name);
        if(collision.tag=="Piece")
            canSetPosition = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag=="Piece")
            canSetPosition = true;
    }





}
