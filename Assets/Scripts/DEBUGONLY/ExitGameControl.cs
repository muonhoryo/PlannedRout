using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGameControl : MonoBehaviour
{
    [SerializeField] private bool IsMainMenu;
    [SerializeField] private string MainMenuSceneName;
    [SerializeField] private string ExitGameInputName;

    private void Update()
    {
        if (Input.GetButtonDown(ExitGameInputName))
        {
            if (IsMainMenu)
            {
                Application.Quit();
            }
            else
            {
                SceneManager.LoadScene(MainMenuSceneName);
            }
        }
    }
}
