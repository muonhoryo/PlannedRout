

using PlannedRout.PlayersRegistry;
using UnityEngine;

namespace PlannedRout.Audio
{
    public sealed class MainMenuSounds : MonoBehaviour
    {
        [SerializeField] private AudioSource AddSymbolSound;
        [SerializeField] private AudioSource RemoveSymbolSound;
        [SerializeField] private AudioSource ChangeHandledFieldSound;
        [SerializeField] private AudioSource StartGameSound;
        [SerializeField] private RegistryControl Owner;
        [SerializeField] private StartButton StartButton;

        private void Awake()
        {
            Owner.AddSymbolEvent += AddSymbolAction;
            Owner.RemoveSymbolEvent += RemoveSymbolAction;
            Owner.ChangeHandledFieldEvent += ChangeHandledFieldAction;
            StartButton.StartGameEvent+= StartGameAction;
        }
        private void AddSymbolAction()
        {
            AddSymbolSound.Play();
        }
        private void RemoveSymbolAction()
        {
            RemoveSymbolSound.Play();
        }
        private void ChangeHandledFieldAction()
        {
            ChangeHandledFieldSound.Play();
        }
        private void StartGameAction()
        {
            StartGameSound.Play();
        }
    }
}