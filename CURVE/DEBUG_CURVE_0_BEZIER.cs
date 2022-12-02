using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace SPACE_CURVE
{

    // --DEBUG_CURVE //
    public class DEBUG_CURVE_0_BEZIER : MonoBehaviour
    {

        private void Update()
        {
            //
            if (Input.GetMouseButtonDown(1))
            {
                StopAllCoroutines();
                StartCoroutine(STIMULATION());
            }
            //
        }



        IEnumerator STIMULATION()
        {
            // -frameRate //
            QualitySettings.vSyncCount = 2;
            yield return null;
            yield return null;
            // frameRate- //








            yield return null;
        }



    }
    // DEBUG_CURVE-- //

}






