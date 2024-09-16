

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
        [SerializeField] private string SwapPartIdentifier;
        public override void Generate()
        {
            StringBuilder str = new StringBuilder(Message);
            str.Replace(SwapPartIdentifier, PlayerDeath.Instance_.RemainedLifesCount_.ToString());
            Owner.text=str.ToString();
        }
    }
}