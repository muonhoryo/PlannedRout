

using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout.Visual 
{
    public sealed class TailLengthInitialization : MonoBehaviour
    {
        [SerializeField] private TrailRenderer TrailRenderer_Text;
        [SerializeField] private TrailRenderer TrailRenderer_Back;

        private void Awake()
        {
            LevelManager.LevelInitializedEvent += ReferredInitialization;
        }
        private void ReferredInitialization()
        {
            LevelManager.LevelInitializedEvent-= ReferredInitialization;
            TrailRenderer_Text.time *= LevelManager.Instance_.GlobalConsts_.TailLengthMod;
            TrailRenderer_Text.textureScale = new Vector2(TrailRenderer_Text.textureScale.x / LevelManager.Instance_.GlobalConsts_.TailLengthMod, TrailRenderer_Text.textureScale.y);
            TrailRenderer_Back.time *= LevelManager.Instance_.GlobalConsts_.TailLengthMod;
        }
    }
}