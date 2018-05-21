using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tubes;
using UnityEngine.UI;


namespace tubes
{

    public class APIcall : MonoBehaviour
    {
        
        public string url = "http://loggerhead.casa.ucl.ac.uk/api/f/trackernet?pattern=trackernet_*.csv";
        public string url2 = "http://images.earthcam.com/ec_metros/ourcams/fridays.jpg";
        public string url3 = "http://weather.casa.ucl.ac.uk/clientraw.txt";
        string tubedata = "";
        TubeObjects tubeObject;


        

        void Start()
        {
            //StartCoroutine(Calltubes());    
        }


        float time = 0.0f; 
        float period = 180f;   // number of secs the function runs;

        void Update()
        {
            //Run function every 3 min. 
            //Debug.Log(Time.time);
            if (Time.time > time)
            {
                Debug.Log("New iterations time: " + Time.time);
                StartCoroutine(Calltubes());
                time += period;
            }
        }


        IEnumerator Calltubes()
        {
            using (WWW www = new WWW(url))
            {
                Debug.Log(www);
                yield return www;
                tubedata = www.text;
                //Debug.Log(tubedata);
                if (tubedata != null)
                {
                    Debug.Log("running");
                    parcetubes(tubedata, tubedataline);
                }
            }
        }




        string[] tubedataline;
        int counttubes = 0;
        int countrequests = 0;
        int counttubesinline = 0;

        public List<TubeObjects> tubelist = new List<TubeObjects>(); // creates a new empty list of tube objects


        public void parcetubes(string tubedata, string[] textSplit)
        {
            tubelist.Clear();   // clears list for each iteration. 
            //counttubes = 0;   //counts the tubes that pass through each station
            tubedataline = tubedata.Split("\n"[0]);
            counttubesinline = 0;


            //Debug.Log(textSplit.Length);
            for (int i = 0; i < tubedataline.Length - 1; i++)
            {
                string[] tubeline;
                tubeline = tubedataline[i].Split(","[0]);
                //Debug.Log(tubeline[9]);



                if (tubedataline[i] != null && i!=0 ) //excludes the titles row.  tubeline[9].Equals("\"OXC\"")) //finds the trains that are scedualed to pass from Oxford station
                {

                    //Debug.Log (tubeline[0]);
                    //Debug.Log(tubeline[9]);

                    TubeObjects tubeObject = new TubeObjects();   // creates a new object to store them. 
                    tubeObject.linecode = tubeline[0];
                    tubeObject.tripnumber = tubeline[1];
                    tubeObject.setnumber = tubeline[2];
                    tubeObject.timeonstation = float.Parse(tubeline[7].Replace("\"", string.Empty).Trim());
                    tubeObject.stationcode = tubeline[9];
                    tubeObject.platformdirection = tubeline[12];
                    //Debug.Log(tubeline[2]);
                    

                    tubelist.Add(tubeObject); //adds tube objects in a list. 
                    counttubes += 1;


                    //this.GetComponent<marblemachine>().StartRun();
                    //Debug.Log(tubelist[i-1].linecode); //time of arrival

                }

            }
            //sorttubesbyarrivaltime(tubelist, counttubesOXC);
            tubesperstation(tubelist);
            Tubelines(tubelist);
            countrequests += 1;
   

            //Debug.Log("1");
            //Debug.Log(textSplit[3]);

        }




        /// <tubesperline>
        //This section creates an array for the tubes per line (B) and 
        //runs the marble run for the total number of trains passing. 
        


        string nextline;
        string linename;
        List<string> Allactivetubelinesnames = new List<string>();
        void Tubelines (List<TubeObjects> tubelist)
        {   
            
            for (int i=1; i < tubelist.Count-1; i++)
            {

                if (i==1)
                {
                    Allactivetubelinesnames.Add(tubelist[i-1].linecode);
                }
                if (i>1 && tubelist[i].linecode != tubelist[i-1].linecode)
                {
                    Allactivetubelinesnames.Add(tubelist[i].linecode);
                }

            }
            aggregateperline(tubelist, Allactivetubelinesnames);

        }





        //Line find tubes

        public string line=" ";
        int countertubelines = 0;
        void aggregateperline (List<TubeObjects> tubelist, List<string> Allactivetubelinesnames)
        {
            countertubelines = 0;
            for (int j=0; j<Allactivetubelinesnames.Count;j++)
            {
                List<string> nooftubesperline = new List<string>();
                for (int i = 0; i < tubelist.Count; i++)
                {
                    if (Allactivetubelinesnames[j] == tubelist[i].linecode)
                    {
                        if (tubelist[i].linecode== "\"" + line + "\"")
                        {
                            // need to do per minute... so create a function that only counts the upt o 60 sec.. otherwise 3 minutes may be in an other stop as well or maybe i dont care.. . 
                            countertubelines++;
                            Debug.Log(tubelist[i].linecode);
                        }
                        
                    }

                }
            }
            Debug.Log(countertubelines);

        }






