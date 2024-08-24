

using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout.LevelObjects.Characters
{
    public sealed class EnemyDeathBringing : MonoBehaviour
    {
        private PlayerDeath Target;

        private void Awake()
        {
            LevelManager.LevelInitializedEvent += ReferredInitialization;
        }
        private void ReferredInitialization()
        {
            LevelManager.LevelInitializedEvent -= ReferredInitialization;
            Target = LevelManager.Instance_.PlayerCharacter_.GetComponent<PlayerDeath>();
        }
        private void LateUpdate()
        {
            if(Vector2.Distance(transform.position,Target.transform.position)<=
                LevelManager.Instance_.GlobalConsts_.MinDistanceToPlayerDeath)
                Target.Death();
        }
    }
}