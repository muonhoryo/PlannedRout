

using System.Collections.Generic;
using UnityEngine;

namespace PlannedRout 
{
    public sealed class TimersManager : MonoBehaviour 
    {
        public static TimersManager Instance_ { get; private set; }

        private void Awake()
        {
            if (Instance_ != null)
                throw new System.Exception("Already have TimersManager.");

            Instance_ = this;
        }
    }
}