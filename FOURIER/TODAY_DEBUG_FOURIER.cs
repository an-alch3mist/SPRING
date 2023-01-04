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




    public class BEZIER_PATH
    {
        // in
        public List<Vector2> P;
    
    
        /* out
        pos(t)
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
        
        
        
        
        
        
        
    }

    
}







