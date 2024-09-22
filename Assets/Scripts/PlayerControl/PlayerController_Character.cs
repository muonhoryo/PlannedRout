

using PlannedRout.LevelManagment;
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

        private float BackToMainMenuTime=float.MaxValue;
        private bool IsActiveInput=false;

        private void Update()
        {
            axisInput_Horizontal = Input.GetAxis(AxisInputName_Horizontal);
            axisInput_Vertical = Input.GetAxis(AxisInputName_Vertical);

            void ChangeDirection(MovingComponent.MovingDirection direction)
            {
                CharacterMovComponent.ChangeDirection(direction);
                if (!IsActiveInput)
                    IsActiveInput = true;
            }

            if (axisInput_Horizontal!=0)
            {
                ChangeDirection(axisInput_Horizontal<0?MovingComponent.MovingDirection.Left:MovingComponent.MovingDirection.Right);
            }
            else if (axisInput_Vertical != 0)
            {
                ChangeDirection(axisInput_Vertical < 0 ? MovingComponent.MovingDirection.Bottom : MovingComponent.MovingDirection.Top);
            }
            else
            {
                if (Time.realtimeSinceStartup >= BackToMainMenuTime)
                {
                    LevelManager.Instance_.PlayerCharacter_.GetComponent<PlayerDeath>().Death();
                    BackToMainMenuTime = Time.realtimeSinceStartup + LevelManager.Instance_.GlobalConsts_.BackMainMenuTime;
                }
                else
                {
                    if (IsActiveInput)
                    {
                        IsActiveInput = false;
                        BackToMainMenuTime = Time.realtimeSinceStartup+LevelManager.Instance_.GlobalConsts_.BackMainMenuTime;
                    }
                }
            }
        }
        private void Awake()
        {
            GamePause.GamePausedEvent += GamePaused;
            LevelManager.LevelInitializedEvent += ReferredInitialization;
        }
        private void ReferredInitialization()
        {
            LevelManager.LevelInitializedEvent -= ReferredInitialization;
            BackToMainMenuTime = Time.realtimeSinceStartup + LevelManager.Instance_.GlobalConsts_.BackMainMenuTime;
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