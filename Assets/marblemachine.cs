using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

namespace tubes
{

    public class marblemachine : MonoBehaviour
    {

        public SerialPort serial = new SerialPort("COM3", 9600);
        private bool lightState = false;
        public GameObject lights;
        public bool marblestate = true;
        public int countbalz = 0;
        //public AudioClip clip;




        public void StartRun()
        {
            StartCoroutine(RunMarble());
        }

        IEnumerator RunMarble()

        {
            if (serial.IsOpen == false)
            {

                Debug.Log("trying to open the port");
                serial.Open();
                Debug.Log("opening port");
            }

            Debug.Log("running the run");
            serial.Write("A");
            //serial.Write("A");
            //sendcommand.. 
            yield return new WaitForSeconds(5);
            serial.Write("Z");
            //serial.Write("b");
            //sendcommand to count.. 
            Debug.Log("stopping the run");
        }



        //float time = 0.0f;
        //float period = 5f;   // number of secs the murblerun runs;

        /*
        public void Updater()
        {
            while (marblestate == true)
            {
                if (Time.time > time) //stops it when it reaches 5 sec. 
                {
                    time += period;
                    marblestate = false;
                    Triggermarblerun();
                }
            }
        }
        */


        //public void OnMouseDown()
        //{
        //    string[] ports = SerialPort.GetPortNames();

        //    foreach (string port in ports)
        //    {
        //        Debug.Log(port);
        //    }


        //    Debug.Log(serial);
        //    if (serial.IsOpen == false)
        //    {
        //        Debug.Log("trying to open the port");
        //        serial.Open();
        //        Debug.Log("opening port");
        //    }
        //    if (lightState == false)
        //    {
        //        serial.Write("A");
        //        Debug.Log("running");
        //        lightState = true;
        //        if (lights != null && lights.GetComponent<Light>() != null)
        //        {
        //            lights.GetComponent<Light>().enabled = lightState;
        //            //light.audio.PlayOneShot(clip); 
        //        }
        //    }
        //    else
        //    {
        //        serial.Write("b");
        //        Debug.Log("stopping");
        //        lightState = false;
        //        if (lights != null && lights.GetComponent<Light>() != null)
        //        {
        //            lights.GetComponent<Light>().enabled = lightState;
        //            //light.audio.PlayOneShot(clip); 
        //        }


        //    }
        //}

        void OnApplicationQuit()
        {
            serial.Write("Z");
            Debug.Log("Application ending after " + Time.time + " seconds");
        }

    }
    
}
