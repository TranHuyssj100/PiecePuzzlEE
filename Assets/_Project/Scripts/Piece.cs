using UnityEngine;
using DG.Tweening;


public class Piece : MonoBehaviour
{

    public bool isOnGridMap=false;
    public bool isMouseDown = false;
    public bool canSetPosition=true;
    
    Vector3 oldPosition;
    Vector3 oldMousePos;
    
    private void OnEnable()
    {
        if(transform.GetChild(0)!=null)
            transform.GetChild(0).localScale= Vector3.one * 0.5f;   
    }
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
        OnPieceSelected();
        //CheckInPuzzleBoard();
    }

    private void OnMouseDrag()
    {
        //Debug.Log("OnMouseDrag");
        OnPieceDrag();
    }
    private void OnMouseUp()
    {
        OnPieceUnselected();
        //SetPositionPiece();
        isMouseDown = false;
        if (isOnGridMap && canSetPosition)
        {
            SetPositionPiece();
        }
        else
        {
            transform.DOMove(oldPosition, 0.5f);
            canSetPosition = true;
        }
    }


    void OnPieceSelected()
    {
        if (transform.GetChild(0)!=null)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder++;
            transform.GetChild(0).DOScale(Vector3.one*1.2f, .1f);
        }
        oldMousePos = Input.mousePosition;
       
    }
    void OnPieceDrag()
    {
     
        Vector2 _directionMouse = Camera.main.ScreenToWorldPoint( Input.mousePosition) - Camera.main.ScreenToWorldPoint(oldMousePos);
        oldMousePos = Input.mousePosition;
        transform.position += (Vector3) _directionMouse;
    }
    void OnPieceUnselected()
    {

        if (transform.GetChild(0) != null)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder--;
            transform.GetChild(0).DOScale(Vector3.one, .1f);
        }
    }
    void SetPositionPiece()
    {
        transform.DOMove(new Vector3(Mathf.RoundToInt(transform.position.x-0.5f)+0.5f, Mathf.RoundToInt(transform.position.y), transform.position.z),0.5f);
    }

    void CheckInPuzzleBoard()
    {
        RaycastHit2D _hit=Physics2D.Raycast(transform.position, transform.position + Vector3.forward, 100f);
        Debug.Log(_hit.collider.name);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.tag == "Piece")
            canSetPosition = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Piece")
            canSetPosition = true;
    }


    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawLine(transform.position, transform.position + Vector3.forward*100);
    //}


}
