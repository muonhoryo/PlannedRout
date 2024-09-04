using System.Collections;
using System.Collections.Generic;
using PlannedRout.LevelObjects.Characters;
using UnityEngine;

namespace PlannedRout.Visual
{
    public sealed class MovingDirRotation : MonoBehaviour
    {
        [SerializeField] private GameObject RotatedObject;
        [SerializeField] private MovingComponent TargetComponent;

        private void Awake()
        {
            TargetComponent.ChangeDirectionEvent += ChangeDirection;
        }
        private void ChangeDirection(MovingComponent.MovingDirection direction)
        {
            RotatedObject.transform.localEulerAngles = new Vector3(
                RotatedObject.transform.localEulerAngles.x,
                RotatedObject.transform.localEulerAngles.y,
                (int)direction * 90);
        }
    }
}
