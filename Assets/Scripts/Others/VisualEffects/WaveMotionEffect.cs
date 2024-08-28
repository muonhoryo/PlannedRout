
using PlannedRout.LevelManagment;
using PlannedRout.LevelObjects.Characters;
using UnityEngine;

namespace PlannedRout.VisualEffects
{
    public sealed class WaveMotionEffect : MonoBehaviour
    {
        [SerializeField] private MovingComponent Owner;

        private bool isHorizontalMotion = true;
        private Vector2 TargetPosition;

        private void Awake()
        {
            enabled = false;
            LevelManager.LevelInitializedEvent += ReferredInitialization;
        }
        private void ReferredInitialization()
        {
            LevelManager.LevelInitializedEvent -= ReferredInitialization;
            Owner.ChangeDirectionEvent += ChangeDirection;
            enabled = true;
            TargetPosition = new Vector2(LevelManager.Instance_.GlobalConsts_.WaveEffectRadius, 0);
        }
        private void ChangeDirection(MovingComponent.MovingDirection direction)
        {
            bool needToChange = (((byte)direction &1) == 0)==isHorizontalMotion;
            if (needToChange)
            {
                isHorizontalMotion = !isHorizontalMotion;
                TargetPosition = new Vector2(TargetPosition.y, TargetPosition.x);
            }
        }
        private void OnDestroy()
        {
            Owner.ChangeDirectionEvent -= ChangeDirection;
        }
        private void FixedUpdate()
        {
            Vector2 diff = TargetPosition - (Vector2)transform.localPosition;
            Vector2 step = diff.normalized * LevelManager.Instance_.GlobalConsts_.WaveEffectSpeed;
            float stepLength = step.magnitude;
            if (stepLength==0||step.magnitude >= diff.magnitude)
            {
                transform.localPosition = TargetPosition;
                ChangeTargetPosition();
            }
            else
            {
                transform.localPosition += (Vector3)step;
            }
        }
        private void ChangeTargetPosition()
        {
            TargetPosition = -TargetPosition;
        }
    }
}
