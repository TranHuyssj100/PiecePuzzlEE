using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPiece : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "TriggerPiece")
        {
            GetComponentInParent<Piece>().canSetPosition = false;
            GetComponentInParent<Piece>().isTriggerOtherPiece=true;
        }
        if (collision.tag == "PreSpace")
            GetComponentInParent<Piece>().isOnPreSpace = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "TriggerPiece")
        {
            GetComponentInParent<Piece>().canSetPosition = true;
            GetComponentInParent<Piece>().isTriggerOtherPiece = false;
        }
        if (collision.tag == "PreSpace")
            GetComponentInParent<Piece>().isOnPreSpace = false;
    }


    public void SetPieceOnGridBoard(bool _value)
    {
        GetComponentInParent<Piece>().isOnGridBoard = _value;
    }
}
