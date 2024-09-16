

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace PlannedRout 
{
    public sealed class StartButton : MonoBehaviour
    {
        public event Action StartGameEvent = delegate { };

        [SerializeField] private string GameSceneName;
        [SerializeField] private float StartDelay;
        [SerializeField] private Graphic SelectButtonSymbol;
        [SerializeField] private Graphic Button;
        [SerializeField] private Color SelectedColor;
        [SerializeField] private Color NormalColor;
        [SerializeField] private Color InactiveColor;

        private bool IsSelected = false;

        private void Awake()
        {
            SelectButtonSymbol.gameObject.SetActive(false);
            OnDisable();
        }
        private void OnEnable()
        {
            ChangeColor(IsSelected ? SelectedColor : NormalColor);
        }
        private void OnDisable()
        {
            ChangeColor(InactiveColor);
        }

        public void StartGame()
        {
            if (enabled)
            {
                enabled = false;
                StartGameEvent();
                StartCoroutine(DelayedTransition());
            }
        }
        public void Select()
        {
            if (enabled)
                ChangeColor(SelectedColor);
            IsSelected = true;
            SelectButtonSymbol.gameObject.SetActive(true);
        }
        public void Unselect()
        {
            if (enabled)
                ChangeColor(NormalColor);
            IsSelected = false;
            SelectButtonSymbol.gameObject.SetActive(false);
        }

        private void ChangeColor(Color color)
        {
            SelectButtonSymbol.color = color;
            Button.color = color;
        }
        private IEnumerator DelayedTransition()
        {
            yield return new WaitForSeconds(StartDelay);
            SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
        }
    }
}
