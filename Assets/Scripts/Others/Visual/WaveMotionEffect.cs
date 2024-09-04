
using PlannedRout.LevelManagment;
using PlannedRout.LevelObjects.Characters;
using TMPro;
using UnityEngine;

namespace PlannedRout.Visual
{
    public sealed class WaveMotionEffect : MonoBehaviour
    {
        [SerializeField] private MovingComponent Owner;

        private MovingComponent.MovingDirection MovingDirection;
        private float TargetRotation;
        private int StepMod=1;

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
            TargetRotation = LevelManager.Instance_.GlobalConsts_.WaveEffectRadius;
            MovingDirection = MovingComponent.MovingDirection.Right;
        }
        private void ChangeDirection(MovingComponent.MovingDirection direction)
        {
            if (direction != MovingDirection)
            {
                int diff = Mathf.Abs((int)direction - (int)MovingDirection);
                StepMod+= diff;
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + diff);
                TargetRotation = ((int)direction * 90 + (TargetRotation - (int)MovingDirection * 90)+360)%360;
                MovingDirection = direction;
            }
        }
        private void OnDestroy()
        {
            Owner.ChangeDirectionEvent -= ChangeDirection;
        }
        private void FixedUpdate()
        {
            float angle = transform.localEulerAngles.z;
            int rotationSide;
            float diff = (TargetRotation - angle+360)%360;
            float step = LevelManager.Instance_.GlobalConsts_.WaveEffectSpeed * StepMod;
            if (diff < 0)
                diff += 360;
            if (diff > 180)
            {
                rotationSide = -1;
                diff = 360 - diff;
            }
            else
            {
                rotationSide = 1;
            }
            if (diff <= step)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, TargetRotation);
                ChangeTargetRotation();
            }
            else
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y,
                    transform.localEulerAngles.z + step*rotationSide);
            }
        }
        private void ChangeTargetRotation()
        {
            TargetRotation = (360 - TargetRotation + (int)MovingDirection * 180) % 360;
            if (StepMod != 1)
                StepMod = 1;
        }
    }
}
