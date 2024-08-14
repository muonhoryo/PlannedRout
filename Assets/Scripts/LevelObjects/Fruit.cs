

namespace PlannedRout.LevelObjects
{
    public sealed class Fruit : PickUppedObject_ScoreIncreaser
    {
        public Fruit(int IncreasedScore) : base()
        {
            this.IncreasedScore = IncreasedScore;
        }

        private int IncreasedScore;

        protected override int AddedScore_ => IncreasedScore;
    }
}