

namespace PlannedRout.LevelManagment
{
    public interface ILevelPart
    {
        public enum LevelPartType:byte
        {
            Wall,
            Door,
            Point,
            Fruit,
            Energy
        }

        public LevelPartType PartType_ { get; }

        public void RemovePart();
    }
}