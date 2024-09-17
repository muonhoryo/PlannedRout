

using System.Text;
using PlannedRout.GameScoreManagment;
using UnityEngine;
using UnityEngine.UI;

namespace PlannedRout.Visual
{
    public sealed class OnDeathMessageGenerating_Death : OnDeathMessageGenerating
    {
        [SerializeField] private Text Owner;
        [SerializeField] private string Message;
        [SerializeField] private string AltMessage_One;
        [SerializeField] private string AltMessage_Two;

        [SerializeField] private string SwapPartIdentifier_LifeCount;
        public override void Generate()
        {
            string mesge;
            int lifeCount = PlayerDeath.Instance_.RemainedLifesCount_;
            if (lifeCount % 10 == 1)
            {
                if (lifeCount == 11)
                    mesge = Message;
                else
                    mesge = AltMessage_One;
            }
            else if (lifeCount % 10 == 2)
            {
                mesge = AltMessage_Two;
            }
            else
            {
                mesge= Message;
            }
            StringBuilder str = new StringBuilder(mesge);
            str.Replace("\\n", "\n");
            str.Replace(SwapPartIdentifier_LifeCount, PlayerDeath.Instance_.RemainedLifesCount_.ToString());
            Owner.text=str.ToString();
        }
    }
}