using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DRAW
{

    public static float dt;
    public static Color col;

    #region LINE
    public static void LINE(Vector2 a, Vector2 b)
    {
        //
        Debug.DrawLine(a, b, col, dt);

    }
    // 
    #endregion



}
