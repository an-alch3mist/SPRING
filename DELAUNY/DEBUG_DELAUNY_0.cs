using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_DELAUNY_0 : MonoBehaviour
{

    private void Update()
    {
        
        if(Input.GetMouseButtonDown(1))
        {
            StopAllCoroutines();
            StartCoroutine(STIMULATE());
        }
        //
    }


    IEnumerator STIMULATE()
    {




        yield return null;
    }






}
