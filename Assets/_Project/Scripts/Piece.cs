using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.Playables;
using System.Collections.Generic;
using System.Linq;

public class Piece : MonoBehaviour
{

    public bool isOnGridMap=false;
    public bool isMouseDown = false;
    
    Vector3 oldPosition;

    private void Start()
    {
        oldPosition = transform.position;
    }


    private void Update()
    {
        CheckOnAnotherPiece();
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
        isMouseDown = false;
        if(isOnGridMap)
            SetPositionPiece();
        else
            transform.DOMove(oldPosition, 0.5f);
    }
    
    

    void SetPositionPiece()
    {
        transform.DOMove(new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)+0.5f, transform.position.z),0.5f);
    }


    void CheckOnAnotherPiece()
    {
        //List<Collider2D> cols = Physics2D.OverlapCircleAll(transform.position, .8f).ToList();
        //if (cols!=null && cols.Find(x=>x.tag=="Piece") && !isMouseDown)
        //{
        //    transform.DOMove(oldPosition, 0.5f);
        //}
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position, .8f);
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
       
        Debug.Log(collision.name);
    }





}
