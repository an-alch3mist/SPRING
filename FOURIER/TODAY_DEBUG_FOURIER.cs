//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace SPACE_FOURIER
{


	public class DEBUG_FOURIER_1 : MonoBehaviour
	{



		public Transform Tr_pos_0;
		public Transform Tr_P_0;

		[Range(0f, 1f)]
		public float t = 0f;


		private void Update()
		{
			if (Input.GetMouseButtonDown(1))
			{
				//
				StopAllCoroutines();
				StartCoroutine(STIMULATE());
			}
		}



		IEnumerator STIMULATE()
		{
			#region frame_rate
			// frame_rate //
			QualitySettings.vSyncCount = 3;
			yield return null;
			yield return null;
			// frame_rate // 
			#endregion




			#region P
			List<Vector2> P = new List<Vector2>();

			for (int i = 0; i < Tr_P_0.childCount; i += 1)
			{ P.Add(Tr_P_0.GetChild(i).position); }

			#endregion


			BEZIER_PATH _bezier_path = new BEZIER_PATH();
			_bezier_path.P = P;

			FOURIER _fourier = new FOURIER();
			_fourier._bezier_path = _bezier_path;
			_fourier.INITIALIZE(Cn_count : 4);
			



			while(true)
			{

				//
				Tr_pos_0.position = _bezier_path.pos(this.t);


				_fourier.update__arrow_1D(this.t);


				Vector2 sum = Vector2.zero;
				for(int i = 0; i < _fourier.arrow_1D.Length; i += 1)
				{
					Vector2 v = new Vector2(_fourier.arrow_1D[i][0], _fourier.arrow_1D[i][1]);
					Debug.DrawLine(
						sum,
						sum + v,
						Color.red,
						Time.deltaTime
					);
					sum += v;
				}
				//

				yield return null;
			}
			

			yield return null;
		}





		/*
		usage -
			initalize fourier.bezier_path = bezier_path
			fourier.INITIALIZE(Cn_count)

			fourier.update__arrow_1D(t)
			do somthng with arrow_1D[i] 
		*/
		public class FOURIER
		{

			// in //
			public BEZIER_PATH _bezier_path;
			
			
			
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

				string str = "";

				for(int i0 = 0; i0 < Cn_1D.Count; i0 += 1)
				{
					int n;

					if (i0 == 0) n = 0;
					else
					{
						if (i0 % 2 != 0)	n = -(int)(i0 / 2 + 1);
						else				n = +(int)(i0 / 2);
					}


					str += n.ToString() + " , ";

					Vector2 v = mul(Cn_1D[i0], polar(n * 2 * Mathf.PI * t));
					//
					arrow_1D[i0] = new float[2]
					{
						v.x , v.y
					};
					//
				}

				LOG.str(str);
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
				int N = 10000;


				Vector2 sum = Vector2.zero;

				// rotate f(t) by -2 * pi * t * n //
				for (int i = 0; i < N; i += 1)
				{
					float t = i * 1f / N;
					sum += mul(f(t), polar(-2 * Mathf.PI * t * n));
				}
				// rotate f(t) by -2 * pi * t * n //

				return sum / N;
			}
			// f(t) .... to .... Cn(n) //



			#region Tool
			// Tool //
			public static Vector2 mul(Vector2 a, Vector2 b)
			{
				return new Vector2()
				{
					x = a.x * b.x - a.y * b.y,
					y = a.x * b.y + a.y * b.x
				};
			}


			public static Vector2 polar(float angle)
			{
				return new Vector2()
				{
					x = Mathf.Cos(angle),
					y = Mathf.Sin(angle)
				};
			}
			// Tool // 
			#endregion


		}






		/*
		usage -
			initalize bezier_path.P = pos_1D 
			do somthng with v2 bezier_path.pos(t) 

			intialize bezier_path.initialize_LUT() 
			do somthng with L<v2> get_const_spaced_points_0(N) 
		*/

		public class BEZIER_PATH
		{
			/*
			in -
				L<v2>
			*/
			public List<Vector2> P;


			/* 
			out -
				v2 pos(t)
			*/

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




			// tool //
			//// bezier_pos ////
			static Vector2 bezier_pos(Vector2[] P, float t)
			{
				return 1 * 1 * (1 - t) * (1 - t) * (1 - t) * P[0] +
						3 * t * (1 - t) * (1 - t) * P[1] +
						3 * t * t * (1 - t) * P[2] +
						1 * t * t * t * (1) * P[3];

			}
			//// bezier_pos ////
			// tool //






			/*
			intialize .... LUT of count
			out -
				L<v2> get_points_between(t_start , t_end , N_full)
			*/

			/* 
			t - dist const_dt 
			0 : t
			1 : dist
			*/
			float[][] LUT;
			public void INTIALIZE_LUT()
			{
				int N = 10000;
				LUT = new float[N][];

				float sum = 0f;
				LUT[0] = new float[2] { 0f, sum };
				//
				for (int i = 0; i <= N; i += 1)
				{
					float t_prev = (i - 1) * 1f / N;
					float t_curr = i * 1f / N;

					sum += mag(pos(t_curr - t_prev));
					LUT[i] = new float[2] { t_curr, sum };
				}
				//
			}


			/*
			P.Add( path_length * i * 1f / N )
			*/
			public List<Vector2> get_const_spaced_points_0(int N = 100)
			{
				float path_length = get_dist(LUT, 1f);

				List<Vector2> P = new List<Vector2>();
				//
				for (int i = 0; i <= N; i += 1)
				{
					float dist = i * path_length * 1f / N;
					P.Add(pos(get_t(LUT, dist)));
				}
				//
				return P;
			}


			/*
			P.Add( path_length * i * 1f / N - de)
			P.Add( path_length * i * 1f / N + de)
			*/
			public List<Vector2> get_const_spaced_points_1(int N = 100, int N_de = 400)
			{
				float path_length = get_dist(LUT, 1f);
				float dist_e = path_length * 1f / N_de;

				List<Vector2> P = new List<Vector2>();
				//

				P.Add(pos(0f));
				P.Add(pos(get_t(LUT, 0f + dist_e)));
				//
				for (int i = 1; i < N; i += 1)
				{
					float dist = i * path_length * 1f / N;
					P.Add(pos(get_t(LUT, dist - dist_e)));
					P.Add(pos(get_t(LUT, dist + dist_e)));
					//
				}
				//
				P.Add(pos(get_t(LUT, path_length - dist_e)));
				P.Add(pos(1f));
				//
				return P;
			}





			#region Tool
			// tool //
			static float mag(Vector2 v) { return Mathf.Sqrt(v.x * v.x + v.y * v.y); }

			static float inv_lerp(float a, float b, float v)
			{
				return (v - a) / (b - a);
			}

			static float lerp(float a, float b, float t)
			{
				float n = b - a;
				return a + n * t;
			}

			// tool // 
			#endregion

			#region ad
			// ad //
			static float get_dist(float[][] LUT, float t)
			{
				if (t <= 0f) return 0f;
				if (t >= 1f) return LUT[LUT.Length - 1][1];

				int i = 1;
				for (; i <= LUT.Length - 1; i += 1)
					if (LUT[i][0] > t)
						break;

				float dt = inv_lerp(LUT[i - 1][0], LUT[i][0], t);

				return lerp(LUT[i - 1][1], LUT[i][1], dt);
			}

			static float get_t(float[][] LUT, float dist)
			{
				if (dist <= 0f) return 0f;
				if (dist >= LUT[LUT.Length - 1][1]) return 1f;

				int i = 1;
				for (; i <= LUT.Length - 1; i += 1)
					if (LUT[i][1] > dist)
						break;

				float d_dist = inv_lerp(LUT[i - 1][1], LUT[i][1], dist);

				return lerp(LUT[i - 1][0], LUT[i][0], d_dist);
			}
			// ad // 
			#endregion

		}





	}


}
