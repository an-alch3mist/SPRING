//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DEBUG_FOURIER_0 : MonoBehaviour
{
    
    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            //
            StopAllCoroutines();
            StartCoroutine(STIMULATE());
        }
    }
    
    
    
    IEnumerator STIMULATE()
    {
        // frame_rate //
        QualitySettings.VsyncCount = 3;
        yield return null;
        yield return null;
        // frame_rate //
        
        
        
        yield return null;
    }
    

    
    
    
    
    // Fourier //
    public class Fourier
    {
        
        // in //
        public BEZIER_PATH _bezier_path;
        // out //
        public List<Vector2> Cn_1D;
        


        public void INITIALIZE(int Cn_count = 10)
        {
            // -10....0....+10 //
            Cn_1D = new List<Vector2>();
            
            
            // 0 , -1 , +1 , -2 , +2 ......... -(Cn_count - 1) , +(Cn_count - 1) // 
            Cn_1D.Add(Cn(0));
            for(int i = 1 ; i < Cn_count ; i += 1)
            {
                Cn_1D.Add(Cn(-i));
                Cn_1D.Add(Cn(+i));
            }
            
        }
        
        
        
        
        // f(t) .... to .... Cn(n) //
        Vector2 f(t)
        {
            //
            return _bezier_path.pos(t);
        }
        
        Vector2 Cn(n)
        {
            int N = 1000;
            

            Vector2 sum = Vector2.zero;
            
            // rotate f(t) by -2 * pi * t * n //
            for(int i = 0; i < N ; i += 1)
            {
                float t = i * 1f / N;
                sum += mul( f(t) * polar(-2 * Mathf.PI * t * n) );
            }
            // rotate f(t) by -2 * pi * t * n //
            
            return sum / N;
        }
        // f(t) .... to .... Cn(n) //
        

        
        // Tool //
        static Vector2 mul(Vector2 a , Vector2 b)
        {
            return new Vector2()
            {
          	  	x = a.x * b.x - a.y * b.y,
                y = a.x * b.y + a.y * b.x
            };
        }
        
        
        static Vector2 polar(float angle)
        {
            return new Vector2()
            {
                x = Mathf.Cos(angle),
                y = Mathf.Sin(angle)
            };
        }        
        // Tool //
        
        
    }
    // Fourier //



    /*
    usage -
        // initalize bezier_path.P = pos_1D //
        // do somthng with v2 bezier_path.pos(t) //

        // intialize bezier_path.initialize_LUT() //
        // do somthng with L<v2> get_points_between(t_start , t_end , N_full) //
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
            if(t <= 0f) return P[0];         
            if(t >= 1f) return P[P.Count - 1];
        
        
            int N = (P.count - 1) / 3;
            
            float i_F = N * t;
            int   i_I = (int)(N * t);
                 
            float dt = i_F - i_I;
            
            return bezier_pos(
                P[i_I * 3 + 0],
                P[i_I * 3 + 1],
                P[i_I * 3 + 2],
                P[i_I * 3 + 3],
                dt
            );
        
        }
        // pos //
        
        
        // tool //
        //// bezier_pos ////
        Vector2 bezier_pos(Vector2 a , Vector2 b , Vector2 c , Vector2 d , float t)
        {
            return 1    *   t * t * t   * (1    )                     * P[0] +
                   3    *   t * t       * (1 - t)                     * P[1] +
                   3    *   t           * (1 - t) * (1 - t)           * P[2] +
                   1    *   1           * (1 - t) * (1 - t) * (1 - t) * P[3];
        
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
            LUT[0] = new float[2] { 0f , sum };
            //
            for(int i = 0 ; i <= N ; i += 1)
            {
                float t_prev = (i - 1) * 1f / N;
                float t_curr = i * 1f / N;         
                
                sum += mag( pos(t_curr - t_prev) );
                LUT[i] = new float[2] { t_curr , sum };
            }
            //
        }
        
        
        /*
        N = (dist_end - dist_start) * N_full
        P.Add( lerp dist_start to dist_end by i * 1f / N )
        */
        public List<Vector2> get_points_between(float t_start , float t_end , int N_full = 100)
        {
            int N = N_full * ( get_dist(t_end) - get_dist(t_start) ) / get_dist(1f);
        
            List<Vector2> P = new List<Vector2>();
            //
            for(int i = 0 ; i <= N ; i += 1)
            {
                float dist = lerp(get_dist(t_start) , get_dist(t_end) , i * 1f/N );
                P.Add(pos(get_t(dist)));
            }
            //
            return P;
        }
        
        
        
        // tool //
        static float mag(Vector2 v) { return Mathf.Sqrt(v.x * v.x + v.y * v.y); }
        
        static float inv_lerp(float a, float b , float v) 
        { 
            return (v - a)/(b - a); 
        }
        
        static float lerp( float a , float b , float t)
        {
            float n = b - a;
            return a + n * t;
        }
        
        // tool //
        
        // ad //
        static float get_dist(float[][] LUT , float t)
        {
            if ( t <= 0f ) return 0f;
            if ( t >= 1f ) return LUT[LUT.Length - 1][1];
        
            int i = 1;
            for(; i <= LUT.Length - 1 ; i +=1)
                if(LUT[i][0] > t)
                    break
        
            float dt = inv_lerp(LUT[i - 1][0] , LUT[i][0] , t);
            
            return lerp( LUT[i - 1][1] , LUT[i][1] , dt );
        }
        
        static float get_t(float[][] LUT , float dist)
        {
            if ( dist <= 0f )                       return 0f;
            if ( dist >= LUT[LUT.Length - 1][1] )   return 1f;
        
            int i = 1;
            for(; i <= LUT.Length - 1 ; i +=1)
                if(LUT[i][1] > dist)
                    break
        
            float d_dist = inv_lerp(LUT[i - 1][1] , LUT[i][1] , dist);
            
            return lerp( LUT[i - 1][1] , LUT[i][1] , d_dist );
        }
        // ad //
        
    }

    
}







