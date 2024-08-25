

using MuonhoryoLibrary;
using UnityEngine;

namespace PlannedRout.LevelObjects.Characters
{
    public abstract class SpeedComponent : MonoBehaviour, ISpeedProvider
    {
        private CompositeFloat Speed;
        public CompositeFloat Speed_ => Speed;

        protected abstract float DefaultSpeed_ { get; }

        private void Awake()
        {
            Speed = new CompositeFloat(DefaultSpeed_);
        }
    }
}