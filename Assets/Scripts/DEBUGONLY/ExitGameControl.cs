using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGameControl : MonoBehaviour
{
    [SerializeField] private bool IsMainMenu;
    [SerializeField] private string MainMenuSceneName;
    [SerializeField] private string ExitGameInputName_1;
    [SerializeField] private string ExitGameInputName_2;
    [SerializeField] private string ExitGameInputName_3;


    private void Update()
    {
        if (Input.GetButton(ExitGameInputName_1)&&
            Input.GetButton(ExitGameInputName_2)&&
            Input.GetButton(ExitGameInputName_3))
        {
            if (IsMainMenu)
            {
                Application.Quit();
            }
            else
            {
                SceneManager.LoadScene(MainMenuSceneName);
            }
            Input.ResetInputAxes();
        }
    }
}
