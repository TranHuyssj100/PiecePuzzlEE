using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPiece : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "TriggerPiece")
            GetComponentInParent<Piece>().canSetPosition = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "TriggerPiece")
            GetComponentInParent<Piece>().canSetPosition = true;
    }


    public void SetPieceOnGridBoard(bool _value)
    {
        GetComponentInParent<Piece>().isOnGridBoard = _value;
    }
}
