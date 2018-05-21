using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace tubes
{
    public class UIscript: MonoBehaviour
    {

        public GameObject backgroundpanel;
        public int trigger = 0;
        public string tubeline = " ";

        //colors definitions:
        public Color lerpedColor = Color.white;
        public Color C;
        public Color V;
        public Color B;



        // Use this for initialization
        void Start()
        {
            color1 = Color.red;

        }



        // Update is called once per frame
        void Update()
        {

            if (trigger == 1)
            {
                StartCoroutine(changebackcolor());

            }


        }



        Color color1;
        Color color2;
        IEnumerator changebackcolor()
        {
            if (tubeline == "\"B\"")  
            {
                color2 = B;
            }
            else if (tubeline == "\"C\"")
            {
                color2 = C;
            }
            else if (tubeline == "\"V\"")
            {
                color2 = V;
            }
            else { color2 = Color.white; }


            lerpedColor = Color.Lerp(color1, color2, Mathf.PingPong(Time.time, 1.5f));
            backgroundpanel.GetComponent<Image>().color = lerpedColor;
            yield return new WaitForSeconds(1);
            trigger = 0;
            color1 = color2;

        }


    }
}
