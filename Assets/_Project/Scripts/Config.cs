using UnityEngine;

public class Config 
{   
   public static Vector3 POSITION_5x5 = new Vector3(-0.5f, -0.5f, 6);
   public static Vector3 POSITION_6x6= new Vector3(0f, -1.2f, 7);
   public static Vector3 POSITION_7x7= new Vector3(0.5f, -2f, 8);
   public static Vector3 POSITION_8x8= new Vector3(1f, -2.8f, 9);
   public static Vector3 POSITION_9x9= new Vector3(1.5f, -3.5f, 10);
   public static Vector3 POSITION_10x10= new Vector3(2f, 4.2f, 11);

   public static Vector2 LIMIT_POS_X_5X5 = new Vector2(-3,1);
   public static Vector2 LIMIT_POS_X_6X6 = new Vector2(-3,2);

   public static Vector2 LIMIT_POS_Y_5X5 = new Vector2(-2, 3);
    public static Vector2 LIMIT_POS_Y_6X6 = new Vector2(-3, 3);


    public static int GOLD_WIN = 0;
    public static int COST_HINT = 100;
    public static int COST_PREVIEW = 100;

    public static float PIECE_START_SCALE = 0.4f;
    public static float PIECE_SELECTED_SCALE = 1f;
}
