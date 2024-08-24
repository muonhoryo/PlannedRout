

using PlannedRout;
using PlannedRout.LevelManagment;
using UnityEngine;
using UnityEngine.UI;

public sealed class LifeCounter : MonoBehaviour 
{
    [SerializeField] private Text CounterText;

    private void Awake()
    {
        PlayerDeath.LifeCountDecreasedEvent += LifeCountDecreased;
        LevelManager.LevelInitializedEvent += ReferredInitialization;
    }
    private void ReferredInitialization()
    {
        LevelManager.LevelInitializedEvent -= ReferredInitialization;
        LifeCountDecreased(LevelManager.Instance_.GlobalConsts_.PlayerLifeCount);
    }
    private void LifeCountDecreased(int remainedLifes)
    {
        CounterText.text=remainedLifes.ToString();
    }
}