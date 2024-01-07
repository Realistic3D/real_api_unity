using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REAL.Items
{
    [Serializable]
    public class Job
    {
        public string result;
        public float charges;
        public bool finished;
        public string jobID;
        public string status;
        public string outExt;
        public string expFrom;
        public float elapsedTime;
        public string renderTime;
    }
}