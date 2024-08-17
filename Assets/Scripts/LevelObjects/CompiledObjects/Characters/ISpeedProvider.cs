

using MuonhoryoLibrary;

namespace PlannedRout.LevelObjects.Characters
{
    public interface ISpeedProvider 
    {
        public CompositeParameter<float> Speed_ { get; }
    }
}