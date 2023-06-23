using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REAL.Items
{
    [Serializable]
    public class Job
    {
        public float used;
        public float charges;
        
        public int samples;
        
        public string jobID;
        public string status;
        public string outExt;
        public string jobExt;
        public string expFrom;
        public string chargeType;
        public string renderTime;
    }
}