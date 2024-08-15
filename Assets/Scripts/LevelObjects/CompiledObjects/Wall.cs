

using PlannedRout.LevelManagment;

namespace PlannedRout.LevelObjects
{
    public sealed class Wall : ILevelPart
    {
        private Wall() { }

        public static Wall Instance_ { get; private set; } = new Wall();

        public ILevelPart.LevelPartType PartType_ => ILevelPart.LevelPartType.Wall;

        public void RemovePart() { }
    }
}