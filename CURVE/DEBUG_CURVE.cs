//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;





namespace SPACE_CURVE
{

// --DEBUG_CURVE //
public class DEBUG_CURVE : MonoBehaviour
{
    
    private void Update()
    {
        //
        if(Input.GetMouseButtonDown(1))
        {
            StopAllCoroutines();
            StartCoroutine(STIMULATION());
        }
        //
    }
    
    
    
    
    
    
    
    IEnumerator STIMULATION()
    {
        // -frameRate //
        Quality.VsyncCount = 2;
        yield return null;
        yield return null;
        // frameRate- //

        
        
        
        
        
        
        
        yield return null;
    }
    
    
    
}
// DEBUG_CURVE-- //









// -BEZIER //
public class BEZIER
{
    
    // count == 3 * i + 1 
    public Vector2[] P;
    
    
    
    
    // -pos //
    public Vector2 pos(float t)
    {
        if( t <= 0f ) { return P[0]; }
        if( t >= 1f ) { return P[P.Length - 1]; }
        
        
        int N = (P.Length - 1) / 3;
        
        float i_F = N * t;
        int   i_I = (int)i_F;
        float dt = i_F - (int)i_F;
        
        
        return Z.bezier( P[ i_I * 3 + 0 ] ,
                         P[ i_I * 3 + 1 ] ,
                         P[ i_I * 3 + 2 ] ,
                         P[ i_I * 3 + 3 ] , dt);  
    }
    // pos- //
    
    
}
// BEZIER- //








// -TOOL //
// 
// -Z //
public static class Z
{
    // -lerp //
    public static Vector3 lerp(Vector3 a , Vector3 b , float t)
    {
        Vector3 n = b - a;
        return a + n * t;
    }
    
    // lerp- //
    
    
    
    // -eval // 
    public static Vector3 bezier(params Vector3[] P , float t)
    {
        Vector3 l0 = Z.lerp(P[0] , P[1] , t),
        		l1 = Z.lerp(P[1] , P[2] , t),
        		l2 = Z.lerp(P[2] , P[3] , t);
        
        Vector3 q0 = Z.lerp(l0 , l1 , t),
        		q1 = Z.lerp(l1 , l2 , t);
        
        return Z.lerp(q0 , q1 , t);                    
    }
    // eval- //
     
     
     
    
    // -ease //
    
    // ease- //
    
}
// Z- //


// -C //
public static class C
{
    
    
    /*
    
    C.time = 30;
    for(C.i = 0; C.i < C.time; C.i += 1)
    {
    	// somthng(C.t);
    	
    	yield return C.wait;
    }
    */
    
    public static int time = 30;
    public static int i = 0;
    
    public static float t
    { get { return C.i * 1f / (C.time - 1); }}
    
    public static WaitforSeconds wait
    { get { /* capture_frame; */ return null; } }
    
    
    
    public static waitforSeconds wait_(int frames_length)
    {
        for(int i = 0 ; i < frames_length ; i += 1)
        { /* capture_frame; */ yield return null; }
    }
    
}
// C- //


// TOOL- //






}







