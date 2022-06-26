using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_ROPE : MonoBehaviour
{

    private void Update()
    {
        
        if(Input.GetMouseButtonDown(1))
        {
            StopAllCoroutines();
            StartCoroutine(STIMULATE());
            //
        }
        //


    }



    public Transform _start,
                     _end;


    [Range(0.1f , 10f)]
    public float Ks = 0.1f;

    public List<Vector2> _points;

    IEnumerator STIMULATE()
    {

        #region frame_rate

        QualitySettings.vSyncCount = 2;
        yield return null;
        yield return null;
        #endregion

        _points = new List<Vector2>();

        #region _points
        int _intermediate_count = 10;

        _points.Add(_start.position);

        for (int i = 0; i < _intermediate_count; i += 1)
        {
            _points.Add(
                Z.lerp(_start.position, _end.position, (i + 1) * 1f / (_intermediate_count + 1))
            );
        }

        _points.Add(_end.position);
        #endregion



        float dt = Time.deltaTime;

        //
        while (true)
        {



            #region spring
            _points[0] = _start.position;
            _points[_points.Count - 1] = _end.position;



            for (int iter = 0; iter < 16; iter += 1)
            {
                for (int i0 = 1; i0 <= _points.Count - 2; i0 += 1)
                {
                    //
                    _points[i0] +=
                    (
                          ((_points[i0 - 1] - _points[i0]) + (_points[i0 + 1] - _points[i0])) * (Ks * _points.Count / 10f) * dt +
                             new Vector2(0f, -1f) * dt
                    );
                    //
                }

            }

            #endregion


            #region DRAW
            for (int i0 = 0; i0 <= _points.Count - 2; i0 += 1)
            {
                //
                Debug.DrawLine(_points[i0], _points[i0 + 1], Color.red, dt);
                Debug.DrawLine(_points[i0], _points[i0] - Vector2.up * 0.5f , Color.gray, dt);
            }
            #endregion




            yield return null;
        }


        yield return null;
    }




    #region Z
    public static class Z
    {

        public static Vector3 lerp(Vector3 a , Vector3 b , float t)
        {
            Vector3 n = b - a;
            return a + n * t;
            //
        }

    }


    #endregion

}
