

using PlannedRout.LevelObjects.Characters;
using UnityEngine;

namespace PlannedRout.PlayerControl
{
    public sealed class PlayerController_Character:MonoBehaviour
    {
        [SerializeField] private string AxisInputName_Horizontal;
        [SerializeField] private string AxisInputName_Vertical;
        [SerializeField] private MovingComponent CharacterMovComponent;

        private float axisInput_Horizontal;
        private float axisInput_Vertical;

        private void Update()
        {
            axisInput_Horizontal = Input.GetAxis(AxisInputName_Horizontal);
            axisInput_Vertical = Input.GetAxis(AxisInputName_Vertical);

            if (axisInput_Horizontal!=0)
            {
                CharacterMovComponent.ChangeDirection(axisInput_Horizontal<0?MovingComponent.MovingDirection.Left:MovingComponent.MovingDirection.Right);
            }
            else if (axisInput_Vertical != 0)
            {
                CharacterMovComponent.ChangeDirection(axisInput_Vertical < 0 ? MovingComponent.MovingDirection.Bottom : MovingComponent.MovingDirection.Top);
            }
        }
        private void Awake()
        {
            GamePause.GamePausedEvent += GamePaused;
        }
        private void OnDestroy()
        {
            GamePause.GamePausedEvent -= GamePaused;
            GamePause.GameUnpausedEvent -= GameUnpaused;
        }
        private void GamePaused()
        {
            GamePause.GamePausedEvent -= GamePaused;
            GamePause.GameUnpausedEvent += GameUnpaused;
            enabled = false;
        }
        private void GameUnpaused()
        {
            GamePause.GamePausedEvent += GamePaused;
            GamePause.GameUnpausedEvent -= GameUnpaused;
            enabled = true;
        }
    }
}