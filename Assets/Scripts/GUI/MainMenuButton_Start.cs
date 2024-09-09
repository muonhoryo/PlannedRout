using System.Collections;
using System.Collections.Generic;
using PlannedRout.LevelManagment;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlannedRout 
{
    public sealed class MainMenuButton_Start : MainMenuButton
    {
        [SerializeField] private string GameSceneName;
        [SerializeField] private float StartDelay;

        protected override void OnClickAction() 
        {
            enabled = false;
            StartCoroutine(DelayedTransition());
        }
        private IEnumerator DelayedTransition()
        {
            yield return new WaitForSeconds(StartDelay);
            SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
        } 
    }

}