using UnityEngine;
using DG.Tweening;


public class Piece : MonoBehaviour
{

    public bool isOnGridMap=false;
    public bool isMouseDown = false;
    public bool canSetPosition=true;
    public float startScale = .3f; 
    public float selectedScale = 1.2f;
    public Vector2 sizeSprite;
    
    Vector3 oldPosition;
    Vector3 oldMousePos;
    Vector2 limitPosX= new Vector2(-3,1);
    Vector2 limitPosY= new Vector2(-2,3);
    
    private void OnEnable()
    {
    }
    private void Start()
    {
        canSetPosition = true;
        oldPosition = transform.position;
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one * startScale,0.2f);
        limitPosX += new Vector2(0, -1 * (sizeSprite.x - 1));
        limitPosY -= new Vector2(0, (sizeSprite.y));
    }


    private void Update()
    {
    }

    private void OnMouseDown()
    {
   
        isMouseDown = true;
        OnPieceSelected();
    }

    private void OnMouseDrag()
    {
        //Debug.Log("OnMouseDrag");
        OnPieceDrag();
    }
    private void OnMouseUp()
    {
        OnPieceUnselected();
        isMouseDown = false;
        if (isOnGridMap && canSetPosition)
        {
            LevelController.indexSpawn--;
            SetPositionPiece();
        }
        else
        {
            transform.DOMove(oldPosition, 0.5f);
            canSetPosition = true;
            transform.DOScale(Vector3.one * startScale, .2f);
        }
    }


    void OnPieceSelected()
    {
        transform.DOScale(Vector3.one, .1f);
        if (transform.GetChild(0)!=null)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder++;
            transform.GetChild(0).DOScale(Vector3.one* selectedScale, .2f);
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
        //transform.DOMove(new Vector3(Mathf.RoundToInt(transform.position.x),
        transform.DOMove(new Vector3(Mathf.Clamp(Mathf.RoundToInt(transform.position.x), limitPosX.x, limitPosX.y),
                                     Mathf.Clamp(Mathf.RoundToInt(transform.position.y), limitPosY.x, limitPosY.y),
                                     //(Mathf.RoundToInt(transform.position.y)),
                                     transform.position.z),0.5f);
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



