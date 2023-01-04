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
    

    
    
    
    
    //
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
            
            // sum (t)....[0 , 1-].... f(t) * polar(-2 * pi * t * n) //
            for(int i = 0; i < N ; i += 1)
            {
                float t = i * 1f / N;
                sum += mul( f(t) * polar(-2 * Mathf.PI * t * n) );
            }
            // sum (t)....[0 , 1-].... f(t) * polar(-2 * pi * t * n) //
            
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
    //
    
}







