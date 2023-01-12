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


			PATH _path = new PATH();
			_path.P = P;
			_path.INTIALIZE_LUT();

			FOURIER _fourier = new FOURIER();
			_fourier._bezier_path = _path;
			_fourier.INITIALIZE(Cn_count : 4);








			OBJ.INITIALIZE_HOLDER();
			//
			OBJ obj = new OBJ("obj_path", 0);
			obj.mesh(MESH.mesh_path(_path.get_const_spaced_points_0(100), 1f / 50));







			while(true)
			{




				//
				Tr_pos_0.position = _path.pos(this.t);


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
			LUT = new float[N][];

			float sum = 0f;
			LUT[0] = new float[2] { 0f, sum };
			//
			for (int i = 0; i < N; i += 1)
			{
				float t_prev = (i - 1) * 1f / N;
				float t_curr = i * 1f / N;

				sum += Z.mag(pos(t_curr - t_prev));
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
	public static class C
	{
		public static float PI = Mathf.PI;
		public static Vector2 zero = Vector2.zero;

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
		public static float t { get { return C.i * 1f / (C.frames - 1); } }




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
				return (1 - t) * (1 - t) * (1 - t) * (1 - t);
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
            if(GameObject.Find("holder") != null)
                GameObject.Destroy(GameObject.Find("holder"));
                
            holder = new GameObject("holder").transform;
        }
        
        #endregion
        
        
        MeshFilter mf;
        MeshRenderer mr;
        GameObject G;
        
        public OBJ(string name , int layer = 0)
        {
            G = new GameObject(name);
            mf = G.AddComponent<MeshFilter>();
            mr = G.AddComponent<MeshRenderer>();
			mr.sharedMaterial = new Material(Shader.Find("unlit"));
            
            G.transform.parent = holder;
            G.transform.position = new Vector3(0f , 0f , -layer * 1f / 10);
        }
        
        
        #region CONTROLS
        public void enable(bool need_to_enable) { G.SetActive(need_to_enable);}
        
        public void POS(Vector2 pos)            { G.transform.position           = new Vector3(pos.x , pos.y , G.transform.position.z ); }
        
        public void ROT(float angle)            { G.transform.localEulerAngles   = new Vector3(0f , 0f , angle / C.PI * 180 ); }

        public void SCALE(float x , float y)    { G.transform.localScale         = new Vector3(x , y , 1f); }

        
        public void mesh(Mesh mesh) { mf.sharedMesh = mesh; }
        
        
        /*
        TODO -
            col
            grad
            texture
        */
        public void col(Color col)
        {
            Texture2D tex2D = new Texture2D(1 , 1);
            tex2D.SetPixel(0, 0, col);
            tex2D.filterMode = FilterMode.Point;
            tex2D.Apply();
            
            mr.sharedMaterial.mainTexture = tex2D;
        }
        #endregion
        
        
    }
    
    
    public class OBJS
    {
        #region HOLDER
        
        public static Transform holder;
        public static void INITIALIZE_HOLDER()
        {
            if(GameObject.Find("holderS") != null)
                GameObject.Destroy(GameObject.Find("holderS"));
                
            holder = new GameObject("holderS").transform;
        }
        
        #endregion
        
        
        SpriteRenderer sr;
        GameObject G;
        
        public OBJS(string name , int layer = 0)
        {
            G = new GameObject(name);
            sr = G.AddComponent<SpriteRenderer>();
            
            G.transform.parent = OBJS.holder;
            G.transform.position = new Vector3(0f , 0f , -layer * 1f / 10);
        }
        
        
        #region CONTROLS
        public void enable(bool need_to_enable) { G.SetActive(need_to_enable);}
        
        public void POS(Vector2 pos)            { G.transform.position          = new Vector3(pos.x , pos.y , G.transform.position.z ); }
        
        public void ROT(float angle)            { G.transform.localEulerAngles  = new Vector3(0f, 0f, angle / C.PI * 180); }

        public void SCALE(float x , float y)    { G.transform.localScale        = new Vector3(x , y , 1f); }

        
        public void mesh(string locate ) { sr.sprite = Resources.Load<Sprite>( locate ); }
        
        

        public void alpha(float alpha) { sr.color = new Color(0f, 0f, 0f, alpha); }
        #endregion
        
        
    }
    

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
    // OBJ //



    // MESH //
    #region MESH
    public static class MESH
    {

		/*
		TODO -
			PATH ( t ) .... with variable specified stroke
			DOTTED_PATH ( t )
        
			QUAD ( t )
		*/

		public static Mesh mesh_path(List<Vector2> P , float e =1f/100)
		{
			List<Vector3> verts = new List<Vector3>();
			List<int> tris = new List<int>();


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

				if (x == 0) continue;

				int index = verts.Count - 1;
				tris.Add(index - 3);
				tris.Add(index - 2);
				tris.Add(index);

				tris.Add(index - 3);
				tris.Add(index);
				tris.Add(index - 1);
			} 
			#endregion


			#region mesh
			Mesh mesh = new Mesh()
			{
				vertices = verts.ToArray(),
				triangles = tris.ToArray(),
			};
			mesh.RecalculateNormals(); 
			#endregion

			return mesh;
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

