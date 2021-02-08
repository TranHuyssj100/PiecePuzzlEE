
using UnityEngine;
using UnityEngine.Tilemaps;
public class GridMap : MonoBehaviour
{
    public BoundsInt area;

        Vector3 tilePosition;
        Vector3Int coordinate = new Vector3Int(0, 0, 0);
    void Start()
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        tilemap.CompressBounds();
        area = tilemap.cellBounds;
        TileBase[] tileArray = tilemap.GetTilesBlock(area);
        for (int x = 0; x < tilemap.size.x; x++)
        {
            for (int y = 0; y < tilemap.size.y; y++)
            {
                coordinate.x = x; 
                coordinate.y = y;
                tilePosition = tilemap.CellToWorld(coordinate);
                //Debug.Log(string.Format("Position of tile [{0}, {1}] = ({2}, {3})", coordinate.x, coordinate.y, tilePosition.x, tilePosition.y));

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.GetComponent<TriggerPiece>() != null)
        {
            //Debug.Log("Piece In");
            collision.GetComponent<TriggerPiece>().SetPieceOnGridBoard(true);
        }
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
     
        if (collision.GetComponent<TriggerPiece>() != null)
        {
            //Debug.Log("Piece In");
            collision.GetComponent<TriggerPiece>().SetPieceOnGridBoard(false);
        }
    }

   
}
