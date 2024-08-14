

namespace PlannedRout.LevelManagment
{
    public interface ILevelPart
    {
        public enum LevelPartType
        {
            Wall,
            Door,
            RoomPoint,
            Point,
            Fruit,
            Energy
        }

        public void RemovePart();
    }
}