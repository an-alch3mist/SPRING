//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace SPACE_FOURIER_1
{


	public class DEBUG_FOURIER_1 : MonoBehaviour
	{

		public Camera cam;


		public Transform Tr_pos_0;

		public Transform Tr_P_0;
		public Transform Tr_P_1;
		public Transform Tr_P_2;

		[Range(0f, 1f)]
		public float t = 0f;

		public Color a, b;



		[Header("sprite")]
		public SpriteRenderer sr;




		private void Update()
		{
			if (Input.GetMouseButtonDown(1))
			{
				//
				StopAllCoroutines();
				StartCoroutine(STIMULATE());
			}
		}



		#region cam
		List<Vector2> P_2;
		List<Vector2> S_2;


		void cam_POS(Vector2 p)
		{
			cam.transform.position = new Vector3(p.x, p.y, -10f);
		}

		void cam_SCALE(float s)
		{
			cam.orthographicSize = s;
		}


		IEnumerator moveTo(int index)
		{
			Vector2 from = cam.transform.position;
			float from_s = cam.orthographicSize;

			Vector2 to = P_2[index];
			float to_s = S_2[index].y / 2;


			for (C.i = 0; C.i < C.frames; C.i += 1)
			{
				cam_POS(Z.lerp(from, to, C.t));
				cam_SCALE(Z.lerp(from_s, to_s, C.t));

				yield return C.wait;
			}
		}
		#endregion


		IEnumerator STIMULATE()
		{
			#region frame_rate
			// frame_rate //
			QualitySettings.vSyncCount = 2;
			yield return null;
			yield return null;
			// frame_rate // 
			#endregion



			#region P
			List<Vector2> P_0 = new List<Vector2>();

			for (int i = 0; i < Tr_P_0.childCount; i += 1)
			{ P_0.Add(Tr_P_0.GetChild(i).position); }

			List<Vector2> P_1 = new List<Vector2>();

			for (int i = 0; i < Tr_P_1.childCount; i += 1)
			{ P_1.Add(Tr_P_1.GetChild(i).position); }


			P_2 = new List<Vector2>();

			for (int i = 0; i < Tr_P_2.childCount; i += 1)
			{ P_2.Add(Tr_P_2.GetChild(i).position); }


			S_2 = new List<Vector2>();

			for (int i = 0; i < Tr_P_2.childCount; i += 1)
			{ S_2.Add(Tr_P_2.GetChild(i).localScale); }

			#endregion


			//Debug.Log(Resources.Load<Sprite>("FOURIER_1/FOURIER_1Sprite"));
			// sr.sprite = Resources.Load<Sprite>("FOURIER_1");



			PATH _path = new PATH();
			_path.P = P_0;
			_path.INTIALIZE_LUT();

			FOURIER _fourier = new FOURIER();
			_fourier._bezier_path = _path;
			_fourier.INITIALIZE(Cn_count : 4);








			OBJ.INITIALIZE_HOLDER();
			OBJS.INITIALIZE_HOLDER();
			//
			OBJ obj_path = new OBJ("obj_path", 0);
			OBJ obj_disk = new OBJ("obj_disk", 1);
			OBJS objs_arrow = new OBJS("objs_arrow", 3);




			EASE._TYPE = EASE.TYPE._smooth;
			yield return moveTo(1);
			



			#region animate disc initialize
			EASE._TYPE = EASE.TYPE._smooth;

			C.frames = 10;
			for (C.i = 0; C.i < C.frames; C.i += 1)
			{
				obj_disk.mesh(MESH.mesh_disc(0.05f, 0.01f, N: 32, C.t));
				obj_disk.tex2D(TEX2D.grad(a, b, 1));

				yield return null;
			}
			// 
			#endregion


			#region anim arrow scale
			objs_arrow.mesh("FOURIER_1");

			EASE._TYPE = EASE.TYPE._smooth;
			C.frames = 10;
			for (C.i = 0; C.i < C.frames; C.i += 1)
			{
				objs_arrow.SCALE(Z.lerp(0f, 0.5f, C.t), Z.lerp(0f, 0.5f, C.t));
				yield return null;
			}

			#endregion

			#region animate arrow rotate
			C.frames = 20;
			for (C.i = 0; C.i < C.frames; C.i += 1)
			{
				objs_arrow.ROT(Z.lerp(0f, C.PI * 0.75f, C.t));
				yield return null;
			}

			#endregion



			EASE._TYPE = EASE.TYPE._smooth;
			yield return moveTo(0);



			//
			yield return C.wait_(3 * 30);



			#region draw path
			C.frames = 70;
			EASE._TYPE = EASE.TYPE._inQuad;


			for (C.i = 0; C.i < C.frames; C.i += 1)
			{
				_path.P = P_0;
				obj_path.mesh(MESH.mesh_dotted_path(_path.get_const_spaced_points_1t(100, 400, C.t), 1f / 50));


				obj_disk.POS(_path.pos(C.t));
				cam_POS(_path.pos(C.t));

				yield return null;
			} 
			#endregion


			#region lerp between paths
			C.frames = 30;
			EASE._TYPE = EASE.TYPE._smooth;

			for (C.i = 0; C.i < C.frames; C.i += 1)
			{
				//
				List<Vector2> P_0_to_1 = new List<Vector2>();

				for (int i = 0; i < P_0.Count; i += 1)
				{
					P_0_to_1.Add(Z.lerp(P_0[i], P_1[i], C.t));
				}

				_path.P = P_0_to_1;
				_path.INTIALIZE_LUT();

				obj_path.mesh(MESH.mesh_dotted_path(_path.get_const_spaced_points_1t(110, 450, 1f), 1f / 50));


				obj_disk.POS(_path.pos(1f));
				cam_POS(_path.pos(1f));


				//
				yield return C.wait;
			} 
			#endregion




			//
			yield return C.wait_(30);





			C.frames = 30;
			for (C.i = 0; C.i < C.frames; C.i += 1)
			{
				float t = Z.lerp(1f, this.t, C.t);
				obj_path.mesh(MESH.mesh_dotted_path(_path.get_const_spaced_points_1t(110, 450, t)));

				obj_disk.POS(_path.pos(t));
				cam_POS(_path.pos(t));
				//
				yield return null;
			}





			while (true)
			{

				//obj_path.mesh(MESH.mesh_path(_path.get_const_spaced_points_0t(10, t), 1f / 50));
				obj_path.mesh(MESH.mesh_dotted_path(_path.get_const_spaced_points_1t(110 , 450 , this.t), 1f / 50));


				//
				Vector2 p = _path.pos(this.t);
				obj_disk.POS(p);
				cam_POS(p);



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

					Vector2 v = mul(Cn_1D[i0], Z.polar(n * 2 * Mathf.PI * t));
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
		*/

		/* 
		0 : t
		1 : dist
		*/
		float[][] LUT;
		public void INTIALIZE_LUT()
		{
			int N = 10000;
			LUT = new float[N + 1][];

			float sum = 0f;
			LUT[0] = new float[2] { 0f, sum };
			//
			for (int i = 1; i <= N; i += 1)
			{
				float t_prev = (i - 1) * 1f / N;
				float t_curr = i * 1f / N;


				sum += Z.mag(pos(t_curr) - pos(t_prev));
				LUT[i] = new float[2] { t_curr, sum };
			}
			//
			Debug.Log(LUT[LUT.Length - 2][1]);
			Debug.Log(LUT[LUT.Length - 1][1]);
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


		public List<Vector2> get_const_spaced_points_0t(int N = 100 , float t = 0f)
		{
			float path_length = get_dist(LUT, 1f);


			List<Vector2> P = new List<Vector2>();


			if (t >= 1f) t = 1f - C.de;
			if (t <= 0f) t = 0f + C.de;

			float i_F = get_dist(LUT, t) / path_length * N;
			int   i_I = (int)i_F;
			

			//
			for (int i = 0; i <= i_I; i += 1)
			{
				float dist = i * path_length * 1f / N;
				P.Add(pos(get_t(LUT, dist)));
			}
			
			if(i_F - i_I > C.de)
			{
				P.Add(pos(t));
			}
			//
			return P;
		}




		/*
		P.Add( path_length * i * 1f / N - de)
		P.Add( path_length * i * 1f / N + de)
		*/
		public List<Vector2> get_const_spaced_points_1(int N = 100 , int N_de = 400)
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

		public List<Vector2> get_const_spaced_points_1t(int N = 100, int N_de = 400 , float t = 0f)
		{
			float path_length = get_dist(LUT, 1f);
			float dist_e = path_length * 1f / N_de;

			List<Vector2> P = new List<Vector2>();
			//

			if (t >= 1f) t = 1f - C.de;
			if (t <= 0f) t = 0f + C.de;  



			P.Add(pos(0f));
			P.Add(pos(get_t(LUT, 0f + dist_e)));
			//
			for (int i = 1; i < N; i += 1)
			{
				float dist = i * path_length * 1f / N;
				float t_prev = get_t(LUT, dist - dist_e),
					  t_next = get_t(LUT, dist + dist_e);

				if(t > t_next)
				{
					P.Add(pos(t_prev));
					P.Add(pos(t_next));
				}
				else if (t >= t_prev && t <= t_next)
				{
					P.Add(pos(t_prev));
					if (t - t_prev >= C.de) P.Add(pos(t));
				}
				else if(t > t_next)
				{
					break;
				}
				//
			}
			//
			return P;
		}




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

			float dt = Z.inv_lerp( LUT[i - 1][0], LUT[i][0], t );

			return Z.lerp(LUT[i - 1][1], LUT[i][1], dt);
		}

		static float get_t(float[][] LUT, float dist)
		{
			if (dist <= 0f)                     return 0f;
			if (dist >= LUT[LUT.Length - 1][1]) return 1f;

			int i = 1;
			for (; i <= LUT.Length - 1; i += 1)
				if (LUT[i][1] > dist)
					break;

			float dt = Z.inv_lerp(LUT[i - 1][1], LUT[i][1], dist);

			return Z.lerp(LUT[i - 1][0], LUT[i][0], dt);
		}
		// ad // 
		#endregion

	}

	#endregion
	//// STUFF ////




	//// TOOL ////
	#region TOOL

	#region Z
	public static class Z
	{
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

		#region dot
		public static float dot(Vector3 a, Vector3 b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}
		#endregion

		#region area
		public static float area(Vector2 a , Vector2 b)
		{
			return a.x * b.y - a.y * b.x;
		}
		#endregion


		#region angle
		public static float angle(Vector2 n0  , Vector2 n1)
		{
			float X = Z.dot(n0, n1);
			float Y = Z.area(n0, n1);

			float a = Mathf.Atan2(Y, X);
			if (a < 0f)
				a += 2 * C.PI;

			return a;
		}
		#endregion

		#region polar
		public static Vector2 polar(float angle)
		{
			return new Vector2()
			{
				x = Mathf.Cos(angle),
				y = Mathf.Sin(angle)
			};
		}
		#endregion



		#region inv_lerp
		public static float inv_lerp(float a, float b, float v)
		{
			return (v - a) / (b - a);
		}
		#endregion


		#region mag
		public static float mag(Vector2 v)
		{
			return Mathf.Sqrt(v.x * v.x + v.y * v.y);
		}
		#endregion

	} 
	#endregion

	#region C
	// TODO C.t return using EASE._TYPE //
	public static class C
	{
		public static float PI = Mathf.PI;
		public static Vector2 zero = new Vector2(0, 0),
							  r = new Vector2(+1, 0),
							  l = new Vector2(-1, 0),
							  u = new Vector2(0, +1),
							  d = new Vector2(0, -1);

		public static float de = 1f / 1000000;

		#region round(x) , sign(x)

		public static int round(float x)
		{
			float frac = x - (int)x;
			if (frac >= 0)
				if (frac >= 0.5f)
					return (int)x + 1;
				else
					return (int)x;
			else
				if (frac <= 0.5f)
				return (int)x - 1;
			else
				return (int)x;


		}
		public static int sign(float x)
		{
			if (Mathf.Abs(x) < 1f / 1000000)
				return 0;

			if (x > 0) return +1;
			if (x < 0) return -1;

			return 0;
		}



		#endregion


		/*
        animate -
            C.frames = 30
            for(C.i = 0 ; C.i < C.frames ; C.i += 1)
            {
                // do somthng with C.t;       
            }
        */

		public static int frames = 30;
		public static int i = 0;
		public static float t { get { return EASE.f(C.i * 1f / (C.frames - 1)); } }



		public static IEnumerator wait
		{
			get { return null; }
		}
		public static IEnumerator wait_(int frames)
		{
			for(int i = 0; i < frames; i += 1)
			{
				yield return null;
			}
		}




	} 
	#endregion

	#region EASE
	public class EASE
	{
		public enum TYPE
		{
			_lerp,
			_smooth,

			_inQuad,
			_outQuad,

			_inoutBack,

		}


		public static TYPE _TYPE;
		public static float f(float t)
		{
			if (t >= 1f) return 1f;
			if (t <= 0f) return 0f;


			if (_TYPE == TYPE._lerp)
			{
				return t;
			}
			if (_TYPE == TYPE._smooth)
			{
				float Y = Mathf.Cos(t * C.PI);
				return (-Y + 1f) / 2f;
			}

			if (_TYPE == TYPE._inQuad)
			{
				return t * t * t * t;
			}
			if (_TYPE == TYPE._outQuad)
			{
				return 1 - (1 - t) * (1 - t) * (1 - t) * (1 - t);
			}
			if (_TYPE == TYPE._inoutBack)
			{
				float c1 = 1.70158f;
				float c2 = c1 * 1.525f;

				return (t < 0.5) ? (Mathf.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2)) / 2
								 : (Mathf.Pow(2 * t - 2, 2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2) / 2;
			}


			return -1;
		}



	}
	#endregion


	#endregion
	//// TOOL ////



	//// RENDER ////
	#region RENDER


	// OBJ //
	#region OBJ

	#region OBJ
	
	/*
	OBJ.INITIALIZE_HOLDER
	OBJ obj = new OBJ(string name , int layer = 0);
		obj.POS(v2)
		obj.ROT(float)
	*/


	public class OBJ
	{
		#region HOLDER

		public static Transform holder;
		public static void INITIALIZE_HOLDER()
		{
			if (GameObject.Find("holder") != null)
				GameObject.Destroy(GameObject.Find("holder"));

			holder = new GameObject("holder").transform;
		}

		#endregion


		MeshFilter mf;
		MeshRenderer mr;
		GameObject G;

		public OBJ(string name, int layer = 0)
		{
			G = new GameObject(name);
			mf = G.AddComponent<MeshFilter>();
			mr = G.AddComponent<MeshRenderer>();
			mr.sharedMaterial = new Material(Shader.Find("Unlit/Transparent"));

			G.transform.parent = holder;
			G.transform.position = new Vector3(0f, 0f, -layer * 1f / 10);
		}


		#region CONTROLS
		public void enable(bool need_to_enable) { G.SetActive(need_to_enable); }

		public void POS(Vector2 pos) { G.transform.position = new Vector3(pos.x, pos.y, G.transform.position.z); }

		public void ROT(float angle) { G.transform.localEulerAngles = new Vector3(0f, 0f, angle / C.PI * 180); }

		public void SCALE(float x, float y) { G.transform.localScale = new Vector3(x, y, 1f); }

		public void align(Vector2 a, Vector2 b, bool scale = false)
		{

			POS(a);
			ROT(Z.angle(C.r, b - a));
			//
			if (scale)
			{
				float mag = Z.mag(b - a);
				SCALE(mag, mag);
			}
		}   //



		public void mesh(Mesh mesh) { mf.sharedMesh = mesh; }

		public void tex2D(Texture2D tex2D) { mr.sharedMaterial.mainTexture = tex2D; }

		/*
        TODO -
            col
            grad
            texture
        */
		public void col(Color col)
		{
			Texture2D tex2D = new Texture2D(1, 1);
			tex2D.SetPixel(0, 0, col);
			tex2D.filterMode = FilterMode.Point;
			tex2D.Apply();

			mr.sharedMaterial.mainTexture = tex2D;
		}
		#endregion


	}

	#endregion

	#region OBJS
	public class OBJS
	{
		#region HOLDER

		public static Transform holder;
		public static void INITIALIZE_HOLDER()
		{
			if (GameObject.Find("holderS") != null)
				GameObject.Destroy(GameObject.Find("holderS"));

			holder = new GameObject("holderS").transform;
		}

		#endregion


		SpriteRenderer sr;
		GameObject G;

		public OBJS(string name, int layer = 0)
		{
			G = new GameObject(name);
			sr = G.AddComponent<SpriteRenderer>();

			G.transform.parent = OBJS.holder;
			G.transform.position = new Vector3(0f, 0f, -layer * 1f / 10);
		}


		#region CONTROLS
		public void enable(bool need_to_enable) { G.SetActive(need_to_enable); }

		public void POS(Vector2 pos) { G.transform.position = new Vector3(pos.x, pos.y, G.transform.position.z); }

		public void ROT(float angle) { G.transform.localEulerAngles = new Vector3(0f, 0f, angle / C.PI * 180); }

		public void SCALE(float x, float y) { G.transform.localScale = new Vector3(x, y, 1f); }

		public void align(Vector2 a, Vector2 b, bool scale = false)
		{
			POS(a);
			ROT(Z.angle(C.r, b - a));

			//
			if (scale)
			{
				float mag = Z.mag(b - a);
				SCALE(mag, mag);
			}
		}   //


		public void mesh(string locate) { sr.sprite = Resources.Load<Sprite>(locate); }



		public void alpha(float alpha) { sr.color = new Color(0f, 0f, 0f, alpha); }
		#endregion


	} 
	#endregion


	#region TEXT
	/*
	TEXT.INITIALIZE_HOLDER
	TEXT txt = new TEXT(string str, int layer = 0);
		txt.orient(int i_pivot ,int i_anchor ,V2 pos_pivot , V2 pos_anchor )
		txt.enable(true);
		yield return txt.write();
		yield return txt.typeWrite_from_rand_characters();

		// TO FIND A WAY TO //
		each characters with its own TMPro.tm component
			control animating of each characters
		// TO FIND A WAY TO //


	*/
	public class TEXT
	{

	}
	#endregion


	
	#region CAM
	/*

	CAM cam = new CAM(MainCamera);
	cam.orient(int i_pivot ,int i_anchor ,V2 pos_pivot , V2 pos_anchor )
	// TO FIND A WAY TO //
		smooth camera motion ....  by laging behind a certain amount 
		switching between .... persp - ortho
	// TO FIND A WAY TO //

	*/
	public class CAM
	{


	} 
	#endregion


	#endregion
	// OBJ //



	// MESH //
	#region MESH
	/*
    process points .... t
    generate mesh
    */

	public static class MESH
    {

		/*
		TODO -
			PATH ( t ) .... with variable specified stroke
			DOTTED_PATH ( t )
		*/


		public static Mesh mesh_path_t(List<Vector2> old_P , float e = 1f/50 , float t = 0f)
		{
		    
		    List<Vector2> P = new List<Vector2>();
		    
		    #region process old_P .... to .... P
		    //
		    if(t >= 1f) t = 1f - C.de;
		    if(t <= 0f) return new Mesh(); 
		    
            int old_N = old_P.Count - 1;

			float i_F = old_N * t;
			int   i_I = (int)i_F;
		    
		    
		    for(int i = 0 ; i <= i_I ; i += 1)
		    {
		        P.Add(old_P[i]);
		    }
			
		    if((i_F - i_I) > C.de )
		    {
		        float dt = i_F - i_I;
				P.Add(Z.lerp(old_P[i_I], old_P[i_I + 1], dt));
		    }
			
		    //
            #endregion
		    
		    
			List<Vector3> verts = new List<Vector3>();
			List<int> tris = new List<int>();
			List<Vector2> uvs = new List<Vector2>();


			int N = P.Count - 1;

			Vector2[] V = new Vector2[N + 1];
			#region V
			V[0] = (P[1] - P[0]).normalized * e;
			//
			for (int i = 1; i <= P.Count - 2; i += 1)
			{
				Vector2 prev = (P[i - 1] - P[i]).normalized,
						next = (P[i + 1] - P[i]).normalized;

				Vector2 sum = (next - prev).normalized;

				V[i] = sum * e / Z.dot(-prev, sum); // -prev

				/*
				Debug.DrawLine(P[i], P[i] + prev, Color.green, 10f);
				Debug.DrawLine(P[i], P[i] + next, Color.gray, 10f);
				Debug.DrawLine(P[i], P[i] + sum, Color.cyan, 10f);
				Debug.DrawLine(P[i], P[i] + V[i], Color.magenta, 10f);
				*/
			}
			//
			V[N] = (P[P.Count - 1] - P[P.Count - 2]).normalized * e;




			for (int i = 0; i < V.Length; i += 1)
			{ V[i] = new Vector2(-V[i].y, V[i].x); }
			#endregion


			#region verts , tris
			for (int x = 0; x <= P.Count - 1; x += 1)
			{
				/*
				Debug.DrawLine(P[x], P[x] - V[x], Color.white, 10f);
				Debug.DrawLine(P[x], P[x] + V[x], Color.red, 10f);
				*/

				verts.Add(P[x] - V[x]);
				verts.Add(P[x] + V[x]);

				uvs.Add(new Vector2(0f , 0f));
				uvs.Add(new Vector2(0f , 0f));
				
				
				if (x == 0) continue;

				int index = verts.Count - 1;
				tris.Add(index - 3);
				tris.Add(index - 2);
				tris.Add(index - 0);

				tris.Add(index - 3);
				tris.Add(index - 0);
				tris.Add(index - 1);
				
				
				
			} 
			#endregion


			#region mesh
			Mesh mesh = new Mesh()
			{
				vertices = verts.ToArray(),
				triangles = tris.ToArray(),
				uv = uvs.ToArray(),
			};
			mesh.RecalculateNormals(); 
			#endregion

			return mesh;
		}

		public static Mesh mesh_path(List<Vector2> P, float e = 1f / 50)
		{

			List<Vector3> verts = new List<Vector3>();
			List<int> tris = new List<int>();
			List<Vector2> uvs = new List<Vector2>();


			int N = P.Count - 1;

			Vector2[] V = new Vector2[N + 1];
			#region V
			V[0] = (P[1] - P[0]).normalized * e;
			//
			for (int i = 1; i <= P.Count - 2; i += 1)
			{
				Vector2 prev = (P[i - 1] - P[i]).normalized,
						next = (P[i + 1] - P[i]).normalized;

				Vector2 sum = (next - prev).normalized;

				V[i] = sum * e / Z.dot(-prev, sum); // -prev

				/*
				Debug.DrawLine(P[i], P[i] + prev, Color.green, 10f);
				Debug.DrawLine(P[i], P[i] + next, Color.gray, 10f);
				Debug.DrawLine(P[i], P[i] + sum, Color.cyan, 10f);
				Debug.DrawLine(P[i], P[i] + V[i], Color.magenta, 10f);
				*/
			}
			//
			V[N] = (P[P.Count - 1] - P[P.Count - 2]).normalized * e;




			for (int i = 0; i < V.Length; i += 1)
			{ V[i] = new Vector2(-V[i].y, V[i].x); }
			#endregion


			#region verts , tris
			for (int x = 0; x <= P.Count - 1; x += 1)
			{
				/*
				Debug.DrawLine(P[x], P[x] - V[x], Color.white, 10f);
				Debug.DrawLine(P[x], P[x] + V[x], Color.red, 10f);
				*/

				verts.Add(P[x] - V[x]);
				verts.Add(P[x] + V[x]);

				uvs.Add(new Vector2(0f, 0f));
				uvs.Add(new Vector2(0f, 0f));


				if (x == 0) continue;

				int index = verts.Count - 1;
				tris.Add(index - 3);
				tris.Add(index - 2);
				tris.Add(index - 0);

				tris.Add(index - 3);
				tris.Add(index - 0);
				tris.Add(index - 1);



			}
			#endregion


			#region mesh
			Mesh mesh = new Mesh()
			{
				vertices = verts.ToArray(),
				triangles = tris.ToArray(),
				uv = uvs.ToArray(),
			};
			mesh.RecalculateNormals();
			#endregion

			return mesh;
		}


		public static Mesh mesh_dotted_path_t(List<Vector2> old_P , float e = 1f/50 , float t = 0f)
		{
		    
		    List<Vector2> P = new List<Vector2>();
		    
		    #region process old_P .... to .... P
		    //
		    if(t >= 1f) t = 1f - C.de;
		    if(t <= 0f) return new Mesh(); 
		    
            int old_N = (old_P.Count / 2);

			float i_F = old_N * t;
			int i_I = (int)i_F;
		    
		    
		    for(int i = 0 ; i < i_I ; i += 1)
		    {
		        P.Add(old_P[i * 2 + 0]);
		        P.Add(old_P[i * 2 + 1]);
		    }
		    

		    if((i_F - i_I) > C.de )
		    {
		        float dt = i_F - i_I;
		        P.Add(old_P[i_I * 2]);
		        P.Add( Z.lerp(old_P[i_I * 2] , old_P[i_I * 2 + 1] , dt) );
		    }
		    //
            #endregion
		    
		    
			List<Vector3> verts = new List<Vector3>();
			List<int> tris = new List<int>();
			List<Vector2> uvs = new List<Vector2>();


			int N = P.Count - 1;

			Vector2[] V = new Vector2[N + 1];
			
			
			#region V
			//
			for (int i = 0; i <= P.Count - 2; i += 2)
			{
				Vector2 next = (P[i + 1] - P[i]).normalized;

				V[i + 0] = next * e; 
				V[i + 1] = next * e; 
			}
			//

			for (int i = 0; i < V.Length; i += 1)
			{ V[i] = new Vector2(-V[i].y, V[i].x); }
			
			#endregion



			#region verts , tris
			for (int x = 0; x <= P.Count - 2; x += 2)
			{
			    
				verts.Add(P[x + 0] - V[x + 0]);
				verts.Add(P[x + 0] + V[x + 0]);
				verts.Add(P[x + 1] - V[x + 1]);
				verts.Add(P[x + 1] + V[x + 1]);
				
				
				uvs.Add(new Vector2(0f , 0f));
				uvs.Add(new Vector2(0f , 0f));
				uvs.Add(new Vector2(0f , 0f));
				uvs.Add(new Vector2(0f , 0f));
				
				

				int index = verts.Count - 1;
				tris.Add(index - 3);
				tris.Add(index - 2);
				tris.Add(index - 0);


				tris.Add(index - 3);
				tris.Add(index - 0);
				tris.Add(index - 1);
				
				
				
			} 
			#endregion


			#region mesh
			Mesh mesh = new Mesh()
			{
				vertices = verts.ToArray(),
				triangles = tris.ToArray(),
				uv = uvs.ToArray()
			};
			mesh.RecalculateNormals(); 
			#endregion

			return mesh;
		}


		public static Mesh mesh_dotted_path(List<Vector2> P, float e = 1f / 50)
		{

			List<Vector3> verts = new List<Vector3>();
			List<int> tris = new List<int>();
			List<Vector2> uvs = new List<Vector2>();


			int N = P.Count - 1;

			Vector2[] V = new Vector2[N + 1];


			#region V
			//
			for (int i = 0; i <= P.Count - 2; i += 2)
			{
				Vector2 next = (P[i + 1] - P[i]).normalized;

				V[i + 0] = next * e;
				V[i + 1] = next * e;
			}
			//

			for (int i = 0; i < V.Length; i += 1)
			{ V[i] = new Vector2(-V[i].y, V[i].x); }

			#endregion



			#region verts , tris
			for (int x = 0; x <= P.Count - 2; x += 2)
			{

				verts.Add(P[x + 0] - V[x + 0]);
				verts.Add(P[x + 0] + V[x + 0]);
				verts.Add(P[x + 1] - V[x + 1]);
				verts.Add(P[x + 1] + V[x + 1]);


				uvs.Add(new Vector2(0f, 0f));
				uvs.Add(new Vector2(0f, 0f));
				uvs.Add(new Vector2(0f, 0f));
				uvs.Add(new Vector2(0f, 0f));



				int index = verts.Count - 1;
				tris.Add(index - 3);
				tris.Add(index - 2);
				tris.Add(index - 0);


				tris.Add(index - 3);
				tris.Add(index - 0);
				tris.Add(index - 1);



			}
			#endregion


			#region mesh
			Mesh mesh = new Mesh()
			{
				vertices = verts.ToArray(),
				triangles = tris.ToArray(),
				uv = uvs.ToArray()
			};
			mesh.RecalculateNormals();
			#endregion

			return mesh;
		}


		public static Mesh mesh_line(Vector2 a , Vector2 b , float e = 1f/50, float t = 1f)
		{
			if (t <= 0f) return new Mesh();


			Vector2 nX = b - a,
					nY = new Vector2(-nX.y, nX.x);
			nY = nY.normalized;


			b = Z.lerp(a, b, t);
			Mesh mesh = new Mesh()
			{
				vertices = new Vector3[4]
				{
					a - nY * e,
					a + nY * e,
					b + nY * e,
					a - nY * e,
				},
				triangles = new int[2 * 3]
				{
					0, 1, 2,
					0, 2, 3,
				},
				uv = new Vector2[4]
				{
					new Vector2(0f , 0f),
					new Vector2(0f , 1f),
					new Vector2(1f , 1f),
					new Vector2(1f , 0f),
				}
			};
			//
			mesh.RecalculateNormals();

			return mesh;

		}


		/*
        TODO wave effect after drawing point
        */
		public static Mesh mesh_disc(float r , float dr , int N = 16 , float t = 0f)
		{
			List<Vector3> verts = new List<Vector3>();
			List<int> tris = new List<int>();
			List<Vector2> uvs = new List<Vector2>();


			/*
            dr
            r
            r
            r
            o
            */

			float[] r_1D = new float[3]
			{
				0f,
				Z.lerp( 0 , r - dr , t),
				r,
			};


			#region verts 
            
            for(int y = 0 ; y <= 2 ; y += 1)
            {
                for(int x = 0 ; x <= N ; x += 1)
                {
                    verts.Add(Z.polar(2 * C.PI * x * 1f / N) * r_1D[y]);

					uvs.Add(new Vector2(y * 1f / 2, 0.5f));
                }
            }
                        
			#endregion

            #region tris
            /*
            
            2 * (N + 1) .... 0, 1 , 2, 3, 4, ..... N 
            1 * (N + 1) .... 0, 1 , 2, 3, 4, ..... N 
            0 +         .... 0, 1 , 2, 3, 4, ..... N 
            */
            
            
            for(int y = 0 ; y <= 1 ; y += 1)
            {
                for(int x = 0; x <= N - 1 ; x += 1)
                {
                    tris.Add((x + 0) + (y + 0) * (N + 1));
                    tris.Add((x + 1) + (y + 0) * (N + 1));
                    tris.Add((x + 1) + (y + 1) * (N + 1));
    
                    tris.Add((x + 0) + (y + 0) * (N + 1));
                    tris.Add((x + 1) + (y + 1) * (N + 1));
                    tris.Add((x + 0) + (y + 1) * (N + 1));
                }
            }
            
            #endregion
            


			#region mesh
			Mesh mesh = new Mesh()
			{
				vertices = verts.ToArray(),
				triangles = tris.ToArray(),
				uv = uvs.ToArray(),
			};
			mesh.RecalculateNormals(); 
			#endregion

			return mesh;
		}
		
		
		
		/*
        unit-quad oriented baased on transform
        */
        public static Mesh mesh_quad()
		{
			List<Vector3> verts = new List<Vector3>()
			{
			    new Vector2( -0.5f , -0.5f ),
			    new Vector2( -0.5f , +0.5f ),
			    new Vector2( +0.5f , +0.5f ),
			    new Vector2( +0.5f , +0.5f ),
			};


			List<int> tris = new List<int>()
			{
				0 , 1, 2,
				0 , 2, 3,

			};
			
			List<Vector2> uvs = new List<Vector2>()
			{
	            new Vector2(0f , 0f),  
	            new Vector2(0f , 1f),  
	            new Vector2(1f , 1f),  
	            new Vector2(1f , 0f),  
			};
            
			#region mesh
			Mesh mesh = new Mesh()
			{
				vertices    = verts.ToArray(),
				triangles   = tris.ToArray(),
				uv          = uvs.ToArray(),
			};
			mesh.RecalculateNormals(); 
			#endregion

			return mesh;
		}
		
		
		/*
        unit-quad oriented baased on transform
        */
        public static Mesh mesh_poly(float r , float offset_angle = 0f , int N = 16)
		{
			List<Vector3> verts = new List<Vector3>();
			List<int> tris = new List<int>();
			List<Vector2> uvs = new List<Vector2>();

            
			#region verts 
            verts.Add(new Vector2(0f , 0f));
			uvs.Add(new Vector2(0f , 0f));
            
            
            for(int x = 0 ; x < N ; x += 1)
            {
                verts.Add(Z.polar( 2 * C.PI * x * 1f / N + offset_angle ) * r );
                uvs.Add(new Vector2(0f , 0f));
            }
            
			#endregion

            #region tris
            for(int i = 1 ; i <= N - 1; i += 1 )
            {
                tris.Add(0);
				tris.Add(i + 1);
                tris.Add(i);
            }
            
            tris.Add(0);
            tris.Add(1);
            tris.Add(N);
            
            #endregion
            


			#region mesh
			Mesh mesh = new Mesh()
			{
				vertices = verts.ToArray(),
				triangles = tris.ToArray(),
				uv = uvs.ToArray(),
			};
			mesh.RecalculateNormals(); 
			#endregion

			return mesh;
		}


    }


	public static class TEX2D
	{
		public static Texture2D grad(Color a , Color b , int N)
		{
			Texture2D tex2D = new Texture2D(2 , 1);
			
			for(int i = 0; i <= N; i += 1)
			{
				tex2D.SetPixel(i, 0, Color.Lerp(a, b, i * 1f / N));
			}

			tex2D.filterMode = FilterMode.Point;
			tex2D.wrapMode = TextureWrapMode.Clamp;
			tex2D.Apply();

			return tex2D;
		}
	}

    #endregion
    // MESH //
    
    
    // DRAW //
    #region DRAW
    public class DRAW
    {
        
        public static float dt = Time.deltaTime;
        public static Color col = Color.red;



		#region LINE
		public static void LINE(Vector2 a, Vector2 b, float e = 1f / 50)
		{
			Vector2 nX = b - a,
					nY = new Vector2(-nX.y, nX.x).normalized;

			Debug.DrawLine(a + nY * e, b + nY * e, DRAW.col, DRAW.dt);
			Debug.DrawLine(a - nY * e, b - nY * e, DRAW.col, DRAW.dt);
		}
		#endregion

		#region QUAD
		public static void QUAD(Vector2 o, float sx, float sy, float se = 1f / 100, float e = 1f / 100)
		{

			Vector2[] o_corner_1D = new Vector2[4]
			{
				o + new Vector2(+sx * 0.5f - se, +sy * 0.5f - se),
				o + new Vector2(-sx * 0.5f + se, +sy * 0.5f - se),
				o + new Vector2(-sx * 0.5f + se, -sy * 0.5f + se),
				o + new Vector2(+sx * 0.5f - se, -sy * 0.5f + se),
			};


			Vector2[] i_corner_1D = new Vector2[4]
			{
				o + new Vector2( +sx * 0.5f - (se + e), +sy * 0.5f - (se + e) ),
				o + new Vector2( -sx * 0.5f + (se + e), +sy * 0.5f - (se + e) ),
				o + new Vector2( -sx * 0.5f + (se + e), -sy * 0.5f + (se + e) ),
				o + new Vector2( +sx * 0.5f - (se + e), -sy * 0.5f + (se + e) ),
			};

			for (int i = 0; i < o_corner_1D.Length; i += 1)
			{
				Debug.DrawLine(o_corner_1D[i], o_corner_1D[(i + 1) % o_corner_1D.Length], DRAW.col, DRAW.dt);
				Debug.DrawLine(i_corner_1D[i], i_corner_1D[(i + 1) % o_corner_1D.Length], DRAW.col, DRAW.dt);
			}

		}
		#endregion

		#region POLYGON
		public static void POLYGON(Vector2 o, float r, float offset_angle, int N = 6, float se = 1f / 100, float e = 1f / 100)
		{

			Vector2[] o_corner_1D = new Vector2[N],
					  i_corner_1D = new Vector2[N];
			for (int i = 0; i < N; i += 1)
			{
				o_corner_1D[i] = Z.polar(2 * C.PI * i * 1f / N + offset_angle) * (r - se);
				i_corner_1D[i] = Z.polar(2 * C.PI * i * 1f / N + offset_angle) * (r - se - e);
			}

			//
			for (int i = 0; i < o_corner_1D.Length; i += 1)
			{
				Debug.DrawLine(o_corner_1D[i], o_corner_1D[(i + 1) % o_corner_1D.Length], DRAW.col, DRAW.dt);
				Debug.DrawLine(i_corner_1D[i], i_corner_1D[(i + 1) % o_corner_1D.Length], DRAW.col, DRAW.dt);
			}
			//

		} 
		#endregion


		#region CHAR
		/*
        TODO char
        0 - +
        1 - x
        */
		public static void CHAR(Vector2 o , float s , int type = 0)
        {
            
        }
        #endregion
        
        
    }
    #endregion
    // DRAW //




    #endregion
    //// RENDER ////
    
     
    
}
