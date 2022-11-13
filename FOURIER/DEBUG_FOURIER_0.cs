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

    public Vector2[] Cn_1D;
    public float[] mag_1D;

    [Range(1, 200)]
    public int num_of_arrows = 1;


    public Transform T_P;

    public Camera cam_rt;

    IEnumerator STIMULATE()
    {

        #region frame_rate
        QualitySettings.vSyncCount = 2;
        yield return null;
        yield return null;

        #endregion



        #region INITIALIZE_P
        //
        List<Vector2> P = new List<Vector2>();

        for (int i = 0; i < transform.childCount; i += 1)
        {
            P.Add(transform.GetChild(i).position);
        }
        #endregion

        FOURIER.P = P.ToArray();
        DRAW.dt = 5f;
        FOURIER.DRAW.path_in__bezier_1D(20 * (P.Count - 1) / 3);

        FOURIER.num_of_arrows = this.num_of_arrows;
        FOURIER.INITAIZLIZE__Cn_1D__fn_1D();
        yield return null;



        while (true)
        {

            T_P.position = FOURIER.draw_arrows(this.t);
            cam_rt.transform.position = FOURIER.Z.lerp(transform.position, (Vector3)FOURIER.draw_arrows(this.t) - Vector3.forward * 0.5f, 0.9f);


            this.t = (this.t + 0.05f * Time.deltaTime) % 1f;
            //
            yield return null;
        }
        

        yield return null;
    }



    #region FOURIER
    public static class FOURIER
    {

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

        // Cn(n)
        public static Vector2 Cn(int n)
        {
            int steps = 40000;

            Vector2 sum = Vector2.zero;
            float dt = 1f / steps;
            // 
            for (int i = 0; i < steps; i += 1)
            {
                float t = i * dt;
                Vector2 p = point_in__bezier_1D(t);

                //rotate....p  by -2 * pi * t * n
                Vector2 nX = p,
                        nY = new Vector2(-nX.y, nX.x);
                float angle = -n * 2 * Mathf.PI * t;
                Vector2 new_p = nX * Mathf.Cos(angle) + nY * Mathf.Sin(angle);
                 

                sum += new_p * dt;

            }
            //
            // if(n != 0) { return sum * 1f / (n * steps); }

            return sum;
        }



        public static List<Vector2> Cn_1D;
        public static List<int> freq_1D;

        public static int num_of_arrows = 1;

        public static void INITAIZLIZE__Cn_1D__fn_1D()
        {
            Cn_1D = new List<Vector2>();
            freq_1D = new List<int>();

            Cn_1D.Add(Cn(0));
            freq_1D.Add(0);
            //
            for(int i = 1; i <= num_of_arrows; i += 1)
            {
                Cn_1D.Add(Cn(+i));
                freq_1D.Add(+i);

                Cn_1D.Add(Cn(-i));
                freq_1D.Add(-i);
            }
            //

        }


        public static Vector2 draw_arrows(float t)
        {

            Vector2 sum = Vector2.zero;
            //
            for (int i = 0; i < Cn_1D.Count; i += 1)
            {
                Vector2 nX = Cn_1D[i],
                        nY = new Vector2(-nX.y, nX.x);

                float a = freq_1D[i] * 2 * Mathf.PI * t;
                Vector2 v = nX * Mathf.Cos(a) + nY * Mathf.Sin(a);



                Color[] col_1D = new Color[2]
                {
                    Color.HSVToRGB(0f , 0.5f , 0.8f),
                    Color.HSVToRGB(0.6f , 0.6f , 0.8f),
                };

                DRAW.col = col_1D[i % col_1D.Length];
                if (i != 0)
                {
                    //
                    DRAW.line(sum, sum + v * 0.95f, 1f / 200);
                }


                col_1D = new Color[2]
                {
                    Color.HSVToRGB(0f , 0.5f , 0.8f),
                    Color.HSVToRGB(0.6f , 0.0f , 0.8f),
                };


                if (i != 0 && i < 20 && i % 2 != 0)
                {
                    DRAW.dt = Time.deltaTime;
                    DRAW.col = DRAW.col = col_1D[i % col_1D.Length];
                    DRAW.cirlce(sum, Cn_1D[i].magnitude, (int)Z.lerp(32, 3, i * 1f / Cn_1D.Count));
                }
                

                sum += v;
            }
            //

            return sum;
        }






        // TOOL
        #region Z
        public static class Z
        {
            //
            #region lerp
            public static Vector3 lerp(Vector3 a, Vector3 b, float t)
            {
                Vector3 n = b - a;
                return a + n * t;
            }

            public static float lerp(float a, float b, float t)
            {
                float n = b - a;
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

            public static void path_in__bezier_1D(int N = 10)
            {
                for (int i = 0; i <= N - 1; i += 1)
                {
                    //
                    Debug.DrawLine(
                        FOURIER.point_in__bezier_1D(i * 1f / N),
                        FOURIER.point_in__bezier_1D((i + 1) * 1f / N),
                        DRAW.col,
                        DRAW.dt
                    );
                    //
                }
                //
            }



            public static void line(Vector2 a, Vector2 b, float e = 1f / 1000)
            {

                Vector2 nX = b - a,
                        nY = new Vector2(-nX.y, nX.x).normalized;


                Debug.DrawLine(a - nY * e, b - nY * e, DRAW.col, DRAW.dt);
                Debug.DrawLine(a + nY * e, b + nY * e, DRAW.col, DRAW.dt);

                //
            }

            public static void cirlce(Vector2 o , float r , int N = 16)
            {

                //
                for (int i = 0; i < N; i += 1)
                {
                    float a_0 = 2 * Mathf.PI * i * 1f / N;
                    float a_1 = 2 * Mathf.PI * (i + 1) * 1f / N;

                    line(
                        o + new Vector2() { x = Mathf.Cos(a_0), y = Mathf.Sin(a_0) } * r,
                        o + new Vector2() { x = Mathf.Cos(a_1), y = Mathf.Sin(a_1) } * r, 1f/200
                    );

                }
                //
            }


        } 
        #endregion

    }
    //
    #endregion






}
