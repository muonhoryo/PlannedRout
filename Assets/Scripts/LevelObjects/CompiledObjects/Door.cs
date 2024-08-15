

using PlannedRout.LevelManagment;

namespace PlannedRout.LevelObjects
{
    public sealed class Door : ILevelPart 
    {
        private Door() { }

        public static Door Instance_ { get; private set; }= new Door();

        public ILevelPart.LevelPartType PartType_ => ILevelPart.LevelPartType.Door;

        public void RemovePart() { }
    }
}