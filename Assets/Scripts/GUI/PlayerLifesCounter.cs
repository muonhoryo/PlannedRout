using System.Collections;
using System.Collections.Generic;
using PlannedRout.LevelManagment;
using UnityEngine;
using UnityEngine.UI;


namespace PlannedRout
{
    public sealed class PlayerLifesCount : MonoBehaviour
    {
        [SerializeField] private Text Counter;

        private void Awake()
        {
            LevelManager.LevelInitializedEvent += ReferredInitialization;
            PlayerDeath.LifeCountDecreasedEvent += LifeDecreased;
        }
        private void OnDestroy()
        {
            PlayerDeath.LifeCountDecreasedEvent-= LifeDecreased;
        }
        private void ReferredInitialization()
        {
            LevelManager.LevelInitializedEvent-= ReferredInitialization;
            Counter.text = LevelManager.Instance_.GlobalConsts_.PlayerLifesCount.ToString();
        }
        private void LifeDecreased(int lifesCount)
        {
            Counter.text=lifesCount.ToString();
        }
    }
}