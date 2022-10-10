using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_TEXT_0 : MonoBehaviour
{

    private void Update()
    {
        //
        if(Input.GetMouseButtonDown(1))
        {
            StopAllCoroutines();
            StartCoroutine(STIMULATE());
        }
        //
    }



    public Transform Tr;


    IEnumerator STIMULATE()
    {

        #region frame_rate

        QualitySettings.vSyncCount = 2;

        yield return null;
        yield return null;
        #endregion


        C.capture_mode = false;


        C.time = 20;
        //
        #region mov_r
        for (C.i = 0; C.i <= C.time; C.i += 1)
        {
            //
            Tr.position = Z.lerp(C.zero, new Vector2(5, 0), C.Ease(C.t));
            Tr.localScale = Z.lerp(new Vector3(1, 1, 1), new Vector3(0.3f, 2.5f, 1), C.Ease(C.t));


            yield return C.wait;
        } 
        #endregion


        C.wait_frames(30);

        #region mov_d
        for (C.i = 0; C.i <= C.time; C.i += 1)
        {
            //
            Tr.position = Z.lerp(new Vector2(5, 0), new Vector2(5, -2), C.Ease(C.t));
            Tr.localScale = Z.lerp(new Vector3(0.3f, 2.5f, 1), new Vector3(1, 1, 1), C.Ease(C.t));


            yield return C.wait;
        }
        #endregion



        #region mov_l
        for (C.i = 0; C.i <= C.time; C.i += 1)
        {
            //
            Tr.position = Z.lerp(new Vector2(5, -2), new Vector2(0, -2), C.Ease(C.t));
            Tr.localScale = Z.lerp(new Vector3(1, 1, 1), new Vector3(2.5f, 0.3f, 1), C.Ease(C.t));


            yield return C.wait;
        } 
        #endregion



        TEXT txt = new TEXT("TEXT txt = new TEXT( <ul>str</ul> )");



        yield return txt.typewrite();



        yield return null;

    }



    #region TEXT
    //
    public class TEXT
    {

        TMPro.TextMeshPro tm;
        public GameObject G;


        public TEXT(string str)
        {
            G = new GameObject("tm_" + str);

            tm = G.AddComponent<TMPro.TextMeshPro>();
            //
            tm.text = str;
            tm.alignment = TMPro.TextAlignmentOptions.Center;


            G.SetActive(false);
        }



        public IEnumerator typewrite()
        {
            //
            G.SetActive(true);

            string source = tm.text;
            string rand_str = "#WAM=+.";
            

            string str = "-_";

            for (int i0 = 0; i0 < source.Length; i0 += 1)
            {
                //
                for (int i1 = 0; i1 <= 1; i1 += 1)
                {
                    str = replace_char_at(str,
                                    str.Length - 2,
                                    rand_str[Random.Range(0, rand_str.Length)]);

                    tm.text = str;

                    yield return C.wait;
                    yield return C.wait;

                }

                str = replace_char_at(str,
                                str.Length - 2,
                                source[i0]);
                str += '_';

                tm.text = str;
            }



            tm.text = source;
            //
            yield return C.wait;
        }



        #region str .... char_1D
        //
        static string replace_char_at(string str,int index , char c)
        {
            string new_str = "";
            //
            for(int i  =0; i < str.Length; i += 1)
            {
                if(index == i) { new_str += c; }
                else { new_str += str[i]; }
            }
            // 
            return new_str;
        }
        //
        #endregion


    }
    //
    #endregion





    #region C
    //
    public static class C
    {
        public static float PI = Mathf.PI;
        public static Vector3 zero = Vector3.zero;

        
        public static int time = 30;
        public static int i = 0;

        public static float t
        {
            get { return C.i * 1f / C.time; }
        }


        #region wait....capture
        public static bool capture_mode = false;
        static int capture_i = 0;
        public static IEnumerator wait
        {

            get
            {
                if (capture_mode)
                {
                    ScreenCapture.CaptureScreenshot("D:/@STORE/CAPTURE/" + capture_i.ToString() + ".png");
                    capture_i += 1;
                }
                //

                return null;
            }
        } 



        public static IEnumerator wait_frames(int frames)
        {
            //
            for(int i = 0; i < frames;  i += 1)
            {
                yield return C.wait;
            }
            //
        }
        #endregion



        #region Ease
        public static float Ease(float x)
        {
            if(x < 0f) { return 0f; }
            if(x > 1f) { return 1f; }


            //
            return (-Mathf.Cos(Mathf.PI * x) + 1) / 2f;
        } 
        #endregion

    }
    //
    #endregion


    #region Z
    //
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

    }
    //
    #endregion

}
