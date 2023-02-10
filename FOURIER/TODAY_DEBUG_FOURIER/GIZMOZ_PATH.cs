using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GIZMOZ_PATH : MonoBehaviour
{
	[Header("gizmos")]
	public Color gizmos_col = Color.gray;
	public bool _gizmos = false;


	private void OnDrawGizmos()
	{
		if (!_gizmos) return;




		List<Vector2> P = new List<Vector2>();
		for(int i = 0; i < transform.childCount; i += 1)
		{
			P.Add(transform.GetChild(i).position);
		}


		Gizmos.color = Color.HSVToRGB(0.66f, 0.5f, 0.6f);
		for (int i = 0; i <= P.Count - 4; i += 3)
		{
			Gizmos.DrawLine(P[i], P[i + 1]);
			Gizmos.DrawLine(P[i + 3], P[i + 2]);
		}



		PATH _path = new PATH();
		_path.P = P;


		Gizmos.color = gizmos_col;
		//
		int N = 1000;
		for (int i = 1; i <= N; i += 1)
		{
			Gizmos.DrawLine(_path.pos(i * 1f / N), _path.pos((i - 1) * 1f / N));
		}


	}




    //// STUFF ////
    #region STUFF
    
    
    /*
	in -    bezier_path.P 
	out -   bezier_path.pos(t) 
	in -    bezier_path.initialize_LUT() 
	out -   get_const_spaced_points_0(N) 
	        get_const_spaced_points_1(N , N_e)         
	*/

	public class PATH
	{
		/*
		in -    L<v2> P
		out -   v2 pos(t)
		*/
		public List<Vector2> P;


		// pos //
		public Vector2 pos(float t)
		{
			if (t <= 0f) return P[0];
			if (t >= 1f) return P[P.Count - 1];


			int N = (P.Count - 1) / 3;

			float i_F = N * t;
			int i_I = (int)(N * t);

			float dt = i_F - i_I;

			return bezier_pos(
				new Vector2[4]
				{
					P[i_I * 3 + 0],
					P[i_I * 3 + 1],
					P[i_I * 3 + 2],
					P[i_I * 3 + 3],
				},
				dt
			);

		}
		// pos //




		#region TOOL
		//// bezier_pos ////
		static Vector2 bezier_pos(Vector2[] P, float t)
		{
			return  1 * 1 *                 (1 - t) * (1 - t) * (1 - t) * P[0] +
					3 * t *                 (1 - t) * (1 - t)           * P[1] +
					3 * t * t *             (1 - t)                     * P[2] +
					1 * t * t * t *         (1)                         * P[3];

		}
		//// bezier_pos ////
		#endregion






		/*
		in  - INTIALIZE_LUT()
		out - get_const_spaced_points_0(int N = 100)
		    - get_const_spaced_points_0(int N = 100 , int N_de = 400)
		.
		.
		.
		.
		*/

	}

	#endregion
	//// STUFF ////


}
