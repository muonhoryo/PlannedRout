

namespace PlannedRout.LevelObjects
{
    public abstract class PickUppedObject_ScoreIncreaser:IPickUppedObject
    {
        protected abstract int AddedScore_=>

        public void PickUp()
        {

        }
        public void RemovePart()
        {

        }
        protected virtual void PickUpAdditionAction() { }
    }
}