        /// </tubesperline>

            


        /// <summary>
        //This section creates an array for the tubes per station (OXC) and 
        //runs the marble run for each train passes a specified station. 
        /// </summary>

        int counttubesOXC =0;
        public string station = " ";
        List<TubeObjects> OXClist = new List<TubeObjects>(); // creates a new empty list of tube objects from OXC
        void tubesperstation (List<TubeObjects> tubelist)
        {
            //Debug.Log("2");
            for (int i=0; i<tubelist.Count; i++)
            {
                if (tubelist[i].stationcode.Equals("\""+station+"\""))  //quotes are there because in the db stations and other variables contain quotes.
                {
                    OXClist.Add(tubelist[i]);
                    counttubesOXC += 1;
                }
            }
            sorttubesbyarrivaltime(OXClist, counttubesOXC);
            //Debug.Log("3");
        }



        void sorttubesbyarrivaltime (List <TubeObjects> OXClist, int counttubes)
        {
            TubeObjects temp;
            //Debug.Log("4");

            for (int i=0; i<OXClist.Count;i++)
            {
                for (int j =0; j<OXClist.Count-1; j++)
                {
                    
                    if (OXClist[j].timeonstation > OXClist[j + 1].timeonstation)
                    {
                        temp = OXClist[j + 1];
                        OXClist[j + 1] = OXClist[j];
                        OXClist[j] = temp;
                    }
                }
                
                
            }
            //Debug.Log("5");


            for (int i=0; i<OXClist.Count;i++)
            {
                Debug.Log("Timeto station: " + OXClist[i].timeonstation);
            }

            //Debug.Log("6");
            StartCoroutine(timermarblerun(OXClist));


            /*
            for (int i = 0; i< OXClist.Count;i++)
            {
                Debug.Log(OXClist[i].timeonstation);
            }
            */
        }



        //public void findcurrenttube (List<TubeObjects> OXClist)
        //{
        //    Debug.Log("Number of tubes this minute: " + OXClist.Count);
        //    int currenttube = 0;
        //    while (currenttube < OXClist.Count)
        //    {
        //        StartCoroutine (timermarblerun(OXClist));
        //        this.GetComponent<marblemachine>().StartRun();
        //        currenttube++;
        //    }

        //}

       
        
        IEnumerator timermarblerun (List <TubeObjects> tubelist)
        {
            Debug.Log("Number of tubes this minute: " + tubelist.Count);
            int currenttube = 0;
            while (currenttube < tubelist.Count)
            {
                if (currenttube == 0)
                {
                    
                    if (tubelist[currenttube].timeonstation == 0)
                    {
                        Debug.Log("tube 0 + time:0 ");

                        this.GetComponent<marblemachine>().StartRun();
                        updathetext(tubelist);
                        yield return new WaitForSeconds(5);
                    }
                    else
                    {
                        Debug.Log("tube 0 +time=!0 ");
                        yield return new WaitForSeconds(tubelist[currenttube].timeonstation);
                        this.GetComponent<marblemachine>().StartRun();
                        updathetext(tubelist);
                    }
                }

                if (currenttube > 0)
                {
                    if (tubelist[currenttube].timeonstation != tubelist[currenttube - 1].timeonstation)
                    {
                        Debug.Log("tube =!");
                        yield return new WaitForSeconds(tubelist[currenttube].timeonstation - tubelist[currenttube - 1].timeonstation);

                        this.GetComponent<marblemachine>().StartRun();
                        updathetext(tubelist);
                    }
                    else
                    {
                        Debug.Log("tube ==");
                        yield return new WaitForSeconds(5);
                        this.GetComponent<marblemachine>().StartRun();
                        updathetext(tubelist);
                    }
                }
                currenttube++;
            }

            
        }

        public Text Counter;
        public Text Line;
        public Text Victoriatubes;
        public GameObject UIobjects;
        int countthetubes = 0;

        public void updathetext(List <TubeObjects> tubelist)
        {
            countthetubes++;
            Counter.text = countthetubes.ToString();
            Line.text = tubelist[0].linecode.ToString();
            Victoriatubes.text = counttubesinline.ToString();
            //Debug.Log("found the child" + UIobjects.GetComponent<UIscript>().trigger);
            UIobjects.GetComponent<UIscript>().trigger = 1;
            UIobjects.GetComponent<UIscript>().tubeline = Line.text;

        }



        public void timemarblerun2 (List <TubeObjects> tubelist)
        {
            Debug.Log( "Number of tubes this minute: " + tubelist.Count);

            float time1 = 0.0f;
            float period1 = tubelist[0].timeonstation;

            if (Time.time > time1)
            {
                Debug.Log("time: " + Time.time);
                StartCoroutine(Calltubes());
                time += period-tubelist[0].timeonstation;
            }

        }

       
    }
}



  
