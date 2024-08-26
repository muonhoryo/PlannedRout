

using PlannedRout.LevelManagment;
using PlannedRout.LevelObjects.Characters;
using UnityEngine;

namespace PlannedRout 
{
    public sealed class GamePause : MonoBehaviour
    {
        public static GamePause Instance_ { get; private set; }

        public bool IsPaused_ { get; private set; } = false;


        private void Awake()
        {
            Instance_ = this;
        }
        public void PauseGame()
        {
            if (!IsPaused_)
            {
                LevelManager.Instance_.PlayerCharacter_.GetComponent<MovingComponent>().enabled = false;
                LevelManager.Instance_.EnemyCharacter_Red_.GetComponent<MovingComponent>().enabled = false;
                LevelManager.Instance_.EnemyCharacter_Blue_.GetComponent<MovingComponent>().enabled = false;
                LevelManager.Instance_.EnemyCharacter_Pink_.GetComponent<MovingComponent>().enabled = false;
                LevelManager.Instance_.EnemyCharacter_Orange_.GetComponent<MovingComponent>().enabled = false;
            }
        }
        public void UnpauseGame()
        {
            if (IsPaused_)
            {
                LevelManager.Instance_.PlayerCharacter_.GetComponent<MovingComponent>().enabled = true;
                LevelManager.Instance_.EnemyCharacter_Red_.GetComponent<MovingComponent>().enabled = true;
                LevelManager.Instance_.EnemyCharacter_Blue_.GetComponent<MovingComponent>().enabled = true;
                LevelManager.Instance_.EnemyCharacter_Pink_.GetComponent<MovingComponent>().enabled = true;
                LevelManager.Instance_.EnemyCharacter_Orange_.GetComponent<MovingComponent>().enabled = true;
            }
        }
    }
}