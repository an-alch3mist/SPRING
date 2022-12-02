using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SPACE_CURVE
{



    // -BEZIER //
    public class BEZIER
    {

        // count == 3 * i + 1 
        public Vector2[] P; // in
        public float[][] LUT;


        #region pos
        public Vector2 pos(float t)
        {
            if (t <= 0f) { return P[0]; }
            if (t >= 1f) { return P[P.Length - 1]; }

            #region i_F , i_I , dt
            int N = (P.Length - 1) / 3;

            float i_F = N * t;
            int i_I = (int)i_F;
            float dt = i_F - (int)i_F;


            return Z.bezier_pos(
                             dt,
                             P[i_I * 3 + 0],
                             P[i_I * 3 + 1],
                             P[i_I * 3 + 2],
                             P[i_I * 3 + 3]
            );
            #endregion

        }
        #endregion



    }




}
