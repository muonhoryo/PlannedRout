

using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlannedRout.PlayerControl
{
    public sealed class EndGameBackToMainMenuControl : MonoBehaviour
    {
        [SerializeField] private string BackToMainMenuInputName;
        [SerializeField] private string MainMenuSceneName;

        private void Awake()
        {
            enabled = false;
            PlayerDeath.LifeCountDecreasedEvent += LifeCountDecreased;
        }
        private void Update()
        {
            if (Input.GetButtonDown(BackToMainMenuInputName))
            {
                SceneManager.LoadScene(MainMenuSceneName);
            }
        }
        private void LifeCountDecreased(int count)
        {
            if (count <= 0)
            {
                PlayerDeath.LifeCountDecreasedEvent -= LifeCountDecreased;
                enabled = true;
            }
        }
    }
}