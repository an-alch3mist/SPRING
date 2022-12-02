using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SPACE_CURVE
{

    public static class Z
    {

        #region lerp
        public static Vector3 lerp(Vector3 a, Vector3 b, float t)
        {
            Vector3 n = b - a;
            return a + n * t;
        } 
        #endregion

        #region bezier
        public static Vector3 bezier_pos(float t , params Vector3[] P)
        {
            return 1 * (1 - t) * (1 - t) * (1 - t) * 1         * P[0] +
                   3 * (1 - t) * (1 - t)           * t         * P[1] +
                   3 * (1 - t)                     * t * t     * P[2] +
                   1 * (1)                         * t * t * t * P[3];
            //
        } 

        public static Vector3 bezier_vel(float t , params Vector3[] P)
        {
            return 3 * 1 * (1 - t) * (1 - t) * 1         * ( P[1] - P[0] ) +
                   3 * 2 * (1 - t)           * t         * ( P[2] - P[1] ) +
                   3 * 1 * (1)               * t * t     * ( P[3] - P[2] );
            //
        }

        public static Vector3 bezier_acc(float t, params Vector3[] P)
        {
            return 2 * 3 * 1 * (1 - t)  * 1  * ( P[0] + P[2] - 2 * P[1] ) +
                   2 * 3 * 1 * (1)      * t  * ( P[1] + P[3] - 2 * P[2] );
            //
        }
        #endregion


        // ease

    }


    public static class C
    {

        #region animate

        #region usage
        /*

        C.time = 30;
        for(C.i = 0; C.i < C.time; C.i += 1)
        {
            // somthng(C.t);

            yield return C.wait;
        }

        */ 
        #endregion

        public static int time = 30;
        public static int i = 0;

        #region t
        public static float t
        {
            get
            {
                if (C.i == C.time - 1) return 1f;
                return C.i * 1f / (C.time - 1);
            }
        }
        #endregion

        #region wait
        public static WaitForSeconds wait
        {
            get
            {
                /* capture_frame; */
                return null;
            }
        }
        #endregion


        #region wait_(frmes)
        public static IEnumerator wait_(int frames_length)
        {
            for (int i = 0; i < frames_length; i += 1)
            {
                /* capture_frame; */
                yield return null;
            }
        } 
        #endregion

        #endregion

    }

}
