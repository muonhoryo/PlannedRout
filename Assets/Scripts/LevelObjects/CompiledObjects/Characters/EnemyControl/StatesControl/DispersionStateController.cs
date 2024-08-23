

using System.Collections;
using PlannedRout.LevelManagment;
using UnityEngine;

namespace PlannedRout.LevelObjects.Characters
{
    public sealed class DispersionStateController:MonoBehaviour
    {
        [SerializeField] private EnemyBehaviour Owner;

        private int DispersionEntersCount = 0;

        private Coroutine ActiveCoroutine;

        private void Awake()
        {
            Owner.ChangeBehaviourStateEvent += StateChanged;
        }
        private void StateChanged(EnemyBehaviour.BehaviourStateType type)
        {
            if(type==EnemyBehaviour.BehaviourStateType.Dispersion)
            {
                DispersionEntersCount++;
                ActiveCoroutine = StartCoroutine(DispersionExit());
            }
            else if (type == EnemyBehaviour.BehaviourStateType.Persecution)
            {
                ActiveCoroutine = StartCoroutine(DispersionEnter());
            }
        }
        private IEnumerator DispersionExit()
        {
            yield return new WaitForSeconds(LevelManager.Instance_.GlobalConsts_.EnemyDispersionTime);
            if (DispersionEntersCount >= LevelManager.Instance_.GlobalConsts_.MaxDispersionCount)
                Owner.ChangeBehaviourStateEvent -= StateChanged;
            Owner.SelectBehaviourState(EnemyBehaviour.BehaviourStateType.Persecution);
        }
        private IEnumerator DispersionEnter()
        {
            yield return new WaitForSeconds(LevelManager.Instance_.GlobalConsts_.NextDispersionTime);
            Owner.SelectBehaviourState(EnemyBehaviour.BehaviourStateType.Dispersion);
        }
    }
}