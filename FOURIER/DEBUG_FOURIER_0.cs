using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_FOURIER_0 : MonoBehaviour
{

    private void Update()
    {
        
        if(Input.GetMouseButtonDown(1))
        {
            StopAllCoroutines();
            StartCoroutine(STIMULATE());
        }
        //
    }


    [Range(0f, 1f)]
    public float t = 0.1f;

    public Transform T_p;


    IEnumerator STIMULATE()
    {

        #region frame_rate
        QualitySettings.vSyncCount = 2;
        yield return null;
        yield return null;

        #endregion


        while (true)
        {
            //
            #region INITIALIZE_P
            //
            List<Vector2> P = new List<Vector2>();

            for (int i = 0; i < transform.childCount; i += 1)
            {
                //
                P.Add(transform.GetChild(i).position);
            }
            #endregion


            T_p.position = FOURIER.point_in__bezier_1D(P.ToArray(), this.t);

            FOURIER.DRAW.path_in__bezier_1D(P.ToArray(), 20 * (P.Count - 1) / 3);


            yield return null;
        }


        yield return null;
    }



    #region FOURIER
    public static class FOURIER
    {

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

        public static Vector2 point_in__bezier_1D(Vector2[] P, float t)
        {
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

        #region DRAW
        public static class DRAW
        {
            public static Color col = Color.red;
            public static float dt = Time.deltaTime;

            public static void path_in__bezier_1D(Vector2[] P, int N = 10)
            {
                for (int i = 0; i <= N - 1; i += 1)
                {
                    //
                    Debug.DrawLine(
                        FOURIER.point_in__bezier_1D(P, i * 1f / N),
                        FOURIER.point_in__bezier_1D(P, (i + 1) * 1f / N),
                        DRAW.col,
                        DRAW.dt
                    );
                    //
                }
                //
            }


        } 
        #endregion

    }
    //
    #endregion






}
