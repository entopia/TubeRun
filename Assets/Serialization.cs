using System.Collections;
using System.Collections.Generic;
using tubes;
using System;


namespace tubes
{
    #region Basic Class
    [Serializable]
    public class TubeObjects
    {
        public string linecode;
        public string tripnumber;
        public string setnumber;
        public float timeonstation; //seconds 0-30-60-90-120-150-180-300
        public string stationcode;
        public string platformdirection;
     

    }
    #endregion
}
