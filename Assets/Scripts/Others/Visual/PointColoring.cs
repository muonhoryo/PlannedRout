
using PlannedRout.LevelManagment;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlannedRout.Visual
{
    public sealed class PointColoring : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer Owner;

        private void Awake()
        {
            LevelManager.LevelInitializedEvent += ReferredInitialization;
        }
        private void ReferredInitialization()
        {
            LevelManager.LevelInitializedEvent -= ReferredInitialization;
            float h,s,v;
            Color.RGBToHSV(Owner.color,out h,out s,out v);
            h = Random.value;
            Owner.color = Color.HSVToRGB(h, s, v);
        }
    }
}
