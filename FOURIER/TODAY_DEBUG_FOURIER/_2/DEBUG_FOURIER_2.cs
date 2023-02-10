using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace SPACE_DEBUG_FOURIER_2
{


	public class DEBUG_FOURIER_2 : MonoBehaviour
	{

		private void Update()
		{

			if (Input.GetMouseButtonDown(1))
			{
				//
				StopAllCoroutines();
				StartCoroutine(STIMULATE());
			}

		}





		public Vector2 pos_pivot, pos_anchor;
		public int i_pivot, i_anchor;

		IEnumerator STIMULATE()
		{
			#region frame_rate
			// frame_rate //
			QualitySettings.vSyncCount = 2;
			yield return null;
			yield return null;
			// frame_rate // 
			#endregion




			OBJ.INITIALIZE_HOLDER();
			OBJS.INITIALIZE_HOLDER();
			TEXT.INITIALIZE_HOLDER();






			TEXT txt = new TEXT("somthng");

			txt.enable(true);
			yield return C.wait_(30 * 3);


			while (true)
			{
				txt.orient(i_pivot, i_anchor, pos_pivot, pos_anchor);
				


				yield return C.wait;
			}



			yield return null;

		}

	}



	#region FOURIER
	/*
	in - fourier.bezier_path
		 fourier.INITIALIZE(Cn_count)
		 fourier.update__arrow_1D(t) 

	out - somthng with arrow_1D[i] 
	*/
	public class FOURIER
	{

		// in //
		public PATH _bezier_path;



		List<Vector2> Cn_1D;

		public void INITIALIZE(int Cn_count = 10)
		{
			// -10....0....+10 //
			Cn_1D = new List<Vector2>();


			// 0 , -1 , +1 , -2 , +2 ......... -(Cn_count - 1) , +(Cn_count - 1) // 
			Cn_1D.Add(Cn(0));
			for (int i = 1; i < Cn_count; i += 1)
			{
				Cn_1D.Add(Cn(-i));
				Cn_1D.Add(Cn(+i));
			}
		}








		/*
		out -
			L < pos , angle , scale >
		*/
		public float[][] arrow_1D;
		public void update__arrow_1D(float t)
		{
			arrow_1D = new float[Cn_1D.Count][];


			for (int i0 = 0; i0 < Cn_1D.Count; i0 += 1)
			{
				int n;

				#region find_n from i0
				if (i0 == 0) n = 0;
				else
				{
					if (i0 % 2 != 0) n = -(int)(i0 / 2 + 1);
					else n = +(int)(i0 / 2);
				}
				#endregion


				Vector2 v = mul(Cn_1D[i0], Z.polar(n * 2 * Mathf.PI * t));
				//
				arrow_1D[i0] = new float[2]
				{
						v.x , v.y
				};
				//
			}
			//

		}





		// f(t) .... to .... Cn(n) //
		Vector2 f(float t)
		{
			//
			return _bezier_path.pos(t);
		}

		Vector2 Cn(int n)
		{
			int N = 40000;


			Vector2 sum = Vector2.zero;

			// rotate f(t) by -2 * pi * t * n //
			for (int i = 0; i < N; i += 1)
			{
				float t = i * 1f / N;
				sum += mul(f(t), Z.polar(-2 * Mathf.PI * t * n));
			}
			// rotate f(t) by -2 * pi * t * n //

			return sum / N;
		}
		// f(t) .... to .... Cn(n) //



		#region Tool
		public static Vector2 mul(Vector2 a, Vector2 b)
		{
			return new Vector2()
			{
				x = a.x * b.x - a.y * b.y,
				y = a.x * b.y + a.y * b.x
			};
		}
		#endregion


	}

	#endregion


}
