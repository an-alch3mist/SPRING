using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_GIZMOZ_BEZIER_0 : MonoBehaviour
{


    public bool _gizmos = true;
    private void OnDrawGizmos()
    {

        if (!_gizmos) { return; }


        P = new Vector2[transform.childCount];
        for(int i = 0; i < transform.childCount; i += 1)
        {
            P[i] = transform.GetChild(i).position;
            Gizmos.DrawWireCube(P[i], Vector3.one * 0.05f);
        }
        //
        if(P.Length < 4) { return; }




        int N = 100;

        Gizmos.color = Color.HSVToRGB(0.2f, 0.3f, 0.9f);
        for (int i = 0; i <= N - 1; i += 1)
        {
            //
            Gizmos.DrawLine(
               point_in__bezier_1D(i * 1f / N),
               point_in__bezier_1D((i + 1) * 1f / N)
            );
            //
        }
        //


        //
        Gizmos.color = Color.HSVToRGB(0.66f, 0.3f, 0.9f);
        for (int i = 0; i <= P.Length - 4; i += 3)
        {
            Gizmos.DrawLine(P[i], P[i + 1]);
            Gizmos.DrawLine(P[i + 3], P[i + 2]);
        }
        //


    }


    public static Vector2[] P;




    #region bezier
    static Vector2 bezier(Vector2[] P, float t)
    {
        //
        Vector2 l0 = Z.lerp(P[0], P[1], t),
                l1 = Z.lerp(P[1], P[2], t),
                l2 = Z.lerp(P[2], P[3], t);
        Vector2 q0 = Z.lerp(l0, l1, t),
                q1 = Z.lerp(l1, l2, t);

        return Z.lerp(q0, q1, t);
        //
    }
    #endregion
    // Point(t)
    #region point_in__bezier_1D
    public static Vector2 point_in__bezier_1D(float t)
    {

        /*
        if(t < 0.5f) { return new Vector2(-5, 0f); }
        else         { return new Vector2(+5, 0f); }
        */

        #region t <= 0  ,  t >=1f
        if (t >= 1f) { return P[P.Length - 1]; }
        if (t <= 0f) { return P[0]; }
        #endregion

        int N = (P.Length - 1) / 3;

        int I = (int)(t * N);
        float dt = t * N - I;


        return bezier(
            new Vector2[4] { P[I * 3], P[I * 3 + 1], P[I * 3 + 2], P[I * 3 + 3] },
             dt
        );
        //
    }
    #endregion



    // TOOL
    #region Z
    static class Z
    {
        //
        #region lerp
        public static Vector2 lerp(Vector2 a, Vector2 b, float t)
        {
            Vector2 n = b - a;
            return a + n * t;
        }
        #endregion
    }
    #endregion


}